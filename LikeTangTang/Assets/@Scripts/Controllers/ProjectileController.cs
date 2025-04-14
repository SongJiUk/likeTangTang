using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Data;
using UnityEngine;

public class ProjectileController : SkillBase
{
    
    Vector3 spawnPos;
    Vector3 targetPos;
    Vector3 dir;
    Rigidbody2D rigid;
    Define.SkillType skillType;
    SkillBase skill;
    
  
    float sclaeMul;
    float lifeTime;
    public ProjectileController() : base(Define.SkillType.None){}

    //초기화, 세팅
    public override bool Init()
    {
        base.Init();
        return true;
    }

    public void SetInfo(CreatureController _owner, Vector3 _pos, Vector3 _dir, Vector3 _targetPos, SkillBase _skill)
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

        transform.localScale = Vector3.one * skill.SkillDatas.ScaleMultiplier;
        switch(skillType)
        {
            case Define.SkillType.PlasmaSpinner :
                rigid.velocity = dir * speed;
                break;

            case Define.SkillType.PlasmaShot :
                rigid.velocity = dir * speed;
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
