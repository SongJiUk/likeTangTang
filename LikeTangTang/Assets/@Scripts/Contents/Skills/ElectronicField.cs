using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ElectronicField : RepeatSkill
{
    [SerializeField]
    GameObject normalEffect;
    [SerializeField]
    GameObject evolutionEffect;


    void Awake()
    {
        Skilltype = Define.SkillType.EletronicField;
        gameObject.SetActive(false);

    }

    public override void ActivateSkill()
    {
        gameObject.SetActive(true);
        if(SkillDatas == null ) base.ActivateSkill();
        SetElectronicField();
        DoSkill();
    }
    public override void OnChangedSkillData()
    {
        SetElectronicField();
    }
    public override void DoSkill()
    {

    }

    public void SetElectronicField()
    {
        
    }

    public void OnEvolutaion()
    {
        normalEffect.SetActive(false);
        evolutionEffect.SetActive(true);

        transform.localScale = Vector3.one * SkillDatas.ScaleMultiplier;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController cc = collision.GetComponent<MonsterController>();
        if(!cc.IsValid()) return;
        if(!cc.IsMonster()) return;

        cc.StartSKillZone(owner, this);

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        MonsterController cc = collision.GetComponent<MonsterController>();
        if(!cc.IsValid()) return;
        if(!cc.IsMonster()) return;
        
        cc.StopSkillZone(this);
    }
}
