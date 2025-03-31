using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

#region 미리 선언해서 사용  
    Type playerType = typeof(PlayerController);
    Type monsterType = typeof(MonsterController);
    Type gemType = typeof(GemController);
    Type gridType = typeof(GridController);
    Type projectileType = typeof(ProjectileController);
    Type egoSwordType = typeof(EgoSword);
    Type skillType = typeof(SkillBase);
#endregion
    
    public T Spawn<T>(Vector3 _pos, int _templateID = 0) where T : BaseController
    {
        System.Type type = typeof(T);

        if (type == playerType)
        {
            //[ ]Data에서 값을 가져와서 생성하기.
            GameObject go = Manager.ResourceM.Instantiate("Slime_01.prefab");
            go.name = "!Player";
            go.transform.position = _pos;
            PlayerController pc = go.GetComponent<PlayerController>();
            Player = pc;

            pc.Init();

            return pc as T;
        }
        else if (type == monsterType)
        {
            
            //[ ] DATA로 받아와서 처리하지.
            string id = (_templateID == 0 ? "Goblin_01.prefab" : "Snake_01.prefab");

            if(_templateID == 3) id = "Boss.prefab";

            GameObject go = Manager.ResourceM.Instantiate(id, null, true);
            go.transform.position = _pos;

            MonsterController mc = Utils.GetOrAddComponent<MonsterController>(go);
            mcSet.Add(mc);
            mc.Init();
            return mc as T;

        }
        else if (type == gemType)
        {
            GameObject go = Manager.ResourceM.Instantiate(Define.GEMNAME, null, true);
            go.transform.position = _pos;

            GemController gc = Utils.GetOrAddComponent<GemController>(go);
            gemSet.Add(gc);
            gc.Init();

            // [ ] sprite이름도 DATA에서 불러와서 효과적으로 진행
            string key = UnityEngine.Random.Range(0, 2) == 0 ? "Gem_01.sprite" : "Gem_02.sprite";
            Sprite sprite = Manager.ResourceM.Load<Sprite>(key);
            go.GetComponent<SpriteRenderer>().sprite = sprite;

            //Explanation : AddGrid
            Grid.AddCell(go);

            return gc as T;


        }
        else if (type == gridType)
        {
            Grid = GameObject.Find(Define.GRIDNAME).GetComponent<GridController>();
            Grid.Init();
        }
        else if (type == projectileType)
        {
            // TemplateID 받아와서 생성
            if (Manager.DataM.SkillDic.TryGetValue(_templateID, out var skilldata) == false) return null;

            GameObject go = Manager.ResourceM.Instantiate(skilldata.prefab, _pooling: true);
            go.transform.position = _pos;

            ProjectileController pc = go.GetOrAddComponent<ProjectileController>();
            pjSet.Add(pc);
            pc.Init();

            return pc as T;
        }
        else if(typeof(T).IsSubclassOf(typeof(SkillBase)))
        {
            if (Manager.DataM.SkillDic.TryGetValue(_templateID, out var skillData) == false) return null;
            GameObject go = Manager.ResourceM.Instantiate(skillData.prefab, _pooling: false);
            go.transform.position = _pos;

            T t = go.GetOrAddComponent<T>();
            t.Init();

            return t;
        }
        else if (type == egoSwordType)
        {
            if (Manager.DataM.SkillDic.TryGetValue(_templateID, out var skillData) == false) return null;

            GameObject go = Manager.ResourceM.Instantiate(skillData.prefab, _pooling: false);
            go.transform.position = _pos;

            T t = go.GetOrAddComponent<T>();
            t.Init();

            return t;
        }
            
        return null;
    }

    public void DeSpawn<T>(T _obj) where T : BaseController
    {

        if (!_obj.IsVaild()) Debug.LogError("DeSpawn Error!!!, ObjectManager 134Line!");


        System.Type type = typeof(T);

        if(type == playerType)
        {
            /*TODO : 플레이어 죽으면 해야될것
             *  플레이어가 죽는다면 게임을 정지하고 UI화면 띄워야함
             *  
             */

        }
        else if(monsterType.IsAssignableFrom(type))
        {
            if(_obj.IsVaild() == false) return;

            mcSet.Remove(_obj as MonsterController);
            Manager.ResourceM.Destory(_obj.gameObject);
        }
        else if(type == monsterType)
        {
            if(_obj.IsVaild() == false) return;

            mcSet.Remove(_obj as MonsterController);
            Manager.ResourceM.Destory(_obj.gameObject);
        }
        else if(type == gemType)
        {
            if(_obj.IsVaild() == false) return;

            gemSet.Remove(_obj as GemController);
            Manager.ResourceM.Destory(_obj.gameObject);

            Grid.RemoveCell(_obj.gameObject);
        }
        //NOTE : IsAssignableFrom을 이용하면 해당 부모의 자식을 모두 찾을 수 있음.
        else if(skillType.IsAssignableFrom(type))
        {
            pjSet.Remove(_obj as ProjectileController);
            Manager.ResourceM.Destory(_obj.gameObject);
        }
    }

    public void DeSpawnAllMonster()
    {
        var monsters = mcSet.ToList();

        foreach(var monster in monsters)
        {
            DeSpawn<MonsterController>(monster);
        }
    }
}
