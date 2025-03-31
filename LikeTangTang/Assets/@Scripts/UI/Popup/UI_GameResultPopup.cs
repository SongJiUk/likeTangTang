using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_GameResultPopup : UI_Base
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
    public override bool Init()
    {
        BindAll();

        GetButton(typeof(Buttons) ,(int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
        GetButton(typeof(Buttons), (int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);

        RefreshUI();
        return true;
    }

    protected override void RefreshUI()
    {
        //[ ] : 데이터 가져와서 데이터로 해결
        GetText(typeof(Texts), (int)Texts.GameResultPopupTitleText).text = "Game Result123123123";
        GetText(typeof(Texts), (int)Texts.ResultStageValueText).text = "4 Stage123";
        GetText(typeof(Texts), (int)Texts.ResultSurvivalTimeText).text = "Survival123 Time";
        GetText(typeof(Texts), (int)Texts.ResultSurvivalTimeValueText).text = "11244 : 23";
        GetText(typeof(Texts), (int)Texts.ResultGoldValueText).text = "9999123";
        GetText(typeof(Texts), (int)Texts.ResultKillValueText).text = "1212434";
        GetText(typeof(Texts), (int)Texts.ConfirmButtonText).text = "OK123";
    }
    void BindAll()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
    }

    // [ ] : 씬이동, 랭킹
    public void OnClickStatisticsButton()
    {
        Debug.Log("Click Statistics Button");
    }

    public void OnClickConfirmButton()
    {
        Debug.Log("Click Confirm Button");
    }
}
