using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PausePopup : UI_Popup
{
    enum Buttons
    {
        ResumeButton,
        StatisticsButton,
        HomeButton,
        SoundButton,
        SettingButton

    }

    enum GameObjects
    {
        ContentObject
    }

    enum Images
    {
        SoundIconImage
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
        ImagesType = typeof(Images);

        BindObject(gameObjectsType);
        BindButton(ButtonsType);
        BindImage(ImagesType);

        GetButton(ButtonsType, (int)Buttons.ResumeButton).gameObject.BindEvent(OnClickResumeButton);
        GetButton(ButtonsType, (int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
        GetButton(ButtonsType, (int)Buttons.HomeButton).gameObject.BindEvent(OnClickHomeButton);
        GetButton(ButtonsType, (int)Buttons.SoundButton).gameObject.BindEvent(SoundButton);
        GetButton(ButtonsType, (int)Buttons.SettingButton).gameObject.BindEvent(SettingButton);
        SetButtonImage();
        return true;
    }


    void OnClickResumeButton()
    {
        Manager.UiM.ClosePopup(this);
    }

    void OnClickStatisticsButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.UiM.ShowPopup<UI_TotalDamagePopup>().SetInfo();
    }

    void OnClickHomeButton()
    {
        Manager.SoundM.PlayButtonClick();

        Manager.UiM.ShowPopup<UI_BackToHomePopup>();
    }

    void SoundButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.SoundM.ToggleSound();
        SetButtonImage();
    }

    void SetButtonImage()
    {
        if (Manager.SoundM.IsSoundOn)
            GetImage(ImagesType, (int)Images.SoundIconImage).sprite = Manager.ResourceM.Load<Sprite>("Ui_Volume_Icon.sprite");
        else
            GetImage(ImagesType, (int)Images.SoundIconImage).sprite = Manager.ResourceM.Load<Sprite>("Ui_Volume_Mute_Icon.sprite");
    }

    void SettingButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.UiM.ShowPopup<UI_SettingPopup>();
    }
}
