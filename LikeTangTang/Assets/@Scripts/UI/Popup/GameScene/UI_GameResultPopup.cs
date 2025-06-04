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
        if (!base.Init()) return false;

        gameObjectsType = typeof(GameObjects);
        TextsType = typeof(Texts);
        ButtonsType = typeof(Buttons);

        BindObject(gameObjectsType);
        BindText(TextsType);
        BindButton(ButtonsType);

        GetButton(typeof(Buttons), (int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
        GetButton(typeof(Buttons), (int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);


        return true;
    }

    public void SetInfo()
    {
        Refresh();
        StageClear();
    }
    void Refresh()
    {
        GetText(typeof(Texts), (int)Texts.ResultStageValueText).text = $"{Manager.GameM.CurrentStageData.StageIndex} STAGE";
        GetText(typeof(Texts), (int)Texts.ResultSurvivalTimeValueText).text = $"{Manager.GameM.minute:D2} : {Manager.GameM.second:D2}";
        GetText(typeof(Texts), (int)Texts.ResultGoldValueText).text = $"{Manager.GameM.CurrentStageData.ClearGold}";
        GetText(typeof(Texts), (int)Texts.ResultKillValueText).text = $"{Manager.GameM.player.KillCount}";
        
            
        Manager.GameM.Gold += Manager.GameM.CurrentStageData.ClearGold;
        Manager.GameM.ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_RandomScroll], 10);
        Manager.GameM.ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_LevelUpCoupon], Manager.GameM.CurrentStageData.StageIndex);

        Transform cont = GetObject(gameObjectsType, (int)GameObjects.ResultRewardScrollContentObject).transform;
        cont.gameObject.DestroyChilds();
         

        //TODO : 이거 확인 + 여기에 플레이어 경험치 쿠폰도 같이 주면 좋을거같음.
        UI_MaterialItem gold = Manager.UiM.MakeSubItem<UI_MaterialItem>(cont);
        gold.SetInfo(Manager.DataM.MaterialDic[Define.ID_GOLD].SpriteName, Manager.GameM.CurrentStageData.ClearGold);


        UI_MaterialItem scroll = Manager.UiM.MakeSubItem<UI_MaterialItem>(cont);
        scroll.SetInfo(Manager.DataM.MaterialDic[Define.ID_RandomScroll].SpriteName, 10);

        UI_MaterialItem coupon = Manager.UiM.MakeSubItem<UI_MaterialItem>(cont);
        coupon.SetInfo(Manager.DataM.MaterialDic[Define.ID_LevelUpCoupon].SpriteName, Manager.GameM.CurrentStageData.StageIndex);

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject(gameObjectsType, (int)GameObjects.ResultGoldObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject(gameObjectsType, (int)GameObjects.ResultKillObject).GetComponent<RectTransform>());
    }

    void StageClear()
    {
        StageClearInfoData info;

        if (Manager.GameM.StageClearInfoDic.TryGetValue(Manager.GameM.CurrentStageData.StageIndex, out info))
        {
            info.MaxWaveIndex = Manager.GameM.CurrentWaveIndex;
            info.isClear = true;
            Manager.GameM.StageClearInfoDic[Manager.GameM.CurrentStageData.StageIndex] = info;
        }
        Manager.GameM.ClearContinueData();
        Manager.GameM.SetNextStage();
    }

    public void OnClickStatisticsButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.UiM.ShowPopup<UI_TotalDamagePopup>().SetInfo();
    }

    public void OnClickConfirmButton()
    {
        Manager.SoundM.PlayButtonClick();
       
        Manager.SceneM.LoadScene(Define.SceneType.LobbyScene, transform);
    }
}
