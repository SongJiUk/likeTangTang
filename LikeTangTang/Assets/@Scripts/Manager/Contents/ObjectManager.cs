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
    Type eliteMonsterType = typeof(EliteMonsterController);
    Type bossMonsterType = typeof(BossController);
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
            //[x]Data에서 값을 가져와서 생성하기.
            GameObject go = Manager.ResourceM.Instantiate($"{Manager.DataM.CreatureDic[_templateID].prefabName}");
            go.name = $"{Manager.GameM.gameData.userName}";
            go.transform.position = _pos;
            PlayerController pc = go.GetComponent<PlayerController>();
            pc.Init();
            pc.SetInfo(_templateID);    
            
            Player = pc;
            return pc as T;
        }
        else if (type == monsterType)
        {

            //[x] DATA로 받아와서 처리하지.(스테이지 데이터를 생성하서, 그 스테이지에 맞는 몬스터들 가져오면 됌.)
            CreatureData cd = Manager.DataM.CreatureDic[_templateID];

            GameObject go = Manager.ResourceM.Instantiate(cd.prefabName, _pooling: true);
            MonsterController mc = Utils.GetOrAddComponent<MonsterController>(go);
            go.transform.position = _pos;
            mc.Init();
            mc.SetInfo(_templateID);
            mc.name = cd.prefabName;
            mcSet.Add(mc);
            
            return mc as T;

        }
        else if(type == eliteMonsterType)
        {
            CreatureData cd = Manager.DataM.CreatureDic[_templateID];
           
            GameObject go = Manager.ResourceM.Instantiate(cd.prefabName, _pooling: true);
            EliteMonsterController emc = go.GetOrAddComponent<EliteMonsterController>();
            emc.transform.position = _pos;
            emc.Init();
            emc.SetInfo(_templateID);
            emc.name = cd.prefabName;
            mcSet.Add(emc);

            return emc as T;
        }
        else if(type == bossMonsterType)
        {
            CreatureData cd = Manager.DataM.CreatureDic[_templateID];

            GameObject go = Manager.ResourceM.Instantiate(cd.prefabName);
            BossController bc = go.GetOrAddComponent<BossController>();
            go.transform.position = _pos;
            bc.Init();
            bc.SetInfo(_templateID);
            go.name = cd.prefabName;
            mcSet.Add(bc);

            return bc as T;

        }
        else if (type == gemType)
        {
            GameObject go = Manager.ResourceM.Instantiate(Define.GEMNAME, null, true);
            GemController gc = Utils.GetOrAddComponent<GemController>(go);
            go.transform.position = _pos;
            gemSet.Add(gc);

            // [ ] sprite이름도 DATA에서 불러와서 효과적으로 진행
            //string key = UnityEngine.Random.Range(0, 2) == 0 ? "Gem_01.sprite" : "Gem_02.sprite";
            //Sprite sprite = Manager.ResourceM.Load<Sprite>(key);
            //go.GetComponent<SpriteRenderer>().sprite = sprite;

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

            GameObject go = Manager.ResourceM.Instantiate(skilldata.PrefabName, _pooling: true);
            go.transform.position = _pos;

            ProjectileController pc = go.GetOrAddComponent<ProjectileController>();
            pjSet.Add(pc);
            pc.Init();

            return pc as T;
        }
        else if(typeof(T).IsSubclassOf(typeof(SkillBase)))
        {
            if (Manager.DataM.SkillDic.TryGetValue(_templateID, out var skillData) == false) return null;
            GameObject go = Manager.ResourceM.Instantiate(skillData.PrefabName, _pooling: false);
            go.transform.position = _pos;

            T t = go.GetOrAddComponent<T>();
            t.Init();

            return t;
        }
        else if (type == egoSwordType)
        {
            if (Manager.DataM.SkillDic.TryGetValue(_templateID, out var skillData) == false) return null;

            GameObject go = Manager.ResourceM.Instantiate(skillData.PrefabName, _pooling: false);
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

    //public void DeSpawnAllMonster()
    //{
    //    var monsters = mcSet.ToList();

    //    foreach(var monster in monsters)
    //    {
    //        DeSpawn<MonsterController>(monster);
    //    }
    //}

    public void LoadMap(string _name)
    {
        Manager.ResourceM.Instantiate(_name);
    }

    public void ShowFont(Vector2 _pos, float _damage, float _heal, Transform _parent, bool _isCritical = false)
    {
        string prefabName;
        if (_isCritical)
            prefabName = "ciri"; // TODO : 프리팹 생성 후 이름 변경.
        else
            prefabName = "nociri";

        GameObject go = Manager.ResourceM.Instantiate(prefabName, _pooling: true);
        DamageFont damageFont = go.GetOrAddComponent<DamageFont>();
        damageFont.SetInfo(_pos, _damage, _heal, _parent, _isCritical);
    }

}
