using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectManager
{
    public PlayerController Player { get; private set; }
    public HashSet<MonsterController> mcSet = new HashSet<MonsterController>();
    public HashSet<ProjectileController> pjSet = new HashSet<ProjectileController>();


    /* 오브젝트 생성, 스폰등 관리
     *  
     * 스폰(ID를 받아서 리턴해줌) - Generic
     * 디스폰
    */

    public T Spawn<T>(int _templateID = 0) where T : BaseController
    {
        System.Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            //TODO: Data에서 값을 가져와서 생성.
            GameObject go = Manager.ResourceM.Instantiate("Slime_01.prefab");
            go.name = "!Player";

            PlayerController pc = go.GetComponent<PlayerController>();
            Player = pc;
            return pc as T;
        }
        else if(type == typeof(MonsterController))
        {
            string id = (_templateID == 0 ? "Goblin_01.prefab" : "Snake_01.prefab");

            GameObject go = Manager.ResourceM.Instantiate(id, null, true);
            MonsterController mc = Utils.GetOrAddComponent<MonsterController>(go);
            return mc as T;

        }
        else if(type == typeof(ProjectileController))
        {

        }
        return null;
    }

    public void DeSpawn(GameObject _go)
    {
        Manager.PoolM.Push(_go);      
    }
}
