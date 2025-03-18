using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectManager
{
    public PlayerController Player { get; private set; }
    public HashSet<MonsterController> mcSet = new HashSet<MonsterController>();
    public HashSet<ProjectileController> pjSet = new HashSet<ProjectileController>();
    public HashSet<GemController> gemSet = new HashSet<GemController>();


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



            //TODO : 프리팹 하나를 이미지파일을 돌려쓰는 코드( 지울예정) -> 이거 하려면 addressalbe받아온 오브젝트의 밑을 받아와야함
            //이유는 처음받아온 오브젝트는 Texture2D, 밑엔 Sprite 두개는 다른거라 첫번째 오브젝트를 사용하면 null값이 반환된다.
            //눌러보면 밑에 값이 뜸 (ex. Gem_01.sprite[Gem_01]) 이런식으로 바꿔서 받아줘야함
            string key = Random.Range(0, 2) == 0 ? "Gem_01.sprite" : "Gem_02.sprite";
            Sprite sprite = Manager.ResourceM.Load<Sprite>(key);
            go.GetComponent<SpriteRenderer>().sprite = sprite;

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
