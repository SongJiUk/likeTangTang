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
    public Action<int> OnChangeSecond;
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

    public override void Init()
    {
        base.Init();
        SceneType = SceneType.GameScene;
        gm = Manager.GameM;
        Manager.UpdateM.Register(this);
        Manager.UiM.ShowPopup<UI_JoyStick>();
        
        if(Manager.GameM.ContinueDatas.isContinue)
        {
            player = Manager.ObjectM.Spawn<PlayerController>(Vector3.zero, Manager.GameM.ContinueDatas.PlayerDataID);
        }
        else
        {
            //
            player = Manager.ObjectM.Spawn<PlayerController>(Vector3.zero, Define.DEFAULT_PLAYER_ID);
        }
        

        StageLoad();

        player.OnPlayerDead = OnPlayerDead;
        Manager.GameM.Camera = FindObjectOfType<CameraController>();
        Manager.GameM.Camera.Target = player.gameObject;
        

        ui = Manager.UiM.ShowPopup<UI_GameScene>();

        OnWaveStart = ui.OnWaveStart;
        OnWaveEnd = ui.OnWaveEnd;
        OnChangeSecond = ui.OnChangeSecond;
        //[ ] : UI 업데이트.

    }
    public override void Clear()
    {
        
    }
    void StageLoad()
    {
        if(spawnManager == null) 
            spawnManager = gameObject.AddComponent<SpawnManager>();
        Manager.ObjectM.LoadMap(Manager.GameM.CurretnStageData.MapName);
        Manager.ObjectM.Spawn<GridController>(Vector3.zero);
        
        StopAllCoroutines();
        StartCoroutine(StartWave(Manager.GameM.CurrentWaveData));
    }

    IEnumerator StartWave(Data.WaveData _wave)
    {
        yield return new WaitForEndOfFrame();
        //웨이브 시작.
        /*
         * [ ]웨이브 시작 이벤트 Invoke
         * [ ] 웨이브가 1단계면 랜덤으로 몇개 생성,
         * [ ] 포션, 자석, 폭탄도 랜덤으로 하나 생성
         * [ ]웨이브 시간으로 할지, 그냥 00 : 00 에서 계속 올릴지 생각해보자.
         * [ ]자기장오게 vs 안오게 고민해보기
         * [ ]몬스터 생성
         */
        OnWaveStart?.Invoke(_wave.WaveIndex);

        if(_wave.WaveIndex == 1)
        {
            CreateRandomExp();
        }
        spawnManager.StartSpawn();
        Manager.GameM.SaveGame();

        EliteMonsterController eliteMonster;
        Vector2 spawnPos = Utils.CreateMonsterSpawnPoint(Manager.GameM.player.Standard.position);
        for (int i =0; i<Manager.GameM.CurrentWaveData.EleteMonsterID.Count; i++)
        {
            eliteMonster = Manager.ObjectM.Spawn<EliteMonsterController>(spawnPos, Manager.GameM.CurrentWaveData.EleteMonsterID[i]);
            eliteMonster.MonsterInfoUpdate -= ui.MonsterInfoUpdate;
            eliteMonster.MonsterInfoUpdate += ui.MonsterInfoUpdate;
        }

        yield break;
    }

    void WaveEnd()
    {
        OnWaveEnd?.Invoke();

        if(gm.CurrentWaveIndex < gm.CurretnStageData.WaveArray.Count - 1)
        {
            gm.CurrentWaveIndex++;
            StopAllCoroutines();

            StartCoroutine(StartWave(gm.CurretnStageData.WaveArray[gm.CurrentWaveIndex]));
        }
        else // BOSS
        {
            Vector2 spawnPos = Utils.CreateMonsterSpawnPoint(gm.player.transform.position);

            for(int i =0; i<gm.CurrentWaveData.BossMonsterID.Count; i++)
            {
                bossMonster = Manager.ObjectM.Spawn<BossController>(spawnPos, gm.CurrentWaveData.BossMonsterID[i]);
                bossMonster.BossMonsterInfoUpdate -= ui.BossMonsterInfoUpdate;
                bossMonster.BossMonsterInfoUpdate += ui.BossMonsterInfoUpdate;
                bossMonster.OnBossDead -= OnBossDead;
                bossMonster.OnBossDead += OnBossDead;

            }
        }
    }
    void OnBossDead()
    {
        //TODO : 보스가 죽으면, 게임을 끝내고, 게임 결과창을 띄움
    }
    float lastSecond = Define.WAVE_REWARD_TIME;
    public void Tick(float _deltaTime)
    {
        if(isGameEnd || gm.CurrentWaveData == null) return;

        if(bossMonster == null) tm.TimeRemaining -= _deltaTime;
        else tm.TimeRemaining = 0f;


        int currentSecond = (int)tm.TimeRemaining;

        if(currentSecond != lastSecond)
        {
            //TOOD : 시간초 UI변경
            OnChangeSecond?.Invoke(currentSecond);

            if(currentSecond == WAVE_REWARD_TIME) SpawnReward();

        }

        if(tm.TimeRemaining < 0) WaveEnd();

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
            GemController gem = Manager.ObjectM.Spawn<GemController>(Utils.CreateObjectAroundPlayer(Manager.GameM.player.transform.position));
            gem.SetInfo(Manager.GameM.GetGemInfo(type));
           
        }
    }

    public void OnPlayerDead()
    {
        if(!Manager.GameM.isGameEnd)
        {
            //[ ] : GameContinuePopup생성해서 수정
            UI_GameResultPopup rp = Manager.UiM.ShowPopup<UI_GameResultPopup>();
            rp.Init();
            rp.SetInfo();
        }
    }

    public void SpawnReward()
    {
        DropItem dropItemType = (DropItem)UnityEngine.Random.Range(0,3);

        Vector3 spawnPos = Utils.CreateObjectAroundPlayer(gm.player.transform.position);
        Data.DropItemData dropItem;

        // switch(dropItemType)
        // {
        //     case DropItem.Potion :
        //         if(Manager.DataM.DropItemDic.TryGetValue(POTION_ID, out dropItem))
                    

        //     break;

        //     case DropItem.Magnet :
        //     break;

        //     case DropItem.Bomb :
        //     break;
        // }
    }

    #region  전에 쓰던 잼, 킬 코드
    // int maxGemCount = 10;
    // void HandleOnChangeGemCount(int _count)
    // {
    //     // [ ] : 젬카운트가 바뀌면 해줘야할것 (개수 파악 후 레벨업, 슬라이더 업데이트 )
    //     Manager.UiM.GetSceneUI<UI_GameScene>().SetGemCountRatio((float)_count / maxGemCount);

    //     if(_count == maxGemCount)
    //     {
    //         Manager.UiM.ShowPopup<UI_SkillSelectPopup>();
    //         Manager.GameM. = 0;
    //         maxGemCount *= 2;
    //         Manager.GameM.player.Level++;
    //         Manager.UiM.GetSceneUI<UI_GameScene>().SetPlayerLevel(Manager.GameM.player.Level);
    //         // [ ]: 플레이어 레벨 관리 (데이터)
    //         Time.timeScale = 0;

    //     }
    // }

    // void HandleOnChangeKillCount(int _count)
    // {   
    //     Manager.UiM.GetSceneUI<UI_GameScene>().SetKillCount(_count);
    //     //[ ] 데이터시트에서 가져와서 계속 수정
    //     if(_count == 5000)
    //     {
    //         StageType = Define.StageType.Boss;
    //         Manager.ObjectM.DeSpawnAllMonster();

    //         // 보스 소환
    //        Vector2 pos = Utils.CreateMonsterSpawnPoint(Manager.GameM.player.transform.position, 10, 15);
    //         Manager.ObjectM.Spawn<MonsterController>(pos, 3);
    //     }
    // }

    // void OnDestroy()
    // {
    //     if(Manager.GameM != null)
    //     {
    //         Manager.GameM.OnChangeGemCount -= HandleOnChangeGemCount;
    //         Manager.GameM.OnChangeKillCount -= HandleOnChangeKillCount;
    //     }
    // }
    #endregion

    void OnDestroy()
    {
        Manager.UpdateM.Unregister(this);
    }
}
