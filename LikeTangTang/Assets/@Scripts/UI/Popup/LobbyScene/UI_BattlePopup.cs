using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattlePopup : UI_Base
{

    public enum Toggles
    {
        GameStartToggle
    }
    public override bool Init()
    {
        SetUIInfo();

        GetToggle(typeof(Toggles), (int)Toggles.GameStartToggle).gameObject.BindEvent(() =>
        {
            Manager.SceneM.LoadScene(Define.SceneType.GameScene);
        });
        return true;
    }

    protected override void SetUIInfo()
    {
        Bind<Toggle>(typeof(Toggles));
    }

    protected override void RefreshUI()
    {
        
    }
}
