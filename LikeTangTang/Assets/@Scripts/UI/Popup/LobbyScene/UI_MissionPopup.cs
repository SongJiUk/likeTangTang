using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class UI_MissionPopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,
        DailyMissionScrollObject,

    }

    enum Texts
    {
        DailyMissionCommentText,

    }

    enum Buttons
    {
        BackgroundButton,

    }

    enum Images
    {

    }

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;
        gameObjectsType = typeof(GameObjects);
        TextsType = typeof(Texts);
        ButtonsType = typeof(Buttons);
        ImagesType = typeof(Images);

        BindObject(gameObjectsType);
        BindText(TextsType);
        BindButton(ButtonsType);
        BindImage(ImagesType);

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
        foreach(KeyValuePair<int, Data.MissionData> data in Manager.DataM.MissionDataDic)
        {
            if (data.Value.MissionType == Define.MissionType.Daily)
            {
                UI_MissionItem mission = Manager.UiM.MakeSubItem<UI_MissionItem>(GetObject(gameObjectsType, (int)GameObjects.DailyMissionScrollObject).transform);
                mission.SetInfo(data.Value);
            }
        }

        Manager.UiM.CheckRedDotObject(Define.RedDotObjectType.Mission);


    }

    void OnClickBgButton()
    {
        Manager.UiM.ClosePopup(this);
    }
}
