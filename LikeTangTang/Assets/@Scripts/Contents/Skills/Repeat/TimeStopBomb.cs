using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopBomb : RepeatSkill, ITickable
{
    void Awake()
    {
        Skilltype = Define.SkillType.TimeStopBomb;
        coolTime = 0f;
    }
    public override void DoSkill()
    {
        SpawnTimeStopBomb();
    }

    


    public override void ActivateSkill()
    {
        base.ActivateSkill();
        OnChangedSkillData();
        Manager.UpdateM.Register(this);
    }

    public override void OnChangedSkillData()
    {
        SetTimeStopBomb();
    }

    public void SetTimeStopBomb()
    {
        projectileCount = SkillDatas.ProjectileCount;
        prefabName = SkillDatas.PrefabName;
        range = SkillDatas.Range;

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

    void SpawnTimeStopBomb()
    {

        if(projectileCount <= 0 || range <= 0) return;
        
        Vector3 pos = Manager.GameM.player.transform.position;
        for(int i =0; i<projectileCount; i++)
        {   
            float angle = Random.Range(0f, 360f);
            Vector3 dir = Quaternion.Euler(0f, 0f, angle) * Vector3.right;

            float randRange = Random.Range(2f, SkillDatas.Range);
            Vector3 endPos = pos + dir.normalized * randRange;
            GenerateProjectile(Manager.GameM.player, prefabName, pos, dir, endPos, _skill:this);
        }
    }

}
