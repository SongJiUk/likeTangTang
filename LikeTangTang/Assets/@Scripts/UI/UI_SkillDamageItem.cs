using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillDamageItem : UI_Base
{
    enum GameObjects
    {

    }

    enum Texts
    {

    }

    enum Buttons
    {

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




        return true;

    }

    public void SetInfo()
    {

    }

    void Refresh()
    {

    }
}
