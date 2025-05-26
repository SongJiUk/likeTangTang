using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CheckOutItem : UI_Base
{
    enum GameObjects
    {
        ClearRewardCompleteObject
    }

    enum Texts
    {
        DayValueText,
        RewardItemCountValueText
    }

    enum Images
    {
        RewardItemImage,
        RewardItemBackgroundImage
    }

    int dayCount;
    int tenDayCount;
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

        GetObject(gameObjectsType, (int)GameObjects.ClearRewardCompleteObject).SetActive(false);


        return true;
    }

    public void SetInfo(int _dayCount, int _tendayCount, bool _isCheckOut)
    {
        dayCount = _dayCount;
        tenDayCount = _tendayCount;
        isCheckOut = _isCheckOut;
        transform.localScale = Vector3.one;
        Refresh();
    }

    void Refresh()
    {
        if (tenDayCount == 0) return;


        int rewardItemID = Manager.DataM.AttendanceCheckDataDic[tenDayCount].RewardItemId;
        int rewardItemCount = Manager.DataM.AttendanceCheckDataDic[tenDayCount].RewardItemValue;
        Define.MaterialGrade gradeType = Manager.DataM.MaterialDic[rewardItemID].MaterialGrade;

        // TODO : 아이템 초기화
        GetText(TextsType, (int)Texts.DayValueText).text = $"{tenDayCount} Day";
        GetText(TextsType, (int)Texts.RewardItemCountValueText).text = $"{rewardItemCount}";
        GetImage(ImagesType, (int)Images.RewardItemImage).sprite = Manager.ResourceM.Load<Sprite>(Manager.DataM.MaterialDic[rewardItemID].SpriteName);
        GetImage(ImagesType, (int)Images.RewardItemBackgroundImage).color = Define.EquipmentUIColors.MaterialGradeStyles[gradeType].BgColor;


        //TODO : 체크아웃해 서 체크표시 초기화
        if(isCheckOut)
        {
            GetObject(gameObjectsType, (int)GameObjects.ClearRewardCompleteObject).SetActive(true);

            if(!Manager.GameM.AttendanceReceived[dayCount -1])
            {
                int num = ((dayCount - 1) % 10 + 1);

                Manager.GameM.AttendanceReceived[dayCount-1] = true;
                int matID = Manager.DataM.AttendanceCheckDataDic[num].RewardItemId;

                
                Queue<string> name = new ();
                Queue<int> count = new ();

                name.Enqueue(Manager.DataM.MaterialDic[matID].SpriteName);
                count.Enqueue(Manager.DataM.AttendanceCheckDataDic[num].RewardItemValue);


                UI_RewardPopup popup =  (Manager.UiM.SceneUI as UI_LobbyScene).Ui_RewardPopup;
                popup.gameObject.SetActive(true);
                Manager.GameM.ExchangeMaterial(Manager.DataM.MaterialDic[matID], Manager.DataM.AttendanceCheckDataDic[num].RewardItemValue);

                if (dayCount > 10 && Manager.DataM.AttendanceCheckDataDic.ContainsKey(dayCount))
                {
                    matID = Manager.DataM.AttendanceCheckDataDic[dayCount].RewardItemId;
                    name.Enqueue(Manager.DataM.MaterialDic[matID].SpriteName);
                    count.Enqueue(Manager.DataM.AttendanceCheckDataDic[dayCount].RewardItemValue);
                    Manager.GameM.ExchangeMaterial(Manager.DataM.MaterialDic[matID], Manager.DataM.AttendanceCheckDataDic[dayCount].RewardItemValue);
                }

                popup.SetInfo(name, count);
                Manager.TimeM.AttendanceDay = dayCount;
                Manager.GameM.SaveGame();
            }

            
        }
        else
            GetObject(gameObjectsType, (int)GameObjects.ClearRewardCompleteObject).SetActive(false);
    }

}
