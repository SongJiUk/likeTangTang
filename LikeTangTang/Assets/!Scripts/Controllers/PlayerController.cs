using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : CretureController
{

    [SerializeField]
    Transform standard;
    [SerializeField]
    Transform firePos;

    Vector2 moveDir;

    public Vector2 MoveDir
    {
        get { return moveDir; }
        set { moveDir = value; }
    }

    public float GetEnvDist {get; set;} = 1f;


    public override bool Init()
    {
        if(base.Init() == false) return false;

        Speed = 10f;
        Manager.GameM.OnMovePlayerDir += HandleOnMoveDirChange;

        //StartFireProjectile();
        StartEgoSword();
        return true;
    }

    private void OnDestroy()
    {
        if(Manager.GameM != null)
            Manager.GameM.OnMovePlayerDir -= HandleOnMoveDirChange;
    }

    void HandleOnMoveDirChange(Vector2 _dir)
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


        //
        if(moveDir != Vector2.zero)
        {
            standard.eulerAngles = new Vector3(0,0, Mathf.Atan2(-dir.x, dir.y) * 180 / Mathf.PI);
        }

        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    void GetGem()
    {
        List<GemController> gems = Manager.ObjectM.gemSet.ToList();
        var FindGem = Manager.ObjectM.Grid.GetObjects(transform.position, GetEnvDist);
        
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

        
        // Debug.Log($"{FindGem.Count}  /  {gems.Count}");

    }

    //TODO : 스킬이 많아지면 따로 관리하기.
    #region FireProjectile
    Coroutine coFireProjectile;
    
    void StartFireProjectile()
    {
        if(coFireProjectile != null)
            StopCoroutine(coFireProjectile);

        coFireProjectile = StartCoroutine(CoStartFireProjectile());
    }

    IEnumerator CoStartFireProjectile()
    {
        while(true)
        {
            ProjectileController pc = Manager.ObjectM.Spawn<ProjectileController>(firePos.position, 1);
            pc.SetInfo(1, this, (firePos.position - standard.position).normalized);

            yield return new WaitForSeconds(0.5f);
        }
    }
    #endregion

    #region  EgoMelee
    EgoSwordController egoSword;
    void StartEgoSword()
    {
        if (egoSword.IsVaild()) return;

        egoSword = Manager.ObjectM.Spawn<EgoSwordController>(standard.position, 10);
        egoSword.transform.SetParent(standard);

        egoSword.ActivateSkill();
    }
    #endregion

    #region 장판
    #endregion
}
