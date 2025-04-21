using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Test : UI_Base
{

    public enum Toggles
    {
        SkillLevelUpToggle
    }
    public override bool Init()
    {
        if(base.Init() == false) return false;

        SetUIInfo();

        GetToggle(typeof(Toggles), (int)Toggles.SkillLevelUpToggle).gameObject.BindEvent(() =>
        {
            Define.SkillType skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.EnergyRing);
            Manager.GameM.player.Skills.LevelUpSkill(skillType);
        });
        return true;
    }

    protected override void SetUIInfo()
    {
        Bind<Toggle>(typeof(Toggles));
    }

    protected override void RefreshUI()
    {

    }
}
