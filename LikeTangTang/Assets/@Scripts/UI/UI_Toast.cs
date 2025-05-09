using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Toast : UI_Base
{
    enum Texts
    {
        ToastMessageValueText
    }
    void Awake()
    {
        Init();

    }

    public override bool Init()
    {
        if(!base.Init()) return false;
        TextsType = typeof(Texts);

        BindText(TextsType);

        return true;
    } 

    public void SetInfo(string _detail)
    {
        GetText(TextsType, (int)Texts.ToastMessageValueText).text = _detail;
        StartCoroutine(CoDestoryToast());
    }

    IEnumerator CoDestoryToast()
    {
        yield return new WaitForSeconds(1f);
        Manager.UiM.CloseToast(this);
    }
}
