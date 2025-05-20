using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattlePopup : UI_Popup
{

    public enum GameObjects
    {
        SettingButtonRedDotObject,
        AttendanceCheckButtonRedDotObject,
        MissionButtonRedDotObject,
        AchievementButtonRedDotObject,
        OfflineRewardButtonRedDotObject,
        FirstClearRedDotObject,
        SecondClearRedDotObject,
        ThirdClearRedDotObject
    }

    public enum Buttons
    {
        SettingButton,
        AttendanceCheckButton,
        MissionButton,
        AchievementButton,
        StageSelectButton,
        FirstClearRewardButton,
        SecondClearRewardButton,
        ThirdClearRewardButton,
        GameStartButton,
        OfflineRewardButton
    }

    public enum Texts
    {
        StageNameText,
        SurvivalWaveValueText
    }

    private void Awake()
    {
        Init();
    }

    //TODO : 여기서 체크
    private void OnEnable()
    {
        StartCoroutine(CoCheckPopup());
        
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

        GetObject(gameObjectsType, (int)GameObjects.AchievementButtonRedDotObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.AttendanceCheckButtonRedDotObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.MissionButtonRedDotObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.OfflineRewardButtonRedDotObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.SettingButtonRedDotObject).SetActive(false);
     

        GetButton(ButtonsType, (int)Buttons.SettingButton).gameObject.BindEvent(OnClickSettingButton);
        GetButton(ButtonsType, (int)Buttons.AttendanceCheckButton).gameObject.BindEvent(OnClickAttendanceCheckButton);
        GetButton(ButtonsType, (int)Buttons.MissionButton).gameObject.BindEvent(OnClickMissionButton);
        GetButton(ButtonsType, (int)Buttons.AchievementButton).gameObject.BindEvent(OnClickAchievementButton);
        GetButton(ButtonsType, (int)Buttons.StageSelectButton).gameObject.BindEvent(OnClickStageSelectButton);
        GetButton(ButtonsType, (int)Buttons.FirstClearRewardButton).gameObject.BindEvent(OnClickFirstClearRewardButton);
        GetButton(ButtonsType, (int)Buttons.SecondClearRewardButton).gameObject.BindEvent(OnClickSecondClearRewardButton);
        GetButton(ButtonsType, (int)Buttons.ThirdClearRewardButton).gameObject.BindEvent(OnClickThirdClearRewardButton);
        GetButton(ButtonsType, (int)Buttons.GameStartButton).gameObject.BindEvent(OnClickGameStartButton);
        GetButton(ButtonsType, (int)Buttons.OfflineRewardButton).gameObject.BindEvent(OnClickOfflineRewardButton);


        return true;
    }

    IEnumerator CoCheckPopup()
    {
        yield return new WaitForEndOfFrame();
        if (PlayerPrefs.GetInt("ISFIRST") == 1)
        {
            //TODO :Managers.UI.ShowPopupUI<UI_BeginnerSupportRewardPopup>();
            PlayerPrefs.SetInt("ISFIRST", 0);
            PlayerPrefs.Save();
        }

        if (Manager.GameM.ContinueDatas.isContinue)
            Manager.UiM.ShowPopup<UI_BackToBattlePopup>();
        else
            Manager.GameM.ClearContinueData();
    }


    void OnClickSettingButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.UiM.ShowPopup<UI_SettingPopup>();
    }

    void OnClickAttendanceCheckButton()
    {
        Manager.SoundM.PlayButtonClick();
        UI_CheckOutPopup popup = Manager.UiM.ShowPopup<UI_CheckOutPopup>();
        popup.SetInfo(Manager.TimeM.AttendanceDay);
    }

    void OnClickMissionButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.UiM.ShowPopup<UI_MissionPopup>();
    }

    void OnClickAchievementButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.UiM.ShowPopup<UI_AchievementPopup>();
    }

    void OnClickStageSelectButton()
    {
        Manager.SoundM.PlayButtonClick();

        UI_StageSelectPopup popup =  Manager.UiM.ShowPopup<UI_StageSelectPopup>();
        
    }

    void OnClickFirstClearRewardButton()
    {
        Manager.SoundM.PlayButtonClick();
        
    }

    void OnClickSecondClearRewardButton()
    {
        Manager.SoundM.PlayButtonClick();
    }

    void OnClickThirdClearRewardButton()
    {
        Manager.SoundM.PlayButtonClick();
    }

    void OnClickGameStartButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.GameM.isGameEnd = false;

        if(Manager.GameM.Stamina < Define.GAMESTART_NEED_STAMINA)
        {
            Manager.UiM.ShowPopup<UI_StaminaChargePopup>();
            return;
        }

        Manager.GameM.Stamina -= Define.GAMESTART_NEED_STAMINA;

        Manager.SceneM.LoadScene(Define.SceneType.GameScene, transform);

    }

    void OnClickOfflineRewardButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.UiM.ShowPopup<UI_OfflineRewardPopup>();
    }
}
