using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using Data;
using System;

public class PlayerController : CreatureController
{

    #region Action
    public Action OnPlayerDataUpdated;
    public Action OnPlayerLevelUp;
    #endregion
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

    public override float Attack 
    { 
        get {return Manager.GameM.ContinueDatas.Attack; } 
        set {Manager.GameM.ContinueDatas.Attack = value;}
    }
    public override float AttackRate 
    { 
        get {return Manager.GameM.ContinueDatas.AttackRate;} 
        set {Manager.GameM.ContinueDatas.AttackRate = value;} 
    }
    public override float Def 
    { 
        get { return Manager.GameM.ContinueDatas.Def;}
        set { Manager.GameM.ContinueDatas.Def= value;}
    }
    public override float DefRate 
    { 
        get {return Manager.GameM.ContinueDatas.DefRate;} 
        set { Manager.GameM.ContinueDatas.DefRate = value;}
    }
    public override float CriticalRate 
    { 
        get {return Manager.GameM.ContinueDatas.CriticalRate;} 
        set {Manager.GameM.ContinueDatas.CriticalRate = value;}
    }
    public override float CriticalDamage 
    { 
        get {return Manager.GameM.ContinueDatas.CriticalDamage;} 
        set {Manager.GameM.ContinueDatas.CriticalDamage = value;}
    }
    public override float DamageReduction 
    { 
        get {return Manager.GameM.ContinueDatas.DamageReduction;} 
        set {Manager.GameM.ContinueDatas.DamageReduction = value;}
    }
    public override float SpeedRate 
    { 
        get {return Manager.GameM.ContinueDatas.MoveSpeedRate;} 
        set {Manager.GameM.ContinueDatas.MoveSpeedRate= value;}
    }
    public override float Speed 
    { 
        get {return Manager.GameM.ContinueDatas.MoveSpeed;} 
        set {Manager.GameM.ContinueDatas.MoveSpeed = value;} 
    }

    public int Level
    {
        get {return Manager.GameM.ContinueDatas.Level;}
        set {Manager.GameM.ContinueDatas.Level = value;}
    }

    public float TotalExp
    {
        get {return Manager.GameM.ContinueDatas.TotalExp;}
        set {Manager.GameM.ContinueDatas.TotalExp = value;}
    }
    public float Exp
    {
        get {return Manager.GameM.ContinueDatas.Exp;}
        set
        {
            Manager.GameM.ContinueDatas.Exp = value;
            
            int level = Level;

            //TODO : 여기서 경험치 획득 및 레벨업
            while(true)
            {
                if(Manager.DataM.LevelDic.TryGetValue(level + 1, out LevelData nextlevel) == false) break;
                if(Manager.DataM.LevelDic.TryGetValue(level, out LevelData currentLevel) == false) break;
                
                if(Manager.GameM.ContinueDatas.Exp < currentLevel.TotalExp) break;
                else level++;
            }

            if(level != Level)
            {
                Level =level;
                if(Manager.DataM.LevelDic.TryGetValue(Level, out LevelData currentLevel))
                {
                    TotalExp = currentLevel.TotalExp;
                    LevelUp();
                }
            }
        }
    }


    public float ExpRatio
    {
        get 
        {
            LevelData currentLevelData;
            if(Manager.DataM.LevelDic.TryGetValue(Level, out currentLevelData))
            {
                float prevLevelExp = 0;
                LevelData prevLevelData;

                if(Manager.DataM.LevelDic.TryGetValue(Level -1, out prevLevelData))
                {
                    prevLevelExp = prevLevelData.TotalExp;
                }
                float currentLevelExp = currentLevelData.TotalExp;

                return (Exp - prevLevelExp) / (currentLevelExp - prevLevelExp);
            }

            return 0f;
        }
    }

    public int KillCount
    {
        get {return Manager.GameM.ContinueDatas.KillCount;}
        set
        {
            Manager.GameM.ContinueDatas.KillCount = value;
            OnPlayerDataUpdated?.Invoke();
        }
    }

    public float ExpBounsRate
    {
        get { return Manager.GameM.ContinueDatas.ExpBonusRate;}
        set { Manager.GameM.ContinueDatas.ExpBonusRate = value;}
    }



    public void LevelUp(int _level = 0)
    {
        //TODO : 여기서 레벨업하고, 
        if(_level > 1) 
            OnPlayerLevelUp?.Invoke();
        
        //[ ] 스킬 업그레이드
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


    //NOTE : 여기임 여기 !! 여기서 스킬 추가하든 없애든 해야됌.
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

        Debug.Log(dir);
        Debug.Log(Speed);
    }

    void HandleOnMoveDirChange(Vector2 _dir)
    {
        moveDir = _dir;
    }

    public void UpdatePlayerDir()
    {
        if(moveDir.x < 0) CreatureSprite.flipX = false;
        else CreatureSprite.flipX = true;
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
                    Manager.ObjectM.DeSpawn(gem);
                }
            }

        
            // Debug.Log($"{FindGem.Count}  /  {gems.Count}");

        }

    #endregion
    public override bool Init()
    {
        base.Init();
       
        Manager.GameM.OnMovePlayerDir += HandleOnMoveDirChange;

        //AddSkill();
      
        return true;
    }

    private void OnDestroy()
    {
        if(Manager.GameM != null)
            Manager.GameM.OnMovePlayerDir -= HandleOnMoveDirChange;
    }

    public override void OnDamaged(BaseController _attacker, SkillBase _skill, float _damage = 0)
    {
        base.OnDamaged(_attacker);
        MonsterController mc = _attacker as MonsterController;

        //TODO : 몸박 삭제하기
        mc?.OnDamaged(this);
    }

    private void FixedUpdate()
    {
        UpdatePlayerDir();
        Move();
        GetGem();
    }
}
