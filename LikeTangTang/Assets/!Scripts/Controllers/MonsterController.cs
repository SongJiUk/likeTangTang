using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterController : CretureController
{

    public override bool Init()
    {
       
        //TODO 초기화
        objType = Define.ObjectType.Monster;
        Speed = 1f;
        return true;
    }

    void FixedUpdate()
    {
        //TODO 플레이어를 찾아서 움직이게
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
        //TODO 충돌시 데미지
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
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

        if (coDotDamage != null) StopCoroutine(coDotDamage);


    }
}
