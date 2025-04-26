using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using static Define;

public class MonsterController : CreatureController, ITickable
{
    
    #region Action
    public Action<MonsterController> MonsterInfoUpdate;
    
    #endregion
    
    Coroutine coDotDamage;

    #region GravityBomb Info
    public SkillZone GravityTarget {get; set;}
    public bool isInGravityZone => GravityTarget != null;

    Coroutine coStartSkillZone;
    #endregion

    #region SKill
    #endregion
    private PlayerController contactPlayer;
    private bool isInContactWithPlayer;
    private float knockBackEndTime;
    private float knockBackCooldownEndTime;
    private float nextDotDamageTime;
    float originalSpeed;
    bool isKnockBack; 
    private SkillBase activeSkillInZone;
    private CreatureController zoneOwner;
    float skillZoneTickTime;
    
    private void Awake()
    {
    }
    private void OnEnable()
    {
        if (DataID != 0) SetInfo(DataID);
        
        Manager.UpdateM.Register(this);
        isDead = false;
        CreatureState = CreatureState.Moving;
        isKnockBack = false;
        contactPlayer = null;
        isInContactWithPlayer = false;
        originalSpeed = Speed;

    }
    void OnDisable()
    {
        Manager.UpdateM.Unregister(this);
    }
    public override bool Init()
    {
        if (!base.Init()) return false;

        string name = gameObject.name;
        objType = ObjectType.Monster;
        
        //DefualtVelocity = Rigid.velocity;

        return true;
    }

    Vector2 moveDir;
    Vector2 DefualtVelocity;
    float pullForce = 0;


    public override void Tick(float _deltaTime)
    {
        base.Tick(_deltaTime);

        if (isDead || !Manager.GameM.player.IsValid()) return;
        
        if(isKnockBack)
        {
            if (Time.time >= knockBackEndTime)
            {
                Rigid.velocity = Vector2.zero;
                isKnockBack = false;
                CreatureState = CreatureState.Moving;
                knockBackCooldownEndTime = Time.time + KNOCKBACK_COOLTIME;
            }
            return;
        }

        if (Time.time < knockBackCooldownEndTime) return;

        moveDir = isInGravityZone ? 
        (GravityTarget.transform.position - transform.position).normalized 
        : (Manager.GameM.player.transform.position - transform.position).normalized;

        Rigid.velocity = isInGravityZone ? moveDir * pullForce : moveDir * Speed;
        
        CreatureSprite.flipX = moveDir.x < 0;

        if (isInContactWithPlayer && Time.time >= nextDotDamageTime && contactPlayer != null)
        {
            contactPlayer.OnDamaged(this, null, Attack);
            nextDotDamageTime = Time.time + 0.1f;
        }
    }
    
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player =  collision.gameObject.GetComponent<PlayerController>();

        if (player == null) return;
        if(player.IsValid() == false) return;

        contactPlayer = player;
        isInContactWithPlayer = true;
        nextDotDamageTime = Time.time;



    }

    public virtual void OnCollisionExit2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player == null) return;
        if(player.IsValid() == false) return;
         
        if(contactPlayer == player)
        {
            isInContactWithPlayer = false;
            contactPlayer = null;
        }
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
        {
            Manager.SoundM.Play(Sound.Effect, _skill.SkillDatas.HitEffectID);
            float totalDamage = Manager.GameM.player.Attack * _skill.SkillDatas.DamageMultiplier;
            base.OnDamaged(_attacker, _skill, totalDamage);
        }
        else
        {
            base.OnDamaged(_attacker, _skill, _damage);
        }

        if(objType == ObjectType.Monster)
        {
            if(_skill != null) KnockBack(_skill);
            else KnockBack();
        }
    }

    public override void OnDead()
    {
        base.OnDead();
        isDead = true;
        Manager.GameM.player.KillCount++;


        if(UnityEngine.Random.value >= Manager.GameM.CurrentWaveData.NonDropRate)
        {
            GemController gem = Manager.ObjectM.Spawn<GemController>(this.transform.position);
            gem.SetInfo(Manager.GameM.GetGemInfo());
        }

         DOTween.Sequence().
            Append(transform.DOScale(0f, 0.2f).SetEase(Ease.InOutBounce))
            .OnComplete(() =>
            {
                Manager.ObjectM.DeSpawn(this);
            });
    }
    

    Coroutine coKnockBackCoroutine;
    public void KnockBack(SkillBase _skill = null)
    {
        if (_skill == null || _skill.Skilltype == SkillType.TimeStopBomb || _skill.Skilltype == SkillType.GravityBomb) return;
        if (isKnockBack) return;

        isKnockBack = true;
        CreatureState = CreatureState.OnDamaged;

        Vector2 Knockdir = -moveDir.normalized;
        float power = _skill.SkillDatas.KnockBackPower;
        
        Rigid.AddForce(Knockdir * KNOCKBACK_POWER * power, ForceMode2D.Impulse);

        knockBackEndTime = Time.time + KNOCKBACK_TIME;
    }

    //IEnumerator CoKnockBackCoolTime(SkillBase _skill = null)
    //{
    //    yield return new WaitForSeconds(KNOCKBACK_TIME);
    //    if (Rigid != null)
    //        Rigid.velocity = Vector2.zero;
    //    isKnockBack = false;
    //    CreatureState = CreatureState.Moving;
    //    yield return new WaitForSeconds(_skill.SkillDatas.KnockBackInterval);
    //    coKnockBackCoroutine = null;
        
    //}

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

        zoneOwner = _owner;
        activeSkillInZone = _skill;

        switch(_skill.Skilltype)
        {
            case SkillType.TimeStopBomb:
                originalSpeed = Speed;
                Speed *= _skill.SkillDatas.SlowRatio;
                break;

            case SkillType.GravityBomb:
                pullForce = _skill.SkillDatas.PullForce;
                SetGravityTarget(_zone);
                break;
                
        }
        skillZoneTickTime = Time.time + _skill.SkillDatas.AttackInterval;
    }

    public void StopSkillZone(SkillBase _skill)
    {
        switch(_skill.Skilltype)
        {
            case SkillType.TimeStopBomb :
                Speed = originalSpeed;
                break;

            case SkillType.GravityBomb :
                ClearGravityTarget(GravityTarget);
                break;
        }
        activeSkillInZone = null;
        zoneOwner = null;

        
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