using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ObjectManager
{
    public PlayerController Player { get; private set; }
    public HashSet<MonsterController> mcSet { get; } = new HashSet<MonsterController>();
    public HashSet<ProjectileController> pjSet { get; } = new HashSet<ProjectileController>();
    public HashSet<GemController> gemSet { get; } = new HashSet<GemController>();
    public HashSet<DropItemController> dropItemSet {get;} = new HashSet<DropItemController>();


    public GridController Grid {get; private set;}

#region 미리 선언해서 사용  
    Type playerType = typeof(PlayerController);
    Type monsterType = typeof(MonsterController);
    Type eliteMonsterType = typeof(EliteMonsterController);
    Type bossMonsterType = typeof(BossController);
    Type gemType = typeof(GemController);
    Type dropItemType = typeof(DropItemController);
    Type gridType = typeof(GridController);
    Type projectileType = typeof(ProjectileController);
    Type egoSwordType = typeof(EgoSword);
    Type skillType = typeof(SkillBase);
#endregion
    
    public T Spawn<T>(Vector3 _pos, int _templateID = 0, string _prefabName = "") where T : BaseController
    {
        System.Type type = typeof(T);

        if (type == playerType)
        {
            //[x]Data에서 값을 가져와서 생성하기.
            GameObject go = Manager.ResourceM.Instantiate($"{Manager.DataM.CreatureDic[_templateID].prefabName}");
            go.name = $"{Manager.GameM.gameData.userName}";
            go.transform.position = _pos;
            PlayerController pc = go.GetComponent<PlayerController>();
          
            pc.SetInfo(_templateID);

            Player = pc;
            return pc as T;
        }
        if(monsterType.IsAssignableFrom(type))
        {
            CreatureData cd = Manager.DataM.CreatureDic[_templateID];
            GameObject go = Manager.ResourceM.Instantiate(cd.prefabName, _pooling: true);
            T mc = go.GetOrAddComponent<T>();
            go.transform.position = _pos;
            mc.Init();
            if(mc is MonsterController monster)
            {
                monster.SetInfo(_templateID);
                mc.name = cd.prefabName;
                mcSet.Add(monster);
            }
               

            return mc as T;
        }

        //if (type == monsterType)
        //{

        //    //[x] DATA로 받아와서 처리하지.(스테이지 데이터를 생성하서, 그 스테이지에 맞는 몬스터들 가져오면 됌.)
        //    CreatureData cd = Manager.DataM.CreatureDic[_templateID];

        //    GameObject go = Manager.ResourceM.Instantiate(cd.prefabName, _pooling: true);
        //    MonsterController mc = Utils.GetOrAddComponent<MonsterController>(go);
        //    go.transform.position = _pos;
        //    mc.Init();
        //    mc.SetInfo(_templateID);
        //    mc.name = cd.prefabName;
        //    mcSet.Add(mc);
            
        //    return mc as T;

        //}
        //else if(type == eliteMonsterType)
        //{
        //    CreatureData cd = Manager.DataM.CreatureDic[_templateID];
           
        //    GameObject go = Manager.ResourceM.Instantiate(cd.prefabName, _pooling: true);
        //    EliteMonsterController emc = go.GetOrAddComponent<EliteMonsterController>();
        //    emc.transform.position = _pos;
        //    emc.Init();
        //    emc.SetInfo(_templateID);
        //    emc.name = cd.prefabName;
        //    mcSet.Add(emc);

        //    return emc as T;
        //}
        //else if(type == bossMonsterType)
        //{
        //    CreatureData cd = Manager.DataM.CreatureDic[_templateID];

        //    GameObject go = Manager.ResourceM.Instantiate(cd.prefabName);
        //    BossController bc = go.GetOrAddComponent<BossController>();
        //    go.transform.position = _pos;
        //    bc.Init();
        //    bc.SetInfo(_templateID);
        //    go.name = cd.prefabName;
        //    mcSet.Add(bc);

        //    return bc as T;
        //}

        if(dropItemType.IsAssignableFrom(type))
        {
            GameObject go = Manager.ResourceM.Instantiate(_prefabName, null, true);
            T dropItem = go.GetOrAddComponent<T>();
            dropItem.transform.position = _pos;

            dropItemSet.Add(dropItem as DropItemController);
            Grid.AddCell(go);

            return dropItem;
        }

        if (typeof(T).IsSubclassOf(typeof(DropItemController)))
        {
            GameObject go = Manager.ResourceM.Instantiate(_prefabName, null, true);
            T obj = Utils.GetOrAddComponent<T>(go);
            go.transform.position = _pos;
            dropItemSet.Add(obj as DropItemController);

            Grid.AddCell(go);

            return obj;
        }
        //else if (type == gemType)
        //{
        //    GameObject go = Manager.ResourceM.Instantiate(Define.GEMNAME, null, true);
        //    GemController gc = Utils.GetOrAddComponent<GemController>(go);
        //    go.transform.position = _pos;
        //    gemSet.Add(gc);

        //    // [ ] sprite이름도 DATA에서 불러와서 효과적으로 진행
        //    //string key = UnityEngine.Random.Range(0, 2) == 0 ? "Gem_01.sprite" : "Gem_02.sprite";
        //    //Sprite sprite = Manager.ResourceM.Load<Sprite>(key);
        //    //go.GetComponent<SpriteRenderer>().sprite = sprite;

        //    //Explanation : AddGrid
        //    Grid.AddCell(go);

        //    return gc as T;


        //}
        //else if( type == dropItemType)
        //{
        //    GameObject go = Manager.ResourceM.Instantiate(_prefabName, null, true);
        //    T dc = Utils.GetOrAddComponent<T>(go);
        //    go.transform.position = _pos;
        //    dropItemSet.Add(dc as DropItemController);

        //    Grid.AddCell(go);

        //    return dc as T;

        //}
        else if (type == gridType)
        {
            Grid = GameObject.Find(Define.GRIDNAME).GetComponent<GridController>();
            Grid.Init();
            return Grid as T;
        }
        if(projectileType.IsAssignableFrom(type))
        {
            var go = Manager.ResourceM.Instantiate(_prefabName, _pooling: true);
            var proj = go.GetOrAddComponent<ProjectileController>();
            proj.transform.position = _pos;
            proj.Init();

            pjSet.Add(proj);

            return proj as T;
        }

        if (type == projectileType)
        {
            // TemplateID 받아와서 생성
            //if (Manager.DataM.SkillDic.TryGetValue(_templateID, out var skilldata) == false) return null;

            GameObject go = Manager.ResourceM.Instantiate(_prefabName, _pooling: true);
            ProjectileController pc = go.GetOrAddComponent<ProjectileController>();
            go.transform.position = _pos;
            pjSet.Add(pc);
            pc.Init();

            return pc as T;
        }

        if(skillType.IsAssignableFrom(type))
        {
            if (!Manager.DataM.SkillDic.TryGetValue(_templateID, out var skillData)) return null;

            GameObject go = Manager.ResourceM.Instantiate(skillData.PrefabName);
            go.transform.position = _pos;

            T skill = go.GetOrAddComponent<T>();
            skill.Init();

            return skill;
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

        if (!_obj.IsValid()) return;


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
            if(_obj.IsValid() == false) return;

            mcSet.Remove(_obj as MonsterController);
            Manager.ResourceM.Destory(_obj.gameObject);
        }
        else if(type == monsterType)
        {
            if(_obj.IsValid() == false) return;

            mcSet.Remove(_obj as MonsterController);
            Manager.ResourceM.Destory(_obj.gameObject);
        }
        else if(type == gemType)
        {
            if(_obj.IsValid() == false) return;

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
        var obj = Manager.ResourceM.Instantiate(_name);

        //NOTE : 이거 해주는 이유는 타일맵은 중심잡기가 생각보다힘듬, 그래서 찾아서 값을 더해주는것
        Tilemap baseTileMap = obj.GetComponentInChildren<Tilemap>();
        Vector3Int centercell = new Vector3Int((int)baseTileMap.cellBounds.center.x, (int)baseTileMap.cellBounds.center.y ,(int)baseTileMap.cellBounds.center.z);
        Vector3 centerWorldPos = baseTileMap.CellToWorld(centercell);
        centerWorldPos.x *= -1;
        obj.transform.position += centerWorldPos;
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

    public List<MonsterController> GetNearMonsters(int _count = 1, int distanceThreshold = 0)
    {
        List<MonsterController> monsterList = mcSet.OrderBy(monster => (Player.transform.position - monster.transform.position).sqrMagnitude).ToList();
        

        if(distanceThreshold > 0)
            monsterList = monsterList.Where(monster => (Player.transform.position - monster.transform.position).sqrMagnitude > distanceThreshold * distanceThreshold).ToList();

        int min = Mathf.Min(_count, monsterList.Count);
        List<MonsterController> nearsMonsters = monsterList.Take(min).ToList();

        if(nearsMonsters.Count == 0) return null;

        while(nearsMonsters.Count < _count)
            nearsMonsters.Add(nearsMonsters.Last());
         
        return nearsMonsters;
    }

}
