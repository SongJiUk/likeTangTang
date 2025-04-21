using System.Collections;
using System.Collections.Generic;
using Data;
using Unity.VisualScripting;
using UnityEngine;

public class CreatureController : BaseController
{

    #region Info
    protected SpriteRenderer CreatureSprite;
    Data.CreatureData creatureData;
    public Rigidbody2D Rigid { get; set; }
    

    protected bool isStartDamageAnim = false;
    public virtual int DataID {get ;set;}
    public virtual float Hp {get; set;}
    public virtual float MaxHp {get; set;}
    public virtual float Attack {get; set;}
    public virtual float AttackRate {get; set;} = 1;
    public virtual float Def {get; set;}
    public virtual float DefRate {get; set;} = 1;
    public virtual float CriticalRate {get; set;}
    public virtual float CriticalDamage {get; set;} = 1.5f;

    public virtual float DamageReduction {get; set;}
    public virtual float SpeedRate {get; set;} = 1;
    public virtual float Speed {get ;set;}
    #endregion
    public SkillComponent Skills {get; protected set;}
    public override bool Init()
    {
        if (!base.Init()) return false;
        Skills = gameObject.GetOrAddComponent<SkillComponent>();
        Rigid = GetComponent<Rigidbody2D>();

        CreatureSprite = GetComponent<SpriteRenderer>();
        if (CreatureSprite == null)
            CreatureSprite = Utils.FindChild<SpriteRenderer>(gameObject);
        
        return true;
    }
    public virtual void OnDamaged(BaseController _attacker, SkillBase _skill = null, float _damage = 0)
    {
        bool isCritical = false;


        PlayerController player = _attacker as PlayerController;
        if(player != null)
        {
            bool isSpectralSlashEvolution = _skill.Skilltype == Define.SkillType.SpectralSlash && _skill.SkillLevel == 6;
            if (isSpectralSlashEvolution)
            {
                _damage = _damage * player.CriticalDamage;
                isCritical = true;
            }
            else if(Random.value <= player.CriticalRate)
            {
                _damage = _damage * player.CriticalDamage;
                isCritical = true;
            }
        }

        if (_skill)
            _skill.TotalDamage += _damage;

        Hp -= _damage;
        //Manager.ObjectM.ShowFont(transform.position,_damage, 0, transform, isCritical);
        
        if (this.IsValid()) 
            StartCoroutine(StartDamageAnim());
;    }
    IEnumerator StartDamageAnim()
    {
        if(!isStartDamageAnim)
        {
            //데미지 피격
            isStartDamageAnim = true;
            yield return new WaitForSeconds(0.1f);

            if(Hp <= 0)
            {
                Hp = 0;
                transform.localScale = Vector3.one;

                switch(objType)
                {
                    case Define.ObjectType.Player:
                        //TODO : 부활? OR 다이

                        OnDead();
                        break;

                    case Define.ObjectType.Monster:
                        OnDead();
                        break;
                }
            }
            isStartDamageAnim = false;
        }
    }
    public virtual void InitStat()
    {
        var waveRate = Manager.GameM.CurrentWaveData.HpIncreaseRate;
        var stageLevel = Manager.GameM.CurretnStageData.StageLevel;

        MaxHp = (creatureData.MaxHp + (creatureData.MaxHpUpForIncreasStage * stageLevel)) * (creatureData.HpRate + waveRate);
        Attack = (creatureData.Attack + (creatureData.AttackUpForIncreasStage * stageLevel)) * creatureData.AttackRate;
        Hp = MaxHp;
        Speed = creatureData.Speed * creatureData.MoveSpeedRate;
    }

    public virtual void OnDead()
    {
        this.GetComponent<Rigidbody2D>().simulated = false;
        transform.localScale = Vector3.one;
    }

    
    public void SetInfo(int _dataID)
    {
        Init();
        Rigid.simulated = true;
        
        DataID = _dataID;
        creatureData = Manager.DataM.CreatureDic[_dataID];
        InitStat();
        CreatureSprite.sprite = Manager.ResourceM.Load<Sprite>(creatureData.Image_Name);
        InitSkill();
        
    }


    public virtual void InitSkill()
    {
        foreach(int skillID in creatureData.SkillTypeList)
        {
            Define.SkillType type = Utils.GetSkillTypeFromInt(skillID);
            if(type != Define.SkillType.None) Skills.AddSkill(type, skillID);
        }
    }

    public bool IsMonster()
    {
        switch(objType)
        {
            case Define.ObjectType.Monster:
                return true;
            default :
                return false;
        }
    }
   
}
