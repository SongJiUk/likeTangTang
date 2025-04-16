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
        for(int i =0; i<projectileCount; i++)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float radius = Random.Range(2f, range);

            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;
            Vector3 spawnPos = Manager.GameM.player.transform.position + offset;
            GenerateProjectile(Manager.GameM.player, prefabName, spawnPos, _skill : this);
        }
        yield break;
    }
}
