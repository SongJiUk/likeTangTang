using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricShock : RepeatSkill
{
    Coroutine coStartElectricShock;
    void Awake()
    {
        Skilltype = Define.SkillType.ElectricShock;

    }
    

    public override void ActivateSkill()
    {
        base.ActivateSkill();
    }

    public override void OnChangedSkillData()
    {
        
    }

    public override void DoSkill()
    {
        if(coStartElectricShock == null) coStartElectricShock = StartCoroutine(CoStartEletricShock());
    }

    public void SetElectricShock()
    {
        duration =SkillDatas.Duration;
        coolTime = SkillDatas.CoolTime;
    }

    IEnumerator CoStartEletricShock()
    {
        while(true)
        {
            yield return new WaitForSeconds(duration + coolTime);
        }
    }
}
