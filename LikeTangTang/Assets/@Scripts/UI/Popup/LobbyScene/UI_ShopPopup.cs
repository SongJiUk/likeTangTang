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
    private void OnEnable()
    {
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.ContentObject));
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
        //TODO : 리워드 데이터..스테이지마다 + 1000
        goldAmount *= Manager.GameM.GetMaxStageIndex();

        GetText(TextsType, (int)Texts.FreeGoldTitleText).text = $"{goldAmount}";
        GetText(TextsType, (int)Texts.FirstGoldProductTitleText).text = $"{goldAmount * 3}";
        GetText(TextsType, (int)Texts.SecondGoldProductTitleText).text = $"{goldAmount * 5}";
    }


    void DoGaCha(Define.GachaType _gachaType, int _count = 1)
    {
        List<Equipment> list = Manager.GameM.DoGaCha(_gachaType, _count).ToList();
        UI_GachaResultsPopup popup =  Manager.UiM.MakeSubItem<UI_GachaResultsPopup>(transform);
        popup.SetInfo(list);
    }


    //열쇠
    void OnClickAdKeyButton()
    {
        Manager.SoundM.PlayButtonClick();
        if(Manager.GameM.SilverKeyCountAds > 0)
        {
            Manager.GameM.SilverKeyCountAds--;
            //TODO : 광고 실행, 리워드 받기
        }
    }

    void OnClickSilverKeyButton()
    {
        Manager.SoundM.PlayButtonClick();
        if(Manager.GameM.Dia >= 150)
        {
            UI_BuyItemPopup popup = Manager.UiM.MakeSubItem<UI_BuyItemPopup>(this.transform);
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
        if(Manager.GameM.Dia >= 300)
        {
            UI_BuyItemPopup popup = Manager.UiM.MakeSubItem<UI_BuyItemPopup>(this.transform);
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
        UI_GachaListPopup popup = Manager.UiM.MakeSubItem<UI_GachaListPopup>(transform);
        popup.SetInfo(Define.GachaType.AdvancedGacha);
    }
    
    void OnClickAdvancedBoxADButton()
    {
       //TODO : 광고
       Manager.SoundM.PlayButtonClick();
       if(Manager.GameM.GachaCountAdsAdvanced > 0)
       {
            Manager.GameM.GachaCountAdsAdvanced--;
            DoGaCha(Define.GachaType.AdvancedGacha, 1);
            Refresh();
       }
       else
       {
            Manager.UiM.ShowToast("오늘은 무료광고를 모두 시청했습니다.");
       }
    }

    void OnClickAdvancedBoxOpenButton()
    {
        Manager.SoundM.PlayButtonClick();
        if(Manager.GameM.ItemDic[Define.ID_GOLD_KEY] > 0)
        {
            Manager.GameM.ItemDic[Define.ID_GOLD_KEY]--;
            DoGaCha(Define.GachaType.AdvancedGacha, 1);
            Refresh();
        }
        else
        {
            Manager.UiM.ShowToast("열쇠가 부족합니다.");
        }
    }

    void OnClickAdvancedBoxTenOpenButton()
    {
        Manager.SoundM.PlayButtonClick();
        if(Manager.GameM.ItemDic[Define.ID_GOLD_KEY] >= 10 )
        {
            Manager.GameM.ItemDic[Define.ID_GOLD_KEY] -= 10;
            DoGaCha(Define.GachaType.AdvancedGacha, 10);
            Refresh();
        }
        else
        {
            Manager.UiM.ShowToast("열쇠가 부족합니다.");
        }


    }

    //일반 장비

    void OnClickCommonGachaListButton()
    {
        Manager.SoundM.PlayButtonClick();
        UI_GachaListPopup popup = Manager.UiM.MakeSubItem<UI_GachaListPopup>(transform);
        popup.SetInfo(Define.GachaType.CommonGacha);
    }

    void OnClickADCommonGachaOpenButton()
    {
        Manager.SoundM.PlayButtonClick();
       if(Manager.GameM.GachaCountAdsCommon > 0)
       {
            Manager.GameM.GachaCountAdsCommon--;
            DoGaCha(Define.GachaType.CommonGacha, 1);
            Refresh();
       }
       else
       {
            Manager.UiM.ShowToast("오늘은 무료광고를 모두 시청했습니다.");
       }
    }

    void OnClickCommonGachaOpenButton()
    {
        Manager.SoundM.PlayButtonClick();
        if(Manager.GameM.ItemDic[Define.ID_SILVER_KEY] > 0)
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

    //골드

    void OnClickFreeGoldADButton()
    {
        Manager.SoundM.PlayButtonClick();
        if(Manager.GameM.GoldCountAds >0)
        {   Manager.GameM.GoldCountAds--;

            //TODO : 광고 람다
            UI_BuyItemPopup popup = Manager.UiM.MakeSubItem<UI_BuyItemPopup>(this.transform);
            Manager.DataM.MaterialDic.TryGetValue(Define.ID_GOLD, out var item);
            popup.SetInfo(item, 0, goldAmount);
            Refresh();
        }
        else
        {
            Manager.UiM.ShowToast("오늘은 무료광고를 모두 시청했습니다");
        }
    }

    void OnClickFirstGoldProductButton()
    {
        Manager.SoundM.PlayButtonClick();
        if(Manager.GameM.Dia >= 300)
        {
            UI_BuyItemPopup popup = Manager.UiM.MakeSubItem<UI_BuyItemPopup>(this.transform);
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
        if(Manager.GameM.Dia >= 500)
        {
            UI_BuyItemPopup popup = Manager.UiM.MakeSubItem<UI_BuyItemPopup>(this.transform);
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
