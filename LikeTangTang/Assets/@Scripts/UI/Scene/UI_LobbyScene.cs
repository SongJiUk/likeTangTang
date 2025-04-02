using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_LobbyScene : UI_Base
{
    #region Enum
    public enum Toggles
    {
        EquipmentToggle,
        BattleToggle,
        ShopToggle
    }
    #endregion

    UI_ShopPopup ui_ShopPopup;
    UI_BattlePopup ui_BattlePopup;
    UI_EquipmentPopup ui_EquipmentPopup;

    public override bool Init()
    {
        SetUIInfo();

        GetToggle(typeof(Toggles), (int)Toggles.EquipmentToggle).gameObject.BindEvent(OnClickEquipmentToggle);
        GetToggle(typeof(Toggles), (int)Toggles.BattleToggle).gameObject.BindEvent(OnClickBattleToggle);
        GetToggle(typeof(Toggles), (int)Toggles.ShopToggle).gameObject.BindEvent(OnClickShopToggle);

        ui_BattlePopup = Manager.UiM.ShowPopup<UI_BattlePopup>();
        ui_ShopPopup = Manager.UiM.ShowPopup<UI_ShopPopup>();
        ui_EquipmentPopup = Manager.UiM.ShowPopup<UI_EquipmentPopup>();
        
        AllOff();

        return true;
    }

    protected override void SetUIInfo()
    {
        Bind<Toggle>(typeof(Toggles));
    }

    void AllOff()
    {
        ui_BattlePopup.gameObject.SetActive(false);
        ui_ShopPopup.gameObject.SetActive(false);
        ui_EquipmentPopup.gameObject.SetActive(false);
    }
    void OnClickEquipmentToggle()
    {
        AllOff();
        ui_EquipmentPopup.gameObject.SetActive(true);
    }

    void OnClickBattleToggle()
    {
        AllOff();
        ui_BattlePopup.gameObject.SetActive(true);
        Manager.GameM.player.DataID = 1;
    }

    void OnClickShopToggle()
    {
        AllOff();
        ui_ShopPopup.gameObject.SetActive(true);
    }
}
