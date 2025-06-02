using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AchievementPopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,
        AchievementScrollObject
    }


    enum Buttons
    {
        BackgroundButton
    }


    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.ContentObject));
    }
    public override bool Init()
    {
        if (!base.Init()) return false;
        gameObjectsType = typeof(GameObjects);
        ButtonsType = typeof(Buttons);

        BindObject(gameObjectsType);
        BindButton(ButtonsType);

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
        foreach(Data.AchievementData achievement in Manager.AchievementM.GetAchievements())
        {
            UI_AchievementItem item = Manager.UiM.MakeSubItem<UI_AchievementItem>(GetObject(gameObjectsType, (int)GameObjects.AchievementScrollObject).transform);
            item.SetInfo(achievement);
        }

        Manager.UiM.CheckRedDotObject(Define.RedDotObjectType.AchievementPopup);
    }

    void OnClickBgButton()
    {
        Manager.SoundM.PlayPopupClose();
        Manager.UiM.ClosePopup(this);
    }
}
