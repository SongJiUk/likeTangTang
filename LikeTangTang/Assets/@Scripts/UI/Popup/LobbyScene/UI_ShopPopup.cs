using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UI_ShopPopup : UI_Popup
{

    enum GameObjects
    {
        ContentObject,
        FreeGoldSoldOutObject,
        FreeGoldRedDotObject,
        AdKeySoldOutObject,
        AdKeyRedDotObject
    }

    enum Buttons
    {
        AdKeyButton,
        SilverKeyProductButton,
        GoldKeyProductButton,
        ADAdvancedGachaOpenButton,
        AdvancedGachaOpenButton,
        AdvancedGachaOpenTenButton,
        AdvancedGachaListButton,
        ADCommonGachaOpenButton,
        CommonGachaOpenButton,
        CommonGachaListButton,
        FreeGoldButton,
        FirstGoldProductButton,
        SecondGoldProductButton
    }

    enum Texts
    {
        AdvancedGachaCostValueText,
        AdvancedGachaTenCostValueText,
        CommonGachaCostValueText,
        FreeGoldTitleText,
        FirstGoldProductTitleText,
        SecondGoldProductTitleText
    }
    int goldAmount = 0;
    Action OnCompleteBuyItem;
    public bool isOpen = false;
    private void OnEnable()
    {
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.ContentObject));
        Refresh();
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
        TextsType = typeof(Texts);

        BindObject(gameObjectsType);
        BindButton(ButtonsType);
        BindText(TextsType);




        //열쇠 상점
        GetObject(gameObjectsType, (int)GameObjects.AdKeySoldOutObject).SetActive(false);
        GetButton(ButtonsType, (int)Buttons.AdKeyButton).gameObject.BindEvent(OnClickAdKeyButton);
        GetButton(ButtonsType, (int)Buttons.SilverKeyProductButton).gameObject.BindEvent(OnClickSilverKeyButton);
        GetButton(ButtonsType, (int)Buttons.GoldKeyProductButton).gameObject.BindEvent(OnClickGoldKeyButton);

        //상급 장비 상점
        GetButton(ButtonsType, (int)Buttons.AdvancedGachaListButton).gameObject.BindEvent(OnClickAdvancedBoxListButton);
        GetButton(ButtonsType, (int)Buttons.ADAdvancedGachaOpenButton).gameObject.BindEvent(OnClickAdvancedBoxADButton);
        GetButton(ButtonsType, (int)Buttons.AdvancedGachaOpenButton).gameObject.BindEvent(OnClickAdvancedBoxOpenButton);
        GetButton(ButtonsType, (int)Buttons.AdvancedGachaOpenTenButton).gameObject.BindEvent(OnClickAdvancedBoxTenOpenButton);


        //일반 장비 
        GetButton(ButtonsType, (int)Buttons.CommonGachaListButton).gameObject.BindEvent(OnClickCommonGachaListButton);
        GetButton(ButtonsType, (int)Buttons.ADCommonGachaOpenButton).gameObject.BindEvent(OnClickADCommonGachaOpenButton);
        GetButton(ButtonsType, (int)Buttons.CommonGachaOpenButton).gameObject.BindEvent(OnClickCommonGachaOpenButton);


        //골드 상점
        GetObject(gameObjectsType, (int)GameObjects.FreeGoldSoldOutObject).SetActive(false);
        GetButton(ButtonsType, (int)Buttons.FreeGoldButton).gameObject.BindEvent(OnClickFreeGoldADButton);
        GetButton(ButtonsType, (int)Buttons.FirstGoldProductButton).gameObject.BindEvent(OnClickFirstGoldProductButton);
        GetButton(ButtonsType, (int)Buttons.SecondGoldProductButton).gameObject.BindEvent(OnClickSecondGoldProductButton);


        Refresh();
        return true;
    }

    void Refresh()
    {
        Manager.GameM.ItemDic.TryGetValue(Define.ID_BORONZE_KEY, out int bronzeKeyCount);
        Manager.GameM.ItemDic.TryGetValue(Define.ID_SILVER_KEY, out int silverKeyCount);
        Manager.GameM.ItemDic.TryGetValue(Define.ID_GOLD_KEY, out int goldKeyCount);

        //키
        GetObject(gameObjectsType, (int)GameObjects.AdKeySoldOutObject).SetActive(Manager.GameM.SilverKeyCountAds == 0);

        //상급상자
        GetButton(ButtonsType, (int)Buttons.ADAdvancedGachaOpenButton).gameObject.SetActive(Manager.GameM.GachaCountAdsAdvanced > 0);
        GetText(TextsType, (int)Texts.AdvancedGachaCostValueText).text = $"{goldKeyCount}/1";
        GetText(TextsType, (int)Texts.AdvancedGachaTenCostValueText).text = $"{goldKeyCount}/10";

        //일반 상자
        GetButton(ButtonsType, (int)Buttons.ADCommonGachaOpenButton).gameObject.SetActive(Manager.GameM.GachaCountAdsCommon > 0);
        GetText(TextsType, (int)Texts.CommonGachaCostValueText).text = $"{silverKeyCount}/1";

        //골드 상자
        GetObject(gameObjectsType, (int)GameObjects.FreeGoldSoldOutObject).SetActive(Manager.GameM.GoldCountAds == 0);

        goldAmount = Define.STAGE_GOLD_UP;
        goldAmount *= Manager.GameM.GetMaxStageIndex();

        GetText(TextsType, (int)Texts.FreeGoldTitleText).text = $"{goldAmount}";
        GetText(TextsType, (int)Texts.FirstGoldProductTitleText).text = $"{goldAmount * 3}";
        GetText(TextsType, (int)Texts.SecondGoldProductTitleText).text = $"{goldAmount * 5}";

        if (Manager.GameM.SilverKeyCountAds == 0)
        {
            GetObject(gameObjectsType, (int)GameObjects.AdKeyRedDotObject).SetActive(false);
        }

        if (Manager.GameM.GoldCountAds == 0)
        {
            GetObject(gameObjectsType, (int)GameObjects.FreeGoldRedDotObject).SetActive(false);
        }
    }


    void DoGaCha(Define.GachaType _gachaType, int _count = 1)
    {
        List<Equipment> list = Manager.GameM.DoGaCha(_gachaType, _count).ToList();
        if (Manager.GameM.MissionDic.TryGetValue(Define.MissionTarget.GachaOpen, out MissionInfo missionInfo))
        {
            missionInfo.Progress++;
            Manager.UiM.CheckRedDotObject(Define.RedDotObjectType.Mission);
        }

        UI_GachaResultsPopup popup = Manager.UiM.MakeSubItem<UI_GachaResultsPopup>(Manager.UiM.Root.transform);
        popup.SetInfo(list);
    }


    //열쇠
    void OnClickAdKeyButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (Manager.GameM.SilverKeyCountAds > 0)
        {
            Manager.AdM.ShowRewardedAd(() =>
            {
                UI_RewardPopup Popup = (Manager.UiM.SceneUI as UI_LobbyScene).Ui_RewardPopup;
                Queue<string> spriteName = new Queue<string>();
                Queue<int> count = new Queue<int>();
                spriteName.Enqueue(Manager.DataM.MaterialDic[Define.ID_SILVER_KEY].SpriteName); ;
                count.Enqueue(1);
                Popup.SetInfo(spriteName, count);
                Popup.gameObject.SetActive(true);
                Manager.GameM.ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_SILVER_KEY], 1);
                Manager.GameM.SilverKeyCountAds--;
                OnCompleteBuyItem?.Invoke();

                Refresh();
            });

        }
        else
        {
            Manager.UiM.ShowToast("오늘은 더이상 광고를 시청할수 없습니다.");
        }
    }

    void OnClickSilverKeyButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (Manager.GameM.Dia >= 150)
        {
            UI_BuyItemPopup popup = Manager.UiM.MakeSubItem<UI_BuyItemPopup>(Manager.UiM.Root.transform);
            Manager.DataM.MaterialDic.TryGetValue(Define.ID_SILVER_KEY, out var item);
            popup.SetInfo(item, 150, 1);
            popup.OnCompleteBuyItem = Refresh;
        }
        else
        {
            Manager.UiM.ShowToast("다이아가 부족합니다");
        }
    }

    void OnClickGoldKeyButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (Manager.GameM.Dia >= 300)
        {
            UI_BuyItemPopup popup = Manager.UiM.MakeSubItem<UI_BuyItemPopup>(Manager.UiM.Root.transform);
            Manager.DataM.MaterialDic.TryGetValue(Define.ID_GOLD_KEY, out var item);
            popup.SetInfo(item, 300, 1);
            popup.OnCompleteBuyItem = Refresh;
        }
        else
        {
            Manager.UiM.ShowToast("다이아가 부족합니다");
        }
    }

    //상급장비
    void OnClickAdvancedBoxListButton()
    {
        Manager.SoundM.PlayButtonClick();
        UI_GachaListPopup popup = Manager.UiM.MakeSubItem<UI_GachaListPopup>(Manager.UiM.Root.transform);
        popup.SetInfo(Define.GachaType.AdvancedGacha);
    }

    void OnClickAdvancedBoxADButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (Manager.GameM.GachaCountAdsAdvanced > 0)
        {
            Manager.AdM.ShowRewardedAd(() =>
            {
                Manager.GameM.GachaCountAdsAdvanced--;
                DoGaCha(Define.GachaType.AdvancedGacha, 1);
                Refresh();
            });
        }
        else
        {
            Manager.UiM.ShowToast("오늘은 무료광고를 모두 시청했습니다.");
        }
    }

    void OnClickAdvancedBoxOpenButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (Manager.GameM.ItemDic.TryGetValue(Define.ID_GOLD_KEY, out int count))
        {
            if (count > 0)
            {
                DoGaCha(Define.GachaType.AdvancedGacha, 1);
                Manager.GameM.ItemDic[Define.ID_GOLD_KEY]--;
                Refresh();
            }
            else
            {
                Manager.UiM.ShowToast("열쇠가 부족합니다.");
            }
        }
        else Debug.LogError("ItemDic에 GoldKey 등록이 안됨.");

    }

    void OnClickAdvancedBoxTenOpenButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (Manager.GameM.ItemDic.TryGetValue(Define.ID_GOLD_KEY, out int count))
        {
            if (count >= 10)
            {
                DoGaCha(Define.GachaType.AdvancedGacha, 10);
                Manager.GameM.ItemDic[Define.ID_GOLD_KEY] -= 10;
                Refresh();
            }
            else
            {
                Manager.UiM.ShowToast("열쇠가 부족합니다.");
            }
        }
        else Debug.LogError("ItemDic에 GoldKey 등록이 안됨.");

    }

    //일반 장비

    void OnClickCommonGachaListButton()
    {
        Manager.SoundM.PlayButtonClick();
        UI_GachaListPopup popup = Manager.UiM.MakeSubItem<UI_GachaListPopup>(Manager.UiM.Root.transform);
        popup.SetInfo(Define.GachaType.CommonGacha);
    }

    void OnClickADCommonGachaOpenButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (Manager.GameM.GachaCountAdsCommon > 0)
        {
            Manager.AdM.ShowRewardedAd(() =>
            {
                Manager.GameM.GachaCountAdsCommon--;
                DoGaCha(Define.GachaType.CommonGacha, 1);
                Refresh();
            });
        }
        else
        {
            Manager.UiM.ShowToast("오늘은 무료광고를 모두 시청했습니다.");
        }
    }

    void OnClickCommonGachaOpenButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (Manager.GameM.ItemDic.TryGetValue(Define.ID_SILVER_KEY, out int count))
        {
            if (count > 0)
            {
                DoGaCha(Define.GachaType.CommonGacha, 1);
                Manager.GameM.ItemDic[Define.ID_SILVER_KEY]--;
                Refresh();
            }
            else
            {
                Manager.UiM.ShowToast("열쇠가 부족합니다.");
            }
        }
        else Debug.LogError("ItemDic에 SilverKey 등록이 안됨.");

    }

    //골드

    void OnClickFreeGoldADButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (Manager.GameM.GoldCountAds > 0)
        {
            Manager.AdM.ShowRewardedAd(() =>
            {
                int goldAmount = 0;
                if (Manager.DataM.OfflineRewardDataDic.TryGetValue(Manager.GameM.GetMaxStageIndex(), out Data.OfflineRewardData data))
                {
                    goldAmount = data.Reward_Gold;
                }

                UI_RewardPopup Popup = (Manager.UiM.SceneUI as UI_LobbyScene).Ui_RewardPopup;

                Queue<string> spriteName = new Queue<string>();
                Queue<int> count = new Queue<int>();

                spriteName.Enqueue(Manager.DataM.MaterialDic[Define.ID_GOLD].SpriteName); ;
                count.Enqueue(goldAmount);
                Popup.SetInfo(spriteName, count);
                Popup.gameObject.SetActive(true);
                OnCompleteBuyItem?.Invoke();
                Manager.GameM.ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_GOLD], 1);
                Manager.GameM.GoldCountAds--;

                Refresh();
            });

        }
        else
        {
            Manager.UiM.ShowToast("오늘은 무료광고를 모두 시청했습니다");
        }
    }

    void OnClickFirstGoldProductButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (Manager.GameM.Dia >= 300)
        {
            UI_BuyItemPopup popup = Manager.UiM.MakeSubItem<UI_BuyItemPopup>(Manager.UiM.Root.transform);
            Manager.DataM.MaterialDic.TryGetValue(Define.ID_GOLD, out var item);
            popup.SetInfo(item, 300, goldAmount * 3);
            popup.OnCompleteBuyItem = Refresh;
        }
        else
        {
            Manager.UiM.ShowToast("다이아가 부족합니다");
        }
    }

    void OnClickSecondGoldProductButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (Manager.GameM.Dia >= 500)
        {
            UI_BuyItemPopup popup = Manager.UiM.MakeSubItem<UI_BuyItemPopup>(Manager.UiM.Root.transform);
            Manager.DataM.MaterialDic.TryGetValue(Define.ID_GOLD, out var item);
            popup.SetInfo(item, 500, goldAmount * 5);
            popup.OnCompleteBuyItem = Refresh;
        }
        else
        {
            Manager.UiM.ShowToast("다이아가 부족합니다");
        }
    }
    
}
