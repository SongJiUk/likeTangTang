using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameScene : MonoBehaviour
{
    SpawnManager spawnManager;
    //
    /*
     일단 여기서 불러오는 역할을 해야됨. ResourceManager에서 Addressable를 이용하여 모두 불러옴.
     다 불러왔으면 로드


    로드에선 데이터정보, Pooling, 조이스틱, 맵, 카메라등을 초기 설정해준다.
     
     */
    private void Start()
    {
        //Manager.ResourceM.LoadAsync<GameObject>("Goblin_01.prefab", (go) => {
        //    Debug.Log($"{go.name}");
        //});

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

        //TODO : UI매니저 만들면 바꾸자.
        var joyStick = Manager.ResourceM.Instantiate("UI_Joystick.prefab");
        joyStick.name = "@UI_Joystick";

        Camera.main.GetComponent<CameraController>().Target = player.gameObject;


        Manager.GameM.OnChangeGemCount += HandleOnChangeGemCount;
        Manager.GameM.OnChangeKillCount += HandleOnChangeKillCount;

    }

    void HandleOnChangeGemCount(int _count)
    {
        //TODO : 젬카운트가 바뀌면 해줘야할것 (개수 파악 후 레벨업, 슬라이더 업데이트 )
        Manager.UiM.GetSceneUI<UI_GameScene>().SetGemCountRatio((float)_count / 10);
    }

    void HandleOnChangeKillCount(int _count)
    {
        Manager.UiM.GetSceneUI<UI_GameScene>().SetKillCount(_count);
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
