using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using Data;
using System;
using UnityEngine.AI;

public class PlayerController : CreatureController, ITickable
{

    Dictionary<string, Transform> EquipmentDic = new();
    #region Action
    public Action OnPlayerDataUpdated;
    public Action OnPlayerLevelUp;
    public Action OnPlayerDead; //GameScene UI랑 연결
    #endregion
    #region 플레이어 정보
    public override int DataID
    {
        get {return Manager.GameM.ContinueDatas.PlayerDataID;}
        set { Manager.GameM.ContinueDatas.PlayerDataID = value;}
    }

    public override float Hp 
    { 
        get {return Manager.GameM.ContinueDatas.Hp;}
        set { Manager.GameM.ContinueDatas.Hp = value;}
    }

    public override float MaxHp
    {
        get { return Manager.GameM.ContinueDatas.MaxHp;}
        set { Manager.GameM.ContinueDatas.MaxHp = value;}
    }

    public override float Attack 
    { 
        get {return Manager.GameM.ContinueDatas.Attack; } 
        set {Manager.GameM.ContinueDatas.Attack = value;}
    }
    public override float AttackRate 
    { 
        get {return Manager.GameM.ContinueDatas.AttackRate;} 
        set {Manager.GameM.ContinueDatas.AttackRate = value;} 
    }
    public override float Def 
    { 
        get { return Manager.GameM.ContinueDatas.Def;}
        set { Manager.GameM.ContinueDatas.Def= value;}
    }
    public override float DefRate 
    { 
        get {return Manager.GameM.ContinueDatas.DefRate;} 
        set { Manager.GameM.ContinueDatas.DefRate = value;}
    }
    public override float CriticalRate 
    { 
        get {return Manager.GameM.ContinueDatas.CriticalRate;} 
        set {Manager.GameM.ContinueDatas.CriticalRate = value;}
    }
    public override float CriticalDamage 
    { 
        get {return Manager.GameM.ContinueDatas.CriticalDamage;} 
        set {Manager.GameM.ContinueDatas.CriticalDamage = value;}
    }
    public override float DamageReduction 
    { 
        get {return Manager.GameM.ContinueDatas.DamageReduction;} 
        set {Manager.GameM.ContinueDatas.DamageReduction = value;}
    }
    public override float SpeedRate 
    { 
        get {return Manager.GameM.ContinueDatas.MoveSpeedRate;} 
        set {Manager.GameM.ContinueDatas.MoveSpeedRate= value;}
    }
    public override float Speed 
    { 
        get {return Manager.GameM.ContinueDatas.MoveSpeed;} 
        set {Manager.GameM.ContinueDatas.MoveSpeed = value;} 
    }

    public int Level
    {
        get {return Manager.GameM.ContinueDatas.Level;}
        set {Manager.GameM.ContinueDatas.Level = value;}
    }

    public float TotalExp
    {
        get {return Manager.GameM.ContinueDatas.TotalExp;}
        set {Manager.GameM.ContinueDatas.TotalExp = value;}
    }
    public float Exp
    {
        get {return Manager.GameM.ContinueDatas.Exp;}
        set
        {
            Manager.GameM.ContinueDatas.Exp = value;
            
            //TODO : 여기서 경험치 획득 및 레벨업
            while(Manager.DataM.LevelDic.TryGetValue(Level + 1, out var nextLevel) &&
            Manager.DataM.LevelDic.TryGetValue(Level, out var currentLevel) &&
            Exp >= currentLevel.TotalExp)
            {
               Level++;
               TotalExp = nextLevel.TotalExp;
               LevelUp(Level);
            }

            OnPlayerDataUpdated();
        }
    }


    public float ExpRatio
    {
        get 
        {
            LevelData currentLevelData;
            if(Manager.DataM.LevelDic.TryGetValue(Level, out currentLevelData))
            {
                float prevLevelExp = 0;
                LevelData prevLevelData;

                if(Manager.DataM.LevelDic.TryGetValue(Level -1, out prevLevelData))
                {
                    prevLevelExp = prevLevelData.TotalExp;
                }
                float currentLevelExp = currentLevelData.TotalExp;

                return (Exp - prevLevelExp) / (currentLevelExp - prevLevelExp);
            }

            return 0f;
        }
    }

    public int KillCount
    {
        get {return Manager.GameM.ContinueDatas.KillCount;}
        set
        {
            Manager.GameM.ContinueDatas.KillCount = value;
            OnPlayerDataUpdated?.Invoke();
        }
    }

    public float ExpBounsRate
    {
        get { return Manager.GameM.ContinueDatas.ExpBonusRate;}
        set { Manager.GameM.ContinueDatas.ExpBonusRate = value;}
    }



    public void LevelUp(int _level = 0)
    {
        //TODO : 여기서 레벨업하고, 
        if(_level > 1) 
            OnPlayerLevelUp?.Invoke();
        
        //[ ] 스킬 업그레이드
        
    }

    Vector3 scale;
#endregion
    #region  스킬 정보

    
    [SerializeField]
    Transform standard;
    [SerializeField]
    Transform firePos;

    [SerializeField]
    Transform WeaponHolder;

    public Transform Standard {get { return standard;}}
    public Vector3 FirePos {get { return firePos.transform.position;}}
    public Vector3 ShootDir {get {return (firePos.position - standard.position).normalized;}}


    //NOTE : 여기임 여기 !! 여기서 스킬 추가하든 없애든 해야됌.
    public override void InitSkill()
    {
        base.InitSkill();


        // NOTE : Temp Code
        Define.SkillType skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.OrbitalBlades);
        Skills.LevelUpSkill(skillType);
        //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.ElectronicField);
        //Skills.LevelUpSkill(skillType);
        //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.EnergyRing);
        //Skills.LevelUpSkill(skillType);
        //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.GravityBomb);
        //Skills.LevelUpSkill(skillType);
        //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.OrbitalBlades);
        //Skills.LevelUpSkill(skillType);
        //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.PlasmaShot);
        //Skills.LevelUpSkill(skillType);
        //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.PlasmaSpinner);
        //Skills.LevelUpSkill(skillType);
        //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.SpectralSlash);
        //Skills.LevelUpSkill(skillType);
        //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.SuicideDrone);
        //Skills.LevelUpSkill(skillType);
        //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.TimeStopBomb);
        //Skills.LevelUpSkill(skillType);

    }

    #endregion

    #region 이동관련
    Vector2 moveDir;

    public Vector2 MoveDir
    {
        get { return moveDir; }
        set { moveDir = value; }
    }

    void Move()
    {
        if(moveDir == Vector2.zero)
        {
            if(Rigid.velocity != Vector2.zero) Rigid.velocity = Vector2.zero;
            CreatureState = Define.CreatureState.Idle;
            return;
        }

        Rigid.velocity = moveDir.normalized * Speed;
        float angle = Mathf.Atan2(-moveDir.x, moveDir.y) * Mathf.Rad2Deg;
        standard.eulerAngles = new Vector3(0, 0, angle);
        CreatureState = Define.CreatureState.Moving;

    }

    void HandleOnMoveDirChange(Vector2 _dir)
    {
        moveDir = _dir;
    }

    
    public void UpdatePlayerDir()
    {

        // TODO : filp코드
        //if (moveDir.x < 0) CreatureSprite.flipX = false;
        //else CreatureSprite.flipX = true;


        

        if (moveDir.x < 0) scale.x = Mathf.Abs(scale.x);
        else if(moveDir.x > 0)scale.x = -Mathf.Abs(scale.x);
        else return;


        transform.localScale = scale;


    }

    #endregion

    #region 드랍 아이템
    float GetDropItemDist = 4f;
    void CollectDropItem()
    {
        var FindDropItem = Manager.GameM.CurrentMap.Grid.GetObjects(transform.position, GetDropItemDist);

        var sqrtDist = GetDropItemDist * GetDropItemDist;
        foreach (DropItemController dropItem in FindDropItem)
        {
            Vector3 dir = dropItem.transform.position - transform.position;
            switch (dropItem.itemType)
            {
                case Define.ItemType.Gem:
                    float dist = dropItem.CollectDist * Manager.GameM.ContinueDatas.CollectDistBonus;
                    if (dir.sqrMagnitude <= dist * dist)
                    {
                        dropItem.GetItem();
                    }
                    break;

                case Define.ItemType.Bomb:
                case Define.ItemType.Magnet:
                case Define.ItemType.Potion:
                case Define.ItemType.DropBox:

                    if (dir.sqrMagnitude <= sqrtDist)
                    {
                        dropItem.GetItem();
                    }
                    break;
            }
        }
    }

    #endregion
    public void OnEnable() => Manager.UpdateM?.Register(this);
    public void OnDisable() => Manager.UpdateM.Unregister(this);
    
    public override bool Init()
    {
        base.Init();
       
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

    private void OnDestroy()
    {
        if(Manager.GameM != null)
            Manager.GameM.OnMovePlayerDir -= HandleOnMoveDirChange;
    }

    public override void OnDamaged(BaseController _attacker, SkillBase _skill, float _damage = 0)
    {
        float totalDamage = 0;
        CreatureController cc = _attacker as CreatureController;

        if (cc != null)
        {
            if (_skill == null)
                totalDamage = cc.Attack;
            else
                totalDamage = cc.Attack + (cc.Attack * _skill.SkillDatas.DamageMultiplier);
        }
        else
        {
            Debug.Log("No Enemy");
        }

        totalDamage *= 1 - DamageReduction;

        // NOTE : Temp Code
        totalDamage = 0;
        //TODO : 카메라 충격 넣을것인지?


        base.OnDamaged(_attacker, null, totalDamage);
    }

    public override void OnDead()
    {
        base.OnDead();
        Time.timeScale = 0f;
        OnPlayerDead?.Invoke();
    }

    public override void Tick(float _deltaTime)
    {
        base.Tick(_deltaTime);
        if(isDead) return;

        UpdatePlayerDir();
        Move();
        CollectDropItem();

    }

    public void FindEquipment()
    {
        if(WeaponHolder == null) WeaponHolder = transform.Find("Weapon");

        if(WeaponHolder != null)
        {
            foreach(Transform weapon in WeaponHolder)
            {
                EquipmentDic.Add(weapon.name, weapon);
                weapon.gameObject.SetActive(false);
            }
        }
    }


    public void Healing(float _amount, bool _isEffect = true)
    {
        if (_amount == 0) return;

        float res = (MaxHp * _amount);

        if (res == 0) return;

        Hp += res;

        //TDOO
        //Manager.ObjectM.ShowFont(transform.position, 0, res, transform);

        //if (_isEffect) Manager.ResourceM.Instantiate("HealEffect", transform);
    }
    #region 플레이어 애니메이션 수정

    public override void UpdateAnim()
    {
        switch(CreatureState)
        {
            case Define.CreatureState.Idle :
            UpdateIdle();
            break;

            case Define.CreatureState.Moving :
            UpdateMoving();
            break;

            case Define.CreatureState.Dead :
            UpdateDead();
            break;

        }
    }
    protected override void UpdateIdle()
    {
        CreatureAnim.Play("Idle");
    }

    protected override void UpdateMoving()
    {
        CreatureAnim.Play("Moving");
    }

    protected override void UpdateDead()
    {
        CreatureAnim.Play("Dead");
    }
    #endregion
}
 