using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Data;

public class ElectronicField : RepeatSkill, ITickable
{
    [SerializeField]
    GameObject normalEffect;
    [SerializeField]
    GameObject evolutionEffect;


    Dictionary<MonsterController, float> targets = new();

    

    void Awake()
    {
        Skilltype = Define.SkillType.ElectronicField;
        gameObject.SetActive(false);
    }
    private void OnEnable() 
    {

    }

    void OnDisable()
    {
        targets.Clear();      
    }

    public override void DoSkill()
    {
        
    }
    private void OnDestroy()
    {
        Manager.UpdateM.Unregister(this);
    }
    public override void ActivateSkill()
    {
        gameObject.SetActive(true);
        if(SkillDatas == null ) base.ActivateSkill();
        Manager.UpdateM.Register(this);
        attackInterval = SkillDatas.AttackInterval;


    }

    public override void OnSkillLevelup()
    {
        base.OnSkillLevelup();
        if(SkillLevel > Define.MAX_SKILL_LEVEL) OnEvolutaion(); 

        attackInterval = SkillDatas.AttackInterval;
        transform.localScale = Vector3.one * SkillDatas.ScaleMultiplier;

    }
    public void Tick(float _deltaTime)
    {
        if (SkillDatas == null) return;

        float now = Time.time;
        var keys = targets.Keys.ToList();

        foreach(var monster in keys)
        {
            if(!monster.IsValid())
            {
                targets.Remove(monster);
                continue;
            }

            if(now - targets[monster] >= SkillDatas.AttackInterval)
            {
                monster.OnDamaged(Manager.GameM.player, this);
                targets[monster] = now;
            }
        }
    }

    public void OnEvolutaion()
    {
        normalEffect.SetActive(false);
        evolutionEffect.SetActive(true);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController cc = collision.GetComponent<MonsterController>();
        if (cc == null || !cc.IsValid() || !cc.IsMonster()) return;

        if(!targets.ContainsKey(cc))
            targets.Add(cc, Time.time - attackInterval);

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        MonsterController cc = collision.GetComponent<MonsterController>();
        if (cc == null || !cc.IsValid() || !cc.IsMonster()) return;

        if (targets.ContainsKey(cc)) targets.Remove(cc);
    }
}
