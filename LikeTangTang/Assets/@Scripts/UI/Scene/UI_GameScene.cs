using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;


public class UI_GameScene : UI_Base
{
    public enum Texts
    {
        KillValueText,
        CharacterLevelValueText
        
    }
    public enum Sliders
    {
        ExpSliderObject,

    }
    public override bool Init()
    {
        SetUIInfo();

        Manager.GameM.player.OnPlayerDataUpdated = OnPlayerDataUpdated;
        Manager.GameM.player.OnPlayerLevelUp = OnPlayerLevelUp;
        return true;
    }

    protected override void SetUIInfo()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));
    }
    //[ ] 이건 플레이어 쪽에서 관리할것임.
    public void OnPlayerDataUpdated()
    {
        GetSlider(typeof(Sliders), (int)Sliders.ExpSliderObject).value = Manager.GameM.player.ExpRatio;
        GetText(typeof(Texts), (int)Texts.KillValueText).text = $"{Manager.GameM.player.KillCount}";
    }
   
    public void OnPlayerLevelUp()
    {
        GetText(typeof(Texts), (int)Texts.CharacterLevelValueText).text = $"{Manager.GameM.ContinueDatas.Level}";
    }
}
