using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SuicideDrone : RepeatSkill, ITickable
{
    void Awake()
    {
        Skilltype = Define.SkillType.SuicideDrone;
        coolTime = 0f;
    }
    public override void DoSkill()
    {
        SpawnDrones();
    }

    private void OnDestroy()
    {
        Manager.UpdateM.Unregister(this);
    }
    public override void ActivateSkill()
    {
        base.ActivateSkill();
        OnChangedSkillData();
        Manager.UpdateM.Register(this);
    }

    public override void OnChangedSkillData()
    {
        SetSuicideDrone();
    }

    public void SetSuicideDrone()
    {
        projectileCount = SkillDatas.ProjectileCount;
        prefabName = SkillDatas.PrefabName;
        range = SkillDatas.Range;
    }
    
    void SpawnDrones()
    {
        if(projectileCount <= 0 || range <= 0) return;

        for(int i =0; i<projectileCount; i++)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float t = Mathf.Sqrt(Random.Range(0f, 1f));
            float radius = Mathf.Lerp(2f, range, t);

            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;
            Vector3 spawnPos = Manager.GameM.player.transform.position + offset;
            GenerateProjectile(Manager.GameM.player, prefabName, spawnPos, _skill : this);
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
