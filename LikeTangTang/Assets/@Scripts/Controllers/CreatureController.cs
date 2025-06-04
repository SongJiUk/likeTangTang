using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using static Define;

public class CreatureController : BaseController, ITickable
{
    #region State Pattern

    private CreatureState creatureState = CreatureState.Moving;
    public virtual CreatureState CreatureState
    {
        get => creatureState;
        set
        {
            creatureState = value;
            UpdateAnim();
        }
    }

    public virtual void UpdateAnim() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateAttack() { }
    protected virtual void UpdateDead() { }

    #endregion

    #region Info

    protected SpriteRenderer CreatureSprite;
    protected Animator CreatureAnim;
    public CreatureData creatureData;
    public Rigidbody2D Rigid { get; private set; }

    // Stats
    public virtual int DataID { get; set; }
    public virtual float Hp { get; set; }
    public virtual float MaxHp { get; set; }
    public virtual float MaxHpRate { get; set; }
    public virtual float HealBounsRate { get; set; }
    public virtual float HpRegen { get; set; }
    public virtual float Attack { get; set; }
    public virtual float AttackRate { get; set; } = 1f;
    public virtual float Def { get; set; }
    public virtual float DefRate { get; set; } = 1f;
    public virtual float CriticalRate { get; set; }
    public virtual float CriticalDamage { get; set; } = 1.5f;
    public virtual float DamageReduction { get; set; }
    public virtual float SpeedRate { get; set; } = 1f;
    public virtual float Speed { get; set; }

    public virtual float CollectDistBonus { get; set; }

    protected bool isDead = false;

    // 데미지 애니메이션
    protected bool isStartDamageAnim = false;

    public Material DefaultMat;
    public Material HitEffectmat;
    protected float damageAnimEndTime = -1f;

    public SkillComponent Skills { get; protected set; }


    #endregion

    #region Init
    public override bool Init()
    {
        if (!base.Init()) return false;

        Rigid = GetComponent<Rigidbody2D>();

        CreatureSprite = Utils.FindChild<SpriteRenderer>(gameObject, recursive: true);
        CreatureAnim = GetComponent<Animator>();
        if (CreatureAnim == null)
            CreatureAnim = Utils.FindChild<Animator>(gameObject, recursive: true);

        return true;
    }

    public virtual void Healing(float _healAmount, bool _isEffect = true) { }

    public virtual void SetInfo(int _dataID)
    {


        DataID = _dataID;
        creatureData = Manager.DataM.CreatureDic[_dataID];
        Init();
        InitStat();

        isStartDamageAnim = false;
        if (DefaultMat != null) CreatureSprite.material = DefaultMat;
        CreatureSprite.sprite = Manager.ResourceM.Load<Sprite>(creatureData.Image_Name);
        Rigid.simulated = true;
    }

    public virtual void InitStat(bool _isHpFull = true)
    {
        //Monster, EliteMonster전용
        var waveRate = Manager.GameM.CurrentWaveData.HpIncreaseRate;
        var stageLevel = Manager.GameM.CurrentStageData.StageLevel;

        MaxHp = (creatureData.MaxHp + creatureData.MaxHpUpForIncreasStage * stageLevel) *
                (creatureData.HpRate + waveRate);

        Attack = (creatureData.Attack + creatureData.AttackUpForIncreasStage * stageLevel) *
                 creatureData.AttackRate;

        Hp = MaxHp;
        Speed = creatureData.Speed * creatureData.MoveSpeedRate;
    }


    public void LoadSkil()
    {
        foreach(KeyValuePair<SkillType, int> pair in Manager.GameM.ContinueDatas.SavedBattleSkill.ToList())
        {
            Skills.LoadSkill(pair.Key, pair.Value);
        }

        foreach (KeyValuePair<SkillType, int> pair in Manager.GameM.ContinueDatas.SavedEvolutionSkill.ToList())
        {
            Skills.TryEvolveSkill(pair.Value);
        }

        foreach (Data.SpecialSkillData specialSkill in Manager.GameM.ContinueDatas.SavedSpecialSkill.ToList())
        {
            Skills.AddSpecialSkill(specialSkill, true);
        }
    }
    public virtual void InitSkill()
    {
        //TODO : 여기에 스킬 추가 

        foreach (int skillID in creatureData.SkillTypeList)
        {
            if (Manager.DataM.SkillDic.TryGetValue(skillID, out var data))
            {
                if (data.SkillType != SkillType.None)
                    Skills.AddSkill(data.SkillType, skillID);
            }
        }
    }

    #endregion

    public virtual void UpdatePlayerStat() { }

    #region 데미지 & 죽음

    public virtual void OnDamaged(BaseController _attacker, SkillBase _skill = null, float _damage = 0)
    {
        bool isCritical = false;

        PlayerController player = _attacker as PlayerController;

        if (player != null)
        {
            bool isSpectralSlashEvolution =
                _skill != null &&
                _skill.Skilltype == SkillType.SpectralSlash &&
                _skill.SkillLevel == 6;

            if (isSpectralSlashEvolution)
            {
                _damage *= player.CriticalDamage;
                isCritical = true;
            }
            else if (Random.value <= player.CriticalRate)
            {
                _damage *= player.CriticalDamage;
                isCritical = true;
            }
        }

        if (_skill != null)
            _skill.TotalDamage += _damage;

        Hp -= _damage;

        Manager.ObjectM.ShowFont(transform.position, _damage, 0, transform, isCritical);

        if (this.IsValid() || gameObject.IsValid())
            StartCoroutine(CoPlayDamageAnim());
    }

    public virtual void OnDead()
    {
        CreatureState = CreatureState.Dead;
        isStartDamageAnim = false;
        StopAllCoroutines();
        if (Rigid != null)
            Rigid.simulated = false;
    }

    public virtual void Tick(float _deltaTime)
    {
        //if (!isDead && Time.time >= damageAnimEndTime && Hp <= 0)
        //{
        //    Hp = 0;
        //    isDead = true;
        //    transform.localScale = Vector3.one;

        //    switch (objType)
        //    {
        //        case ObjectType.Player:
        //        case ObjectType.Monster:
        //        case ObjectType.EliteMonster:
        //        case ObjectType.Boss:
        //            OnDead();
        //            break;
        //    }
        //}
    }

    #endregion

    #region 기타

    public bool IsMonster()
    {
        return objType == ObjectType.Monster ||
               objType == ObjectType.EliteMonster ||
               objType == ObjectType.Boss;
    }

    #endregion


    IEnumerator CoPlayDamageAnim()
    {
        if (!isStartDamageAnim)
        {
            isStartDamageAnim = true;
            DefaultMat = Manager.ResourceM.Load<Material>("CreatureDefaultMat");
            HitEffectmat = Manager.ResourceM.Load<Material>("DamagedEffectMat");

            CreatureSprite.material = HitEffectmat;
            yield return new WaitForSeconds(0.1f);
            CreatureSprite.material = DefaultMat;

            if (Hp <= 0)
            {
                Hp = 0;
                transform.localScale = Vector3.one;
                switch (objType)
                {
                    case ObjectType.Player:
                        SpecialSkillData resurrection = Skills.SpecialSkills.FirstOrDefault(x => x.SpecialSkillName == SpecialSkillName.Resurrection);

                        if (resurrection == null) OnDead();
                        else
                        {
                            Resurrection(resurrection.HealingBouns, resurrection.MoveSpeedBonus, resurrection.AttackBonus);
                            Skills.SpecialSkills.Remove(resurrection);
                            //Skills.OnSkillBookChanged();
                        }
                        break;

                    default:
                        OnDead();
                        break;
                }

            }
            isStartDamageAnim = false;

        }
    }

    public void Resurrection(float _healRate, float _moveSpeed = 0, float _attackRate = 0)
    {
        Healing(_healRate, false);
        Manager.ResourceM.Instantiate("Revival", transform);
        SpeedRate += _moveSpeed;
        AttackRate += _attackRate;
        UpdatePlayerStat();
        Manager.ObjectM.KillAllMonsters();
    }

   
}