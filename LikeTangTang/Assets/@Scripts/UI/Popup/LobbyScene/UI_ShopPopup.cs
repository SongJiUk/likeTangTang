using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UI_ShopPopup : UI_Base
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
        GetButton(ButtonsType, (int)Buttons.FreeGoldButton).gameObject.BindEvent(OnClickFreeGoldButton);
        GetButton(ButtonsType, (int)Buttons.FirstGoldProductButton).gameObject.BindEvent(OnClickFirstGoldProductButton);
        GetButton(ButtonsType, (int)Buttons.SecondGoldProductButton).gameObject.BindEvent(OnClickSecondGoldProductButton);


        Refresh();
        return true;
    }

    void Refresh()
    {
        Manager.GameM.ItemDic.TryGetValue(Define.ID_BORONZE_KEY, out int bronzeKeyCount);
        Manager.GameM.ItemDic.TryGetValue(Define.ID_BORONZE_KEY, out int silverKeyCount);
        Manager.GameM.ItemDic.TryGetValue(Define.ID_BORONZE_KEY, out int goldKeyCount);

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

        int goldAmount = Define.STAGE_GOLD_UP;
        //TODO : 리워드 데이터..스테이지마다 + 1000
        goldAmount *= Manager.GameM.GetMaxStageIndex();

        GetText(TextsType, (int)Texts.FreeGoldTitleText).text = $"{goldAmount}";
        GetText(TextsType, (int)Texts.FirstGoldProductTitleText).text = $"{goldAmount * 3}";
        GetText(TextsType, (int)Texts.SecondGoldProductTitleText).text = $"{goldAmount * 5}";
    }


    void DoGaCha(Define.GachaType _gachaType, int _count =1)
    {
        List<Equipment> list = new List<Equipment>();

        list = Manager.GameM.DoGaCha(_gachaType, _count).ToList();
        Manager.UiM.ShowPopup<UI_GachaResultsPopup>().SetInfo(list);
    }


    //열쇠
    void OnClickAdKeyButton()
    {
        Manager.SoundM.PlayButtonClick();
        if(Manager.GameM.SilverKeyCountAds > 0)
        {
            //TODO : 광고 실행, 리워드 받기
        }
    }

    void OnClickSilverKeyButton()
    {
        Manager.SoundM.PlayButtonClick();
        if(Manager.GameM.Dia > 150)
        {
            string[] spriteName = new string[1];
            int[] count = new int[1];

            spriteName[0] = Manager.DataM.MaterialDic[Define.ID_SILVER_KEY].SpriteName;
            count[0] = 1;

            //리워드
        }
    }

    void OnClickGoldKeyButton()
    {
    
    }

    //상급장비
    void OnClickAdvancedBoxListButton()
    {
        Manager.SoundM.PlayButtonClick();
    }
    
    void OnClickAdvancedBoxADButton()
    {
        Manager.SoundM.PlayButtonClick();
        if(Manager.GameM.GachaCountAdsAdvanced > 0)
        {
            DoGaCha(Define.GachaType.AdvancedGacha, 1);
            Refresh();
        }
    }

    void OnClickAdvancedBoxOpenButton()
    {

    }

    void OnClickAdvancedBoxTenOpenButton()
    {

    }

    //일반 장비

    void OnClickCommonGachaListButton()
    {

    }

    void OnClickADCommonGachaOpenButton()
    {

    }

    void OnClickCommonGachaOpenButton()
    {

    }

    //골드

    void OnClickFreeGoldButton()
    {

    }

    void OnClickFirstGoldProductButton()
    {

    }

    void OnClickSecondGoldProductButton()
    {

    }
}
