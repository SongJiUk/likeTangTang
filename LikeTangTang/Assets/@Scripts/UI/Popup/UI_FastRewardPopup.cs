using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FastRewardPopup : UI_Popup
{
    
    enum GameObjects
    {
        ContentObject,
        ItemContainer,

    }

    enum Buttons
    {
        BackgroundButton,
        ADFreeButton,
        ClaimButton
    }

    enum Texts
    {
        EemainingCountValueText
    }
    Data.OfflineRewardData OfflineRewardData;
    bool isClaim = false;
    private void Awake()
    {
        Init();
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

        GetButton(ButtonsType, (int)Buttons.ADFreeButton).gameObject.BindEvent(OnClickAdButton);
        GetButton(ButtonsType, (int)Buttons.ClaimButton).gameObject.BindEvent(OnClickClaimButton);
        GetButton(ButtonsType, (int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBgButton);


        return true;
    }


    public void SetInfo(Data.OfflineRewardData _data)
    {
        OfflineRewardData = _data;
        Refresh();
    }

    void Refresh()
    {
        GameObject cont = GetObject(gameObjectsType, (int)GameObjects.ItemContainer);

        if(Manager.GameM.Stamina >= 15 && Manager.GameM.FastRewardCountStamina > 0)
        {
            GetButton(ButtonsType, (int)Buttons.ClaimButton).gameObject.GetComponent<Image>().color = Utils.HexToColor("50D500");
            isClaim = true;
        }
        else
        {
            GetButton(ButtonsType, (int)Buttons.ClaimButton).gameObject.GetComponent<Image>().color = Utils.HexToColor("989898");
            isClaim = false;
        }

        UI_MaterialItem gold = Manager.UiM.MakeSubItem<UI_MaterialItem>(cont.transform);
        int count = OfflineRewardData.Reward_Gold * 5;
        gold.SetInfo(Manager.DataM.MaterialDic[Define.ID_GOLD].SpriteName, count);

        UI_MaterialItem scroll = Manager.UiM.MakeSubItem<UI_MaterialItem>(cont.transform);
        scroll.SetInfo(Manager.DataM.MaterialDic[Define.ID_RandomScroll].SpriteName, OfflineRewardData.FastReward_Scroll);

        UI_MaterialItem key = Manager.UiM.MakeSubItem<UI_MaterialItem>(cont.transform);
        key.SetInfo(Manager.DataM.MaterialDic[Define.ID_SILVER_KEY].SpriteName, OfflineRewardData.FastReward_Scroll);

        GetText(TextsType, (int)Texts.EemainingCountValueText).text = Manager.GameM.FastRewardCountStamina.ToString();

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetButton(ButtonsType, (int)Buttons.ADFreeButton).gameObject.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetButton(ButtonsType, (int)Buttons.ClaimButton).gameObject.GetComponent<RectTransform>());
    }

    void OnClickAdButton()
    {
        Manager.SoundM.PlayButtonClick();

        if (Manager.GameM.FastRewardCountAd > 0)
        {
            Manager.AdM.ShowRewardedAd(() =>
            {
                Manager.GameM.FastRewardCountAd--;
                Manager.TimeM.GiveFastOfflioneReward(OfflineRewardData);
                Manager.UiM.ClosePopup(this);
            });
        }
        else
        {
            Manager.UiM.ShowToast("오늘은 더 이상 광고를 시청할 수 없습니다.");
        }
    }
    void OnClickClaimButton()
    {
        Manager.SoundM.PlayButtonClick();

        if (Manager.GameM.Stamina >= 15 && Manager.GameM.FastRewardCountStamina > 0 && isClaim)
        {
            Manager.GameM.Stamina -= 15;
            Manager.GameM.FastRewardCountStamina--;
            Manager.TimeM.GiveFastOfflioneReward(OfflineRewardData);
            Manager.UiM.ClosePopup(this);
            Refresh();
        }
        else return;
    }

    void OnClickBgButton()
    {
        Manager.UiM.ClosePopup(this);
    }
}
