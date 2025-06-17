using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalBlades : RepeatSkill, ITickable
{

    void Awake()
    {
        Skilltype = Define.SkillType.OrbitalBlades;
        coolTime = 0f;
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
        prefabName = SkillDatas.PrefabName;
    }

    public override void DoSkill()
    {
        Manager.SoundM.Play(Define.Sound.Effect, SkillDatas.CastingSoundLabel);
        Vector3 pos = Manager.GameM.player.transform.position;
        Vector3 baseDir = Manager.GameM.player.ShootDir;
        Transform standard = Manager.GameM.player.Standard.transform;

        Quaternion baseRot = standard.rotation;

        float totalAngle = 30f;
        int count = projectileCount;
        for(int i =0; i< count; i++)
        {
            float angle = Mathf.Lerp(-totalAngle / 2f, totalAngle /2f, count == 1 ? 0.5f : (float)i / (count -1));
            Quaternion offsetRot = Quaternion.Euler(0,0,angle);
            Vector3 finalDir = offsetRot * baseDir;
            Quaternion finalRot = offsetRot * baseRot;
            
            var obj =  GenerateProjectile(Manager.GameM.player, prefabName, pos, finalDir, _skill:this);
            obj.transform.rotation = finalRot;
        }
    }



    public void Tick(float _deltaTime)
    {
        coolTime -= _deltaTime;
        if(coolTime <= 0)
        {
            DoSkill();
            coolTime = SkillDatas.CoolTime * (1 - Manager.GameM.CurrentCharacter.Evol_CoolTimeBouns); ;
        }
    }

}
