using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CretureController : BaseController
{

    public int MaxHP { get; set; }
    public float Hp { get; set; }

    public virtual void OnDamaged(BaseController _attacker, float _damage)
    {
        Hp -= _damage;
        if (Hp <= 0)
        {
            Hp = 0;
            IsDead();
        }

    }

    public void IsDead()
    {

    }
}
