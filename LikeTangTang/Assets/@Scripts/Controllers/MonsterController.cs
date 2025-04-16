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
    void FixedUpdate()
    {
        if(Manager.GameM.player.IsValid() == false) return;
        if(isKnockBack) return;

        moveDir = Manager.GameM.player.transform.position - transform.position;
        Vector3 newPos = transform.position + moveDir.normalized * Time.deltaTime * Speed;
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
            // 피해자에서 처리하는게 좋음
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
        if(objType == ObjectType.Monster) KnockBack();
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
    public void KnockBack()
    {
        if(skillType == SkillType.TimeStopBomb || skillType == SkillType.GravityBomb) return;
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


}
