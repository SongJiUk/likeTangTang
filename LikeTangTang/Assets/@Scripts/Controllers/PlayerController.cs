using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.ComponentModel.Design;
using Unity.VisualScripting;

public class PlayerController : CreatureController
{

    int level;

#region  스킬 정보

    
    [SerializeField]
    Transform standard;
    [SerializeField]
    Transform firePos;

    public Transform Standard {get { return standard;}}
    public Vector3 FirePos {get { return firePos.transform.position;}}
    public Vector3 ShootDir {get {return (firePos.position - standard.position).normalized;}}


    public void AddSkill()
    {
        Debug.Log("PlayerController AddSkill");
        Skills.AddSkill<EgoSword>(standard.position, gameObject.transform);
        Skills.AddSkill<FireBall>(transform.position, gameObject.transform);

    }

    #endregion

#region 이동관련
    Vector2 moveDir;

    public Vector2 MoveDir
    {
        get { return moveDir; }
        set { moveDir = value; }
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

    void HandleOnMoveDirChange(Vector2 _dir)
    {
        moveDir = _dir;
    }

#endregion

#region 젬 관련
    public float GetEnvDist {get; set;} = 1f;

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

#endregion

    public override bool Init()
    {
        if(base.Init() == false) return false;
        Debug.Log("Init");
        Speed = 10f;
        Manager.GameM.OnMovePlayerDir += HandleOnMoveDirChange;

        //TODO : 스킬 관리는 누가? - 스킬 컴포넌트를 하나 만들어서 사용
        AddSkill();
        level = 1;
        if(Manager.DataM.PlayerDic.TryGetValue(level, out var data) != false)
        {
            Debug.Log($"Damage : {data.attack}, MaxHP : {data.maxHp}, Exp : {data.totalExp}");
            
        }
        return true;
    }

    private void OnDestroy()
    {
        if(Manager.GameM != null)
            Manager.GameM.OnMovePlayerDir -= HandleOnMoveDirChange;
    }

    public override void OnDamaged(BaseController _attacker, float _damage)
    {
        base.OnDamaged(_attacker, _damage);
        MonsterController mc = _attacker as MonsterController;

        //TODO : 몸박 삭제하기
        mc?.OnDamaged(this, 10);
    }

    private void FixedUpdate()
    {
        Move();
        GetGem();
    }
}
