using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_GameResultPopup : UI_Popup
{

    enum GameObjects
    {
        ContentObject,
        ResultRewardScrollContentObject,
        ResultGoldObject,
        ResultKillObject
    }

    enum Texts
    {
        GameResultPopupTitleText,
        ResultStageValueText,
        ResultSurvivalTimeText,
        ResultSurvivalTimeValueText,
        ResultGoldValueText,
        ResultKillValueText,
        ConfirmButtonText
    }
    
    enum Buttons
    {
        StatisticsButton,
        ConfirmButton
    }

    private void Awake()
    {
        Init();
    }
    public override bool Init()
    {
        if (base.Init() == false) return false;
            
        SetUIInfo();

        GetButton(typeof(Buttons) ,(int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
        GetButton(typeof(Buttons), (int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);

        return true;
    }

    protected override void RefreshUI()
    {
        //[ ] : 데이터 가져와서 데이터로 해결
        GetText(typeof(Texts), (int)Texts.ResultStageValueText).text = $"{Manager.GameM.CurrentStageData.StageIndex} Stage";
        GetText(typeof(Texts), (int)Texts.ResultSurvivalTimeValueText).text = $"{Manager.GameM.minute:D2} : {Manager.GameM.second:D2}";
        GetText(typeof(Texts), (int)Texts.ResultGoldValueText).text = $"{Manager.GameM.CurrentStageData.ClearGold}";
        GetText(typeof(Texts), (int)Texts.ResultKillValueText).text = $"{Manager.GameM.player.KillCount}";

        Manager.GameM.Gold += Manager.GameM.CurrentStageData.ClearGold;
        
    }

    protected override void SetUIInfo()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
    }

    public void SetInfo()
    {
        RefreshUI();
    }

    // [ ] : 씬이동, 랭킹
    public void OnClickStatisticsButton()
    {

    }

    public void OnClickConfirmButton()
    {
        Manager.SoundM.PlayButtonClick();
        StageClearInfoData info;

        if(Manager.GameM.StageClearInfoDic.TryGetValue(Manager.GameM.CurrentStageData.StageIndex, out info))
        {
            info.MaxWaveIndex = Manager.GameM.CurrentWaveIndex;
            info.isClear = true;
            Manager.GameM.StageClearInfoDic[Manager.GameM.CurrentStageData.StageIndex] = info;
        }
        Manager.GameM.ClearContinueData();
        Manager.GameM.SetNextStage();
        Manager.SceneM.LoadScene(Define.SceneType.LobbyScene, transform);
    }
}
