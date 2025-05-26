using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_StaminaChargePopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,

    }

    enum Texts
    {
        ChargeInfoValueText,
        DiaRemainingValueText,
        ADRemainingValueText,
        HaveStaminaValueText
    }

    enum Buttons
    {
        BuyDiaButton,
        BuyADButton,
        BackgroundButton
    }


    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.ContentObject));
        StartCoroutine(CoTimeCheck());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
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

        GetButton(ButtonsType, (int)Buttons.BuyDiaButton).gameObject.BindEvent(OnClickBuyDiaButton);
        GetButton(ButtonsType, (int)Buttons.BuyADButton).gameObject.BindEvent(OnClickBuyADButton);
        GetButton(ButtonsType, (int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBgButton);
        Refresh();

        return true;

    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        GetText(TextsType, (int)Texts.HaveStaminaValueText).text = $"+ 5";
        GetText(TextsType, (int)Texts.ADRemainingValueText).text = $"오늘 남은 횟수 : {Manager.GameM.StaminaCountAds}";
        GetText(TextsType, (int)Texts.DiaRemainingValueText).text = $"오늘 남은 횟수 : {Manager.GameM.RemainBuyStaminaForDia}";
    }

    IEnumerator CoTimeCheck()
    {
        while(true)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(Manager.TimeM.StaminaTime);

            string formattedTime = string.Format("{0:D2} : {1:D2} : {2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

            GetText(TextsType, (int)Texts.ChargeInfoValueText).text = formattedTime;
            yield return new WaitForSeconds(1f);
        }
    }

    void OnClickBuyDiaButton()
    {
        Manager.SoundM.PlayButtonClick();

        if (Manager.GameM.RemainBuyStaminaForDia > 0)
        {
            Queue<string> name = new();
            name.Enqueue(Manager.DataM.MaterialDic[Define.ID_STAMINA].SpriteName);
            Queue<int> count = new();
            count.Enqueue(15);
            UI_RewardPopup popup = (Manager.UiM.SceneUI as UI_LobbyScene).Ui_RewardPopup;
            popup.gameObject.SetActive(true);
            Manager.GameM.RemainBuyStaminaForDia--;
            Manager.GameM.ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_STAMINA], 15);
            Refresh();
            popup.SetInfo(name, count);
        }
        else
        {
            Manager.UiM.ShowToast("오늘은 더이상 구매할 수 없습니다.");
        }
    }

    void OnClickBuyADButton()
    {
        Manager.SoundM.PlayButtonClick();

        if (Manager.GameM.StaminaCountAds > 0)
        {
            Manager.AdM.ShowRewardedAd(() =>
            {
                Queue<string> name = new();
                name.Enqueue(Manager.DataM.MaterialDic[Define.ID_STAMINA].SpriteName);
                Queue<int> count = new();
                count.Enqueue(5);
                UI_RewardPopup popup = (Manager.UiM.SceneUI as UI_LobbyScene).Ui_RewardPopup;
                popup.gameObject.SetActive(true);
                Manager.GameM.StaminaCountAds--;
                Manager.GameM.ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_STAMINA], 5);
                Refresh();
                popup.SetInfo(name, count);
            });
            
        }
        else
        {
            Manager.UiM.ShowToast("오늘은 더이상 광고를 시청할수 없습니다.");
        }
    }

    void OnClickBgButton()
    {
        Manager.UiM.ClosePopup(this);
    }
}
