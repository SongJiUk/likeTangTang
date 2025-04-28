using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Test : UI_Base
{

    public enum Toggles
    {
        SkillLevelUpToggle,
        WaveEndToggle
    }
    public override bool Init()
    {
        if(base.Init() == false) return false;

        SetUIInfo();

        GetToggle(typeof(Toggles), (int)Toggles.SkillLevelUpToggle).gameObject.BindEvent(() =>
        {
            Define.SkillType skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.ElectricShock);
            Manager.GameM.player.Skills.LevelUpSkill(skillType);
            //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.ElectronicField);
            //Manager.GameM.player.Skills.LevelUpSkill(skillType);
            //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.EnergyRing);
            //Manager.GameM.player.Skills.LevelUpSkill(skillType);
            //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.GravityBomb);
            //Manager.GameM.player.Skills.LevelUpSkill(skillType);
            //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.OrbitalBlades);
            //Manager.GameM.player.Skills.LevelUpSkill(skillType);
            //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.PlasmaShot);
            //Manager.GameM.player.Skills.LevelUpSkill(skillType);
            //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.PlasmaSpinner);
            //Manager.GameM.player.Skills.LevelUpSkill(skillType);
            //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.SpectralSlash);
            //Manager.GameM.player.Skills.LevelUpSkill(skillType);
            //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.SuicideDrone);
            //Manager.GameM.player.Skills.LevelUpSkill(skillType);
            //skillType = Utils.GetSkillTypeFromInt((int)Define.SkillType.TimeStopBomb);
            //Manager.GameM.player.Skills.LevelUpSkill(skillType);
        });

        GetToggle(typeof(Toggles), (int)Toggles.WaveEndToggle).gameObject.BindEvent(() => {
            GameScene gameScene = GameObject.Find("@GameScene").GetComponent<GameScene>();
            gameScene.WaveEnd();
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
