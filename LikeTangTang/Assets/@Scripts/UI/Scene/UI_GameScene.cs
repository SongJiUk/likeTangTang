using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using DG.Tweening;


public class UI_GameScene : UI_Scene
{
    public enum Texts
    {
        KillValueText,
        CharacterLevelValueText,
        WaveValueText,
        TimeLimitValueText
    }
    public enum Sliders
    {
        ExpSliderObject,

    }

    public enum Buttons
    {
        PauseButton
    }

   

    public override bool Init()
    {
        TextsType = typeof(Texts);
        ButtonsType = typeof(Buttons);
        SlidersType = typeof(Sliders);

        BindText(TextsType);
        BindButton(ButtonsType);
        BindSlider(SlidersType);

        GetButton(ButtonsType, (int)Buttons.PauseButton).gameObject.BindEvent(OnClickPauseButton);

        Manager.GameM.player.OnPlayerDataUpdated = OnPlayerDataUpdated;
        Manager.GameM.player.OnPlayerLevelUp = OnPlayerLevelUp;
        return true;
    }

    public void OnWaveStart(int _currentStageIndex)
    {
        GetText(typeof(Texts), (int)(Texts.WaveValueText)).text = _currentStageIndex.ToString();
    }

    public void OnWaveEnd()
    {

    }

    public void OnChangeSecond(int _minute, int _second)
    {
        if (_second == 3 && Manager.GameM.CurrentWaveIndex < 9)
        {
            //TOOD : 알람
        }

        if(Manager.GameM.CurrentWaveData.BossMonsterID.Count > 0)
        {
           // TODO : 알람
        }

        GetText(typeof(Texts), (int)Texts.TimeLimitValueText).text = $"{_minute:D2} : {_second:D2}";

        if (_second == 60)
            GetText(typeof(Texts), (int)Texts.TimeLimitValueText).text = "";

    }

    public void OnPlayerDataUpdated()
    {
        GetSlider(typeof(Sliders), (int)Sliders.ExpSliderObject).value = Manager.GameM.player.ExpRatio;
        GetText(typeof(Texts), (int)Texts.KillValueText).text = $"{Manager.GameM.player.KillCount}";
    }
   
    public void OnPlayerLevelUp()
    {
        if(Manager.GameM.isGameEnd) return;

        //TODO : 스킬 개수 가져와서 팝업 띄우기
        //List<SkillBase> list = Manager.GameM.player.Skills.RecommendSkills();
        List<object> list = Manager.GameM.player.Skills.GetSkills();
        if(list.Count > 0) Manager.UiM.ShowPopup<UI_SkillSelectPopup>();

        GetSlider(typeof(Sliders), (int)Sliders.ExpSliderObject).value = Manager.GameM.player.ExpRatio;
        GetText(typeof(Texts), (int)Texts.CharacterLevelValueText).text = $"{Manager.GameM.ContinueDatas.Level}";
    }

    public void MonsterInfoUpdate(MonsterController _mc)
    {

    }

    public void BossMonsterInfoUpdate(BossController _bc)
    {

    }

    public void WhiteFlash()
    {
        StartCoroutine(CoWhiteFlash());
    }

    IEnumerator CoWhiteFlash()
    {
        Color color = Color.white;
        yield return null;

        //DOTween.Sequence().Append(GetObject(int)gameObjects.WhiteFlash)
    }

    void OnClickPauseButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.UiM.ShowPopup<UI_PausePopup>();
    }
}
