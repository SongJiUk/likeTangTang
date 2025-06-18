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


#region 미리 선언해서 사용  
    Type playerType = typeof(PlayerController);
    Type monsterType = typeof(MonsterController);
    Type eliteMonsterType = typeof(EliteMonsterController);
    Type bossMonsterType = typeof(BossController);
    Type gemType = typeof(GemController);
    Type dropItemType = typeof(DropItemController);
    Type gridType = typeof(GridController);
    Type projectileType = typeof(ProjectileController);
    Type skillType = typeof(SkillBase);
#endregion
    
    public T Spawn<T>(Vector3 _pos, int _templateID = 0, string _prefabName = "") where T : BaseController
    {
        Type type = typeof(T);

        if (type == playerType)
        {
            GameObject go = Manager.ResourceM.Instantiate($"{Manager.DataM.CreatureDic[_templateID].prefabName}");
            go.name = $"{Manager.GameM.gameData.userName}";
            go.transform.position = _pos;
            PlayerController pc = go.GetComponent<PlayerController>();
            Player = pc;
            pc.SetInfo(_templateID);
            
            return pc as T;
        }

        if(monsterType.IsAssignableFrom(type))
        {
            CreatureData cd = Manager.DataM.CreatureDic[_templateID];
            GameObject go = Manager.ResourceM.Instantiate(cd.prefabName, _pooling: true);
            T mc = go.GetOrAddComponent<T>();
            go.transform.position = _pos;
            if(mc is MonsterController monster)
            {
                monster.SetInfo(_templateID);
                mc.name = cd.prefabName;
                mcSet.Add(monster);
            }
            return mc as T;
        }

       
        if(dropItemType.IsAssignableFrom(type))
        {
            GameObject go = Manager.ResourceM.Instantiate(_prefabName, null, true);
            T dropItem = go.GetOrAddComponent<T>();
            dropItem.transform.position = _pos;
            var dropItemCotnroller = dropItem as DropItemController;

            dropItemSet.Add(dropItemCotnroller);
            Manager.GameM.CurrentMap.Grid.AddCell(dropItemCotnroller);

            return dropItem;
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


        if(skillType.IsAssignableFrom(type))
        {
            if (!Manager.DataM.SkillDic.TryGetValue(_templateID, out var skillData)) return null;

            GameObject go = Manager.ResourceM.Instantiate(skillData.PrefabName);
            go.transform.position = _pos;

            T skill = go.GetOrAddComponent<T>();
            skill.Init();

            return skill;
        }
       
            
        return null;
    }

    public void DeSpawn<T>(T _obj) where T : BaseController
    {

        if (_obj == null ||  !_obj.IsValid()) return;


        Type type = typeof(T);

        if(type == playerType)
        {
           //어차피 게임끝나서 안해줘도 될듯

        }
        else if(monsterType.IsAssignableFrom(type))
        {
            mcSet.Remove(_obj as MonsterController);
            Manager.ResourceM.Destory(_obj.gameObject);
        }
        else if(dropItemType.IsAssignableFrom(type))
        {
            var drop = _obj as DropItemController;

            dropItemSet.Remove(drop);
            Manager.ResourceM.Destory(_obj.gameObject);
        }
        else if(projectileType.IsAssignableFrom(type))
        {
            pjSet.Remove(_obj as ProjectileController);
            Manager.ResourceM.Destory(_obj.gameObject);
        }
        else if(skillType.IsAssignableFrom(type))
        {
            Manager.ResourceM.Destory(_obj.gameObject);
        }
    }


    public void Clear()
    {
        foreach (var mc in mcSet) mc.Clear();
        mcSet.Clear();

        foreach (var dc in dropItemSet) dc.Clear();
        dropItemSet.Clear();

        pjSet.Clear();
    }

    public void LoadMap(string _name)
    {
        var obj = Manager.ResourceM.Instantiate(_name);
        obj.transform.position = Vector3.zero;
        obj.name = "@Map";
        obj.GetComponent<Map>().init();

        //NOTE : 이거 해주는 이유는 타일맵은 중심잡기가 생각보다힘듬, 그래서 찾아서 값을 더해주는것
        //Tilemap baseTileMap = obj.GetComponentInChildren<Tilemap>();
        //Vector3Int centercell = new Vector3Int((int)baseTileMap.cellBounds.center.x, (int)baseTileMap.cellBounds.center.y ,(int)baseTileMap.cellBounds.center.z);
        //Vector3 centerWorldPos = baseTileMap.CellToWorld(centercell);
        //centerWorldPos.x *= -1;
        //obj.transform.position += centerWorldPos;

    }

    public void ShowFont(Vector2 _pos, float _damage, float _heal, Transform _parent, bool _isCritical = false)
    {
        string prefabName;
        prefabName = _isCritical == true ?  Define.CRITICAL_DAMANGEFONT : Define.DAMAGEFONT;

        GameObject go = Manager.ResourceM.Instantiate(prefabName, _pooling: true);
        DamageFont damageFont = go.GetOrAddComponent<DamageFont>();
        damageFont.SetInfo(_pos, _damage, _heal, _parent, _isCritical);
    }

    public List<MonsterController> GetNearMonsters(int _count = 1, int _distanceThreshold = 0)
    {
        List<MonsterController> result = new List<MonsterController>();

        float thresholdSqr = _distanceThreshold > 0 ? _distanceThreshold * _distanceThreshold : 0;

        float[] dist = new float[_count];

        for (int i = 0; i < dist.Length; i++) dist[i] = float.MaxValue;

        MonsterController[] nearest = new MonsterController[_count];

        foreach (var monster in mcSet)
        {
            if (monster == null || !monster.IsValid()) continue;
            float distSqr = (Manager.GameM.player.transform.position - monster.transform.position).sqrMagnitude;

            if (distSqr > 0 && distSqr < thresholdSqr) continue;

            for (int i = 0; i < _count; i++)
            {
                if (distSqr < dist[i])
                {
                    for (int j = _count - 1; j > i; j--)
                    {
                        dist[j] = dist[j - 1];
                        nearest[j] = nearest[j - 1];
                    }

                    dist[i] = distSqr;
                    nearest[i] = monster;
                    break;
                }
            }
        }
        for (int i = 0; i < _count; i++)
        {
            if (nearest[i] != null)
                result.Add(nearest[i]);
        }

        // 없으면 null 리턴
        if (result.Count == 0) return null;

        // 부족하면 마지막 값 복제
        while (result.Count < _count)
            result.Add(result[result.Count - 1]);


        return result;
    }

    public void KillAllMonsters()
    {
        UI_GameScene scene = Manager.UiM.SceneUI as UI_GameScene;

        if(scene != null) scene.WhiteFlash();

        var snapshot = new List<MonsterController>(mcSet);
        foreach (MonsterController monster in snapshot)
        {
            if (monster.objType == Define.ObjectType.Monster)
                monster.OnDead();
        }
    }

    public void ColletAllItem()
    {
        var snapshot = new List<DropItemController>(dropItemSet);
        foreach(var item in snapshot)
        {
            if(item is GemController gem)
            {
                gem.GetItem();
            }
        }
    }

}
