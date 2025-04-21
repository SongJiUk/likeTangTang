using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public abstract class RepeatSkill : SkillBase
{
    public RepeatSkill() : base(Define.SkillType.None) {}

    public override void ActivateSkill()
    {
        base.ActivateSkill();
    }

    public abstract void DoSkill();

    
}
