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
        EquipmentOptionImage,
        UncommonSkillLockImage,
        RareSkillLockImage,
        EpicSkillLockImage,
        UniqueSkillLockImage,
        UncommonSkillCheckImage,
        RareSkillCheckImage,
        EpicSkillCheckImage,
        UniqueSkillCheckImage,

        CostMaterialImage,

    }

    enum Texts
    {
        EquipmentGradeValueText,
        EquipmentNameValueText,
        EnforceValueText,
        EquipmentLevelValueText,
        EquipmentOptionValueText,

        EquipmentWeaponSkillText,
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
        LevelupButton,
        MergeButton
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
        GetButton(ButtonsType, (int)Buttons.MergeButton).gameObject.BindEvent(OnClickMergeButton);


        GetImage(ImagesType, (int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(false);

        for(int i =0; i < GradeNum; i++)
        {
            GetImage(ImagesType, (int)Images.UncommonSkillLockImage + i).gameObject.SetActive(true);
            GetImage(ImagesType, (int)Images.UncommonSkillCheckImage + i).gameObject.SetActive(false);
        }
            
        

        return true;
    }
    public void SetInfo(Equipment _equipment)
    {
        equipment = _equipment;
        Define.EquipmentGrade grade = equipment.EquipmentData.EquipmentGarde;
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

        //올려주는 스탯에 맞게 이미지 변경
        if (equipment.EquipmentData.Grade_Attack != 0)
            GetImage(ImagesType, (int)Images.EquipmentOptionImage).sprite = Manager.ResourceM.Load<Sprite>("Ui_Attack_Icon.sprite");
        else
            GetImage(ImagesType, (int)Images.EquipmentOptionImage).sprite = Manager.ResourceM.Load<Sprite>("Ui_Hp_Icon.sprite");


        //TODO : 장비 옵션초기화
        for (int i = 0; i < GradeNum; i++)
        {
            GetText(TextsType, (int)Texts.UncommonSkillOptionDescriptionValueText + i).text = Manager.DataM.SpecialSkillDic[gradeAbilities[i]].Description;
            GetText(TextsType, (int)Texts.UncommonSkillOptionDescriptionValueText + i).color = Define.EquipmentUIColors.EquipGradeStyles[gradeColor[i]].NameColor;
            GetImage(ImagesType, (int)Images.UncommonSkillLockImage + i).gameObject.SetActive(true);
            GetImage(ImagesType, (int)Images.UncommonSkillCheckImage + i).gameObject.SetActive(false);
        }


        int gradeNum = Define.GetGradeNum(grade);
        for (int i = 0; i <= gradeNum; i++)
        {
            GetImage(ImagesType, (int)Images.UncommonSkillLockImage + i).gameObject.SetActive(false);
            GetImage(ImagesType, (int)Images.UncommonSkillCheckImage + i).gameObject.SetActive(true);
        }

        if (equipment.EquipmentData.EquipmentType == Define.EquipmentType.Weapon)
        {
            GetText(TextsType, (int)Texts.EquipmentWeaponSkillText).gameObject.SetActive(true);
            GetText(TextsType, (int)Texts.EquipmentWeaponSkillText).text = $"{Manager.DataM.SkillDic[equipment.EquipmentData.BaseSkill].SkillName} 스킬 기본 획득";
        }
        else GetText(TextsType, (int)Texts.EquipmentWeaponSkillText).gameObject.SetActive(false);


        if (equipment.IsEquiped)
        {
            GetButton(ButtonsType, (int)Buttons.EquipButton).gameObject.SetActive(false);
            GetButton(ButtonsType, (int)Buttons.UnquipButton).gameObject.SetActive(true);
        }
        else
        {
            GetButton(ButtonsType, (int)Buttons.EquipButton).gameObject.SetActive(true);
            GetButton(ButtonsType, (int)Buttons.UnquipButton).gameObject.SetActive(false);
        }

        //초기화
        Refresh();
    }

    void Refresh()
    {
        //여기 들어와야 되는것들(Level, 스탯 변화, costGold, costMarterial)

        if (equipment.Level > 1) GetButton(ButtonsType, (int)Buttons.EquipmentResetButton).gameObject.SetActive(true);
        else GetButton(ButtonsType, (int)Buttons.EquipmentResetButton).gameObject.SetActive(false);

        GetText(TextsType, (int)Texts.EquipmentLevelValueText).text = $"{equipment.Level}/{equipment.EquipmentData.Grade_MaxLevel}";

        if (equipment.EquipmentData.Grade_Attack != 0)
            GetText(TextsType, (int)Texts.EquipmentOptionValueText).text = $"{equipment.EquipmentData.Grade_Attack + ((equipment.Level - 1) * equipment.EquipmentData.GradeUp_Attack)}";
        else
            GetText(TextsType, (int)Texts.EquipmentOptionValueText).text = $"{equipment.EquipmentData.Grade_Hp + ((equipment.Level - 1) * equipment.EquipmentData.GradeUp_Hp)}";

        //TODO : 강화에 필요한 골드, 스크롤 양
        if (Manager.DataM.EquipmentLevelDic.TryGetValue(equipment.Level, out var data))
        {
            GetText(TextsType, (int)Texts.CostGoldValueText).text = data.Cost_Gold.ToString();
            GetImage(ImagesType, (int)Images.CostMaterialImage).sprite = Manager.ResourceM.Load<Sprite>($"Scroll_{equipment.EquipmentData.EquipmentType}.sprite");
            GetText(TextsType, (int)Texts.CostMaterialValueText).text = data.Cost_Material.ToString();
        }

    }
    public void OnClickBG()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClickResetButton()
    {
        //TODO : 장비 초기화 팝업
        Manager.SoundM.PlayButtonClick();
        UI_EquipmentResetPopup equipmentResetPopup = (Manager.UiM.SceneUI as UI_LobbyScene).Ui_EquipmentResetPopup;
        equipmentResetPopup.SetInfo(equipment);
        equipmentResetPopup.gameObject.SetActive(true);
        
    }

    void OnClickEquipButton()
    {
        //TODO : Sound

        Manager.GameM.EquipItem(equipment.EquipmentData.EquipmentType, equipment);
        Refresh();
        gameObject.SetActive(false);
        (Manager.UiM.SceneUI as UI_LobbyScene).Ui_EquipmentPopup.SetInfo();

        Manager.GameM.SaveGame();

    }

    void OnClickUnEquipButton()
    {
        Manager.GameM.UnEquipItem(equipment);
        Refresh();
        gameObject.SetActive(false);
        (Manager.UiM.SceneUI as UI_LobbyScene).Ui_EquipmentPopup.SetInfo();

        Manager.GameM.SaveGame();
    }

    void OnClickLevelUpButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (equipment.Level >= equipment.EquipmentData.Grade_MaxLevel)
        {
            Manager.UiM.ShowToast("장비가 최대레벨입니다.");
            return;
        }

            int UpgradeCostGold = Manager.DataM.EquipmentLevelDic[equipment.Level].Cost_Gold;
        int UpgradeCostMaterial = Manager.DataM.EquipmentLevelDic[equipment.Level].Cost_Material;
        int MaterialCount = 0;

        Manager.GameM.ItemDic.TryGetValue(equipment.EquipmentData.LevelUpMaterial, out MaterialCount);

        if(Manager.GameM.Gold >= UpgradeCostGold && MaterialCount >= UpgradeCostMaterial)
        {
            equipment.LevelUp();
            Manager.GameM.Gold -= UpgradeCostGold;
            Manager.GameM.RemoveMaterialItem(equipment.EquipmentData.LevelUpMaterial, UpgradeCostMaterial);

            //TODO : LevelUP sound;
            Refresh();
        }
        else
        {
            Manager.UiM.ShowToast("재화가 부족합니다.");
        }

        (Manager.UiM.SceneUI as UI_LobbyScene).Ui_EquipmentPopup.SetInfo();
    }

    void OnClickMergeButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (equipment.IsEquiped)
        {
            Manager.UiM.ShowToast("장착을 해제하고 시도해주세요.");
            return;
        }

            UI_MergePopup mergePopup =  (Manager.UiM.SceneUI as UI_LobbyScene).Ui_MergePopup;
        mergePopup.SetInfo(equipment);
        mergePopup.gameObject.SetActive(true);


        gameObject.SetActive(false);
    }
}
