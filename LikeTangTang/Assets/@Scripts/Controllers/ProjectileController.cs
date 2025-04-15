using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Data;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : SkillBase
{
    
    Vector3 spawnPos;
    Vector3 targetPos;
    Vector3 dir;
    Rigidbody2D rigid;
    Define.SkillType skillType;
    SkillBase skill;
    [SerializeField]
    public float rotationOffset;
  
    float sclaeMul;
    float lifeTime;
    public ProjectileController() : base(Define.SkillType.None){}

    //초기화, 세팅
    public override bool Init()
    {
        base.Init();
        return true;
    }
    HashSet<MonsterController> sharedTarget = new();
    public void SetInfo(CreatureController _owner, Vector3 _pos, Vector3 _dir, Vector3 _targetPos, SkillBase _skill, HashSet<MonsterController> _sharedTarget = null)
    {

        owner = _owner;
        spawnPos = _pos;
        dir = _dir;
        targetPos = _targetPos;
        skill = _skill;
        rigid = GetComponent<Rigidbody2D>();
        duration = skill.SkillDatas.Duration;
        numBounce = skill.SkillDatas.NumBounce;
        bounceSpeed = skill.SkillDatas.BounceSpeed;
        speed = skill.SkillDatas.Speed;
        sclaeMul = skill.SkillDatas.ScaleMultiplier;
        skillType = skill.Skilltype;
        numPenerations = skill.SkillDatas.NumPenerations;
        if(_sharedTarget != null) sharedTarget = _sharedTarget;

        transform.localScale = Vector3.one * skill.SkillDatas.ScaleMultiplier;
        switch(skillType)
        {
            case Define.SkillType.PlasmaSpinner :
                rigid.velocity = dir * speed;
                break;

            case Define.SkillType.PlasmaShot :
                rigid.velocity = dir * speed;
                break;

            case Define.SkillType.ElectricShock :
                MonsterController target = Utils.FindClosestMonster(targetPos);
                if(target != null)
                    StartCoroutine(CoElectricShock(spawnPos, targetPos, target));
                break;
        }


        if(gameObject.activeInHierarchy)
            StartCoroutine(CoDestroy(duration));
    }
    


    public override void UpdateController()
    {
        base.UpdateController();

        //transform.position += dir * speed * Time.deltaTime;
    }
    
    void HandlePlasmaSpinner(CreatureController _cc)
    {

        numBounce--;
        rigid.velocity = -rigid.velocity.normalized * bounceSpeed;
        if(numBounce < 0)
        {
            rigid.velocity = Vector2.zero;
            StartDestory();
        }
    }

    void HandlePlasmaShot(CreatureController _cc)
    {  
        numPenerations--;
        if(numPenerations < 0)
        {
            rigid.velocity = Vector2.zero;
            StartDestory();
        }
    }
    
    
    IEnumerator CoElectricShock(Vector3 _startPos, Vector3 _endPos, MonsterController _target)
    {


        MonsterController currentTarget = _target;
        Vector3 currentPos = _startPos;

        for(int i =0; i< numBounce; i++)
        {
            if(currentTarget == null || !currentTarget.IsValid()) break;

            sharedTarget.Add(currentTarget);
            
            Vector3 nextPos = currentTarget.transform.position;
            HandleElectricShock(currentPos, nextPos, currentTarget);
            
            yield return new WaitForSeconds(0.1f);

            currentPos = nextPos;
            currentTarget = Utils.FindClosestMonster(currentPos, sharedTarget);
        }

        StartDestory();
    }
   
    void HandleElectricShock(Vector3 _startPos, Vector3 _endPos, MonsterController _target)
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        particle.Clear();
        var main = particle.main;
        main.startRotation = 0f;
        // Scale
        transform.position = _startPos;
        float dist = Vector3.Distance(_startPos, _endPos);
        main.startSizeX = dist;
        main.startSizeY = 8;
        
        // rotatate
        Vector3 dir = (_endPos - _startPos).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x);
        main.startRotation = angle * -1f;
        
        particle.Play();

        if(_target != null && _target.IsValid())
        {
            _target.OnDamaged(owner,skill);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        CreatureController cc = collision.gameObject.GetComponent<MonsterController>();
        if(cc == null) return;
        if(cc.IsValid() == false) return;
        if(this.IsValid()==false) return;


        //NOTE : 이렇게되면, 보스가 쏘는 투사체는 여기 통과를 못함(만들떄 생각해보고 수정하기)
        if(cc.objType != Define.ObjectType.Monster) return;

        switch(skillType)
        {
            case Define.SkillType.PlasmaSpinner :
                HandlePlasmaSpinner(cc);
                break;

            case Define.SkillType.PlasmaShot :
                HandlePlasmaShot(cc);
                break;

        }

        cc.OnDamaged(owner, skill);
        
    }

}
