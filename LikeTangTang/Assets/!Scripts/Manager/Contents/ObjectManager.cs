using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Unity.VisualScripting;
using UnityEngine;


public class ObjectManager
{
    public PlayerController Player { get; private set; }
    public HashSet<MonsterController> mcSet { get; } = new HashSet<MonsterController>();
    public HashSet<ProjectileController> pjSet { get; } = new HashSet<ProjectileController>();
    public HashSet<GemController> gemSet { get; } = new HashSet<GemController>();


    public GridController Grid {get; private set;}

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
        else if (type == typeof(MonsterController))
        {

            string id = (_templateID == 0 ? "Goblin_01.prefab" : "Snake_01.prefab");

            GameObject go = Manager.ResourceM.Instantiate(id, null, true);
            go.transform.position = _pos;

            MonsterController mc = Utils.GetOrAddComponent<MonsterController>(go);
            mcSet.Add(mc);
            mc.Init();
            return mc as T;

        }
        else if (type == typeof(GemController))
        {
            GameObject go = Manager.ResourceM.Instantiate(Define.GEMNAME, null, true);
            go.transform.position = _pos;

            GemController gc = Utils.GetOrAddComponent<GemController>(go);
            gemSet.Add(gc);
            gc.Init();



            //TODO : 프리팹 하나를 이미지파일을 돌려쓰는 코드( 지울예정) -> 이거 하려면 addressalbe받아온 오브젝트의 밑을 받아와야함
            //이유는 처음받아온 오브젝트는 Texture2D, 밑엔 Sprite 두개는 다른거라 첫번째 오브젝트를 사용하면 null값이 반환된다.
            //눌러보면 밑에 값이 뜸 (ex. Gem_01.sprite[Gem_01]) 이런식으로 바꿔서 받아줘야함
            string key = UnityEngine.Random.Range(0, 2) == 0 ? "Gem_01.sprite" : "Gem_02.sprite";
            Sprite sprite = Manager.ResourceM.Load<Sprite>(key);
            go.GetComponent<SpriteRenderer>().sprite = sprite;

            //Explanation : AddGrid
            Grid.AddCell(go);

            return gc as T;


        }
        else if (type == typeof(GridController))
        {
            Grid = GameObject.Find(Define.GRIDNAME).GetComponent<GridController>();
            Grid.Init();
        }
        //CHECKLIST : 스킬 오브젝트가 LifeTime이 지나서 자동으로 소멸될때 타입이 맞지않아서 소멸이 안되는 문제가 있음
        else if (type == typeof(ProjectileController)) //Explanation : 상속받는 코드들도 찾아줌
        {
            // TemplateID 받아와서 생성
            if (Manager.DataM.SkillDic.TryGetValue(_templateID, out var skilldata) == false) return null;

            GameObject go = Manager.ResourceM.Instantiate(skilldata.prefab, _pooing: true);
            go.transform.position = _pos;

            ProjectileController pc = go.GetOrAddComponent<ProjectileController>();
            pjSet.Add(pc);
            pc.Init();

            return pc as T;
        }
        else if (type == typeof(EgoSwordController))
        {
            if (Manager.DataM.SkillDic.TryGetValue(_templateID, out var skillData) == false) return null;

            GameObject go = Manager.ResourceM.Instantiate(skillData.prefab, _pooing: false);
            go.transform.position = _pos;

            T t = go.GetOrAddComponent<T>();
            t.Init();

            return t;
        }
            
        return null;
    }

    public void DeSpawn<T>(T _obj) where T : BaseController
    {

        if (!_obj.IsVaild()) Debug.LogError("DeSpawn Error!!!");


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
            if(_obj.IsVaild() == false) return;

            mcSet.Remove(_obj as MonsterController);
            Manager.ResourceM.Destory(_obj.gameObject);
        }
        else if(type == typeof(GemController))
        {
            if(_obj.IsVaild() == false) return;

            gemSet.Remove(_obj as GemController);
            Manager.ResourceM.Destory(_obj.gameObject);

            Grid.RemoveCell(_obj.gameObject);
        }
        else if(type == typeof(ProjectileController))
        {
            pjSet.Remove(_obj as ProjectileController);
            Manager.ResourceM.Destory(_obj.gameObject);
        }
        else if(type == typeof(EgoSwordController))
        {

        }

    }
}
