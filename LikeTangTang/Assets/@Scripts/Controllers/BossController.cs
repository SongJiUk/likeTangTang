using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BossController : MonsterController
{
    public Action OnBossDead;
    // [ ] DATA LOAD
    float range = 2.0f;

    protected override void OnEnable()
    {
        base.OnEnable();
        transform.localScale = new Vector3(3f, 3f, 3f);
    }

    private void Start()
    {
        Init();


        Skills.StartNextSequenceSkill();
        InvokeMonsterData();
    }
    public override bool Init()
    {
        if (!base.Init()) return false;
        transform.localScale = new Vector3(3f, 3f, 3f);
        objType = Define.ObjectType.Boss;
        CreatureState = Define.CreatureState.Attack;

        Skills = gameObject.GetOrAddComponent<SkillComponent>();
        //if (Skills)
        //{
        //    foreach (SkillBase skill in Skills.skillList)
        //    {
        //        skill.SkillLevel = 0;
        //        skill.UpdateSkillData();
        //    }
        //}

        if (creatureData != null)
        {
            if (creatureData.SkillTypeList.Count != 0)
            {
                InitSkill();
                Skills.LevelUpSkill((Define.SkillType)creatureData.SkillTypeList[0]);
            }
        }
        InvokeMonsterData();

        return true;
    }

    
    public override void OnDamaged(BaseController _attacker, SkillBase _skill = null, float _damage = 0)
    {
        base.OnDamaged(_attacker, _skill, _damage);
    }

    public override void OnDead()
    {
        base.OnDead();

        if (Manager.GameM.MissionDic.TryGetValue(Define.MissionTarget.BossKill, out MissionInfo mission))
            mission.Progress++;
        Manager.GameM.TotalBossKillCount++;

        OnBossDead?.Invoke();
    }
    public override void InitStat(bool _isHpFull = false)
    {
        var stageLevel = Manager.GameM.CurrentStageData.StageLevel;

        MaxHp = (creatureData.MaxHp + (creatureData.MaxHpUpForIncreasStage * stageLevel)) * creatureData.HpRate;
        Attack = (creatureData.Attack + (creatureData.AttackUpForIncreasStage * stageLevel)) * creatureData.AttackRate;

        Hp = MaxHp;
        Speed = creatureData.Speed * creatureData.MoveSpeedRate;
    }

    #region 사용 x
    public override void UpdateAnim()
    {
        if(CreatureAnim == null) return;

        switch(CreatureState)
        {
            case Define.CreatureState.Moving:
                CreatureAnim.Play("Moving");
                break;
            case Define.CreatureState.Attack :
                CreatureAnim.Play("Attack");
                break;
            case Define.CreatureState.Dead :
                CreatureAnim.Play("Dead");
                break;
        }
    }
    
    protected override void UpdateMoving()
    {
        PlayerController pc = Manager.GameM.player;
        if(pc.IsValid() == false) return;

        Vector3 dir = pc.transform.position - transform.position;

        if(dir.sqrMagnitude <= range * range)
        {
            CreatureState = Define.CreatureState.Attack;
            // WaitTime(0.5f);
        }
    }
    protected override void UpdateAttack()
    {
        if(coWait == null) 
            CreatureState = Define.CreatureState.Moving;
    }
    protected override void UpdateDead()
    {

    }
    #endregion


    #region CoolTime 계산(사용 x)
    Coroutine coWait;

    void WaitTime(float _time)
    {
        if(coWait != null) StopCoroutine(coWait);

        coWait = StartCoroutine(coWaitTime(_time));
    }

    IEnumerator coWaitTime(float _time)
    {
        yield return new WaitForSeconds(_time);
        coWait = null;
    }

    IEnumerator coWaitAndDo(float _time, Action _action)
    {
        yield return new WaitForSeconds(_time);
        _action?.Invoke();
    }
    #endregion
    
}
