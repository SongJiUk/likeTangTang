using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameScene : MonoBehaviour
{
    SpawnManager spawnManager;
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


    private void Start()
    {
        Manager.ResourceM.LoadAllAsync<Object>("PrevLoad", (key, loadCount, maxCount) =>
        {
            Debug.Log($"{key}, {loadCount} / {maxCount}");
            if (loadCount == maxCount)
            {
                StartLoad();
            }
                
        });

    }

    void StartLoad()
    {
        Manager.DataM.Init();
        Manager.UiM.ShowSceneUI<UI_GameScene>();
        
        Manager.ObjectM.Spawn<GridController>(Vector3.zero);
        var player = Manager.ObjectM.Spawn<PlayerController>(Vector3.zero);

        spawnManager = gameObject.AddComponent<SpawnManager>();

        //[ ] : 이름 자동화(이건 데이터에서 쉽게 가져 올 수 있을듯)
        Manager.UiM.GetSceneUI<UI_GameScene>().CreateJoyStick("UI_Joystick.prefab");

        // var joyStick = Manager.ResourceM.Instantiate("UI_Joystick.prefab");
        // joyStick.name = "@UI_Joystick";

        Camera.main.GetComponent<CameraController>().Target = player.gameObject;
        
        Manager.GameM.OnChangeGemCount += HandleOnChangeGemCount;
        Manager.GameM.OnChangeKillCount += HandleOnChangeKillCount;

    }

    int maxGemCount = 10;
    void HandleOnChangeGemCount(int _count)
    {
        // [ ] : 젬카운트가 바뀌면 해줘야할것 (개수 파악 후 레벨업, 슬라이더 업데이트 )
        Manager.UiM.GetSceneUI<UI_GameScene>().SetGemCountRatio((float)_count / maxGemCount);

        if(_count == maxGemCount)
        {
            Manager.UiM.ShowPopup<UI_SkillSelectPopup>();
            Manager.GameM.Gem = 0;
            maxGemCount *= 2;

            // [ ]: 플레이어 레벨 관리 (데이터)
            Time.timeScale = 0;
        }
    }

    void HandleOnChangeKillCount(int _count)
    {   
        Manager.UiM.GetSceneUI<UI_GameScene>().SetKillCount(_count);
        //[ ] 데이터시트에서 가져와서 계속 수정
        if(_count == 5)
        {
            StageType = Define.StageType.Boss;
            Manager.ObjectM.DeSpawnAllMonster();

            // 보스 소환
            Vector2 pos = Utils.CreateMonsterSpawnPoint(Manager.GameM.player.transform.position, 10, 15);
            Manager.ObjectM.Spawn<MonsterController>(pos, 3);
        }
    }

    void OnDestroy()
    {
        if(Manager.GameM != null)
        {
            Manager.GameM.OnChangeGemCount -= HandleOnChangeGemCount;
            Manager.GameM.OnChangeKillCount -= HandleOnChangeKillCount;
        }
    }
}
