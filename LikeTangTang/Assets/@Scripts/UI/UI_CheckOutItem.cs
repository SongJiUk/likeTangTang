using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CheckOutItem : UI_Base
{
    enum GameObjects
    {
        RewardItemImage
    }

    enum Texts
    {
        DayValueText,
        RewardItemCountValueText
    }

    enum Images
    {
        RewardItemImage
    }

    int dayCount;
    bool isCheckOut;
    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;

        gameObjectsType = typeof(GameObjects);
        TextsType = typeof(Texts);
        ImagesType = typeof(Images);

        BindObject(gameObjectsType);
        BindText(TextsType);
        BindImage(ImagesType);

        GetObject(gameObjectsType, (int)GameObjects.RewardItemImage).SetActive(false);


        return true;
    }

    public void SetInfo(int _dayCount, bool _isCheckOut)
    {
        dayCount = _dayCount;
        isCheckOut = _isCheckOut;

        Refresh();
    }

    void Refresh()
    {
        if (dayCount == 0) return;


        int rewardItemID = Manager.DataM.AttendanceCheckDataDic[dayCount].RewardItemId;
        int rewardItemCount = Manager.DataM.AttendanceCheckDataDic[dayCount].RewardItemValue;

        // TODO : 아이템 초기화


        //TODO : 체크아웃해서 체크표시 초기화
        if(isCheckOut)
        {

        }
    }

}
