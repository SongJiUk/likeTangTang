using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_MergeResultPopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,
        ImprovATKObject,
        ImprovHPObject,

    }

    enum Images
    {
        EquipmentGradeBackgroundImage,
        EquipmentImage,
        EquipmentTypeBackgroundImage,
        EquipmentTypeImage,
        EquipmentEnforceBackgroundImage,

    }

    enum Buttons
    {
        BackgroundButton,

    }

    enum Texts
    {
        EquipmentNameValueText,
        EquipmentGradeValueText,
        EquipmentLevelValueText,
        EnforceValueText,
        BeforeLevelValueText,
        AfterLevelValueText,
        BeforeATKValueText,
        AfterATKValueText,
        BeforeHPValueText,
        AfterHPValueText,
        ImprovOptionValueText,

    }

    Equipment beforeEquipment;
    Equipment newEquipment;
    Action closeAction;
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
        ImagesType = typeof(Images);
        ButtonsType = typeof(Buttons);
        TextsType = typeof(Texts);

        BindObject(gameObjectsType);
        BindImage(ImagesType);
        BindButton(ButtonsType);
        BindText(TextsType);

        GetObject(gameObjectsType, (int)GameObjects.ImprovATKObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.ImprovHPObject).SetActive(false);
        GetImage(ImagesType, (int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(false);

        GetButton(ButtonsType, (int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackButton);
        return true;
    }

    public void SetInfo(Equipment _beforeEquipment, Equipment _newEquipment, Action _callBack = null)
    {
        beforeEquipment = _beforeEquipment;
        newEquipment = _newEquipment;
        closeAction = _callBack;
        Refresh();
    }

    void Refresh()
    {
        if (beforeEquipment == null) return;
        if (newEquipment == null) return;

        GetText(TextsType, (int)Texts.EquipmentGradeValueText).text = $"{newEquipment.EquipmentData.EquipmentGarde}";
        GetText(TextsType, (int)Texts.EquipmentNameValueText).text = newEquipment.EquipmentData.NameTextID;

        GetImage(ImagesType, (int)Images.EquipmentTypeBackgroundImage).gameObject.SetActive(true);
        GetImage(ImagesType, (int)Images.EquipmentTypeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[newEquipment.EquipmentData.EquipmentGarde].BorderColor;
        GetImage(ImagesType, (int)Images.EquipmentGradeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[newEquipment.EquipmentData.EquipmentGarde].BorderColor;
        GetImage(ImagesType, (int)Images.EquipmentTypeImage).sprite = Manager.ResourceM.Load<Sprite>($"{newEquipment.EquipmentData.EquipmentType}_Icon.sprite");
        GetImage(ImagesType, (int)Images.EquipmentImage).sprite = Manager.ResourceM.Load<Sprite>(newEquipment.EquipmentData.SpriteName);
        GetText(TextsType, (int)Texts.EquipmentLevelValueText).text = $"Lv. {newEquipment.Level}";

        int grade = Utils.GetUpgradeNumber(newEquipment.EquipmentData.EquipmentGarde);
        if(grade == 0)
        {
            GetText(TextsType, (int)Texts.EnforceValueText).text = "";
            GetImage(ImagesType, (int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(false);
        }
        else
        {
            GetText(TextsType, (int)Texts.EnforceValueText).text = grade.ToString();
            GetImage(ImagesType, (int)Images.EquipmentEnforceBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[newEquipment.EquipmentData.EquipmentGarde].BorderColor;
            GetImage(ImagesType, (int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(true);
        }

        GetText(TextsType, (int)Texts.BeforeLevelValueText).text = $"{beforeEquipment.EquipmentData.Grade_MaxLevel}";
        GetText(TextsType, (int)Texts.AfterLevelValueText).text = $"{newEquipment.EquipmentData.Grade_MaxLevel}";

        if (newEquipment.EquipmentData.Grade_Attack != 0)
        {
            GetObject(gameObjectsType, (int)GameObjects.ImprovATKObject).SetActive(true);
            GetObject(gameObjectsType, (int)GameObjects.ImprovHPObject).SetActive(false);
            GetText(TextsType, (int)Texts.BeforeATKValueText).text = $"{beforeEquipment.EquipmentData.Grade_Attack}";
            GetText(TextsType, (int)Texts.AfterATKValueText).text = $"{newEquipment.EquipmentData.Grade_Attack}";
        }
        else
        {  
            GetObject(gameObjectsType, (int)GameObjects.ImprovATKObject).SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.ImprovHPObject).SetActive(true);
            GetText(TextsType, (int)Texts.BeforeHPValueText).text = $"{beforeEquipment.EquipmentData.Grade_Hp}";
            GetText(TextsType, (int)Texts.AfterHPValueText).text = $"{newEquipment.EquipmentData.Grade_Hp}";
        }
        
    }

    void OnClickBackButton()
    {
        Manager.SoundM.PlayPopupClose();
        closeAction?.Invoke();
        gameObject.SetActive(false);
    }
}
