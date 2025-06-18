using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_OfflineRewardPopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,
        RewardItemScrollContentObject,
        OfflineRewardGoldEffect,

    }

    enum Texts
    {
        TotalTimeValueText,
        ResultGoldValueText,
        ClaimButtonText,

    }

    enum Buttons
    {
        BackgroundButton,
        FastRewardButton,
        ClaimButton
    }

    enum Images
    {

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
        TextsType = typeof(Texts);
        ButtonsType = typeof(Buttons);
        ImagesType = typeof(Images);

        BindObject(gameObjectsType);
        BindText(TextsType);
        BindButton(ButtonsType);
        BindImage(ImagesType);

        GetButton(ButtonsType, (int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton(ButtonsType, (int)Buttons.FastRewardButton).gameObject.BindEvent(OnClickFastRewardButton);
        GetButton(ButtonsType, (int)Buttons.ClaimButton).gameObject.BindEvent(OnClickClaimButton);
        GetObject(gameObjectsType, (int)GameObjects.OfflineRewardGoldEffect).SetActive(false);

        Refresh();
        StartCoroutine(CoTimeCheck());
        
        return true;

    }


    void Refresh()
    {
        StopAllCoroutines();

        if(Manager.DataM.OfflineRewardDataDic.TryGetValue(Manager.GameM.GetMaxStageIndex(), out Data.OfflineRewardData offlineRewardData))
        {
            GetText(TextsType, (int)Texts.ResultGoldValueText).text = $"{offlineRewardData.Reward_Gold} / 1시간";
        }

        GameObject cont = GetObject(gameObjectsType, (int)GameObjects.RewardItemScrollContentObject);


        if (Manager.TimeM.TimeSinceLastReward.TotalMinutes > 10)
        {
            UI_MaterialItem item = Manager.UiM.MakeSubItem<UI_MaterialItem>(cont.transform);
            int count = (int)Manager.TimeM.CalculateGoldPerMinute(offlineRewardData.Reward_Gold);
            item.SetInfo(Manager.DataM.MaterialDic[Define.ID_GOLD].SpriteName, count);
        }
    }

    IEnumerator CoTimeCheck()
    {
        while(true)
        {
            TimeSpan timeSpan = Manager.TimeM.TimeSinceLastReward;

            string formattedTime = string.Format("{0:D2} : {1:D2} : {2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            if(timeSpan == TimeSpan.FromHours(24))
                formattedTime = string.Format("{0:D2} : {1:D2} : {2:D2}", 24, 0, 0);

            GetText(TextsType, (int)Texts.TotalTimeValueText).text = formattedTime;

            if (timeSpan.TotalMinutes < 10)
            {
                TimeSpan remainingTime = TimeSpan.FromMinutes(10) - timeSpan;

                // Display remaining time
                //남은시간 표기
                string remaining = string.Format("{0:D2}분 {1:D2}초", remainingTime.Minutes, remainingTime.Seconds);
                GetText(TextsType, (int)Texts.ClaimButtonText).text = remaining;
                GetButton(ButtonsType, (int)Buttons.ClaimButton).GetComponent<Image>().color = Utils.HexToColor("989898");

            }
            else
            {
                GetText(TextsType, (int)Texts.ClaimButtonText).text = "받기";
                GetButton(ButtonsType, (int)Buttons.ClaimButton).GetComponent<Image>().color = Utils.HexToColor("50D500");
            }
            yield return new WaitForSeconds(1);
        }
    }


    void OnClickBackgroundButton()
    {
        Manager.SoundM.PlayPopupClose();
        Manager.UiM.ClosePopup(this);
    }

    void OnClickFastRewardButton()
    {
        Manager.SoundM.PlayButtonClick();
        if(Manager.DataM.OfflineRewardDataDic.TryGetValue(Manager.GameM.GetMaxStageIndex(), out Data.OfflineRewardData data))
        {
            UI_FastRewardPopup popup = Manager.UiM.ShowPopup<UI_FastRewardPopup>();
            popup.SetInfo(data);
        }
    }

    void OnClickClaimButton()
    {
        Manager.SoundM.PlayButtonClick();

        if (Manager.TimeM.TimeSinceLastReward.TotalMinutes < 10) return;

        if(Manager.DataM.OfflineRewardDataDic.TryGetValue(Manager.GameM.GetMaxStageIndex(), out Data.OfflineRewardData data))
        {
            GetObject(gameObjectsType, (int)GameObjects.OfflineRewardGoldEffect).SetActive(true);
            Manager.TimeM.GiveOfflioneReward(data);
        }

        Refresh();
        Manager.UiM.ClosePopup(this);
    }
}
