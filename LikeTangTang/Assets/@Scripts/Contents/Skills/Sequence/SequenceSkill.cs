using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class SequenceSkill : SkillBase
{
    public int DataId;
    public abstract void DoSkill(Action _callback = null);
    public SequenceSkill() : base(Define.SkillType.None) {}
    
}
