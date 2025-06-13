using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillProjectile : ProjectileController
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        CreatureController cc = collision.gameObject.GetComponent<CreatureController>();

        
        if (cc == null || !cc.IsValid() || !this.IsValid() || !cc.IsPlayer()) return;

        switch (skillType)
        {
            case Define.SkillType.BossSkill:
                HandleBossSkill();
                cc.OnDamaged(owner, skill);
                break;
        }
    }
}
