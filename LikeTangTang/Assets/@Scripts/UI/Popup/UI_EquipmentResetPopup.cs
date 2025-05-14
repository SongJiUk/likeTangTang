using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipmentResetPopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,
        TargetEquipment,
        ToggleGroupObject,
        ResetInfoGroupObject,
        DowngradeGroupObject,
        
    }

    enum Buttons
    {
        BackgroundButton,
        EquipmentResetButton,
        EquipmentDowngradeButton
    }

    enum Images
    {
        TargetEquipmentGradeBackgroundImage,
        TargetEquipmentImage,
        TargetEquipmentEnforceBackgroundImage,
        ResultEquipmentGradeBackgroundImage,
        ResultEquipmentImage,
        ResultEquipmentEnforceBackgroundImage,
        ResultGoldBackgroundImage,
        ResultMaterialBackgroundImage,
        ResultMaterialImage,
        DowngradeTargetEquipmentGradeBackgroundImage,
        DowngradeTargetEquipmentImage,
        DowngradeTargetEquipmentEnforceBackgroundImage,
        DowngradeEquipmentGradeBackgroundImage,
        DowngradeEquipmentImage,
        DowngradeResultMaterialImage,
        DowngradEnchantStoneBackgroundImage,
        DowngradEnchantStoneImage

    }

    enum Texts
    {
        TargetEquipmentLevelValueText,
        TargetEnforceValueText,
        ResultEquipmentLevelText,
        ResultEnforceValueText,
        ResultGoldCountValueText,
        ResultMaterialCountValueText,
        DowngradeTargetEquipmentLevelValueText,
        DowngradeTargetEnforceValueText,
        DowngradeEquipmentLevelText,
        DowngradEnchantStoneCountValueText,
        DowngradeResultGoldCountValueText,
        DowngradeResultMaterialCountValueText
    }

    enum Toggles
    {
        ResetTapToggle,
        DowngradeTapToggle
    }


    Equipment equipment;
    bool isSelectResetTap = false;
    bool isSelectDownGradeTap = false;
    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.ContentObject));
    }
    public override bool Init()
    {
        if (!base.Init()) return false;

        gameObjectsType = typeof(GameObjects);
        ButtonsType = typeof(Buttons);
        ImagesType = typeof(Images);
        TextsType = typeof(Texts);
        TogglesType = typeof(Toggles);

        BindObject(gameObjectsType);
        BindButton(ButtonsType);
        BindImage(ImagesType);
        BindText(TextsType);
        BindToggle(TogglesType);


        GetButton(ButtonsType, (int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBgButton);
        GetButton(ButtonsType, (int)Buttons.EquipmentResetButton).gameObject.BindEvent(OnClickResetButton);
        GetButton(ButtonsType, (int)Buttons.EquipmentDowngradeButton).gameObject.BindEvent(OnClickDowngradeButton);
        GetToggle(TogglesType, (int)Toggles.DowngradeTapToggle).gameObject.BindEvent(OnClickDowngradeToggle);
        GetToggle(TogglesType, (int)Toggles.ResetTapToggle).gameObject.BindEvent(OnClickResetTapToggle);

        return true;
    }

    public void SetInfo(Equipment _equipment)
    {
        equipment = _equipment;
        Refresh();

    }


    void Refresh()
    {
        if (equipment == null)
            GetObject(gameObjectsType, (int)GameObjects.TargetEquipment).SetActive(false);
        else
            EquipmentResetRefresh();

        int num = Utils.GetUpgradeNumber(equipment.EquipmentData.EquipmentGarde);
        if (num == 0)
            GetObject(gameObjectsType, (int)GameObjects.ToggleGroupObject).SetActive(false);
        else
        {
            EquipmentDowngradeRefresh();
            GetObject(gameObjectsType, (int)GameObjects.ToggleGroupObject).SetActive(true);
        }
        
        
        OnClickResetTapToggle();
    }

    void EquipmentResetRefresh()
    {
        //TODO : 초기화 리플래시
        //레벨 초기화
        //현재 장비, 초기화될 장비
        Define.EquipmentGrade equipmentGrade =  equipment.EquipmentData.EquipmentGarde;
        GetImage(ImagesType, (int)Images.TargetEquipmentGradeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipmentGrade].BgColor;
        GetImage(ImagesType, (int)Images.TargetEquipmentImage).sprite = Manager.ResourceM.Load<Sprite>(equipment.EquipmentData.SpriteName);
        GetText(TextsType, (int)Texts.TargetEquipmentLevelValueText).text = $"Lv. {equipment.Level}";

        GetImage(ImagesType, (int)Images.ResultEquipmentGradeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipmentGrade].BgColor;
        GetImage(ImagesType, (int)Images.ResultEquipmentImage).sprite = Manager.ResourceM.Load<Sprite>(equipment.EquipmentData.SpriteName);
        GetText(TextsType, (int)Texts.ResultEquipmentLevelText).text = $"Lv. 1";

        int grade = Utils.GetUpgradeNumber(equipmentGrade);
        if(grade == 0)
        {
            GetImage(ImagesType, (int)Images.TargetEquipmentEnforceBackgroundImage).gameObject.SetActive(false);
            GetImage(ImagesType, (int)Images.ResultEquipmentEnforceBackgroundImage).gameObject.SetActive(false);
            
        }
        else
        {
            GetImage(ImagesType, (int)Images.TargetEquipmentEnforceBackgroundImage).gameObject.SetActive(true);
            GetImage(ImagesType, (int)Images.TargetEquipmentEnforceBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipmentGrade].BorderColor;
            GetImage(ImagesType, (int)Images.ResultEquipmentEnforceBackgroundImage).gameObject.SetActive(false);
            GetImage(ImagesType, (int)Images.ResultEquipmentEnforceBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipmentGrade].BorderColor;
            GetText(TextsType, (int)Texts.TargetEnforceValueText).text = $"{grade}";
            GetText(TextsType, (int)Texts.ResultEnforceValueText).text = $"{grade}";
        }

        //장비에 들어간 골드
        int gold = CalculateResetGold();
        //GetImage(ImagesType, (int)Images.ResultGoldBackgroundImage).color = Define.EquipmentUIColors.MaterialGradeStyles[]
        GetText(TextsType, (int)Texts.ResultGoldCountValueText).text = $"x {gold}";

        //장비에 들어간 재료
        int material = CalculateResetMaterialCount();
        GetImage(ImagesType, (int)Images.ResultMaterialImage).sprite = Manager.ResourceM.Load<Sprite>(Manager.DataM.MaterialDic[equipment.EquipmentData.LevelUpMaterial].SpriteName);
        GetText(TextsType, (int)Texts.ResultMaterialCountValueText).text = $"x {material}";
    }

    void EquipmentDowngradeRefresh()
    {
        //TODO : 다운그레이드 리플래시
        // Epic1 -> epic으로 다운
        // ex epic->2 재료, 돈, Material
        Define.EquipmentGrade equipmentGrade = equipment.EquipmentData.EquipmentGarde;
        int grade = Utils.GetUpgradeNumber(equipmentGrade);
        
        //선택된 아이템
        GetImage(ImagesType, (int)Images.DowngradeTargetEquipmentGradeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipmentGrade].BgColor;
        GetImage(ImagesType, (int)Images.DowngradeTargetEquipmentImage).sprite = Manager.ResourceM.Load<Sprite>(equipment.EquipmentData.SpriteName);
        GetText(TextsType, (int)Texts.DowngradeTargetEquipmentLevelValueText).text = $"Lv. {equipment.Level}";
        GetImage(ImagesType, (int)Images.DowngradeTargetEquipmentEnforceBackgroundImage).gameObject.SetActive(true);
        GetImage(ImagesType, (int)Images.DowngradeTargetEquipmentEnforceBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipmentGrade].BorderColor;
        GetText(TextsType, (int)Texts.DowngradeTargetEnforceValueText).text = $"{grade}";

        //다운그레이드될 아이템
        GetImage(ImagesType, (int)Images.DowngradeEquipmentGradeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipmentGrade].BgColor;
        GetImage(ImagesType, (int)Images.DowngradeEquipmentImage).sprite = Manager.ResourceM.Load<Sprite>(equipment.EquipmentData.SpriteName);
        GetText(TextsType, (int)Texts.DowngradeEquipmentLevelText).text = $"Lv. 1";

        //grade에 따라 아이템 개수


        GetImage(ImagesType, (int)Images.DowngradEnchantStoneBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipmentGrade].BgColor;
        GetImage(ImagesType, (int)Images.DowngradEnchantStoneImage).sprite = Manager.ResourceM.Load<Sprite>(equipment.EquipmentData.SpriteName);
        GetText(TextsType, (int)Texts.DowngradEnchantStoneCountValueText).text = $"x {grade}";

        int gold = CalculateResetGold();
        GetText(TextsType, (int)Texts.DowngradeResultGoldCountValueText).text = $"x {gold}";

        //장비에 들어간 재료
        int material = CalculateResetMaterialCount();
        
        GetImage(ImagesType, (int)Images.DowngradeResultMaterialImage).sprite = Manager.ResourceM.Load<Sprite>(Manager.DataM.MaterialDic[equipment.EquipmentData.LevelUpMaterial].SpriteName);
        GetText(TextsType, (int)Texts.DowngradeResultMaterialCountValueText).text = $"x {material}";
    }

    void EquipmentResetPopupReset()
    {
        isSelectDownGradeTap = false;
        isSelectResetTap = false;

        GetObject(gameObjectsType, (int)GameObjects.DowngradeGroupObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.ResetInfoGroupObject).SetActive(false);
    }

    void OnClickResetTapToggle()
    {
        if (isSelectResetTap) return;
        EquipmentResetPopupReset();
        isSelectResetTap = true;
        GetObject(gameObjectsType, (int)GameObjects.ResetInfoGroupObject).SetActive(true);
        GetToggle(TogglesType, (int)Toggles.ResetTapToggle).isOn = true;
    }

    void OnClickDowngradeToggle()
    {
        if (isSelectDownGradeTap) return;
        EquipmentResetPopupReset();
        isSelectDownGradeTap = true;
        GetObject(gameObjectsType, (int)GameObjects.DowngradeGroupObject).SetActive(true);
        GetToggle(TogglesType, (int)Toggles.DowngradeTapToggle).isOn = true;
    }

    void OnClickBgButton()
    {
        Manager.SoundM.PlayPopupClose();
        OnClickResetTapToggle();
        gameObject.SetActive(false);
    }

    void OnClickResetButton()
    {
        
    }

    void OnClickDowngradeButton()
    {
        
    }

    int CalculateResetGold()
    {
        int gold = 0;
        for (int i = 1; i < equipment.Level; i++)
        {
            gold += Manager.DataM.EquipmentLevelDic[i].Cost_Gold;
        }

        return gold;
    }

    int CalculateResetMaterialCount()
    {
        int Material = 0;
        for (int i = 1; i < equipment.Level; i++)
        {
            Material += Manager.DataM.EquipmentLevelDic[i].Cost_Material;
        }
        return Material;

    }
}
