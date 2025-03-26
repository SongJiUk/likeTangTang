using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterController : CreatureController
{


    #region State Pattern
    Define.CreatureState creatureState = Define.CreatureState.Moving;
    protected Animator animator;
    public virtual Define.CreatureState CreatureState
    {
        get { return creatureState; }
        set 
        {
            creatureState = value;
            UpdateAnim();
        }
    }

    public virtual void UpdateAnim() {}
    
    public override void UpdateController()
    {
        base.UpdateController();

        switch(creatureState)
        {
            case Define.CreatureState.Moving:
            UpdateMoving();
                break;

            case Define.CreatureState.Attack :
            UpdateAttack();
                break;

            case Define.CreatureState.Dead :
            UpdateDead();
                break;
        }
    }

    protected virtual void UpdateMoving() {}

    protected virtual void UpdateAttack() {}

    protected virtual void UpdateDead() {}
    #endregion


    public override bool Init()
    {
        base.Init();

        objType = Define.ObjectType.Monster;
        Speed = 1f;
        animator = GetComponent<Animator>();
        creatureState = Define.CreatureState.Moving;
        return true;
    }

    void FixedUpdate()
    {

        if(creatureState != Define.CreatureState.Moving) return;

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
        if (coDotDamage != null) StopCoroutine(coDotDamage);

        Manager.ObjectM.Spawn<GemController>(this.transform.position);

        Manager.ObjectM.DeSpawn(this);
        Manager.GameM.KillCount++;
        
        
    }
}
