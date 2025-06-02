using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BeginnerSupportRewardPopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject
    }
    enum Buttons
    {
        BackgroundButton
    }

    public void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;

        gameObjectsType = typeof(GameObjects);
        ButtonsType = typeof(Buttons);

        BindObject(gameObjectsType);
        BindButton(ButtonsType);

        GetButton(ButtonsType, (int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBgButton);

        return true;
    }

    void OnClickBgButton()
    {
        Manager.SoundM.PlayPopupClose();
        Manager.UiM.ClosePopup(this);
    }
}
