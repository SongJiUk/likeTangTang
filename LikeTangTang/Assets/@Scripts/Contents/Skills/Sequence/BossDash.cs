using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDash : SequenceSkill
{
    Rigidbody2D rigid;
    Coroutine coroutine;

    private void Awake()
    {
        Skilltype = Define.SkillType.BossDash;
        owner = GetComponent<CreatureController>();
    }
    public override void DoSkill(Action _callback = null)
    {
        UpdateSkillData(DataId);
        if (coroutine != null) StopCoroutine(coroutine);

        coroutine = StartCoroutine(CoDash(_callback));
    }



    IEnumerator CoDash(Action _callback = null)
    {
        rigid = GetComponent<Rigidbody2D>();

        float elapsed = 0;
        Vector3 dir;
        Vector2 targetPos = Manager.GameM.player.transform.position;

        GameObject obj = Manager.ResourceM.Instantiate("SkillRange", _pooling: true);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;

        SkillRange skillRange = obj.GetOrAddComponent<SkillRange>();

        while (true)
        {
            elapsed += Time.deltaTime;
            if (elapsed > SkillDatas.Duration) break;

            dir = ((Vector2)Manager.GameM.player.transform.position - rigid.position);
            targetPos = Manager.GameM.player.transform.position + dir.normalized * 3;

            skillRange.SetInfo(dir, targetPos, Vector3.Distance(rigid.position, targetPos));
            yield return null;
        }

        Manager.ResourceM.Destory(obj);

        while (Vector3.Distance(rigid.position, targetPos) > 0.3f)
        {
            Vector2 dirVec = targetPos - rigid.position;

            Vector2 nextVec = dirVec.normalized * owner.Speed * 2 * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);

            yield return null;
        }

        yield return new WaitForSeconds(SkillDatas.AttackInterval);
        _callback?.Invoke();
    }
}
