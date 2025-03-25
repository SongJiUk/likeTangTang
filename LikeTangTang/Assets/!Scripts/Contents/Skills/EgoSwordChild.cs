using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgoSwordChild : MonoBehaviour
{
    BaseController owner;
    float damage;

    public void SetInfo(BaseController _owner, float _damage)
    {
        owner = _owner;
        damage = _damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mc = collision.transform.GetComponent<MonsterController>();

        if (mc.IsVaild() == false) return;

        mc.OnDamaged(owner, damage);

    }
}
