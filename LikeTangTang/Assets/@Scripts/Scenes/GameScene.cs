using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameScene : BaseScene
{
    SpawnManager spawnManager;
    PlayerController player;
    UI_GameScene ui;
    Define.StageType stageType;
    public Define.StageType StageType
    {
        get { return stageType; }
        set 
        { 
            stageType = value; 
            if(spawnManager != null)
            {
                switch(stageType)
                {
                    case Define.StageType.Normal :
                        spawnManager.isStop = false;
                    break;

                    case Define.StageType.Boss :
                        spawnManager.isStop = true;
                    break;
                }
            }
        }
    }

    public override void Init()
    {
        base.Init();
        SceneType = Define.SceneType.GameScene;
        
        Manager.UiM.ShowPopup<UI_JoyStick>();
        
        if(Manager.GameM.ContinueDatas.isContinue)
        {
            Manager.GameM.gameData.userID = Manager.DataM.CreatureDic[Manager.GameM.ContinueDatas.PlayerDataID].DataID;
            player = Manager.ObjectM.Spawn<PlayerController>(Vector3.zero, Manager.GameM.ContinueDatas.PlayerDataID);
        }
        else
        {
            Manager.GameM.gameData.userID = Manager.DataM.CreatureDic[Define.DEFAULT_PLAYER_ID].DataID;
            player = Manager.ObjectM.Spawn<PlayerController>(Vector3.zero, Define.DEFAULT_PLAYER_ID);
        }
        

        StageLoad();
        Manager.GameM.Camera = FindObjectOfType<CameraController>();
        Manager.GameM.Camera.Target = player.gameObject;
        

        ui = Manager.UiM.ShowPopup<UI_GameScene>();
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
        StartCoroutine(StartWave());
    }

    IEnumerator StartWave()
    {
        yield return new WaitForEndOfFrame();
        //웨이브 시작.

        
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
    //         Vector2 pos = Utils.CreateMonsterSpawnPoint(Manager.GameM.player.transform.position, 10, 15);
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
}
