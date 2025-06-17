using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Data;
using System;

public class PlayerController : CreatureController, ITickable
{
    public Dictionary<string, Transform> EquipmentDic = new();
    [SerializeField] private UI_HP_Bar hp_bar;
    #region Action
    public Action OnPlayerDataUpdated;
    public Action OnPlayerLevelUp;
    public Action OnPlayerDead;
    public Action OnPlayerDamaged;
    #endregion

    #region 플레이어 스탯
    public bool IsDead
    {
        get => Manager.GameM.ContinueDatas.IsDead;
        set => Manager.GameM.ContinueDatas.IsDead = value;
    }

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
    public override float MaxHpRate
    {
        get => Manager.GameM.ContinueDatas.MaxHpBonusRate;
        set => Manager.GameM.ContinueDatas.MaxHpBonusRate = value;
    }

    public override float HealBounsRate
    {
        get => Manager.GameM.ContinueDatas.HealBonusRate;
        set => Manager.GameM.ContinueDatas.HealBonusRate = value;
    }

    public override float HpRegen
    {
        get => Manager.GameM.ContinueDatas.HpRegen;
        set => Manager.GameM.ContinueDatas.HpRegen = value;
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

    public override float CollectDistBonus
    {
        get => Manager.GameM.ContinueDatas.CollectDistBonus;
        set => Manager.GameM.ContinueDatas.CollectDistBonus = value;
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
                Exp -= currentLevel.TotalExp;
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


            return Exp / currentLevelData.TotalExp;
        }
    }

    public int KillCount
    {
        get => Manager.GameM.ContinueDatas.KillCount;
        set
        {
                Manager.GameM.ContinueDatas.KillCount = value;

            if (Manager.GameM.MissionDic.TryGetValue(Define.MissionTarget.MonsterKill, out MissionInfo missionInfo))
                missionInfo.Progress = value;

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

    public int SpecialSkillHealCount
    {
        get => Manager.GameM.ContinueDatas.SpecialSkillHealCount;
        set => Manager.GameM.ContinueDatas.SpecialSkillHealCount = value;
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

        Equipment item;
        Manager.GameM.EquipedEquipments.TryGetValue(Define.EquipmentType.Weapon, out item);

        if (item != null)
        {
            string str = Manager.DataM.EquipmentDic[item.key].SpriteName;
            string result = str.Replace(".sprite", "");

            EquipmentDic[result].gameObject.SetActive(true);
            Define.SkillType type = Utils.GetSkillTypeFromInt(item.EquipmentData.BaseSkill);
            if(type != Define.SkillType.None)
            {
                Skills.LevelUpSkill(type);
            }
        }

        foreach (Equipment equip in Manager.GameM.EquipedEquipments.Values)
        {
            int[] SpecialSkills = new int[]
            {
                    equip.EquipmentData.UnCommonGradeAbility,
                    equip.EquipmentData.RareGradeAbility,
                    equip.EquipmentData.EpicGradeAbility,
                    equip.EquipmentData.UniqueGradeAbility
            };

            int grade = Define.GetGradeNum(equip.EquipmentData.EquipmentGarde);
            Data.SpecialSkillData Skill;

            for (int i = 0; i <= grade; i++)
            {
                if (Manager.DataM.SpecialSkillDic.TryGetValue(SpecialSkills[i], out Skill))
                    Skills.AddSpecialSkill(Skill);
            }
        }


    }

    #endregion

    #region 이동

    Vector2 moveDir;
    Vector3 scale;
    Vector3 hpbar_Scale;

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
        {
            scale.x = Mathf.Abs(scale.x);
            hpbar_Scale.x = Mathf.Abs(hpbar_Scale.x);
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x);
            hpbar_Scale.x = -Mathf.Abs(hpbar_Scale.x);
        }

        transform.localScale = scale;
        hp_bar.transform.localScale = hpbar_Scale;
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
        Manager.GameM.OnMovePlayerDir += HandleOnMoveDirChange;
        scale = transform.localScale;
        hpbar_Scale = hp_bar.transform.localScale;
        Skills = gameObject.GetOrAddComponent<SkillComponent>();
        objType = Define.ObjectType.Player;
        FindEquipment();

        return true;
    }

    public override void SetInfo(int _dataID)
    {
        base.SetInfo(_dataID);

        if (Manager.GameM.ContinueDatas.isContinue)
            LoadSkil();
        else
            InitSkill();

        if (CreatureAnim != null)
            CreatureAnim.runtimeAnimatorController =
                Manager.ResourceM.Load<RuntimeAnimatorController>(creatureData.CreatureAnimName);
        Manager.GameM.SaveGame();
    }

    bool isFirst = true;
    public override void InitStat(bool _isHpFull = false)
    {
        MaxHp = Manager.GameM.CurrentCharacter.MaxHp;       
        Attack = Manager.GameM.CurrentCharacter.Attack;  
        Def = Manager.GameM.CurrentCharacter.Def;
        Speed = Manager.GameM.CurrentCharacter.MoveSpeed;
       
        if(isFirst)
        {
            MaxHpRate = Manager.GameM.CurrentCharacter.MaxHpRate;
            AttackRate = Manager.GameM.CurrentCharacter.AttackRate;
            DefRate = Manager.GameM.CurrentCharacter.DefRate;
            SpeedRate = Manager.GameM.CurrentCharacter.SpeedRate;
            CriticalRate = Manager.GameM.CurrentCharacter.CriticalRate;
            CriticalDamage = Manager.GameM.CurrentCharacter.CriticalDamage;
        }

        var (equip_Hp, equip_Attack) = Manager.GameM.GetCurrentCharacterStat();
        MaxHp += equip_Hp;
        Attack += equip_Attack;

        MaxHp *= MaxHpRate;
        Attack *= AttackRate;
        Def *= DefRate;
        Speed *= SpeedRate;


        Hp = MaxHp;
        isFirst = false;
    }

    public override void UpdatePlayerStat()
    {
        InitStat();
        Hp = MaxHp;

        //MaxHp *= MaxHpRate;
        //Attack *= AttackRate;
        //Def *= DefRate;
        //Speed *= SpeedRate;
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
        Manager.GameM.Camera.Shake();
        base.OnDamaged(_attacker, _skill, totalDamage);
        //base.OnDamaged(_attacker, null, 0); // 현재 테스트용 데미지 0
    }

    public override void OnDead()
    {
        IsDead = true;
        OnPlayerDead?.Invoke();
    }

    #endregion

    #region Tick
    float regenTimer = 0;
    public override void Tick(float _deltaTime)
    {
        base.Tick(_deltaTime);

        UpdatePlayerDir();
        Move();
        CollectDropItem();

        regenTimer += _deltaTime;
        if(regenTimer >= 2f)
        {
            regenTimer = 0f;
            HpSelfRecovery();
        }
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
    void HpSelfRecovery()
    {
        Healing(HpRegen, false);
    }

    public override void Healing(float _amount, bool _isEffect = true)
    {
        if (_amount <= 0) return;

        float res = (MaxHp * _amount) * (HealBounsRate + Manager.GameM.CurrentCharacter.Evol_HealingBouns);
        if (res == 0) return;
        Hp += res;

        if (Hp > MaxHp) Hp = MaxHp;


        Manager.ObjectM.ShowFont(transform.position, 0, res, transform);
        if (_isEffect)
            Manager.ResourceM.Instantiate("HealEffect", transform);
    }
    #endregion

    public void OnSafeZoneExit(BaseController _attacker)
    {
        float damage = MaxHp * 0.05f;
        OnDamaged(_attacker, null, damage);
        CreatureSprite.color = new Color(1, 1, 1, 0.5f);
        OnPlayerDamaged?.Invoke();
    }

    public void OnSafeZoneEnter()
    {
        CreatureSprite.color = new Color(1, 1, 1, 1f);
    }

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