using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Threading;

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
    
    public float EffectScaleMultiplier;
    public float SlowRatio;
    public float pullForce;
    float sclaeMul;
    float lifeTime;
    public ProjectileController() : base(Define.SkillType.None){}
    private ParticleSystem electricEffect;
    //초기화, 세팅
    public override bool Init()
    {
        base.Init();
        electricEffect = GetComponent<ParticleSystem>();
        return true;
    }
    
    MonsterController target;
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
        range = skill.SkillDatas.Range;
        effectRange = skill.SkillDatas.EffectRange;
        EffectScaleMultiplier = skill.SkillDatas.EffectScaleMultiplier;
        SlowRatio = skill.SkillDatas.SlowRatio;
        pullForce = skill.SkillDatas.PullForce;


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
                target = Utils.FindClosestMonster(targetPos);
                if(target != null)
                    StartCoroutine(CoElectricShock(spawnPos, targetPos, target));
                break;
            case Define.SkillType.SuicideDrone :
                //TODO : Range안에 있는 적들을 찾아서 랜덤으로 돌격후 자살

                StartCoroutine(CoStartSuicideDrone());

                break;

                case Define.SkillType.TimeStopBomb :
                //TODO : 설정
                rigid.velocity = dir * speed;
                StartCoroutine(CoExplosionTimeStopBomb());

                break;

                case Define.SkillType.GravityBomb:
                rigid.velocity = dir * speed;
                StartCoroutine(CoExplosionGravityBomb());
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
    
  #region  GravityBomb
    IEnumerator CoExplosionGravityBomb()
    {
        float timeOut =2f;
        float timer =0f;
        while(Vector3.Distance(targetPos, transform.position) > 0.1f)
        {
            if(timer > timeOut) break;
            timer += Time.deltaTime;

            yield return null;
        }
        ExplosionGravityBomb();
        StartDestory();
    }

    void ExplosionGravityBomb()
    {
        string explosionName = skill.SkillDatas.ExplosionName;
        GameObject go = Manager.ResourceM.Instantiate(explosionName, _pooling : true);
        go.transform.position = transform.position;
        go.GetComponent<GravityBombZone>().SetInfo(owner, skill);
    }
  #endregion
   
    #region TimeStopBomb
    IEnumerator CoExplosionTimeStopBomb()
    {
        float timeOut = 2f;
        float timer = 0f;
        while(Vector3.Distance(targetPos, transform.position) > 0.1f)
        {
            if(timer > timeOut) break;
            timer += Time.deltaTime;
            yield return null;
        }
        ExplosionTimeStopBomb();
        StartDestory();
    }

    void ExplosionTimeStopBomb()
    {
        
        //TODO : effectRange, SlowRatio, EffectScaleMultiplie사용, 
        // 폭탄이 터지면, 디버프를 생성(근데 새로운 이펙트 생성)
        string explosionName = skill.SkillDatas.ExplosionName;
        GameObject go = Manager.ResourceM.Instantiate(explosionName, _pooling : true);
        go.transform.position = transform.position;
        go.GetComponent<TimeStopBombZone>().SetInfo(owner, skill);
        
    }
    #endregion

    #region SuicideDrone
    bool isBoom = false;
    IEnumerator CoStartSuicideDrone()
    {
        yield return new WaitForSeconds(1f);

        if(!this.IsValid()) yield break;

        target = Utils.FindClosestMonster(transform.position);
        if(target != null && target.IsValid())
        {
            dir = (target.transform.position - transform.position).normalized;
            rigid.velocity = dir * speed;

            yield return new WaitForSeconds(1f);
            if(!this.IsValid()) yield break;
            if(!target.IsValid()) target = null;
        }
        //없으면 제자리 폭발.
        isBoom = true;
        HandleSuicideDrone();
        
    }

    void HandleSuicideDrone()
    {
        //TODO : 폭발 하면서 주변 몬스터 함께 데미지
        if(!isBoom) return;

        ExplosionDrone();
        StartDestory();
        isBoom = false;
    }

    void ExplosionDrone()
    {   
        //TODO : 자폭할때는 굳이 필요할까 굳이 raycast를 안해줘도 될거같음.
        string explosionName = skill.SkillDatas.ExplosionName;
        GameObject go = Manager.ResourceM.Instantiate(explosionName, _pooling : true);
        go.transform.position = transform.position;

        if(target == null) return;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, effectRange, Vector2.zero);
        foreach(var target in hits)
        {
            CreatureController cc = target.collider.GetComponent<MonsterController>();
            if(cc?.IsMonster() == true && cc.IsValid())
                cc.OnDamaged(owner, skill);
        }
    }

    #endregion
    
    #region  ElectricShock
    IEnumerator CoElectricShock(Vector3 _startPos, Vector3 _endPos, MonsterController _target)
    {

        HashSet<MonsterController> sharedTarget = new();
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
        if(electricEffect == null) electricEffect = GetComponent<ParticleSystem>();

        electricEffect.Clear();
        var main = electricEffect.main;

        // Scale
        transform.position = _startPos;
        float dist = Vector3.Distance(_startPos, _endPos);
        main.startSizeX = dist;
        main.startSizeY = 8;
        
        // rotatate
        Vector3 dir = (_endPos - _startPos).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x);
        main.startRotation = angle * -1f;
        
        electricEffect.Play();

        if(_target != null && _target.IsValid())
        {
            _target.OnDamaged(owner,skill);
        }
    }

    #endregion

    void OnTriggerEnter2D(Collider2D collision)
    {
        CreatureController cc = collision.gameObject.GetComponent<MonsterController>();
        if(cc == null || !cc.IsValid() || !this.IsValid() || !cc.IsMonster()) return;

        //NOTE : 이렇게되면, 보스가 쏘는 투사체는 여기 통과를 못함(만들떄 생각해보고 수정하기)
        //if(!cc.IsMonster())return;

        switch(skillType)
        {
            case Define.SkillType.PlasmaSpinner :
                HandlePlasmaSpinner(cc);
                cc.OnDamaged(owner, skill);
                break;

            case Define.SkillType.PlasmaShot :
                HandlePlasmaShot(cc);
                cc.OnDamaged(owner, skill);
                break;
            case Define.SkillType.SuicideDrone :
                HandleSuicideDrone();
                break;
        }

        
        
    }

}
