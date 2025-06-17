using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using System;

public class ProjectileController : SkillBase
{
    
    Vector3 spawnPos;
    Vector3 targetPos;
    Vector3 dir;
    Rigidbody2D rigid;
    public Define.SkillType skillType;
    public SkillBase skill;
    [SerializeField]
    public float rotationOffset;
    
    public float EffectScaleMultiplier;
    public float SlowRatio;
    public float pullForce;
    float sclaeMul;
    float lifeTime;
    public ProjectileController() : base(Define.SkillType.None){}

    Transform particleT;
    
    private ParticleSystem electricEffect;
    //초기화, 세팅

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public override bool Init()
    {
        base.Init();
        electricEffect = GetComponent<ParticleSystem>();
        return true;
    }
    
    MonsterController target;
    public virtual void SetInfo(CreatureController _owner, Vector3 _pos, Vector3 _dir, Vector3 _targetPos, SkillBase _skill, HashSet<MonsterController> _sharedTarget = null)
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
        float angle;
        switch (skillType)
        {
            case Define.SkillType.PlasmaSpinner:
                rigid.velocity = dir * speed;
                particleT = GetComponentInChildren<ParticleSystem>()?.transform;

                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
                particleT.rotation = Quaternion.Euler(0, 0, angle);
                break;

            case Define.SkillType.PlasmaShot:
                rigid.velocity = dir * speed;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(0, 0, angle);
                break;

            case Define.SkillType.ElectricShock:
                target = Utils.FindClosestMonster(targetPos);
                if (target != null)
                    StartCoroutine(CoElectricShock(spawnPos, targetPos, target));
                break;
            case Define.SkillType.SuicideDrone:
                StartCoroutine(CoStartSuicideDrone());
                break;

            case Define.SkillType.TimeStopBomb:
                rigid.velocity = dir * speed;
                StartCoroutine(CoExplosionTimeStopBomb());

                break;

            case Define.SkillType.GravityBomb:
                rigid.velocity = dir * speed;
                StartCoroutine(CoExplosionGravityBomb());
                break;

            case Define.SkillType.OrbitalBlades:
                rigid.velocity = dir * speed;
                break;

            case Define.SkillType.BossSkill:
                rigid.velocity = dir * speed;
                break;

        }


        if(gameObject.activeInHierarchy)
            StartCoroutine(CoDestroy(duration));
    }
    
    
    void HandlePlasmaSpinner(CreatureController _cc)
    {

        numBounce--;
        rigid.velocity = -rigid.velocity.normalized * bounceSpeed;

        float angle = Mathf.Atan2(rigid.velocity.y, rigid.velocity.x) * Mathf.Rad2Deg - 90f;;
        particleT.rotation = Quaternion.Euler(0, 0, angle);
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
    
    void HandleOrbitalBlades(CreatureController _cc)
    {
        //TODO : 몬스터 부딪히면 어떻게할지?
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

        float chaseTime = 1f;
        float elapsedTIme = 0f;
        float dist = 0f;
        while (elapsedTIme < chaseTime)
        {   
            if(!this.IsValid()) yield break;
            target = Utils.FindClosestMonster(transform.position);
            
            if(target != null && target.IsValid()) dist = Vector2.Distance(target.transform.position, transform.position);

            if(dist < 0.1f && target != null) break;
        

            if(target != null && target.IsValid())
            {
                dir = (target.transform.position - transform.position).normalized;
                rigid.velocity = dir * speed;
                if (dir.x != 0) 
                {
                    Vector3 newScale= transform.localScale;
                    newScale.x = Mathf.Abs(newScale.x) * (dir.x > 0 ? -1 : 1);
                    transform.localScale = newScale;
                }
            }
            else
            {
                rigid.velocity = Vector2.zero;
            }

            elapsedTIme += Time.deltaTime;
            yield return null;
        }

        //없으면 제자리 폭발.
        isBoom = true;
        HandleSuicideDrone();
        
    }

    void HandleSuicideDrone()
    {
        if(!isBoom) return;

        ExplosionDrone();
        StartDestory();
        isBoom = false;
    }

    void ExplosionDrone()
    {   
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


    public void HandleBossSkill()
    {
        rigid.velocity = Vector2.zero;
        StartDestory();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        CreatureController cc = collision.gameObject.GetComponent<CreatureController>();

        if (cc == null || !cc.IsValid() || !this.IsValid() || !cc.IsMonster()) return;

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

            case Define.SkillType.OrbitalBlades :
                HandleOrbitalBlades(cc);
                cc.OnDamaged(owner, skill);
                break;
        }
    }

}
