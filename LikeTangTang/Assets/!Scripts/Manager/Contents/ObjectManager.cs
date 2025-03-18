using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectManager
{
    public PlayerController Player { get; private set; }
    public HashSet<MonsterController> mcSet = new HashSet<MonsterController>();
    public HashSet<ProjectileController> pjSet = new HashSet<ProjectileController>();
    public HashSet<GemController> gemSet = new HashSet<GemController>();


    /* 오브젝트 생성, 스폰등 관리
     *  
     * 스폰(ID를 받아서 리턴해줌) - Generic
     * 디스폰
    */

    public T Spawn<T>(Vector3 _pos, int _templateID = 0) where T : BaseController
    {
        System.Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            //TODO: Data에서 값을 가져와서 생성.
            GameObject go = Manager.ResourceM.Instantiate("Slime_01.prefab");
            go.name = "!Player";
            go.transform.position = _pos;
            PlayerController pc = go.GetComponent<PlayerController>();
            Player = pc;

            pc.Init();

            return pc as T;
        }
        else if(type == typeof(MonsterController))
        {
            string id = (_templateID == 0 ? "Goblin_01.prefab" : "Snake_01.prefab");

            GameObject go = Manager.ResourceM.Instantiate(id, null, true);
            go.transform.position = _pos;

            MonsterController mc = Utils.GetOrAddComponent<MonsterController>(go);

            mc.Init();
            return mc as T;

        }
        else if(type == typeof(GemController))
        {
            GameObject go = Manager.ResourceM.Instantiate("Gem.prefab", null, true);
            go.transform.position = _pos;

            GemController gc = Utils.GetOrAddComponent<GemController>(go);

            gc.Init();
            return gc as T;


        }
        return null;
    }

    public void DeSpawn<T>(T _obj) where T : BaseController
    {
        System.Type type = typeof(T);

        if(type == typeof(PlayerController))
        {
            /*TODO : 플레이어 죽으면 해야될것
             *  플레이어가 죽는다면 게임을 정지하고 UI화면 띄워야함
             *  
             */

        }
        else if(type == typeof(MonsterController))
        {
            mcSet.Remove(_obj as MonsterController);
            Manager.ResourceM.Destory(_obj.gameObject);
        }
        else if(type == typeof(GemController))
        {
            gemSet.Remove(_obj as GemController);
            Manager.ResourceM.Destory(_obj.gameObject);
        }

    }
}
