using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using Unity.VisualScripting;


public class UI_EquipmentInfoPopup : UI_Popup
{
    const int GradeNum = 4;
    enum GameObjects
    {
        ContentObject,

    }

    enum Images
    {
        EquipmentTypeImage,
        EquipmentImage,
        EquipmentTypeBackgroundImage,
        EquipmentEnforceBackgroundImage,
        EquipmentGradeBackgroundImage,

        UncommonSkillLockImage,
        RareSkillLockImage,
        EpicSkillLockImage,
        UniqueSkillLockImage,
        CostMaterialImage,

    }

    enum Texts
    {
        EquipmentGradeValueText,
        EquipmentNameValueText,
        EnforceValueText,
        EquipmentLevelValueText,
        EquipmentOptionValueText,

        UncommonSkillOptionDescriptionValueText,
        RareSkillOptionDescriptionValueText,
        EpicSkillOptionDescriptionValueText,
        UniqueSkillOptionDescriptionValueText,
        CostGoldValueText,
        CostMaterialValueText,

        
    }

    enum Buttons
    {
        BackgroundButton,
        EquipmentResetButton,
        EquipButton,
        UnquipButton,
        LevelupButton

    }

    Equipment equipment;
    private void Awake()
    {
        Init();
    }

    void OnEnable()
    {
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.ContentObject));
    }

    public override bool Init()
    {
        if (!base.Init()) return false;
        gameObjectsType = typeof(GameObjects);
        ImagesType = typeof(Images);
        TextsType = typeof(Texts);
        ButtonsType = typeof(Buttons);

        BindObject(gameObjectsType);
        BindImage(ImagesType);
        BindText(TextsType);
        BindButton(ButtonsType);


        GetButton(ButtonsType, (int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBG);
        GetButton(ButtonsType, (int)Buttons.EquipmentResetButton).gameObject.BindEvent(OnClickResetButton);
        GetButton(ButtonsType, (int)Buttons.EquipButton).gameObject.BindEvent(OnClickEquipButton);
        GetButton(ButtonsType, (int)Buttons.UnquipButton).gameObject.BindEvent(OnClickUnEquipButton);
        GetButton(ButtonsType, (int)Buttons.LevelupButton).gameObject.BindEvent(OnClickLevelUpButton);


        GetImage(ImagesType, (int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(false);

        for(int i =0; i < GradeNum; i++)
            GetImage(ImagesType, (int)Images.UncommonSkillLockImage + i).gameObject.SetActive(true);
            

        return true;
    }
    public void SetInfo(Equipment _equipment)
    {
        equipment = _equipment;
        Define.EquipmentGrade grade = equipment.EquipmentData.EquipmentGarde;

        //초기화
        Refresh();

        //상단 등급, 이름
        GetText(TextsType, (int)Texts.EquipmentGradeValueText).text = grade.ToString();
        GetText(TextsType, (int)Texts.EquipmentNameValueText).text = equipment.EquipmentData.NameTextID;
        GetText(TextsType, (int)Texts.EquipmentNameValueText).color = Define.EquipmentUIColors.EquipGradeStyles[grade].NameColor;

        // 왼쪽 상단 이미지
        GetImage(ImagesType, (int)Images.EquipmentTypeImage).sprite = Manager.ResourceM.Load<Sprite>($"{equipment.EquipmentData.EquipmentType}_Icon.sprite");
        GetImage(ImagesType, (int)Images.EquipmentImage).sprite = Manager.ResourceM.Load<Sprite>(equipment.EquipmentData.SpriteName);
        
        GetImage(ImagesType, (int)Images.EquipmentTypeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[grade].BgColor;
        GetImage(ImagesType, (int)Images.EquipmentGradeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[grade].BorderColor;

        int num = Utils.GetUpgradeNumber(grade);    
        if (num == 0)
        {
            GetText(TextsType, (int)Texts.EnforceValueText).text = "";
            GetImage(ImagesType, (int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(false);
        }
        else
        {
            GetText(TextsType, (int)Texts.EnforceValueText).text = num.ToString();
            GetImage(ImagesType, (int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(true);
            GetImage(ImagesType, (int)Images.EquipmentEnforceBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[grade].BgColor;
        }

        // 장비 레벨, 공격력
        GetText(TextsType, (int)Texts.EquipmentLevelValueText).text = $"{equipment.Level}/{equipment.EquipmentData.Grade_MaxLevel}";
        GetText(TextsType, (int)Texts.EquipmentOptionValueText).text = $"{equipment.EquipmentData.Grade_Attack + (equipment.Level * equipment.EquipmentData.GradeUp_Attack)}";

        //TODO : 장비 옵션(등급에 따라)
        int gradeNum = Define.GetGradeNum(grade);
        if(gradeNum < 0 ) Debug.Log("Grade Error!");

        for(int i =0; i<=gradeNum; i++) 
            GetImage(ImagesType, (int)Images.UncommonSkillLockImage +i).gameObject.SetActive(false);


        //TODO : 강화에 필요한 골드, 스크롤 양
        if(Manager.DataM.EquipmentLevelDic.TryGetValue(equipment.Level, out var data))
        {
            GetText(TextsType, (int)Texts.CostGoldValueText).text = data.Cost_Gold.ToString();
            GetImage(ImagesType, (int)Images.CostMaterialImage).sprite = Manager.ResourceM.Load<Sprite>($"Scroll_{equipment.EquipmentData.EquipmentType}.sprite");
            GetText(TextsType, (int)Texts.CostMaterialValueText).text = data.Cost_Material.ToString();
        }

    }

    void Refresh()
    {
        //초기화
        var gradeAbilities = new int[]
        {
            equipment.EquipmentData.UnCommonGradeAbility,
            equipment.EquipmentData.RareGradeAbility,
            equipment.EquipmentData.EpicGradeAbility,
            equipment.EquipmentData.UniqueGradeAbility
        };

        var gradeColor = new Define.EquipmentGrade[]
        {
            Define.EquipmentGrade.UnCommon,
            Define.EquipmentGrade.Rare,
            Define.EquipmentGrade.Epic,
            Define.EquipmentGrade.Unique
        };

        for(int i =0; i<GradeNum; i++)
        {
            GetText(TextsType, (int)Texts.UncommonSkillOptionDescriptionValueText + i).text = Manager.DataM.SpecialSkillDic[gradeAbilities[i]].Description;
            GetText(TextsType, (int)Texts.UncommonSkillOptionDescriptionValueText+ i).color = Define.EquipmentUIColors.EquipGradeStyles[gradeColor[i]].NameColor;
            GetImage(ImagesType, (int)Images.UncommonSkillLockImage +i).gameObject.SetActive(true);
        }

    }
    public void OnClickBG()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClickResetButton()
    {
        //TODO : 장비 초기화 팝업
    }

    void OnClickEquipButton()
    {

    }

    void OnClickUnEquipButton()
    {

    }

    void OnClickLevelUpButton()
    {

    }
}
