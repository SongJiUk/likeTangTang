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
        AchievementButtonRedDotObject
    }

    public enum Toggles
    {
        GameStartToggle
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

    //TODO : 여기서 체크
    private void OnEnable()
    {
        StartCoroutine(CoCheckPopup());
    }
   
    public override bool Init()
    {
        gameObjectsType = typeof(GameObjects);
        TogglesType = typeof(Toggles);
        ButtonsType = typeof(Buttons);
        TextsType = typeof(Texts);

        BindObject(gameObjectsType);
        BindToggle(TogglesType);
        BindButton(ButtonsType);
        BindText(TextsType);



        SetUIInfo();

        GetToggle(typeof(Toggles), (int)Toggles.GameStartToggle).gameObject.BindEvent(() =>
        {
            Manager.GameM.isGameEnd = false;
            Manager.SceneM.LoadScene(Define.SceneType.GameScene);
        });
        return true;
    }

   

    protected override void RefreshUI()
    {
        
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
}
