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

    #region GravityBomb Info
    public SkillZone GravityTarget { get; private set; }
    public bool isInGravityZone => GravityTarget != null;
    #endregion

    #region Player Contact
    private PlayerController contactPlayer;
    private bool isInContactWithPlayer;
    private float nextDotDamageTime;
    #endregion

    #region KnockBack
    private bool isKnockBack;
    private float knockBackEndTime;
    private float knockBackCooldownEndTime;
    #endregion

    #region Skill Zone
    private bool isInZone;
    private SkillBase activeSkillInZone;
    private CreatureController zoneOwner;
    private float skillZoneTickTime;
    #endregion

    #region Movement
    private Vector2 moveDir;
    private float pullForce = 0f;
    private float originalSpeed;
    #endregion

    #region Unity 기본

    protected virtual void OnEnable()
    {
        if (DataID != 0) SetInfo(DataID);

        Manager.UpdateM.Register(this);

        isDead = false;
        CreatureState = CreatureState.Moving;
        isKnockBack = false;

        contactPlayer = null;
        isInContactWithPlayer = false;

        originalSpeed = Speed;
        transform.localScale = Vector3.one;
    }

    private void OnDisable()
    {
        Manager.UpdateM.Unregister(this);
    }

    public override bool Init()
    {
        if (!base.Init()) return false;

        objType = ObjectType.Monster;
        return true;
    }

    #endregion

    #region Tick

    public override void Tick(float _deltaTime)
    {
        base.Tick(_deltaTime);

        if (isDead || !Manager.GameM.player.IsValid()) return;

        // Skill Zone 데미지 Tick
        if (isInZone && activeSkillInZone != null && Time.time >= skillZoneTickTime)
        {
            OnDamaged(zoneOwner, activeSkillInZone);
            skillZoneTickTime = Time.time + activeSkillInZone.SkillDatas.AttackInterval;
        }

        // KnockBack 처리
        if (isKnockBack)
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

        // KnockBack 쿨타임 중이면 이동 안 함
        if (Time.time < knockBackCooldownEndTime) return;

        // 이동 방향 계산
        moveDir = isInGravityZone
            ? (GravityTarget.transform.position - transform.position).normalized
            : (Manager.GameM.player.transform.position - transform.position).normalized;

        Rigid.velocity = isInGravityZone ? moveDir * pullForce : moveDir * Speed;

        CreatureSprite.flipX = moveDir.x < 0;

        // 플레이어 접촉 시 DOT 데미지
        if (isInContactWithPlayer && contactPlayer != null && Time.time >= nextDotDamageTime)
        {
            contactPlayer.OnDamaged(this, null, Attack);
            nextDotDamageTime = Time.time + 0.1f;
        }
    }

    #endregion

    #region Collision

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player == null || !player.IsValid()) return;

        contactPlayer = player;
        isInContactWithPlayer = true;
        nextDotDamageTime = Time.time;
    }

    public virtual void OnCollisionExit2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player == null || !player.IsValid()) return;

        if (contactPlayer == player)
        {
            isInContactWithPlayer = false;
            contactPlayer = null;
        }
    }

    #endregion

    #region 데미지 & 죽음

    public override void OnDamaged(BaseController _attacker, SkillBase _skill = null, float _damage = 0)
    {
        if (_skill != null)
        {
            //TODO : 다 만들고 마지막 사운드Manager.SoundM.Play(Sound.Effect, _skill.SkillDatas.HitEffectID);
            float totalDamage = Manager.GameM.player.Attack * _skill.SkillDatas.DamageMultiplier;
            base.OnDamaged(_attacker, _skill, totalDamage);
        }
        else
        {
            base.OnDamaged(_attacker, _skill, _damage);
        }

        if (objType == ObjectType.Monster)
        {
            if (_skill != null) KnockBack(_skill);
            else KnockBack();
        }
    }

    public override void OnDead()
    {
        base.OnDead();
        InvokeMonsterData();
        Manager.GameM.player.KillCount++;
        Manager.GameM.TotalMonsterKillCount++;

        // 드롭
        if (objType == ObjectType.Monster && UnityEngine.Random.value >= Manager.GameM.CurrentWaveData.NonDropRate)
        {
            GemController gem = Manager.ObjectM.Spawn<GemController>(transform.position, _prefabName: Define.DROPITEMNAME);
            gem.SetInfo(Manager.GameM.GetGemInfo());
        }

        DOTween.Sequence()
            .Append(transform.DOScale(0f, 0.2f).SetEase(Ease.InOutBounce))
            .OnComplete(() =>
            {
                Rigid.velocity = Vector2.zero;
                Manager.ObjectM.DeSpawn(this);
            });
    }

    #endregion

    #region KnockBack

    public void KnockBack(SkillBase _skill = null)
    {
        if (_skill == null || _skill.Skilltype == SkillType.TimeStopBomb || _skill.Skilltype == SkillType.GravityBomb || _skill.Skilltype == SkillType.ElectronicField) return;
        if (isKnockBack) return;

        isKnockBack = true;
        CreatureState = CreatureState.OnDamaged;

        Vector2 Knockdir = -moveDir.normalized;
        float power = _skill.SkillDatas.KnockBackPower;

        Rigid.AddForce(Knockdir * KNOCKBACK_POWER * power, ForceMode2D.Impulse);
        knockBackEndTime = Time.time + KNOCKBACK_TIME;
    }

    #endregion

    #region Skill Zone

    public void SetGravityTarget(SkillZone _target)
    {
        GravityTarget = _target;
    }

    public void ClearGravityTarget(SkillZone _target)
    {
        if (GravityTarget == _target) GravityTarget = null;
    }

    public void StartSKillZone(CreatureController _owner, SkillBase _skill, SkillZone _zone = null)
    {
        isInZone = true;
        zoneOwner = _owner;
        activeSkillInZone = _skill;

        switch (_skill.Skilltype)
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

        skillZoneTickTime = Time.time;
    }

    public void StopSkillZone(SkillBase _skill)
    {
        isInZone = false;

        switch (_skill.Skilltype)
        {
            case SkillType.TimeStopBomb:
                Speed = originalSpeed;
                break;

            case SkillType.GravityBomb:
                ClearGravityTarget(GravityTarget);
                break;
        }

        activeSkillInZone = null;
        zoneOwner = null;
    }

    #endregion

    #region 기타

    public void InvokeMonsterData()
    {
        if (this.IsValid() && gameObject.IsValid() && objType != ObjectType.Monster)
        {
            MonsterInfoUpdate?.Invoke(this);
        }
    }

    #endregion
}