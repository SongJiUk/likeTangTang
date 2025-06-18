using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaSpinner : RepeatSkill, ITickable
{
    private UpdateManager updateM;
    private ObjectManager objectM;
    private GameManager gameM;
    private SoundManager soundM;
    private Transform playerTransform;
    private float timeAccumulator;
    private float baseCoolTime;
    void Awake()
    {
        Skilltype = Define.SkillType.PlasmaSpinner;
        updateM = Manager.UpdateM;
        objectM = Manager.ObjectM;
        gameM = Manager.GameM;
        soundM = Manager.SoundM;
        playerTransform = gameM.player.transform;

        baseCoolTime = 0f;
    }
     private void OnDestroy()
    {
        Manager.UpdateM.Unregister(this);
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        updateM.Register(this);
        OnChangedSkillData();
    }

    public override void OnChangedSkillData()
    {
        duration = SkillDatas.Duration;
        baseCoolTime = SkillDatas.CoolTime * (1 - Manager.GameM.CurrentCharacter.Evol_CoolTimeBouns);
        projectileCount = SkillDatas.ProjectileCount;
    }
    
    public override void DoSkill()
    {
        soundM.Play(Define.Sound.Effect, SkillDatas.CastingSoundLabel);
        List<MonsterController> targets = objectM.GetNearMonsters(projectileCount);
        if(targets == null || targets.Count == 0) return;


        var prefabName = SkillDatas.PrefabName;
        var player = gameM.player;

        foreach(var monster in targets)
        {
            if (monster == null || !monster.IsValid()) return;
            Vector3 dir = (monster.transform.position - playerTransform.position).normalized;
            GenerateProjectile(player, prefabName, playerTransform.position, dir, monster.transform.position, this);

        }
    }

    public void Tick(float _deltaTime)
    {
        timeAccumulator += _deltaTime;
        if (timeAccumulator < baseCoolTime) return;

        DoSkill();
        timeAccumulator -= baseCoolTime;
    }

}
