using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UI_LobbyScene : UI_Scene
{
    #region Enum
    public enum Toggles
    {
        EquipmentToggle,
        BattleToggle,
        ShopToggle,
        EvolutionToggle
    }

    public enum Texts
    {
        EquipmentToggleText,
        BattleToggleText,
        ShopToggleText,
        EvolutionToggleText
    }

    public enum GameObjects
    {
        CheckEquipmentImageObject,
        CheckBattleImageObject,
        CheckShopImageObject,
        CheckEvolutionImageObject
    }

    public enum Images
    {
        BackGroundImage
    }

    #endregion

    Vector2 ClickedToggleSize = new Vector2(280, 150);
    Vector2 ClickedImageSize = new Vector2(200, 200);
    Vector2 OriginToggleSize = new Vector2(200, 155);
    Vector2 OriginImageSize = new Vector2(200, 150);

    RectTransform EvolutionImageRect;
    RectTransform EquipmentImageRect;
    RectTransform BattleImageRect;
    RectTransform ShopImageRect;

    RectTransform EvolutionToggleRect;
    RectTransform EquipmentToggleRect;
    RectTransform BattleToggleRect;
    RectTransform ShopToggleRect;
    
    UI_ShopPopup ui_ShopPopup;
    UI_BattlePopup ui_BattlePopup;
    UI_EquipmentPopup ui_EquipmentPopup;
    UI_EvolutionPopup ui_EvolutionPopup;
    UI_EquipmentInfoPopup ui_equipmentInfoPopup;
    UI_MergePopup ui_MergePopup;
    UI_MergeResultPopup ui_MergeResultPopup;
    UI_MergeAllResultPopup ui_MergeAllResultPopup;
    UI_EquipmentResetPopup ui_EquipmentResetPopup;
    UI_RewardPopup ui_RewardPopup;
    public UI_EquipmentPopup Ui_EquipmentPopup { get { return ui_EquipmentPopup; } }
    public UI_EquipmentInfoPopup Ui_EquipmentInfoPopup { get { return ui_equipmentInfoPopup; } }
    public UI_MergePopup Ui_MergePopup { get { return ui_MergePopup; } }
    public UI_MergeResultPopup Ui_MergeResultPopup { get { return ui_MergeResultPopup; } }
    public UI_MergeAllResultPopup Ui_MergeAllResultPopup { get { return ui_MergeAllResultPopup; } }
    public UI_EquipmentResetPopup Ui_EquipmentResetPopup { get { return ui_EquipmentResetPopup; } }
    public UI_RewardPopup Ui_RewardPopup { get { return ui_RewardPopup; } }

    private void OnDestroy()
    {
        if(Manager.GameM != null)
            Manager.GameM.OnResourcesChanged -= Refresh;
    }

    public override bool Init()
    {
        gameObjectsType = typeof(GameObjects);
        TogglesType = typeof(Toggles);
        TextsType = typeof(Texts);
        ImagesType = typeof(Images);

        BindObject(gameObjectsType);
        BindToggle(TogglesType);
        BindText(TextsType);
        BindImage(ImagesType);

        
        GetToggle(TogglesType, (int)Toggles.EquipmentToggle).gameObject.BindEvent(OnClickEquipmentToggle);
        GetToggle(TogglesType, (int)Toggles.BattleToggle).gameObject.BindEvent(OnClickBattleToggle);
        GetToggle(TogglesType, (int)Toggles.ShopToggle).gameObject.BindEvent(OnClickShopToggle);
        GetToggle(TogglesType, (int)Toggles.EvolutionToggle).gameObject.BindEvent(OnClickEvolutionToggle);

        EquipmentImageRect = GetObject(gameObjectsType, (int)GameObjects.CheckEquipmentImageObject).GetComponent<RectTransform>();
        BattleImageRect = GetObject(gameObjectsType, (int)GameObjects.CheckBattleImageObject).GetComponent<RectTransform>();
        ShopImageRect = GetObject(gameObjectsType, (int)GameObjects.CheckShopImageObject).GetComponent<RectTransform>();
        EvolutionImageRect = GetObject(gameObjectsType, (int)GameObjects.CheckEvolutionImageObject).GetComponent<RectTransform>();

        EquipmentToggleRect = GetToggle(TogglesType, (int)Toggles.EquipmentToggle).GetComponent<RectTransform>();
        BattleToggleRect = GetToggle(TogglesType, (int)Toggles.BattleToggle).GetComponent<RectTransform>();
        ShopToggleRect = GetToggle(TogglesType, (int)Toggles.ShopToggle).GetComponent<RectTransform>();
        EvolutionToggleRect = GetToggle(TogglesType, (int)Toggles.EvolutionToggle).GetComponent<RectTransform>();

        ui_BattlePopup = Manager.UiM.ShowPopup<UI_BattlePopup>();
        ui_ShopPopup = Manager.UiM.ShowPopup<UI_ShopPopup>();
        ui_EquipmentPopup = Manager.UiM.ShowPopup<UI_EquipmentPopup>();
        ui_EvolutionPopup = Manager.UiM.ShowPopup<UI_EvolutionPopup>();
        ui_equipmentInfoPopup = Manager.UiM.ShowPopup<UI_EquipmentInfoPopup>();
        ui_MergePopup = Manager.UiM.ShowPopup<UI_MergePopup>();
        ui_MergeResultPopup = Manager.UiM.ShowPopup<UI_MergeResultPopup>();
        ui_MergeAllResultPopup = Manager.UiM.ShowPopup<UI_MergeAllResultPopup>();
        ui_EquipmentResetPopup = Manager.UiM.ShowPopup<UI_EquipmentResetPopup>();
        ui_RewardPopup = Manager.UiM.ShowPopup<UI_RewardPopup>();



        AllOff();
        OnClickBattleToggle();

        
        Manager.GameM.OnResourcesChanged += Refresh;
        Refresh();

        return true;
    }


    void Refresh()
    {
        //TODO : 이거 다이아, 골드, 스테미너 변경할때 사용해주면 됌

        Manager.UiM.CheckRedDotObject(Define.RedDotObjectType.Mission);
        Manager.UiM.CheckRedDotObject(Define.RedDotObjectType.AchievementPopup);
    }

   

    void AllOff()
    {
        ui_EquipmentPopup.isOpen = false;
        ui_BattlePopup.isOpen = false;
        ui_ShopPopup.isOpen = false;
        ui_EvolutionPopup.isOpen = false;

        ui_BattlePopup.gameObject.SetActive(false);
        ui_ShopPopup.gameObject.SetActive(false);
        ui_EquipmentPopup.gameObject.SetActive(false);
        ui_EvolutionPopup.gameObject.SetActive(false);
        ui_equipmentInfoPopup.gameObject.SetActive(false);
        ui_MergePopup.gameObject.SetActive(false);
        ui_MergeResultPopup.gameObject.SetActive(false);
        ui_MergeAllResultPopup.gameObject.SetActive(false);
        ui_EquipmentResetPopup.gameObject.SetActive(false);
        ui_RewardPopup.gameObject.SetActive(false);
    }

    void TooglesInit()
    {
        GetObject(gameObjectsType, (int)GameObjects.CheckEquipmentImageObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.CheckBattleImageObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.CheckShopImageObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.CheckEvolutionImageObject).SetActive(false);

        EquipmentImageRect.sizeDelta = OriginImageSize;
        BattleImageRect.sizeDelta = OriginImageSize;
        ShopImageRect.sizeDelta = OriginImageSize;
        EvolutionImageRect.sizeDelta = OriginImageSize;

        GetText(TextsType, (int)Texts.EquipmentToggleText).gameObject.SetActive(false);
        GetText(TextsType, (int)Texts.BattleToggleText).gameObject.SetActive(false);
        GetText(TextsType, (int)Texts.ShopToggleText).gameObject.SetActive(false);
        GetText(TextsType, (int)Texts.EvolutionToggleText).gameObject.SetActive(false);

        EquipmentToggleRect.sizeDelta = OriginToggleSize;
        BattleToggleRect.sizeDelta = OriginToggleSize;
        ShopToggleRect.sizeDelta = OriginToggleSize;
        EvolutionToggleRect.sizeDelta = OriginToggleSize;

    }

   

    void OnClickEquipmentToggle()
    {
        if(!ui_EquipmentPopup.isOpen)
        {
            Manager.SoundM.PlayButtonClick();

            GetImage(ImagesType, (int)Images.BackGroundImage).color = Utils.HexToColor("5C3A25");

            AllOff();
            TooglesInit();


            ui_EquipmentPopup.gameObject.SetActive(true);

            EquipmentToggleRect.sizeDelta = ClickedToggleSize;
            GetText(TextsType, (int)Texts.EquipmentToggleText).gameObject.SetActive(true);
            GetObject(gameObjectsType, (int)GameObjects.CheckEquipmentImageObject).SetActive(true);
            EquipmentImageRect.DOSizeDelta(ClickedImageSize, 0.1f).SetEase(Ease.InOutQuad);

            ui_EquipmentPopup.SetInfo();
            
            ui_EquipmentPopup.isOpen = true;

        }
        
    }

    void OnClickBattleToggle()
    {
        Manager.SoundM.PlayButtonClick();

        if (!ui_BattlePopup.isOpen)
        {
            GetImage(ImagesType, (int)Images.BackGroundImage).color = Utils.HexToColor("1F90A0");
            AllOff();
            TooglesInit();

            ui_BattlePopup.gameObject.SetActive(true);

            BattleToggleRect.sizeDelta = ClickedToggleSize;
            GetText(TextsType, (int)Texts.BattleToggleText).gameObject.SetActive(true);
            GetObject(gameObjectsType, (int)GameObjects.CheckBattleImageObject).SetActive(true);
            BattleImageRect.DOSizeDelta(ClickedImageSize, 0.1f).SetEase(Ease.InOutQuad);


            ui_BattlePopup.isOpen = true;

            
        }

        
        //Manager.GameM.player.DataID = 1;
    }

    void OnClickShopToggle()
    {
        Manager.SoundM.PlayButtonClick();
        if (!ui_ShopPopup.isOpen)
        {
            GetImage(ImagesType, (int)Images.BackGroundImage).color = Utils.HexToColor("A7AAC3");

            AllOff();
            TooglesInit();
            ui_ShopPopup.gameObject.SetActive(true);

            ShopToggleRect.sizeDelta = ClickedToggleSize;
            GetText(TextsType, (int)Texts.ShopToggleText).gameObject.SetActive(true);
            GetObject(gameObjectsType, (int)GameObjects.CheckShopImageObject).SetActive(true);
            ShopImageRect.DOSizeDelta(ClickedImageSize, 0.1f).SetEase(Ease.InOutQuad);

            ui_ShopPopup.isOpen = true;

        }
    }

    void OnClickEvolutionToggle()
    {
        Manager.SoundM.PlayButtonClick();
        if (!ui_EvolutionPopup.isOpen)
        {
            GetImage(ImagesType, (int)Images.BackGroundImage).color = Utils.HexToColor("46325D");

            AllOff();
            TooglesInit();
            ui_EvolutionPopup.gameObject.SetActive(true);

            EvolutionToggleRect.sizeDelta = ClickedToggleSize;
            GetText(TextsType, (int)Texts.EvolutionToggleText).gameObject.SetActive(true);
            GetObject(gameObjectsType, (int)GameObjects.CheckEvolutionImageObject).SetActive(true);
            EvolutionImageRect.DOSizeDelta(ClickedImageSize, 0.1f).SetEase(Ease.InOutQuad);

            ui_EvolutionPopup.isOpen = true;

        }
    }

    void ShowUI(GameObject _contentPopup, Toggle _toggle, TMP_Text text, GameObject _obj, float _duration = 0.2f)
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Manager.GameM.Stamina += 10;
            Manager.GameM.Gold += 10000;
            Manager.GameM.Dia += 10000;
            Manager.GameM.ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_LevelUpCoupon], 10000);
            Manager.GameM.ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_WeaponScroll], 100);
        }
    }
}
