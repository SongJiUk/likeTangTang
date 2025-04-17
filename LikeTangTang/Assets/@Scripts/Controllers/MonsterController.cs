using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using static Define;

public class MonsterController : CreatureController
{
    
    #region Action
    public Action<MonsterController> MonsterInfoUpdate;
    #endregion
    #region State Pattern
    CreatureState creatureState = CreatureState.Moving;
    protected Animator animator;
    public virtual CreatureState CreatureState
    {
        get { return creatureState; }
        set 
        {
            creatureState = value;
            UpdateAnim();
        }
    }

    public virtual void UpdateAnim() {}
    
    public override void UpdateController()
    {
        base.UpdateController();

        switch(creatureState)
        {
            case CreatureState.Moving:
            UpdateMoving();
                break;

            case CreatureState.Attack :
            UpdateAttack();
                break;

            case CreatureState.Dead :
            UpdateDead();
                break;
        }
    }

    protected virtual void UpdateMoving() {}

    protected virtual void UpdateAttack() {}

    protected virtual void UpdateDead() {}
    #endregion
    Coroutine coDotDamage;

    #region GravityBomb Info
    public SkillZone GravityTarget {get; set;}
    public bool isInGravityZone => GravityTarget != null;

    Coroutine coStartSkillZone;
#endregion

    #region SKill
    #endregion
    private void OnEnable()
    {
        if (DataID != 0) SetInfo(DataID);

    }
    public override bool Init()
    {
        if (!base.Init()) return false;

        string name = gameObject.name;
        objType = ObjectType.Monster;
        animator = GetComponent<Animator>();
        creatureState = CreatureState.Moving;
        //DefualtVelocity = Rigid.velocity;

        return true;
    }

    Vector3 moveDir;
    Vector2 DefualtVelocity;
    float pullForce = 0;
    void FixedUpdate()
    {
        if(Manager.GameM.player.IsValid() == false) return;
        if(isKnockBack) return;

    
        Vector3 newPos;
        if(isInGravityZone)
        {
            moveDir = GravityTarget.transform.position - transform.position;
            newPos = transform.position + moveDir.normalized * Time.deltaTime * pullForce;
        }
        else
        {
            moveDir = Manager.GameM.player.transform.position - transform.position;
            newPos = transform.position + moveDir.normalized * Time.deltaTime * Speed;
        }

        Debug.Log(Speed);

        Rigid.MovePosition(newPos);
        CreatureSprite.flipX = moveDir.x < 0;
    }
    
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController target =  collision.gameObject.GetComponent<PlayerController>();

        if (target == null) return;
        if(target.IsValid() == false) return;


        if (coDotDamage != null)
            StopCoroutine(coDotDamage);
        coDotDamage = StartCoroutine(CoStartDotDamage(target));


    }

    public virtual void OnCollisionExit2D(Collision2D collision)
    {
        PlayerController target = collision.gameObject.GetComponent<PlayerController>();
        if (target == null) return;
        if(target.IsValid() == false) return;
        
        if (coDotDamage != null ) 
            StopCoroutine(coDotDamage);
        coDotDamage = null;
    }

    
    public IEnumerator CoStartDotDamage(PlayerController _target)
    {
        while (true)
        {
            _target.OnDamaged(this, null, Attack);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override void OnDamaged(BaseController _attacker, SkillBase _skill = null, float _damage = 0)
    {
        if (_skill != null)
            Manager.SoundM.Play(Sound.Effect, _skill.SkillDatas.HitEffectID);
            

        float totalDamage = Manager.GameM.player.Attack * _skill.SkillDatas.DamageMultiplier;
        base.OnDamaged(_attacker, _skill, totalDamage);

        if(objType == ObjectType.Monster)
        {
            if(_skill != null) KnockBack(_skill);
            else KnockBack();
        }
    }

    public override void OnDead()
    {
        base.OnDead();

        Manager.GameM.player.KillCount++;

        if(UnityEngine.Random.value >= Manager.GameM.CurrentWaveData.NonDropRate)
        {
            GemController gem = Manager.ObjectM.Spawn<GemController>(this.transform.position);
            gem.SetInfo(Manager.GameM.GetGemInfo());
        }

        Sequence sq = DOTween.Sequence();
        sq.Append(transform.DOScale(0f, 0.2f).SetEase(Ease.InOutBounce)).OnComplete(() =>
        {
            StopAllCoroutines();


            Manager.ObjectM.DeSpawn(this);
        });


        if (coDotDamage != null) StopCoroutine(coDotDamage);

    }
    
    bool isKnockBack = false;

    Coroutine coKnockBackCoroutine;
    public void KnockBack(SkillBase _skill = null)
    {
        if(_skill != null)
        {
            if(_skill.Skilltype == SkillType.TimeStopBomb || _skill.Skilltype == SkillType.GravityBomb)
            {
                return;
            }
        }

        if (isKnockBack) return;
        if (coKnockBackCoroutine != null)
        {
            StopCoroutine(coKnockBackCoroutine);
            coKnockBackCoroutine = null;
        }

        isKnockBack = true;
        CreatureState = CreatureState.OnDamaged;
        Vector2 dir = -moveDir.normalized;
        Rigid.AddForce(dir * KNOCKBACK_POWER, ForceMode2D.Impulse);

        coKnockBackCoroutine = StartCoroutine(CoKnockBackCoolTime());
    }

    IEnumerator CoKnockBackCoolTime()
    {
        yield return new WaitForSeconds(KNOCKBACK_TIME);
        if (Rigid != null)
            Rigid.velocity = Vector2.zero;
        isKnockBack = false;
        CreatureState = CreatureState.Moving;
        yield return new WaitForSeconds(KNOCKBACK_COOLTIME);
        coKnockBackCoroutine = null;
        
    }

#region  Set SkillZone Info
    
    public void SetGravityTarget(SkillZone _target)
    {
        if(_target == null) return;
        GravityTarget = _target;
    }

    public void ClearGravityTarget(SkillZone _target)
    {
        if(_target == null) return;

        if(GravityTarget == _target) GravityTarget = null;
    }
    
    public void StartSKillZone(CreatureController _owner, SkillBase _skill, SkillZone _zone = null)
    {
        if(_zone != null)
            coStartSkillZone = StartCoroutine(CoStartSkillZone(_owner, _skill, _zone));
        else
            coStartSkillZone = StartCoroutine(CoStartSkillZone(_owner, _skill));
    }

    public void StopSkillZone(SkillBase _skill)
    {
        switch(_skill.Skilltype)
        {
            case SkillType.TimeStopBomb :
                Speed /= _skill.SkillDatas.SlowRatio;
                break;

            case SkillType.GravityBomb :
                ClearGravityTarget(GravityTarget);
                break;
        }

        if(coStartSkillZone != null) 
        {
            StopCoroutine(coStartSkillZone);
            coStartSkillZone = null;
        }

        
    }
    IEnumerator CoStartSkillZone(CreatureController _owner,SkillBase _skill, SkillZone _zone = null)
    {
        SkillType skillType = _skill.Skilltype;
        
        switch(skillType)
        {
            case SkillType.TimeStopBomb :
                Speed *= _skill.SkillDatas.SlowRatio;
                break;

            case SkillType.GravityBomb :
                pullForce = _skill.SkillDatas.PullForce;
                SetGravityTarget(_zone);
            break;

            case SkillType.EletronicField :
                
            break;
        }

        while(true)
        {
            OnDamaged(_owner, _skill);
            yield return new WaitForSeconds(_skill.SkillDatas.AttackInterval);
        }

        yield break;
    }
#endregion

}