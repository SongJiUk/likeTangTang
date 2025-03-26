using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;


public class UI_GameScene : UI_Base
{


/*  TODO : 코드에서 불러와서 완전 자동화 하기  
    public enum Texts
    {

    }
    public enum Sliders
    {

    }
*/

    [SerializeField]
    TextMeshProUGUI killCount;
    
    [SerializeField]
    Slider gemSlider;

    [SerializeField]
    TextMeshProUGUI level;

    public void SetPlayerLevel(int _level)
    {
        level.text = $"{_level}";
    }
    public void SetGemCountRatio(float _ratio)
    {
        gemSlider.value = _ratio;
    }

    public void SetKillCount(int _killCount)
    {
        killCount.text = $"{_killCount}";
    }

    public void CreateJoyStick(string _key)
    {
        var joyStick = Manager.ResourceM.Instantiate(_key);
        joyStick.name = "@UI_JoyStick";
    }

    public void SetUI()
    {
        
    }
    
    public void RefreshUI()
    {

    }

   
}
