using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DiaChargePopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,

    }

    enum Texts
    {
        ADRemainingValueText
    }

    enum Buttons
    {
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

        GetButton(ButtonsType, (int)Buttons.BuyADButton).gameObject.BindEvent(OnClickBuyAdButton);
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
        GetText(TextsType, (int)Texts.ADRemainingValueText).text = $"오늘 남은 횟수 : {Manager.GameM.DiaCountAds}";
    }

    void OnClickBuyAdButton()
    {
        Manager.SoundM.PlayButtonClick();

        if (Manager.GameM.DiaCountAds > 0)
        {
            Manager.AdM.ShowRewardedAd(() =>
            {
                Queue<string> name = new();
                name.Enqueue(Manager.DataM.MaterialDic[Define.ID_DIA].SpriteName);
                Queue<int> count = new();
                count.Enqueue((int)(200 * Manager.GameM.CurrentCharacter.Evol_DiaBouns));
                UI_RewardPopup popup = (Manager.UiM.SceneUI as UI_LobbyScene).Ui_RewardPopup;
                popup.gameObject.SetActive(true);
                Manager.GameM.DiaCountAds--;
                Manager.GameM.ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_DIA], (int)(200 * Manager.GameM.CurrentCharacter.Evol_DiaBouns));
                Refresh();
                popup.SetInfo(name, count);
            });

        }
        else
        {
            Manager.UiM.ShowToast("오늘은 더이상 광고를 시청하실 수 없습니다.");
        }
    }

    void OnClickBgButton()
    {
        Manager.UiM.ClosePopup(this);
    }
}
