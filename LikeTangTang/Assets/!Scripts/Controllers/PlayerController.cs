using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : CretureController
{

    Vector2 moveDir;

    public Vector2 MoveDir
    {
        get { return moveDir; }
        set { moveDir = value; }
    }

    public float GetEnvDist {get; set;} = 1f;


    private void Start()
    {
        Speed = 2f;
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

    private void FixedUpdate()
    {
        Move();
        GetGem();
    }

    void Move()
    {
        Vector3 dir = moveDir * Speed * Time.deltaTime;
        transform.position += dir;
    }

    void GetGem()
    {
        List<GemController> gems = Manager.ObjectM.gemSet.ToList();

        var sqrtDist = GetEnvDist * GetEnvDist;
        foreach (var gem in gems)
        {
            Vector3 dir = gem.transform.position - transform.position;

            if(dir.sqrMagnitude <= sqrtDist)
            {
                Manager.GameM.Gem += 1;
                Manager.ObjectM.DeSpawn(gem);
            }
        }

        // var FindGem = Manager.ObjectM.Grid.GetObjects(transform.position, GetEnvDist);
        // Debug.Log($"{FindGem.Count}  /  {gems.Count}");

    }
}
