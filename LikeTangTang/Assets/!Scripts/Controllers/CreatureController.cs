using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreatureController : BaseController
{

    public int MaxHP { get; set; }
    public float Hp { get; set; }
    public float Speed { get; set; }

    public float Damage { get; set; }

    public SkillComponent Skills {get; protected set;}

    public override bool Init()
    {
        base.Init();
        Skills = gameObject.GetOrAddComponent<SkillComponent>();

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
}
