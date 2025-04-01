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


        return true;
    }

    protected override void SetUIInfo()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));
    }

    public void SetPlayerLevel(int _level)
    {
        GetText(typeof(Texts), (int)Texts.CharacterLevelValueText).text = $"{_level}";
    }
    public void SetGemCountRatio(float _ratio)
    {
        GetSlider(typeof(Sliders), (int)Sliders.ExpSliderObject).value = _ratio;
    }

    public void SetKillCount(int _killCount)
    {
        GetText(typeof(Texts), (int)Texts.KillValueText).text = $"{_killCount}";
    }

    public void CreateJoyStick(string _key)
    {
        var joyStick = Manager.ResourceM.Instantiate(_key);
        joyStick.name = "@UI_JoyStick";
    }


   
}
