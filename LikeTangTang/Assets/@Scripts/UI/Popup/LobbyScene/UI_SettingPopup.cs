using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SettingPopup : UI_Popup
{
    
    enum Buttons
    {
        BackgroundButton,
        SoundEffectOffButton,
        SoundEffectOnButton,
        BackgroundSoundOffButton,
        BackgroundSoundOnButton,
        //JoystickFixedOffButton,
        //JoystickFixedOnButton
    }

    enum GameObjects
    {
        ContentObject
    }

    enum Texts
    {
        VersionValueText
    }

    private void OnEnable()
    {
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.ContentObject));
    }

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;
        ButtonsType = typeof(Buttons);
        gameObjectsType = typeof(GameObjects);
        TextsType = typeof(Texts);


        BindButton(ButtonsType);
        BindObject(gameObjectsType);
        BindText(TextsType);

        GetButton(ButtonsType, (int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBgButton);
        GetButton(ButtonsType, (int)Buttons.BackgroundSoundOffButton).gameObject.BindEvent(OnClickBackgrounSoundOnButton);
        GetButton(ButtonsType, (int)Buttons.BackgroundSoundOnButton).gameObject.BindEvent(OnClickBackgrounSoundOffButton);
        //GetButton(ButtonsType, (int)Buttons.JoystickFixedOffButton).gameObject.BindEvent(OnClickJoystickFixedOnButton);
        //GetButton(ButtonsType, (int)Buttons.JoystickFixedOnButton).gameObject.BindEvent(OnClickJoystickFixedOffButton);
        GetButton(ButtonsType, (int)Buttons.SoundEffectOffButton).gameObject.BindEvent(OnClickSoundEffectOnButton);
        GetButton(ButtonsType, (int)Buttons.SoundEffectOnButton).gameObject.BindEvent(OnClickSoundEffectOffButton);

        GetText(TextsType, (int)Texts.VersionValueText).text = $"버전 : {Application.version}";

        if (Manager.GameM.BGMOn)
            OnClickBackgrounSoundOnButton();
        else
            OnClickBackgrounSoundOffButton();

        if (Manager.GameM.EffectSoundOn)
            OnClickSoundEffectOnButton();
        else
            OnClickSoundEffectOffButton();

        //if (Manager.GameM.JoyStickType == Define.JoyStickType.Fixed)
        //    OnClickJoystickFixedOnButton();
        //else
        //    OnClickJoystickFixedOffButton();

        return true;
    }

    void OnClickBgButton()
    {
        Manager.SoundM.PlayPopupClose();
        Manager.UiM.ClosePopup(this);
    }


    void OnClickBackgrounSoundOnButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.GameM.BGMOn = true;
        GetButton(ButtonsType, (int)Buttons.BackgroundSoundOnButton).gameObject.SetActive(true);
        GetButton(ButtonsType, (int)Buttons.BackgroundSoundOffButton).gameObject.SetActive(false);
    }
    void OnClickBackgrounSoundOffButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.GameM.BGMOn = false;
        GetButton(ButtonsType, (int)Buttons.BackgroundSoundOnButton).gameObject.SetActive(false);
        GetButton(ButtonsType, (int)Buttons.BackgroundSoundOffButton).gameObject.SetActive(true);
    }

    void OnClickJoystickFixedOnButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.GameM.JoyStickType = Define.JoyStickType.Fixed;
        //GetButton(ButtonsType, (int)Buttons.JoystickFixedOnButton).gameObject.SetActive(true);
        //GetButton(ButtonsType, (int)Buttons.JoystickFixedOffButton).gameObject.SetActive(false);
    }
    void OnClickJoystickFixedOffButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.GameM.JoyStickType = Define.JoyStickType.Flexible;
        //GetButton(ButtonsType, (int)Buttons.JoystickFixedOnButton).gameObject.SetActive(false);
        //GetButton(ButtonsType, (int)Buttons.JoystickFixedOffButton).gameObject.SetActive(true);
    }

   
    void OnClickSoundEffectOnButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.GameM.EffectSoundOn = true;
        GetButton(ButtonsType, (int)Buttons.SoundEffectOnButton).gameObject.SetActive(true);
        GetButton(ButtonsType, (int)Buttons.SoundEffectOffButton).gameObject.SetActive(false);
    }

    void OnClickSoundEffectOffButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.GameM.EffectSoundOn = false;
        GetButton(ButtonsType, (int)Buttons.SoundEffectOnButton).gameObject.SetActive(false);
        GetButton(ButtonsType, (int)Buttons.SoundEffectOffButton).gameObject.SetActive(true);
    }

   

}
