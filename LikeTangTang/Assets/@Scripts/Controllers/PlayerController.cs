using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.ComponentModel.Design;
using Unity.VisualScripting;

public class PlayerController : CreatureController
{
    #region 플레이어 정보
    public override int DataID
    {
        get {return DataID;}
        set {DataID = value;}
    }

    public override float Hp 
    { 
        get {return Hp;}
        set {Hp = value;}
    }

    public override float MaxHp
    {
        get {return MaxHp;}
        set {MaxHp = value;}
    }

    public override float Attack { get {return Attack; } set {Attack = value;}}
    public override float AttackRate { get {return AttackRate;} set {AttackRate = value;} }
    public override float Def 
    { 
        get { return Def;}
        set {Def = value;}
    }
    public override float DefRate { get {return DefRate;} set {DefRate =value;}}
    public override float CriticalRate { get {return CriticalRate;} set {CriticalRate = value;}}
    public override float CriticalDamage { get {return CriticalDamage;} set {CriticalDamage = value;}}
    public override float DamageReduction { get {return DamageReduction;} set {DamageReduction = value;}}
    public override float SpeedRate { get {return SpeedRate;} set {SpeedRate = value;}}
    public override float Speed { get {return Speed;} set {Speed = value;} }

    public int Level
    {
        get {return Level;}
        set {Level = value;}
    }

    public float Exp
    {
        get {return Exp;}
        set
        {
            Exp = value;
            
            int level = Level;

            //TODO : 여기서 경험치 획득 및 레벨업  
        }
    }

    public void LevelUp()
    {
        //TODO : 여기서 레벨업하고, 
    }
#endregion
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
       
        Manager.GameM.OnMovePlayerDir += HandleOnMoveDirChange;

        AddSkill();
      
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
