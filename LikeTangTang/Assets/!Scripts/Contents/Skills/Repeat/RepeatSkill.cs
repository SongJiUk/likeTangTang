using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public abstract class RepeatSkill : SkillBase
{
    public float coolTime {get; set;}
    public RepeatSkill() : base(Define.SkillType.Repeat) {}


#region  스킬 코루틴
    Coroutine _coSkill;

    public override void ActivateSkill()
    {
        if(_coSkill != null) StopCoroutine(_coSkill);

        _coSkill = StartCoroutine(coStartSkill());
    }

    public abstract void DoSkill();
    protected virtual IEnumerator coStartSkill()
    {
        WaitForSeconds waitTime = new WaitForSeconds(coolTime);

        while(true)
        {
            DoSkill();

            yield return waitTime;
        }
    }
    #endregion
}
