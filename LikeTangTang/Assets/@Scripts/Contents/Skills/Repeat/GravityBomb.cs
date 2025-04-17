using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBomb : RepeatSkill
{
    void Awake()
    {
        Skilltype = Define.SkillType.GravityBomb;
    }
    public override void DoSkill()
    {
        StartCoroutine(CoStartGravityBomb());
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
        prefabName = SkillDatas.PrefabName;;
    }

    IEnumerator CoStartGravityBomb()
    {
        Vector3 pos = Manager.GameM.player.transform.position;
        for(int i =0; i<  projectileCount; i++)
        {
            float angle = Random.Range(0f, 360f);
            Vector3 dir = Quaternion.Euler(0f, 0f, angle) * Vector3.right;

            float randRange = Random.Range(2f, SkillDatas.Range);
            Vector3 endPos = pos + dir.normalized * randRange;
            GenerateProjectile(Manager.GameM.player, prefabName, pos, dir, endPos, _skill:this);
        }

        yield break;
    }
}
