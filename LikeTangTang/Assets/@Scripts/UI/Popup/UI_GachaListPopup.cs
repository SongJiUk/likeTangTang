using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GachaListPopup : UI_Popup
{
    enum GameObjects
    {

    }

    enum Texts
    {

    }

    enum Images
    {

    }

    enum Buttons
    {
        
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
        ImagesType = typeof(Images);

        BindObject(gameObjectsType);
        BindButton(ButtonsType);
        BindText(TextsType);
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
