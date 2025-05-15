using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_LobbyScene : UI_Scene
{
    #region Enum
    public enum Toggles
    {
        EquipmentToggle,
        BattleToggle,
        ShopToggle
    }

    public enum Texts
    {

    }

    public enum GameObjects
    {

    }

    public enum Images
    {

    }

    #endregion
    
    UI_ShopPopup ui_ShopPopup;
    UI_BattlePopup ui_BattlePopup;
    UI_EquipmentPopup ui_EquipmentPopup;
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



        GetToggle(typeof(Toggles), (int)Toggles.EquipmentToggle).gameObject.BindEvent(OnClickEquipmentToggle);
        GetToggle(typeof(Toggles), (int)Toggles.BattleToggle).gameObject.BindEvent(OnClickBattleToggle);
        GetToggle(typeof(Toggles), (int)Toggles.ShopToggle).gameObject.BindEvent(OnClickShopToggle);

        ui_BattlePopup = Manager.UiM.ShowPopup<UI_BattlePopup>();
        ui_ShopPopup = Manager.UiM.ShowPopup<UI_ShopPopup>();
        ui_EquipmentPopup = Manager.UiM.ShowPopup<UI_EquipmentPopup>();
        ui_equipmentInfoPopup = Manager.UiM.ShowPopup<UI_EquipmentInfoPopup>();
        ui_MergePopup = Manager.UiM.ShowPopup<UI_MergePopup>();
        ui_MergeResultPopup = Manager.UiM.ShowPopup<UI_MergeResultPopup>();
        ui_MergeAllResultPopup = Manager.UiM.ShowPopup<UI_MergeAllResultPopup>();
        ui_EquipmentResetPopup = Manager.UiM.ShowPopup<UI_EquipmentResetPopup>();
        ui_RewardPopup = Manager.UiM.ShowPopup<UI_RewardPopup>();

        AllOff();

        Manager.GameM.OnResourcesChanged -= Refresh;
        Manager.GameM.OnResourcesChanged += Refresh;
        Refresh();

        return true;
    }


    void Refresh()
    {
        //TODO : 이거 다이아, 골드, 스테미너 변경할때 사용해주면 됌
    }

   

    void AllOff()
    {
        ui_BattlePopup.gameObject.SetActive(false);
        ui_ShopPopup.gameObject.SetActive(false);
        ui_EquipmentPopup.gameObject.SetActive(false);
        ui_equipmentInfoPopup.gameObject.SetActive(false);
        ui_MergePopup.gameObject.SetActive(false);
        ui_MergeResultPopup.gameObject.SetActive(false);
        ui_MergeAllResultPopup.gameObject.SetActive(false);
        ui_EquipmentResetPopup.gameObject.SetActive(false);
        ui_RewardPopup.gameObject.SetActive(false);
    }
    void OnClickEquipmentToggle()
    {
        AllOff();
        ui_EquipmentPopup.gameObject.SetActive(true);
        ui_EquipmentPopup.SetInfo();
    }

    void OnClickBattleToggle()
    {
        AllOff();
        ui_BattlePopup.gameObject.SetActive(true);
        //Manager.GameM.player.DataID = 1;
    }

    void OnClickShopToggle()
    {
        AllOff();
        ui_ShopPopup.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Manager.GameM.Gold += 10000;
            Manager.GameM.ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_WeaponScroll], 100);
        }
    }
}
