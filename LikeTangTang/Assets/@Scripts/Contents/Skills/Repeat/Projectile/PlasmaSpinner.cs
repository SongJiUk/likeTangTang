using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaSpinner : RepeatSkill
{
    Coroutine coStartPlasmaSpinner;

    void Awake()
    {
        Skilltype = Define.SkillType.PlasmaSpinner;
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        OnChangedSkillData();
    }

    public override void OnChangedSkillData()
    {
        SetPlasmaSpinner();
    }


    public void SetPlasmaSpinner()
    {
        //USE : Duration, ProjectileCount, NumBounce, BounceSpeed, Speed, CoolTime, Scale, Damage

        /*
        일단 스킬레벨을 찍으면 여기 들어오게된다. 
        스킬 사용을 시작해야됌. 
        */
        duration = SkillDatas.Duration;
        coolTime = SkillDatas.CoolTime;
        projectileCount = SkillDatas.ProjectileCount;


    }
    
    public override void DoSkill()
    {
        StartCoroutine(CoStartPlasmaSpinner());
    }

    IEnumerator CoStartPlasmaSpinner()
    {
       
            List<MonsterController> targets = Manager.ObjectM.GetNearMonsters(projectileCount);
            if(targets == null) yield break;


        for(int i =0; i< targets.Count; i++)
        {
            if(targets != null)
            {
                if(targets[i].IsValid() == false) continue;

                Vector3 dir = (targets[i].transform.position - Manager.GameM.player.transform.position).normalized;
                Vector3 startPos = Manager.GameM.player.transform.position;
                GenerateProjectile(Manager.GameM.player, SkillDatas.PrefabName, startPos, dir, targets[i].transform.position, this);
            }
        }
        yield return new WaitForSeconds(duration + coolTime);
        
        
    }

}
