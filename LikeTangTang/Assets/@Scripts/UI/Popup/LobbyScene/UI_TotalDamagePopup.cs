using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UI_TotalDamagePopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,
        TotalDamageContentObject,
    }

    enum Buttons
    {
        BackgroundButton,

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


        return true;

    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        GetObject(gameObjectsType, (int)GameObjects.TotalDamageContentObject).DestroyChilds();
        List<SkillBase> skillList = Manager.GameM.player.Skills.skillList
            .Where(skill => skill.isLearnSkill)
            .OrderByDescending(skill => skill.TotalDamage)
            .ToList();
        foreach(SkillBase skill in skillList)
        {

            UI_SkillDamageItem item = Manager.UiM.MakeSubItem<UI_SkillDamageItem>(GetObject(gameObjectsType, (int)GameObjects.TotalDamageContentObject).transform);
            item.SetInfo(skill);
            item.transform.localScale = Vector3.one;
        }
    }

    void OnClickBgButton()
    {
        Manager.UiM.ClosePopup(this);
    }
}
