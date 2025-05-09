using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class UI_EquipItem : UI_Base
{
    enum Buttons
    {
        EquipmentGradeBackgroundImage
    }
    enum GameObjects
    {
        NewTextObject,
        SpecialImage,
        LockObject,
        EquippedObject,
        EquipmentRedDotObject,
        EquipmentEnforceBackgroundImage,
        EquipmentTypeBackgroundImage,
        GetEffectObject,
    }

    enum Texts
    {
        EquipmentLevelValueText,
        EnforceValueText,

    }

    enum Images
    {
        EquipmentGradeBackgroundImage,
        EquipmentImage,
        EquipmentTypeImage,
        EquipmentEnforceBackgroundImage,
        EquipmentTypeBackgroundImage

    }


    public Equipment Equipment;
    public Action OnClickEquipItem;
    Define.UI_ItemParentType parentType;
    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;

        gameObjectsType = typeof(GameObjects);
        TextsType = typeof(Texts);
        ImagesType = typeof(Images);
        ButtonsType = typeof(Buttons);

        BindObject(gameObjectsType);
        BindText(TextsType);
        BindImage(ImagesType);
        BindButton(ButtonsType);

        GetButton(ButtonsType, (int)Buttons.EquipmentGradeBackgroundImage).gameObject.BindEvent(OnClickEquipItemButton);
        GetObject(gameObjectsType, (int)GameObjects.SpecialImage).SetActive(false);

        return true;


    }

    public void SetInfo(Equipment _item, Define.UI_ItemParentType _parentType)
    {
        Equipment = _item;
        parentType = _parentType;
        var style = Define.EquipmentUIColors.EquipGradeStyles[Equipment.EquipmentData.EquipmentGarde];
        //TODO : 이미지.style
        GetImage(ImagesType, (int)Images.EquipmentGradeBackgroundImage).color = style.BorderColor;
        GetImage(ImagesType, (int)Images.EquipmentTypeBackgroundImage).color = style.BorderColor;
        GetImage(ImagesType, (int)Images.EquipmentEnforceBackgroundImage).color = style.BgColor;

        #region Epic1 -> 1 Unique2 -> 2
        int num = Utils.GetUpgradeNumber(Equipment.EquipmentData.EquipmentGarde);

        if (num == 0)
        {
            GetText(TextsType, (int)Texts.EnforceValueText).text = "";
            GetImage(ImagesType, (int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(false);
        }
        else
        {
            GetText(TextsType, (int)Texts.EnforceValueText).text = num.ToString();
            GetImage(ImagesType, (int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(true);
        }
        #endregion

        //장비 아이템
        GetImage(ImagesType, (int)Images.EquipmentImage).sprite = Manager.ResourceM.Load<Sprite>(Equipment.EquipmentData.SpriteName);
        GetImage(ImagesType, (int)Images.EquipmentTypeImage).sprite = Manager.ResourceM.Load<Sprite>($"{Equipment.EquipmentData.EquipmentType}_Icon.sprite");
        GetText(TextsType, (int)Texts.EquipmentLevelValueText).text = $"Lv. {Equipment.Level}";
        //강화 가능할때 
        GetObject(gameObjectsType, (int)GameObjects.EquipmentRedDotObject).SetActive(Equipment.IsUpgradeable);
        //처음 획득시
        GetObject(gameObjectsType, (int)GameObjects.NewTextObject).SetActive(!Equipment.IsConfirmed);
        GetObject(gameObjectsType, (int)GameObjects.EquippedObject).SetActive(Equipment.IsEquiped);
        GetObject(gameObjectsType, (int)GameObjects.LockObject).SetActive(Equipment.IsUnvailable);

        if(parentType == Define.UI_ItemParentType.CharacterEquipment)
        {
            GetObject(gameObjectsType, (int)GameObjects.EquippedObject).SetActive(false);
        }
    }

    public void OnClickEquipItemButton()
    {
        //Manager.SoundM.PlayButtonClick();
        
        if(parentType == Define.UI_ItemParentType.GachaResultPopup)
        {
            //Manager.UiM.ShowPopup<UI_GhachaEquipmentInfoPopup>().SetInfo(Equipment);
        }
        else
        {
            Equipment.IsConfirmed = true;
            Manager.GameM.SaveGame();

            UI_EquipmentInfoPopup infoPopup = (Manager.UiM.SceneUI as UI_LobbyScene).EquipmentInfoPopup;
            if(infoPopup != null)
            {
                infoPopup.SetInfo(Equipment);
                infoPopup.gameObject.SetActive(true);
            }
            
        }
    }

}
