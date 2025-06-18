using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBomb : RepeatSkill, ITickable
{
    private float baseCoolTime;
    private float timeAccumulator;
    void Awake()
    {
        Skilltype = Define.SkillType.GravityBomb;
    }

   
    public override void DoSkill()
    {
        Manager.SoundM.Play(Define.Sound.Effect, SkillDatas.CastingSoundLabel);
        SpawnGravityBomb();
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
        SetTimeStopBomb();
    }

    public void SetTimeStopBomb()
    {
        projectileCount = SkillDatas.ProjectileCount;
        prefabName = SkillDatas.PrefabName;;
        range = SkillDatas.Range;
    }
    void SpawnGravityBomb()
    {
        if(projectileCount <= 0 || range <=0) return;

        Vector3 pos = Manager.GameM.player.transform.position;
        for(int i =0; i<  projectileCount; i++)
        {
            float angle = Random.Range(0f, 360f);
            Vector3 dir = Quaternion.Euler(0f, 0f, angle) * Vector3.right;

            float randRange = Random.Range(2f, SkillDatas.Range);
            Vector3 endPos = pos + dir.normalized * randRange;
            GenerateProjectile(Manager.GameM.player, prefabName, pos, dir, endPos, _skill:this);
        }
    }
    public void Tick(float _deltaTime)
    {
        timeAccumulator += _deltaTime;
        baseCoolTime = SkillDatas.CoolTime * (1 - Manager.GameM.CurrentCharacter.Evol_CoolTimeBouns);
        if (timeAccumulator >= baseCoolTime)
        {
            DoSkill();
            timeAccumulator -= baseCoolTime;
        }
    }
}
