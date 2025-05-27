using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameoverPopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,

    }

    enum Buttons
    {
        ConfirmButton,
        StatisticsButton,

    }

    enum Texts
    {
        GameoverStageValueText,
        GameoverLastWaveValueText,
        GameoverKillValueText,

    }

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.ContentObject));
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

        GetButton(ButtonsType, (int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
        GetButton(ButtonsType, (int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);

        Refresh();
        return true;
    }
    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        GetText(TextsType, (int)Texts.GameoverStageValueText).text = $"{Manager.GameM.CurrentStageData.StageIndex} STAGE";
        GetText(TextsType, (int)Texts.GameoverLastWaveValueText).text = $"{Manager.GameM.CurrentWaveIndex + 1 }";
        GetText(TextsType, (int)Texts.GameoverKillValueText).text = $"{Manager.GameM.player.KillCount}";
    }

    void OnClickStatisticsButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.UiM.ShowPopup<UI_TotalDamagePopup>().SetInfo();
    }

    void OnClickConfirmButton()
    {
        Manager.SoundM.PlayButtonClick();

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
}
