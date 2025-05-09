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
    UI_EquipmentInfoPopup equipmentInfoPopup;
    public UI_EquipmentInfoPopup EquipmentInfoPopup { get { return equipmentInfoPopup; } }
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
        equipmentInfoPopup = Manager.UiM.ShowPopup<UI_EquipmentInfoPopup>();
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
        equipmentInfoPopup.gameObject.SetActive(false);
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
}
