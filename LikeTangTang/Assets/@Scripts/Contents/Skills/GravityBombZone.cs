using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System;
using System.Linq;

public class GravityBombZone : SkillZone
{

    public void SetInfo(CreatureController _owner, SkillBase _skill)
    {
        owner = _owner;
        skill = _skill;

        PlayAnim(() =>
        {
            if(gameObject.activeInHierarchy) StartCoroutine(CoDestory(gameObject, skill.SkillDatas.Duration));
        });
        
    }

    public void PlayAnim(Action _action)
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one * skill.SkillDatas.EffectScaleMultiplier, 0.5f).OnComplete(() => _action.Invoke());
    }

    IEnumerator CoDestory(GameObject _go, float _time)
    {
        yield return new WaitForSeconds(_time);
        transform.localScale = Vector3.one * skill.SkillDatas.EffectScaleMultiplier;
        transform.DOScale(0, 0.5f).OnComplete(() =>
        {
            Manager.ResourceM.Destory(_go);
        });
    }
    

    void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mc = collision.GetComponent<MonsterController>();
        if(!mc.IsValid() || skill?.SkillDatas == null) return;
        if(!mc.IsMonster()) return;

        mc.StartSKillZone(owner, skill, this);

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        MonsterController mc= collision.GetComponent<MonsterController>();
        if(mc ==null) return;

        if(!mc.IsValid() || skill?.SkillDatas == null) return;
        if(!mc.IsMonster()) return;

        mc.StopSkillZone(skill);
    }
}
