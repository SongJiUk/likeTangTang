using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : SequenceSkill
{
    Rigidbody2D rigid;
    Coroutine coroutine;


    private void Awake()
    {
        owner = GetComponent<CreatureController>();
        Skilltype = Define.SkillType.BossMove;
    }

    public override void DoSkill(Action _callback = null)
    {
        UpdateSkillData(DataId);
        if (coroutine != null) StopCoroutine(coroutine);

        coroutine = StartCoroutine(CoMove(_callback));
    }

    IEnumerator CoMove(Action _callback = null)
    {
        rigid = GetComponent<Rigidbody2D>();
        float elapsed = 0;

        while(true)
        {
            elapsed += Time.deltaTime;
            
            if (elapsed > SkillDatas.Duration) break;

            Vector3 dir = (Manager.GameM.player.transform.position - owner.transform.position).normalized;
            Vector2 targetPosition = Manager.GameM.player.transform.position + dir * UnityEngine.Random.Range(1, 5);

            if (Vector3.Distance(rigid.position, targetPosition) <= 0.1f)
                continue;

            Vector2 dirVec = targetPosition - rigid.position;
            Vector2 nextVec = dirVec.normalized * owner.Speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            yield return null;
        }

        _callback?.Invoke();
    }
}
