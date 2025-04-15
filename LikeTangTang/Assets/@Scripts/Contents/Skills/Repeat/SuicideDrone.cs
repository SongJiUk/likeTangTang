using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SuicideDrone : RepeatSkill
{
    void Awake()
    {
        Skilltype = Define.SkillType.SuicideDrone;
    }
    public override void DoSkill()
    {
        StartCoroutine(CoStartSuicideDrone());
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        OnChangedSkillData();
    }

    public override void OnChangedSkillData()
    {
        SetSuicideDrone();
    }

    public void SetSuicideDrone()
    {
        projectileCount = SkillDatas.ProjectileCount;
        prefabName = SkillDatas.PrefabName;
    }

    IEnumerator CoStartSuicideDrone()
    {
        Vector3 pos = Manager.GameM.player.transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0f);

        for(int i =0; i<projectileCount; i++)
        {
            GenerateProjectile(Manager.GameM.player, prefabName, pos, _skill : this);
        }
        yield break;
    }
}
