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
        AlreadyDieConfirmButton
    }
    enum Texts
    {
        BackToBattleContentText
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
        TextsType = typeof(Texts);

        BindObject(gameObjectsType);
        BindButton(ButtonsType);
        BindText(TextsType);


        GetButton(ButtonsType, (int)Buttons.CancelButton).gameObject.BindEvent(OnClickBackButton);
        GetButton(ButtonsType, (int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
        GetButton(ButtonsType, (int)Buttons.AlreadyDieConfirmButton).gameObject.BindEvent(OnClickAlreadyDieConfirmButton);

        GetButton(ButtonsType, (int)Buttons.CancelButton).gameObject.SetActive(false);
        GetButton(ButtonsType, (int)Buttons.ConfirmButton).gameObject.SetActive(false);
        GetButton(ButtonsType, (int)Buttons.AlreadyDieConfirmButton).gameObject.SetActive(false);

        Refresh();
        return true;
    }
    void Refresh()
    {
        if (Manager.GameM.ContinueDatas.IsDead)
        {
            GetText(TextsType, (int)Texts.BackToBattleContentText).text = "사망하였습니다. 다시 시작해주세요.";
            GetButton(ButtonsType, (int)Buttons.AlreadyDieConfirmButton).gameObject.SetActive(true);
        }
        else
        {
            GetText(TextsType, (int)Texts.BackToBattleContentText).text = "진행중인 전투가 있습니다.            계속하시겠습니까? ";
            GetButton(ButtonsType, (int)Buttons.CancelButton).gameObject.SetActive(true);
            GetButton(ButtonsType, (int)Buttons.ConfirmButton).gameObject.SetActive(true);
        }
            
    }

    void OnClickConfirmButton()
    {
        Manager.SoundM.PlayPopupClose();
        Manager.SceneM.LoadScene(Define.SceneType.GameScene, transform);
    }

    void OnClickBackButton()
    {
        Manager.SoundM.PlayPopupClose();
        Manager.GameM.ClearContinueData();
        Manager.UiM.ClosePopup(this);
    }

    void OnClickAlreadyDieConfirmButton()
    {
        Manager.SoundM.PlayPopupClose();
        Manager.GameM.ClearContinueData();
        Manager.UiM.ClosePopup(this);
    }
}
