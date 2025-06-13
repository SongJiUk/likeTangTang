using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill : SequenceSkill
{
    Coroutine coroutine;
    private void Awake()
    {
        Skilltype = Define.SkillType.BossSkill;
        owner = GetComponent<CreatureController>();
    }


    public override void DoSkill(Action _callback = null)
    {
        UpdateSkillData(DataId);
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(CoSetProjectile(_callback));
        
    }

    IEnumerator CoSetProjectile(Action _callback = null)
    {
        GameObject obj = Manager.ResourceM.Instantiate("SkillRange", _pooling: true);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        SkillRange skillrange = obj.GetComponent<SkillRange>();
        float radius = SkillDatas.Range;
        float wait = skillrange.SetCircle(radius * 2);

        yield return new WaitForSeconds(wait);
        Manager.ResourceM.Destory(obj);

        int boltCount = 10;
        float boltSpeed = SkillDatas.Speed;
        string prefabName = SkillDatas.PrefabName;
        Vector3 pos = transform.position;

        for(int i =0; i<boltCount; i++)
        {
            float angle = i * (360f / boltCount) * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            GenerateProjectile(owner, prefabName, pos, dir, _skill: this);
        }

        yield return new WaitForSeconds(SkillDatas.Duration);
        _callback?.Invoke();
    }

    protected override ProjectileController GenerateProjectile(CreatureController _owner, string _prefabName, Vector3 _startPos = default, Vector3 _dir = default, Vector3 _targetPos = default, SkillBase _skill = null, HashSet<MonsterController> _sharedTarget = null)
    {
        ProjectileController pc = Manager.ObjectM.Spawn<ProjectileController>(_startPos, _prefabName: _prefabName);
        pc.SetInfo(_owner, _startPos, _dir, _targetPos, _skill);
        return null;
    }

}
