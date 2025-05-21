using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UI_CheckOutPopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,
        CheckOutBoardObject,
        FirstClearRewardCompleteObject,
        SecondClearRewardCompleteObject,
        ThirdClearRewardCompleteObject
    }

    enum Sliders
    {
        CheckOutProgressSliderObject,
    }

    enum Texts
    {
        DaysCountText,

    }

    enum Buttons
    {
        BackgroundButton,
        FirstCheckOutClearRewardButton,
        SecondCheckOutClearRewardButton,
        ThirdCheckOutClearRewardButton
    }

    enum Images
    {

    }

    public int userCheckOutDay;
    int monthlyCount;
    int dailyCount;
    Transform makeItemParent;
    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;
        gameObjectsType = typeof(GameObjects);
        SlidersType = typeof(Sliders);
        TextsType = typeof(Texts);
        ButtonsType = typeof(Buttons);
        ImagesType = typeof(Images);

        BindObject(gameObjectsType);
        BindSlider(SlidersType);
        BindText(TextsType);
        BindButton(ButtonsType);
        BindImage(ImagesType);

        GetButton(ButtonsType, (int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBgButton);
        GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardCompleteObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.SecondClearRewardCompleteObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.ThirdClearRewardCompleteObject).SetActive(false);

        return true;

    }

    public void SetInfo(int _checkOutDay)
    {
        userCheckOutDay = _checkOutDay;
        Refresh();
    }

    void Refresh()
    {
        if (userCheckOutDay == 0) return; 

        monthlyCount = userCheckOutDay % DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        
        if(monthlyCount == 0)
        {
            for(int i =0; i<Manager.GameM.AttendanceReceived.Length; i++)
            {
                Manager.GameM.AttendanceReceived[i] = false;
            }
            monthlyCount = 1;
            userCheckOutDay = 1;
            Manager.TimeM.AttendanceDay = userCheckOutDay;
        }

        dailyCount = userCheckOutDay % 10;

        if (dailyCount == 0)
            dailyCount = 10;


        Transform parent = GetObject(gameObjectsType, (int)GameObjects.CheckOutBoardObject).transform;
        makeItemParent = parent;


        for(int count = 1; count <=10; count++)
        {
            UI_CheckOutItem item = Manager.UiM.MakeSubItem<UI_CheckOutItem>(makeItemParent);
            item.transform.SetAsLastSibling();
            
            item.SetInfo(userCheckOutDay, count, dailyCount >= count);
        }

        if(monthlyCount >= 10 && monthlyCount < 20)
            GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardCompleteObject).gameObject.SetActive(true);
        else if(monthlyCount >=20 && monthlyCount <30)
        {
            GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardCompleteObject).gameObject.SetActive(true);
            GetObject(gameObjectsType, (int)GameObjects.SecondClearRewardCompleteObject).gameObject.SetActive(true);
        }
        else if(monthlyCount >= 30)
        {
            GetObject(gameObjectsType, (int)GameObjects.ThirdClearRewardCompleteObject).gameObject.SetActive(true);
            GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardCompleteObject).gameObject.SetActive(true);
            GetObject(gameObjectsType, (int)GameObjects.SecondClearRewardCompleteObject).gameObject.SetActive(true);
        }
            


        GetText(TextsType, (int)Texts.DaysCountText).text = $"{monthlyCount}일";
        GetSlider(SlidersType, (int)Sliders.CheckOutProgressSliderObject).value = monthlyCount;
    }

    void OnClickBgButton()
    {
        Manager.SoundM.PlayPopupClose();
        userCheckOutDay = 0;
        Manager.ResourceM.Destory(gameObject);
    }

    public void OnClickButtonTest()
    {
        Debug.Log("On click start button");
        userCheckOutDay += 1;
        Refresh();
    }
}
