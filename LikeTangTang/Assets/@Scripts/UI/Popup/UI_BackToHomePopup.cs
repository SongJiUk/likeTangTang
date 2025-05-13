using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BackToHomePopup : UI_Popup
{


    enum Buttons
    { }

    private void Awake()
    {
        Init();
    }
    public override bool Init()
    {
        if (!base.Init()) return false;
        ButtonsType = typeof(Buttons);

        BindButton(ButtonsType);




        return true;
    }

}
