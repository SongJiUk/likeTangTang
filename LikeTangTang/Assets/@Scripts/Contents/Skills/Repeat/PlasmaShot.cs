using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaShot : RepeatSkill, ITickable
{
    private float baseCoolTime;
    private float timeAccumulator;
    void Awake()
    {
        Skilltype = Define.SkillType.PlasmaShot;
    }
    private void OnDestroy()
    {
        Manager.UpdateM.Unregister(this);
    }
    public override void ActivateSkill()
    {
        base.ActivateSkill();
        Manager.UpdateM.Register(this);
        OnChangedSkillData();
    }

    public override void OnChangedSkillData()
    {
        projectileCount = SkillDatas.ProjectileCount;
    }

    public override void DoSkill()
    {
        Manager.SoundM.Play(Define.Sound.Effect, SkillDatas.CastingSoundLabel);
        Vector3 pos = Manager.GameM.player.transform.position;
        
        for(int i =0; i< projectileCount; i++)
        {
            float randRange = Random.Range(0f, 360f);

            Vector3 dir = Quaternion.Euler(0f,0f, randRange) * Vector3.right;
            GenerateProjectile(Manager.GameM.player, SkillDatas.PrefabName, pos, dir, _skill:this);
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
