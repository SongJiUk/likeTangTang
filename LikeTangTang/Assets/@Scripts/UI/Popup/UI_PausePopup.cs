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

        GetButton(ButtonsType, (int)Buttons.ResumeButton).gameObject.BindEvent(OnClickResumeButton);
        GetButton(ButtonsType, (int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
        GetButton(ButtonsType, (int)Buttons.HomeButton).gameObject.BindEvent(OnClickHomeButton);
        GetButton(ButtonsType, (int)Buttons.SoundButton).gameObject.BindEvent(SoundButton);
        GetButton(ButtonsType, (int)Buttons.SettingButton).gameObject.BindEvent(SettingButton);

        return true;
    }


    void OnClickResumeButton()
    {

    }

    void OnClickStatisticsButton()
    {

    }

    void OnClickHomeButton()
    {
        Manager.SoundM.PlayButtonClick();

        Manager.GameM.isGameEnd = true;

        StageClearInfoData info;

        if(Manager.GameM.StageClearInfoDic.TryGetValue(Manager.GameM.CurrentStageData.StageIndex, out info))
        {
            if(Manager.GameM.CurrentWaveIndex > info.MaxWaveIndex)
            {
                info.MaxWaveIndex = Manager.GameM.CurrentWaveIndex;
                Manager.GameM.StageClearInfoDic[Manager.GameM.CurrentStageData.StageIndex] = info;
            }
        }

        Manager.GameM.ClearContinueData();
        Manager.SceneM.LoadScene(Define.SceneType.LobbyScene, transform);
    }

    void SoundButton()
    {

    }

    void SettingButton()
    {

    }
}
