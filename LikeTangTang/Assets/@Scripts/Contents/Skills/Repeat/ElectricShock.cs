using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ElectricShock : RepeatSkill, ITickable
{
    private readonly HashSet<MonsterController> sharedTarget = new();
    void Awake()
    {
        Skilltype = Define.SkillType.ElectricShock;
        coolTime = 0f;
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        Manager.UpdateM.Register(this);
        OnChangedSkillData();
    }

    public override void OnChangedSkillData()
    {
        duration = SkillDatas.Duration;
        coolTime = SkillDatas.CoolTime;
        projectileCount = SkillDatas.ProjectileCount;
        boundDist = SkillDatas.BoundDist;
    }

    public override void DoSkill()
    {
        var player = Manager.GameM.player;
        if (player == null) return;

        sharedTarget.Clear();
        string prefabName = SkillDatas.PrefabName;


        for (int i = 0; i < projectileCount; i++)
        {
            List<MonsterController> targets = GetElectricShockTargets(
                numBounce,
                boundDist - 1,
                boundDist + 1,
                i
            );
            if (targets == null || targets.Count == 0) continue;

            Vector3 startPos = player.transform.position;

            foreach (var target in targets)
            {
                if (target == null || !target.IsValid()) continue;

                Vector3 dir = (target.transform.position - startPos).normalized;
                GenerateProjectile(player, prefabName, startPos, dir, target.transform.position, this, sharedTarget);
                startPos = target.transform.position;

            }
        }

    }

    public void Tick(float _deltaTime)
    {
        coolTime -= _deltaTime;
        if (coolTime <= 0)
        {
            DoSkill();
            coolTime = SkillDatas.CoolTime;
        }
    }

    public List<MonsterController> GetElectricShockTargets(int _numTarget, float _minDist, float _maxDist, int _index = 0)
    {
        var Monsters = new List<MonsterController>();
        var nearMonster = Manager.ObjectM.GetNearMonsters(SkillDatas.ProjectileCount);

        if(nearMonster == null || nearMonster.Count == 0) return Monsters;
        
        int index = Mathf.Clamp(_index, 0, nearMonster.Count -1);
        var first = nearMonster[index];
        if(first == null || !first.IsValid())   return Monsters;
        Monsters.Add(first);

        for(int i =1; i<_numTarget; i++)
        {
            var next = GetElectricShockTarget(Monsters[i-1].transform.position, _minDist, _maxDist, Monsters);
            if(next == null) break;
            Monsters.Add(next);
        }

        return Monsters;

    }


    public MonsterController GetElectricShockTarget(Vector3 _origin, float _minDist, float _maxDist, List<MonsterController> _igonerMonsters)
    {

        var hits = Physics2D.OverlapCircleAll(_origin, _maxDist, LayerMask.GetMask("Monster", "Boss"));
        MonsterController target = null;
        float nearTargetDist = Mathf.Infinity;

        foreach(var hit in hits)
        {
            var mc = hit.GetComponent<MonsterController>();

            if(mc== null || _igonerMonsters.Contains(mc) || !mc.IsValid()) continue;

            float dist = Vector3.Distance(_origin, mc.transform.position);
            if(dist < _minDist || dist > _maxDist) continue;
            
            if(dist < nearTargetDist)
            {
                nearTargetDist = dist;
                target = mc;
            }
        }

        return target;
    }
}
