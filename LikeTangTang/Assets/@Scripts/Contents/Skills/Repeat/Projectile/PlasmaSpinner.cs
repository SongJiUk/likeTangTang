using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaSpinner : RepeatSkill, ITickable
{
    Coroutine coStartPlasmaSpinner;

    void Awake()
    {
        Skilltype = Define.SkillType.PlasmaSpinner;
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
    }
    
    public override void DoSkill()
    {
        List<MonsterController> targets = Manager.ObjectM.GetNearMonsters(projectileCount);
        if(targets == null) return;

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets != null)
            {
                if (targets[i].IsValid() == false) continue;

                Vector3 dir = (targets[i].transform.position - Manager.GameM.player.transform.position).normalized;
                Vector3 startPos = Manager.GameM.player.transform.position;
                GenerateProjectile(Manager.GameM.player, SkillDatas.PrefabName, startPos, dir, targets[i].transform.position, this);
            }
        }
    }

    public void Tick(float _deltaTime)
    {
        coolTime -= _deltaTime;
        if(coolTime <= 0)
        {
            DoSkill();
            coolTime = SkillDatas.CoolTime;
        }
    }

}
