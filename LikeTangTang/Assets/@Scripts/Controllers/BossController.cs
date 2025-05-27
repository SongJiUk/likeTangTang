using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BossController : MonsterController
{
    public Action<BossController> BossMonsterInfoUpdate;
    // [ ] DATA LOAD
    float range = 2.0f;
    public override bool Init()
    {
        if (!base.Init()) return false;
        Hp = 10000;
        transform.localScale = new Vector3(2f,2f,2f);
        objType = Define.ObjectType.Boss;
        CreatureState = Define.CreatureState.Moving;

        Skills = gameObject.GetOrAddComponent<SkillComponent>();
        InvokeMonsterData();

        return true;
    }

    
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
            // WaitTime(0.5f); //TODO : 이걸 꼭 0.5초를 기다려야하나?(근데 스킬같은건 기다리면 좋긴함.)
        }
    }
    protected override void UpdateAttack()
    {
        if(coWait == null) 
            CreatureState = Define.CreatureState.Moving;
    }
    protected override void UpdateDead()
    {
        // TODO : 떨구는 아이템, simulated 끄기,

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
    }
    public override void InitStat(bool _isHpFull = false)
    {
        var stageLevel = Manager.GameM.CurrentStageData.StageLevel;

        MaxHp = (creatureData.MaxHp + (creatureData.MaxHpUpForIncreasStage * stageLevel)) * creatureData.HpRate;
        Attack = (creatureData.Attack + (creatureData.AttackUpForIncreasStage * stageLevel)) * creatureData.AttackRate;

        Hp = MaxHp;
        Speed = creatureData.Speed * creatureData.MoveSpeedRate;
    }


    #region CoolTime 계산
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
