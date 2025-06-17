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

        int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        int maxDataDays = Manager.GameM.AttendanceReceived.Length;

        int usableDays = Mathf.Min(daysInMonth, maxDataDays);

        if(Manager.TimeM.LastAttendanceResetDate != DateTime.Today && DateTime.Now.Day == 1)
        {
            Manager.TimeM.LastAttendanceResetDate = DateTime.Today;
            for (int i = 0; i < maxDataDays; i++)
            {
                Manager.GameM.AttendanceReceived[i] = false;
            }
            userCheckOutDay = 1;
            Manager.TimeM.AttendanceDay = 1;
        }

        if(userCheckOutDay > usableDays)
            userCheckOutDay = ((userCheckOutDay - 1) % usableDays) + 1;

        monthlyCount = userCheckOutDay;
        dailyCount = (userCheckOutDay - 1) % 10 + 1;

       

        //monthlyCount = userCheckOutDay % DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        
        ////초기화 데이
        //if(Manager.TimeM.LastAttendanceResetDate != DateTime.Today &&  DateTime.Now.Day == 1)
        //{
        //    Manager.TimeM.LastAttendanceResetDate = DateTime.Today;
        //    for (int i = 0; i < Manager.GameM.AttendanceReceived.Length; i++)
        //    {
        //        Manager.GameM.AttendanceReceived[i] = false;
        //    }
        //    monthlyCount = 1;
        //    userCheckOutDay = 1;
        //    Manager.TimeM.AttendanceDay = userCheckOutDay;
        //}

        //dailyCount = (userCheckOutDay - 1) % 10 + 1;

        //if (dailyCount == 0)
        //    dailyCount = 10;



        GetObject(gameObjectsType, (int)GameObjects.CheckOutBoardObject).DestroyChilds(); 
         Transform parent = GetObject(gameObjectsType, (int)GameObjects.CheckOutBoardObject).transform;
        makeItemParent = parent;
            

        for(int count = 1; count <=10; count++)
        {
            UI_CheckOutItem item = Manager.UiM.MakeSubItem<UI_CheckOutItem>(makeItemParent);
            item.transform.SetAsLastSibling();
            
            item.SetInfo(userCheckOutDay, count, dailyCount >= count);
        }

        if (usableDays >= 15 && monthlyCount >= 15)
            GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardCompleteObject).gameObject.SetActive(true);
        if (usableDays >= 20 && monthlyCount >= 20)
            GetObject(gameObjectsType, (int)GameObjects.SecondClearRewardCompleteObject).gameObject.SetActive(true);
        if (usableDays >= 30 && monthlyCount >= 30)
            GetObject(gameObjectsType, (int)GameObjects.ThirdClearRewardCompleteObject).gameObject.SetActive(true);

        GetText(TextsType, (int)Texts.DaysCountText).text = $"{monthlyCount}일";
        var slider = GetSlider(SlidersType, (int)Sliders.CheckOutProgressSliderObject);
        slider.maxValue = usableDays;
        slider.value = monthlyCount;
    }

    void OnClickBgButton()
    {
        Manager.SoundM.PlayPopupClose();
        userCheckOutDay = 0;
        Manager.UiM.ClosePopup(this);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            userCheckOutDay += 1;
            Refresh();

        }
        
    }
}
