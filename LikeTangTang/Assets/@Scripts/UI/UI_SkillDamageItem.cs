using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillDamageItem : UI_Base
{


    enum Texts
    {
        SkillNameValueText,
        SkillDamageValueText,
        DamageProbabilityValueText,
    }

    enum Images
    {
        SkillImage
    }

    enum Sliders
    {
        DamageSliderObject
    }

    SkillBase skill;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;
        TextsType = typeof(Texts);
        ImagesType = typeof(Images);
        SlidersType = typeof(Sliders);

        BindText(TextsType);
        BindImage(ImagesType);
        BindSlider(SlidersType);

        return true;
    }

    public void SetInfo(SkillBase _skill)
    {
        skill = _skill;
        Refresh();
    }

    void Refresh()
    {
        GetImage(ImagesType, (int)Images.SkillImage).sprite = Manager.ResourceM.Load<Sprite>(skill.SkillDatas.SkillIcon);
        GetText(TextsType, (int)Texts.SkillNameValueText).text = $"{skill.SkillDatas.SkillName}";
        GetText(TextsType, (int)Texts.SkillDamageValueText).text = $"{(int)skill.TotalDamage}";

        float allSkillDamage = Manager.GameM.GetTotalDamage();
        float percentage = skill.TotalDamage / allSkillDamage;

        if (allSkillDamage == 0) percentage = 1;

        GetText(TextsType, (int)Texts.DamageProbabilityValueText).text = (percentage * 100).ToString("F2") + "%";
        GetSlider(SlidersType, (int)Sliders.DamageSliderObject).value = percentage;
    }
}
