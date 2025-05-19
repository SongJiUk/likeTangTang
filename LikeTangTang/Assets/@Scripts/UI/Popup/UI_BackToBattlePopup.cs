using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BackToBattlePopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject
    }

    enum Buttons
    { 
        CancelButton,
        ConfirmButton,

    }


    private void Awake()
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


        GetButton(ButtonsType, (int)Buttons.CancelButton).gameObject.BindEvent(OnClickBackButton);
        GetButton(ButtonsType, (int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
        return true;
    }

    void OnClickConfirmButton()
    {
        Manager.SoundM.PlayPopupClose();
        Manager.SceneM.LoadScene(Define.SceneType.GameScene, transform);
    }

    void OnClickBackButton()
    {
        Manager.SoundM.PlayPopupClose();
        Manager.UiM.ClosePopup(this);
    }
}
