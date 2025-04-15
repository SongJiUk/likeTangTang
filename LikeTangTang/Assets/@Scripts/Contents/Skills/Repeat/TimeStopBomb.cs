using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopBomb : RepeatSkill
{
    void Awake()
    {
        Skilltype = Define.SkillType.TimeStopBomb;
    }
    public override void DoSkill()
    {
        StartCoroutine(CoStartTimeStopBomb());
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        OnChangedSkillData();
    }

    public override void OnChangedSkillData()
    {
        SetTimeStopBomb();
    }

    public void SetTimeStopBomb()
    {
        projectileCount = SkillDatas.ProjectileCount;
        prefabName = SkillDatas.PrefabName;
    }
    IEnumerator CoStartTimeStopBomb()
    {
        Vector3 pos = Manager.GameM.player.transform.position;
        for(int i =0; i<projectileCount; i++)
        {   float randRange = Random.Range(0f, 360f);
            Vector3 dir = Quaternion.Euler(0f,0f, randRange) * Vector3.right;

            GenerateProjectile(Manager.GameM.player, prefabName, pos, dir, _skill:this);
        }
        yield break;
    }
}
