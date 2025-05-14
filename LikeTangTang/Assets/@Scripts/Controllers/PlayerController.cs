using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Data;
using System;

public class PlayerController : CreatureController, ITickable
{
    Dictionary<string, Transform> EquipmentDic = new();
    string weaponName = "";
    #region Action
    public Action OnPlayerDataUpdated;
    public Action OnPlayerLevelUp;
    public Action OnPlayerDead;
    #endregion

    #region 플레이어 스탯

    public override int DataID
    {
        get => Manager.GameM.ContinueDatas.PlayerDataID;
        set => Manager.GameM.ContinueDatas.PlayerDataID = value;
    }

    public override float Hp
    {
        get => Manager.GameM.ContinueDatas.Hp;
        set => Manager.GameM.ContinueDatas.Hp = value;
    }

    public override float MaxHp
    {
        get => Manager.GameM.ContinueDatas.MaxHp;
        set => Manager.GameM.ContinueDatas.MaxHp = value;
    }

    public override float Attack
    {
        get => Manager.GameM.ContinueDatas.Attack;
        set => Manager.GameM.ContinueDatas.Attack = value;
    }

    public override float AttackRate
    {
        get => Manager.GameM.ContinueDatas.AttackRate;
        set => Manager.GameM.ContinueDatas.AttackRate = value;
    }

    public override float Def
    {
        get => Manager.GameM.ContinueDatas.Def;
        set => Manager.GameM.ContinueDatas.Def = value;
    }

    public override float DefRate
    {
        get => Manager.GameM.ContinueDatas.DefRate;
        set => Manager.GameM.ContinueDatas.DefRate = value;
    }

    public override float CriticalRate
    {
        get => Manager.GameM.ContinueDatas.CriticalRate;
        set => Manager.GameM.ContinueDatas.CriticalRate = value;
    }

    public override float CriticalDamage
    {
        get => Manager.GameM.ContinueDatas.CriticalDamage;
        set => Manager.GameM.ContinueDatas.CriticalDamage = value;
    }

    public override float DamageReduction
    {
        get => Manager.GameM.ContinueDatas.DamageReduction;
        set => Manager.GameM.ContinueDatas.DamageReduction = value;
    }

    public override float SpeedRate
    {
        get => Manager.GameM.ContinueDatas.MoveSpeedRate;
        set => Manager.GameM.ContinueDatas.MoveSpeedRate = value;
    }

    public override float Speed
    {
        get => Manager.GameM.ContinueDatas.MoveSpeed;
        set => Manager.GameM.ContinueDatas.MoveSpeed = value;
    }

    public int Level
    {
        get => Manager.GameM.ContinueDatas.Level;
        set => Manager.GameM.ContinueDatas.Level = value;
    }

    public float TotalExp
    {
        get => Manager.GameM.ContinueDatas.TotalExp;
        set => Manager.GameM.ContinueDatas.TotalExp = value;
    }

    public float Exp
    {
        get => Manager.GameM.ContinueDatas.Exp;
        set
        {
            Manager.GameM.ContinueDatas.Exp = value;

            while (Manager.DataM.LevelDic.TryGetValue(Level + 1, out var nextLevel) &&
                   Manager.DataM.LevelDic.TryGetValue(Level, out var currentLevel) &&
                   Exp >= currentLevel.TotalExp)
            {
                Level++;
                TotalExp = nextLevel.TotalExp;
                LevelUp(Level);
            }

            OnPlayerDataUpdated?.Invoke();
        }
    }

    public float ExpRatio
    {
        get
        {
            if (!Manager.DataM.LevelDic.TryGetValue(Level, out var currentLevelData))
                return 0f;

            float prevExp = 0f;
            if (Manager.DataM.LevelDic.TryGetValue(Level - 1, out var prevLevelData))
                prevExp = prevLevelData.TotalExp;

            return (Exp - prevExp) / (currentLevelData.TotalExp - prevExp);
        }
    }

    public int KillCount
    {
        get => Manager.GameM.ContinueDatas.KillCount;
        set
        {
            Manager.GameM.ContinueDatas.KillCount = value;
            OnPlayerDataUpdated?.Invoke();
        }
    }

    public float ExpBounsRate
    {
        get => Manager.GameM.ContinueDatas.ExpBonusRate;
        set => Manager.GameM.ContinueDatas.ExpBonusRate = value;
    }

    public int SkillRefreshCount
    {
        get => Manager.GameM.ContinueDatas.SkillRefreshCount;
        set => Manager.GameM.ContinueDatas.SkillRefreshCount = value;
    }

    #endregion

    #region 레벨업

    public void LevelUp(int _level = 0)
    {
        if (_level > 1)
            OnPlayerLevelUp?.Invoke();
    }

    #endregion

    #region 스킬

    [SerializeField] Transform standard;
    [SerializeField] Transform firePos;
    [SerializeField] Transform WeaponHolder;

    public Transform Standard => standard;
    public Vector3 FirePos => firePos.position;
    public Vector3 ShootDir => (firePos.position - standard.position).normalized;

    public override void InitSkill()
    {
        base.InitSkill();
    }

    #endregion

    #region 이동

    Vector2 moveDir;
    Vector3 scale;

    public Vector2 MoveDir
    {
        get => moveDir;
        set => moveDir = value;
    }

    void Move()
    {
        if (moveDir == Vector2.zero)
        {
            if (Rigid.velocity != Vector2.zero)
                Rigid.velocity = Vector2.zero;

            CreatureState = Define.CreatureState.Idle;
            return;
        }

        Rigid.velocity = moveDir.normalized * Speed;

        float angle = Mathf.Atan2(-moveDir.x, moveDir.y) * Mathf.Rad2Deg;
        standard.eulerAngles = new Vector3(0, 0, angle);

        CreatureState = Define.CreatureState.Moving;
    }

    void HandleOnMoveDirChange(Vector2 _dir) => moveDir = _dir;

    void UpdatePlayerDir()
    {
        if (moveDir == Vector2.zero) return;

        if (moveDir.x < 0)
            scale.x = Mathf.Abs(scale.x);
        else
            scale.x = -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    #endregion

    #region 드랍 아이템

    float GetDropItemDist = 2f;

    void CollectDropItem()
    {
        var FindDropItem = Manager.GameM.CurrentMap.Grid.GetObjects(transform.position, GetDropItemDist);

        float sqrtDist = GetDropItemDist * GetDropItemDist;
        foreach (DropItemController dropItem in FindDropItem)
        {
            Vector3 dir = dropItem.transform.position - transform.position;
            switch (dropItem.itemType)
            {
                case Define.ItemType.Gem:
                    float dist = dropItem.CollectDist * Manager.GameM.ContinueDatas.CollectDistBonus;
                    if (dir.sqrMagnitude <= dist * dist)
                        dropItem.GetItem();
                    break;

                case Define.ItemType.Bomb:
                case Define.ItemType.Magnet:
                case Define.ItemType.Potion:
                case Define.ItemType.DropBox:
                    if (dir.sqrMagnitude <= sqrtDist)
                        dropItem.GetItem();
                    break;
            }
        }
    }

    #endregion

    #region Unity 기본

    private void OnEnable() => Manager.UpdateM?.Register(this);
    private void OnDisable() => Manager.UpdateM.Unregister(this);

    public override bool Init()
    {
        base.Init();
        weaponName = "";
        Manager.GameM.OnMovePlayerDir += HandleOnMoveDirChange;
        scale = transform.localScale;

        FindEquipment();

        return true;
    }

    public override void SetInfo(int _dataID)
    {
        base.SetInfo(_dataID);

        
        if (CreatureAnim != null)
            CreatureAnim.runtimeAnimatorController =
                Manager.ResourceM.Load<RuntimeAnimatorController>(creatureData.CreatureAnimName);
    }

    public override void InitStat(bool _isHpFull = false)
    {
        MaxHp = Manager.GameM.CurrentCharacter.MaxHp;
        Attack = Manager.GameM.CurrentCharacter.Attack;
        Speed = creatureData.Speed * creatureData.MoveSpeedRate;

        var (equip_Hp, equip_Attack) = Manager.GameM.GetCurrentCharacterStat();
        MaxHp += equip_Hp;
        Attack += equip_Attack;

        Attack *= AttackRate;
        Def *= DefRate;
        Speed *= SpeedRate;


        if (_isHpFull) Hp = MaxHp;
    }

    private void OnDestroy()
    {
        if (Manager.GameM != null)
            Manager.GameM.OnMovePlayerDir -= HandleOnMoveDirChange;
    }

    #endregion

    #region 데미지 & 사망

    public override void OnDamaged(BaseController _attacker, SkillBase _skill, float _damage = 0)
    {
        float totalDamage = 0f;
        CreatureController cc = _attacker as CreatureController;

        if (cc != null)
        {
            if (_skill == null)
                totalDamage = cc.Attack;
            else
                totalDamage = cc.Attack + (cc.Attack * _skill.SkillDatas.DamageMultiplier);
        }

        totalDamage *= 1 - DamageReduction;

        // TODO: 실제 데미지 계산이 확정되면 아래 주석 해제
        // base.OnDamaged(_attacker, _skill, totalDamage);
        base.OnDamaged(_attacker, null, 0); // 현재 테스트용 데미지 0
    }

    public override void OnDead()
    {
        base.OnDead();
        Time.timeScale = 0f;
        OnPlayerDead?.Invoke();
    }

    #endregion

    #region Tick

    public override void Tick(float _deltaTime)
    {
        base.Tick(_deltaTime);
        if (isDead) return;

        UpdatePlayerDir();
        Move();
        CollectDropItem();
    }

    #endregion

    #region 장비 찾기

    void FindEquipment()
    {
        if (WeaponHolder == null)
            WeaponHolder = transform.Find("Weapon");

        if (WeaponHolder != null)
        {
            foreach (Transform weapon in WeaponHolder)
            {
                EquipmentDic.Add(weapon.name, weapon);
                weapon.gameObject.SetActive(false);
            }
        }
    }

    #endregion

    #region 힐

    public void Healing(float _amount, bool _isEffect = true)
    {
        if (_amount <= 0) return;

        float res = MaxHp * _amount;
        Hp += res;

        // TODO: Heal 이펙트 추가
    }

    #endregion

    #region 애니메이션

    public override void UpdateAnim()
    {
        switch (CreatureState)
        {
            case Define.CreatureState.Idle:
                UpdateIdle();
                break;
            case Define.CreatureState.Moving:
                UpdateMoving();
                break;
            case Define.CreatureState.Dead:
                UpdateDead();
                break;
        }
    }

    protected override void UpdateIdle() => CreatureAnim.Play("Idle");
    protected override void UpdateMoving() => CreatureAnim.Play("Moving");
    protected override void UpdateDead() => CreatureAnim.Play("Dead");

    #endregion
}