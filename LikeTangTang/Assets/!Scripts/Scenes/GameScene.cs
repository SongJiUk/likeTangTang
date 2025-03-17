using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameScene : MonoBehaviour
{

    //TODO
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

        Manager.ResourceM.LoadAllAsync<GameObject>("PrevLoad", (key, loadCount, maxCount) =>
        {
            Debug.Log($"{loadCount} / {maxCount}");

            if (loadCount == maxCount) StartLoad();
        });

    }

    void StartLoad()
    {
        var player = Manager.ResourceM.Instantiate("Slime_01.prefab");

        var joyStick = Manager.ResourceM.Instantiate("UI_Joystick.prefab");
        joyStick.name = "@UI_Joystick";

        Camera.main.GetComponent<CameraController>().Target = player;
    }

}
