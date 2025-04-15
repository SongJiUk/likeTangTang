using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaShot : RepeatSkill
{

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
        StartCoroutine(CoStartPlasmaShot());
    }

    public void SetPlasmaShot()
    {
        projectileCount = SkillDatas.ProjectileCount;
    }

    IEnumerator CoStartPlasmaShot()
    {
       
            Vector3 pos = Manager.GameM.player.transform.position;
            for(int i =0; i< projectileCount; i++)
            {
                float randRange = Random.Range(0f, 360f);

                Vector3 dir = Quaternion.Euler(0f,0f, randRange) * Vector3.right;
                GenerateProjectile(Manager.GameM.player, SkillDatas.PrefabName, pos, dir, _skill:this);
            }

            yield break;
        }
    
}
