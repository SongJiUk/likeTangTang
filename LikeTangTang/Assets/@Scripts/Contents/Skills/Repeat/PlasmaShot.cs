using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaShot : RepeatSkill
{

    Coroutine coStartPlasmaShot;
    void Awake()
    {
        Skilltype = Define.SkillType.PlasmaShot;
    }
    
    public override void ActivateSkill()
    {
        base.ActivateSkill();
        OnChangedSkillData();
        DoSkill();
    }

    public override void OnChangedSkillData()
    {
        SetPlasmaShot();
    }


    public override void DoSkill()
    {
        if(coStartPlasmaShot == null) coStartPlasmaShot = StartCoroutine(CoStartPlasmaShot());
    }

    public void SetPlasmaShot()
    {
        duration = SkillDatas.Duration;
        coolTime = SkillDatas.CoolTime;
        projectileCount = SkillDatas.ProjectileCount;
    }

    IEnumerator CoStartPlasmaShot()
    {
        while(true)
        {   
            Vector3 pos = Manager.GameM.player.transform.position;
            for(int i =0; i< projectileCount; i++)
            {
                float randRange = Random.Range(0f, 360f);

                Vector3 dir = Quaternion.Euler(0f,0f, randRange) * Vector3.right;
                GenerateProjectile(Manager.GameM.player, SkillDatas.PrefabName, pos, dir, _skill:this);
            }
            yield return new WaitForSeconds(duration + coolTime);
        }
    }
}
