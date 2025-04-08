using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreatureController : BaseController
{

    #region Info
    protected SpriteRenderer CreatureSprite;
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

    void Awake()
    {
        Init();
    }
    public override bool Init()
    {
        base.Init();
        Skills = gameObject.GetOrAddComponent<SkillComponent>();

        CreatureSprite = GetComponent<SpriteRenderer>();
        if (CreatureSprite == null)
            CreatureSprite = Utils.FindChild<SpriteRenderer>(gameObject);
        
        return true;
    }
    public virtual void OnDamaged(BaseController _attacker, float _damage)
    {
        Hp -= _damage;
        if (Hp <= 0)
        {
            Hp = 0;
            OnDead();
        }

    }

    public virtual void OnDead()
    {
        
    }

    public void SetInfo(int _dataID)
    {
        //TODO : 여기서 초기화 해주기.
    }
}
