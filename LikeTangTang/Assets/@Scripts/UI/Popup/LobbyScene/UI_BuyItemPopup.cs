using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class UI_BuyItemPopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,

    }

    enum Buttons
    {
        BuyButton,
        QuitButton
    }

    enum Texts
    {
        BuyItemContentText,

    }

    int UseItemCount = 0;
    int GetItemCount = 0;
    Data.MaterialData item;
    public Action OnCompleteBuyItem;
    void OnEnable()
    {
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.ContentObject));
    }
    void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if(!base.Init()) return false;

        gameObjectsType = typeof(GameObjects);
        ButtonsType = typeof(Buttons);
        TextsType = typeof(Texts);
        BindObject(gameObjectsType);
        BindButton(ButtonsType);
        BindText(TextsType);
        
        GetButton(ButtonsType, (int)Buttons.BuyButton).gameObject.BindEvent(OnClickBuyButton);
        GetButton(ButtonsType, (int)Buttons.QuitButton).gameObject.BindEvent(OnClickQuitButton);
        return true;
    }

    public void SetInfo(Data.MaterialData _item , int _useItemCount, int _getItemCount)
    {
        UseItemCount = _useItemCount;
        GetItemCount = _getItemCount;
        item = _item;
        GetText(TextsType, (int)Texts.BuyItemContentText).text =$"{item.NameTextID}를 구매하시겠습니까? ";
    }


    void OnClickBuyButton()
    {
        //TODO : 그 전에 재료가 없으면 걸러져서 들어오기때문에, itme, item의 소모개수, 획득개수를 받아와야하나?
        Manager.GameM.Dia -= UseItemCount;
        if( item.MaterialType == Define.MaterialType.Gold)
        {
            
            Manager.GameM.Gold += GetItemCount * (int)Manager.GameM.CurrentCharacter.Evol_GoldBonus;
        }
        else if(item.MaterialType == Define.MaterialType.Stamina)
        {
            Manager.GameM.Stamina += GetItemCount;
        }
        else
        {
            Manager.GameM.AddMaterialItem(item.MaterialID, GetItemCount);
        }
        
        UI_RewardPopup Popup = (Manager.UiM.SceneUI as UI_LobbyScene).Ui_RewardPopup;

        Queue<string> spriteName = new Queue<string>();
        Queue<int> count = new Queue<int>();

        spriteName.Enqueue(item.SpriteName);
        count.Enqueue(GetItemCount);
        Popup.SetInfo(spriteName, count);
        Popup.gameObject.SetActive(true);
        OnCompleteBuyItem?.Invoke();


        gameObject.SetActive(false);
        Manager.ResourceM.Destory(gameObject);
        
    }

    void OnClickQuitButton()
    {
        Manager.ResourceM.Destory(gameObject);
    }
}
