using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using static Define;


public class GameScene : BaseScene, ITickable
{
    GameManager gm;
    TimeManager tm;
    SpawnManager spawnManager;
    PlayerController player;
    UI_GameScene ui;
    Define.StageType stageType;
    BossController bossMonster;

    #region Action
    public Action<int> OnWaveStart;
    public Action<int, int> OnChangeSecond;
    public Action OnWaveEnd;
    
    #endregion
    public StageType StageType
    {
        get { return stageType; }
        set 
        { 
            stageType = value; 
            if(spawnManager != null)
            {
                switch(stageType)
                {
                    case StageType.Normal :
                        spawnManager.isStop = false;
                    break;

                    case StageType.Boss :
                        spawnManager.isStop = true;
                    break;
                }
            }
        }
    }

    bool isGameEnd = false;

    private void Awake()
    {
        Init();
        SceneChangeAnimation_Out anim = Manager.ResourceM.Instantiate("SceneChangeAnimation_Out").GetOrAddComponent<SceneChangeAnimation_Out>();
        anim.SetInfo(SceneType, () => { });
        Manager.GameM.TimeRemaining = Manager.GameM.CurrentStageData.WaveArray[Manager.GameM.CurrentWaveIndex].RemainsTime;
    }
    public override void Init()
    {
        base.Init();
        SceneType = SceneType.GameScene;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        gm = Manager.GameM;
        tm = Manager.TimeM;
        Manager.UpdateM.Register(this);
        Manager.UiM.ShowSceneUI<UI_JoyStick>();
        
        if(Manager.GameM.ContinueDatas.isContinue)
        {
            player = Manager.ObjectM.Spawn<PlayerController>(Vector3.zero, Manager.GameM.ContinueDatas.PlayerDataID);
        }
        else
        {
            gm.ClearContinueData();
            player = Manager.ObjectM.Spawn<PlayerController>(Vector3.zero, Define.DEFAULT_PLAYER_ID);
        }
        

        StageLoad();

        player.OnPlayerDead = OnPlayerDead;
        Manager.GameM.Camera = FindObjectOfType<CameraController>();
        Manager.GameM.Camera.Target = player.gameObject;
        

        ui = Manager.UiM.ShowSceneUI<UI_GameScene>();
        
        player.Skills.RefreshSkillUI();

        OnWaveStart = ui.OnWaveStart;
        OnWaveEnd = ui.OnWaveEnd;
        OnChangeSecond = ui.OnChangeSecond;

    }
    public override void Clear()
    {
        
    }
    void StageLoad()
    {
        if(spawnManager == null) 
            spawnManager = gameObject.AddComponent<SpawnManager>();

        Manager.ObjectM.LoadMap(Manager.GameM.CurrentStageData.MapName);
        

        tm.TimeReset();

        StopAllCoroutines();
        StartCoroutine(StartWave(Manager.GameM.CurrentStageData.WaveArray[Manager.GameM.CurrentWaveIndex]));
    }

    IEnumerator StartWave(Data.WaveData _wave)
    {
        yield return new WaitForEndOfFrame();
        
        OnWaveStart?.Invoke(_wave.WaveIndex);
        if(_wave.WaveIndex == 1)
        {
            CreateRandomExp();
        }
        SpawnWaveReward();
        Manager.GameM.TimeRemaining = Manager.GameM.CurrentStageData.WaveArray[Manager.GameM.CurrentWaveIndex].RemainsTime;
        Manager.GameM.CurrentMap.MagneticFieldReduction();


        spawnManager.StartSpawn();
        Manager.GameM.SaveGame();

        if(Manager.GameM.CurrentWaveData.EleteMonsterID.Count > 0)
        {
            EliteMonsterController eliteMonster;
            Vector2 spawnPos = Utils.CreateMonsterSpawnPoint(Manager.GameM.player.Standard.position);
            for (int i =0; i<Manager.GameM.CurrentWaveData.EleteMonsterID.Count; i++)
            {
                eliteMonster = Manager.ObjectM.Spawn<EliteMonsterController>(spawnPos, Manager.GameM.CurrentWaveData.EleteMonsterID[i]);
                eliteMonster.MonsterInfoUpdate -= ui.MonsterInfoUpdate;
                eliteMonster.MonsterInfoUpdate += ui.MonsterInfoUpdate;
            }
        }

        yield break;
    }

    public void WaveEnd()
    {
        OnWaveEnd?.Invoke();

        if(gm.CurrentWaveIndex < gm.CurrentStageData.WaveArray.Count - 1)
        {
            gm.CurrentWaveIndex++;
            StopAllCoroutines();

            StartCoroutine(StartWave(gm.CurrentStageData.WaveArray[gm.CurrentWaveIndex]));
        }
        else
        {
            Vector2 spawnPos = Utils.CreateMonsterSpawnPoint(gm.player.transform.position, 10, 15);

            for(int i =0; i<gm.CurrentWaveData.BossMonsterID.Count; i++)
            {
                bossMonster = Manager.ObjectM.Spawn<BossController>(spawnPos, gm.CurrentWaveData.BossMonsterID[i]);
                bossMonster.MonsterInfoUpdate -= ui.MonsterInfoUpdate;
                bossMonster.MonsterInfoUpdate += ui.MonsterInfoUpdate;
                bossMonster.OnBossDead -= OnBossDead;
                bossMonster.OnBossDead += OnBossDead;
            }
        }
    }

    void SpawnWaveReward()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        DropItemType dropitemType;

        if(rand < 60)
            dropitemType = DropItemType.Potion;
        else if (rand < 80)
            dropitemType = DropItemType.Magnet;
        else
            dropitemType = DropItemType.Bomb;

        Vector3 spawnPos = Utils.CreateObjectAroundPlayer(Manager.GameM.player.transform.position);

        Data.DropItemData dropItem;
        
        switch(dropitemType)
        {
            case DropItemType.Potion :
            if(Manager.DataM.DropItemDic.TryGetValue(POTION_ID, out dropItem))
            {
                var obj = Manager.ObjectM.Spawn<PotionController>(spawnPos, _prefabName: Define.DROPITEMNAME);
                obj.Init();
                obj.SetInfo(dropItem);
            }

            break;

            case DropItemType.Magnet :
            if(Manager.DataM.DropItemDic.TryGetValue(MAGNET_ID, out dropItem))
            {
                var obj = Manager.ObjectM.Spawn<MagnetController>(spawnPos, _prefabName: Define.DROPITEMNAME);
                obj.Init();
                obj.SetInfo(dropItem);
            }
            break;

            case DropItemType.Bomb :
            if(Manager.DataM.DropItemDic.TryGetValue(BOMB_ID, out dropItem))
            {
                var obj = Manager.ObjectM.Spawn<BombController>(spawnPos,_prefabName: Define.DROPITEMNAME);
                obj.Init();
                obj.SetInfo(dropItem);
            }
            break;

        }

    }

    void OnBossDead()
    {
        StartCoroutine(CoGameEnd());
    }
    
    IEnumerator CoGameEnd()
    {
        yield return new WaitForSeconds(1f);
        isGameEnd = true;
        if (Manager.GameM.MissionDic.TryGetValue(MissionTarget.StageClear, out MissionInfo missionInfo))
            missionInfo.Progress++;

        Manager.GameM.isGameEnd = true;
        UI_GameResultPopup rp = Manager.UiM.ShowPopup<UI_GameResultPopup>();
        rp.SetInfo();
    }

 
    float lastSecond = WAVE_REWARD_TIME;
    public void Tick(float _deltaTime)
    {
        Manager.GameM.ElapsedTime += _deltaTime;

        if (isGameEnd || gm.CurrentWaveData == null) return;

        if (bossMonster == null)
            Manager.GameM.TimeRemaining -= _deltaTime;
        else
            Manager.GameM.TimeRemaining = 0;

        int currentMinute = tm.GetCurrentMinute();
        int currentSecond = tm.GetCurrentSecond();

        if (currentSecond != lastSecond)
        {
            OnChangeSecond?.Invoke(currentMinute, currentSecond);

            Manager.GameM.minute = currentMinute;
            Manager.GameM.second = Mathf.Clamp(59 - currentSecond, 0, 59);
            
            if (currentSecond == WAVE_REWARD_TIME) SpawnWaveReward();
        }
        if (Manager.GameM.TimeRemaining < 0)
        {
            WaveEnd();
        }


        lastSecond = currentSecond;
    }

    public void CreateRandomExp()
    {
        int[] randBox = new int[] { 1, 2, 5, 10 };
        List<GemInfo.GemType> gems = new List<GemInfo.GemType>();

        int remainValue = 30;
        while(remainValue > 0)
        {
            int randindex = UnityEngine.Random.Range(0, randBox.Length);
            int randBoxValue = randBox[randindex];

            if(remainValue >= randBoxValue)
            {
                GemInfo.GemType gemType = (GemInfo.GemType)randBoxValue;
                gems.Add(gemType);
                remainValue -= randBoxValue;
            }
        }

        foreach(GemInfo.GemType type in gems)
        {
            GemController gem = Manager.ObjectM.Spawn<GemController>(Utils.CreateObjectAroundPlayer(Manager.GameM.player.transform.position), _prefabName : Define.DROPITEMNAME);
            gem.SetInfo(Manager.GameM.GetGemInfo(type));
           
        }
    }

    public void OnPlayerDead()
    {
        if(!Manager.GameM.isGameEnd)
        {
            //[ ] : GameContinuePopup생성해서 수정
            UI_ContinuePopup popup = Manager.UiM.ShowPopup<UI_ContinuePopup>();
            popup.SetInfo();
            Manager.GameM.SaveGame();
        }
    }

   
    void OnDestroy()
    {
        Manager.UpdateM.Unregister(this);
    }

    //TODO : 지우기
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Manager.GameM.player.Exp += 10;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Manager.ObjectM.KillAllMonsters();
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            WaveEnd();
        }
    }

}
