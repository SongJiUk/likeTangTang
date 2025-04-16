using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TimeStopBombZone : MonoBehaviour
{
    SkillBase skill;
    CreatureController owner;
    Coroutine coApplyDamage;

    HashSet<CreatureController> monsters = new HashSet<CreatureController>();
    void OnDisable()
    {
        foreach (var target in monsters)
        {
            if (target.IsValid())
            target.Speed /= skill.SkillDatas.SlowRatio;
        }

        StopApplyDamage();
    }


    public void SetInfo(CreatureController _owner, SkillBase _skill)
    {
        owner = _owner;
        skill = _skill;
        monsters.Clear();

        PlayAnim(() => 
        {
            if(gameObject.activeInHierarchy) StartCoroutine(CoDestory(gameObject, skill.SkillDatas.Duration));
            StartApplyDamage();
        });
    }

    public void PlayAnim(Action _action)
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one * skill.SkillDatas.EffectScaleMultiplier, 0.5f).OnComplete(()=> _action.Invoke());
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

    void StartApplyDamage()
    {
        StopApplyDamage();
        coApplyDamage = StartCoroutine(CoApplyDamage());
    }

    void StopApplyDamage()
    {
        if(coApplyDamage != null) StopCoroutine(coApplyDamage);
        coApplyDamage = null;
    }

    IEnumerator CoApplyDamage()
    {
        while(true)
        {
            var targets = monsters.ToList();

            foreach(var target in targets)
            {
                if(!target.IsValid())
                {
                    monsters.Remove(target);
                    continue;
                }

                target.OnDamaged(owner, skill);
            }

            yield return new WaitForSeconds(1f);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        CreatureController cc = collision.GetComponent<CreatureController>();

        if(!cc.IsValid() || skill?.SkillDatas == null) return;
        if(!cc.IsMonster()) return;

        if(monsters.Add(cc))
            cc.Speed  *= skill.SkillDatas.SlowRatio;
        
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        CreatureController cc = collision.GetComponent<CreatureController>();
        if(cc == null) return;
        
        if(!cc.IsValid() || skill?.SkillDatas == null) return;
        if(!cc.IsMonster()) return;

        if(monsters.Remove(cc))
            cc.Speed /= skill.SkillDatas.SlowRatio;
    }
}
