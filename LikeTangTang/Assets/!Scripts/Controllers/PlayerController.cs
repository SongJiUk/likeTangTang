using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CretureController
{

    Vector2 moveDir;
    
    public Vector2 MoveDir
    {
        get { return moveDir; }
        set { moveDir = value; }
    }



    private void Start()
    {
        Speed = 1f;
        Manager.GameM.OnMovePlayerDir += OnMoveDirChange;
    }

    private void OnDestroy()
    {
        if(Manager.GameM != null)
            Manager.GameM.OnMovePlayerDir -= OnMoveDirChange;
    }

    void OnMoveDirChange(Vector2 _dir)
    {
        moveDir = _dir;
    }

    public override void OnDamaged(BaseController _attacker, float _damage)
    {
        base.OnDamaged(_attacker, _damage);
        MonsterController mc = _attacker as MonsterController;

        mc?.OnDamaged(this, 10000);
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 dir = moveDir * Speed * Time.deltaTime;
        transform.position += dir;
    }
}
