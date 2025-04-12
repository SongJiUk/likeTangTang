using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BossController : MonsterController
{
    // [ ] DATA LOAD
    float range = 2.0f;
    public override bool Init()
    {
        if (!base.Init()) return false;
        Hp = 10000;
        CreatureState = Define.CreatureState.Moving;

        return true;
    }

    
    public override void UpdateAnim()
    {
        switch(CreatureState)
        {
            case Define.CreatureState.Moving:
                animator.Play("Moving");
                break;
            case Define.CreatureState.Attack :
                animator.Play("Attack");
                break;
            case Define.CreatureState.Dead :
                animator.Play("Dead");
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
        this.GetOrAddComponent<Rigidbody2D>().simulated = false;
    }

    public override void OnDamaged(BaseController _attacker, SkillBase _skill = null, float _damage = 0)
    {
        base.OnDamaged(_attacker, _skill, _damage);
    }


    public override void OnDead()
    {
        CreatureState = Define.CreatureState.Dead;

        //WaitTime(2.0f);
        //Manager.ObjectM.DeSpawn(this);

        StartCoroutine(coWaitAndDo(2.0f, () => Manager.ObjectM.DeSpawn(this)));
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
