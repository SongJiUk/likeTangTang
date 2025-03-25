using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterController : CretureController
{
    [SerializeField]
    float _hp;
    public override bool Init()
    {
        base.Init();

        objType = Define.ObjectType.Monster;
        Speed = 1f;
        return true;
    }

    void FixedUpdate()
    {
        PlayerController pc = Manager.ObjectM.Player;
        if (pc == null) return;

        Vector3 dir = pc.transform.position - transform.position;
        Vector3 newPos = transform.position + dir.normalized * Time.deltaTime * Speed;

        //transform.position += newPos;
        GetComponent<Rigidbody2D>().MovePosition(newPos);
        GetComponent<SpriteRenderer>().flipX = dir.x > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController target =  collision.gameObject.GetComponent<PlayerController>();

        if (target == null) return;

        if (coDotDamage == null)
            StartCoroutine(CoStartDotDamage(target));
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        PlayerController target = collision.gameObject.GetComponent<PlayerController>();
        if (target == null) return;

        if (coDotDamage != null) 
            StopCoroutine(coDotDamage);

        coDotDamage = null;
    }

    Coroutine coDotDamage;
    public IEnumerator CoStartDotDamage(PlayerController _target)
    {
        while (true)
        {
            // 피해자에서 처리하는게 좋음
            _target.OnDamaged(this, Damage);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override void OnDamaged(BaseController _attacker, float _damage)
    {
        base.OnDamaged(_attacker, _damage);
    }

    public override void OnDead()
    {
        base.OnDead();

        GemController gc = Manager.ObjectM.Spawn<GemController>(this.transform.position);

        Manager.ObjectM.DeSpawn(this);

        if (coDotDamage != null) StopCoroutine(coDotDamage);


    }
}
