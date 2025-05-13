using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_MergeEquipItem : UI_Base
{
    enum Buttons
    {
        EquipmentGradeBackgroundImage
    }

    enum GameObjects
    {
        NewTextObject,
        EquipmentRedDotObject,
        EquippedObject,
        SelectObject,
        LockObject
    }


    enum Texts
    {
        EnforceValueText,
        EquipmentLevelValueText
    }

    enum Images
    {
        EquipmentGradeBackgroundImage,
        EquipmentImage,
        EquipmentTypeBackgroundImage,
        EquipmentTypeImage,
        EquipmentEnforceBackgroundImage,

    }


    public Equipment equipment;



    private void Awake()
    {
        Init();
    }


    public override bool Init()
    {
        if (!base.Init()) return false;
        gameObjectsType = typeof(GameObjects);
        ButtonsType = typeof(Buttons);
        TextsType = typeof(Texts);
        ImagesType = typeof(Images);

        BindObject(gameObjectsType);
        BindButton(ButtonsType);
        BindText(TextsType);
        BindImage(ImagesType);

        GetObject(gameObjectsType, (int)GameObjects.NewTextObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.EquipmentRedDotObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.EquippedObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.SelectObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.LockObject).SetActive(false);
        GetImage(ImagesType, (int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(false);

        GetButton(ButtonsType, (int)Buttons.EquipmentGradeBackgroundImage).gameObject.BindEvent(OnClickEquipmentItemButton);

        return true;
    }

    public void SetInfo(Equipment _item, Define.UI_ItemParentType _parentType, bool _isSelected = false, bool _isLock = false)
    {
        equipment = _item;
        transform.localScale = Vector3.one;
        GetImage(ImagesType, (int)Images.EquipmentGradeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipment.EquipmentData.EquipmentGarde].BgColor;
        GetImage(ImagesType, (int)Images.EquipmentTypeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipment.EquipmentData.EquipmentGarde].BorderColor;

        int grade = Utils.GetUpgradeNumber(equipment.EquipmentData.EquipmentGarde);

        if (grade == 0)
        {
            GetImage(ImagesType, (int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(false);
            GetText(TextsType, (int)Texts.EnforceValueText).text = $"";
        }
        else
        {
            GetImage(ImagesType, (int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(true);
            GetText(TextsType, (int)Texts.EnforceValueText).text = $"{grade}";
            GetImage(ImagesType, (int)Images.EquipmentEnforceBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipment.EquipmentData.EquipmentGarde].BorderColor;
        }
            


        GetImage(ImagesType, (int)Images.EquipmentImage).sprite = Manager.ResourceM.Load<Sprite>(equipment.EquipmentData.SpriteName);
        GetImage(ImagesType, (int)Images.EquipmentTypeImage).sprite = Manager.ResourceM.Load<Sprite>($"{equipment.EquipmentData.EquipmentType}_Icon.sprite");
        GetText(TextsType, (int)Texts.EquipmentLevelValueText).text = $"Lv. {equipment.Level}";
        GetObject(gameObjectsType, (int)GameObjects.EquipmentRedDotObject).SetActive(equipment.IsUpgradeable);
        GetObject(gameObjectsType, (int)GameObjects.NewTextObject).SetActive(equipment.IsConfirmed);
        GetObject(gameObjectsType, (int)GameObjects.EquippedObject).SetActive(equipment.IsEquiped);
        GetObject(gameObjectsType, (int)GameObjects.SelectObject).SetActive(_isSelected);
        GetObject(gameObjectsType, (int)GameObjects.LockObject).SetActive(_isLock);
    }

    void OnClickEquipmentItemButton()
    {
        Manager.SoundM.PlayButtonClick();

        (Manager.UiM.SceneUI as UI_LobbyScene).Ui_MergePopup.SetMergeItem(equipment);
    }
}
