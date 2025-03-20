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

        Manager.ObjectM.Spawn<GridController>(Vector3.zero);
        var player = Manager.ObjectM.Spawn<PlayerController>(Vector3.zero);

        spawnManager = gameObject.AddComponent<SpawnManager>();

        
        //for(int i = 0; i< 10; i++)
        //{
        //    int RandNum = Random.Range(0, 2);
        //    var monster = Manager.ObjectM.Spawn<MonsterController>(RandNum);

        //    monster.transform.position = new Vector2(Random.RandomRange(-10, 10), Random.RandomRange(-10, 10));
        //    monster.name = $"!Monster{i}";
        //}



        //Todo : UI매니저 만들면 바꾸자.
        var joyStick = Manager.ResourceM.Instantiate("UI_Joystick.prefab");
        joyStick.name = "@UI_Joystick";

        Camera.main.GetComponent<CameraController>().Target = player.gameObject;


        Manager.DataM.Init();
        //foreach(var PlayerData in Manager.DataM.PlayerDic.Values)
        //{
        //    Debug.Log($"LV : {PlayerData.level}, Hp : {PlayerData.maxHp}");
        //}

        foreach (var MonsterData in Manager.DataM.MonsterDic.Values)
        {
            Debug.Log($"name : {MonsterData.name}, DropData : {MonsterData.rare}");
        }
    }

}
