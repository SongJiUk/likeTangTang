using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using DG.Tweening;


public class UI_GameScene : UI_Scene
{
    enum GameObjects
    {
        BossInfoObject,
        EliteInfoObject,
        MonsterAlarmObject,
        BossAlarmObject,
    }

    enum Images
    {
        WhiteFlash,
        OnDamaged
    }

    public enum Texts
    {
        KillValueText,
        CharacterLevelValueText,
        WaveValueText,
        TimeLimitValueText,
        BossNameValueText,
        EliteNameValueText
    }
    public enum Sliders
    {
        ExpSliderObject,
        BossHpSliderObject,
        EliteHpSliderObject,
    }

    public enum Buttons
    {
        PauseButton
    }

    enum AlramType
    {
        Wave,
        Boss
    }

    public override bool Init()
    {
        TextsType = typeof(Texts);
        ButtonsType = typeof(Buttons);
        SlidersType = typeof(Sliders);
        gameObjectsType = typeof(GameObjects);
        ImagesType = typeof(Images);

        BindText(TextsType);
        BindButton(ButtonsType);
        BindSlider(SlidersType);
        BindObject(gameObjectsType);
        BindImage(ImagesType);

        GetButton(ButtonsType, (int)Buttons.PauseButton).gameObject.BindEvent(OnClickPauseButton);

        GetImage(ImagesType, (int)Images.WhiteFlash).gameObject.SetActive(false);
        GetImage(ImagesType, (int)Images.OnDamaged).gameObject.SetActive(false);

        GetObject(gameObjectsType, (int)GameObjects.BossInfoObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.EliteInfoObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.MonsterAlarmObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.BossAlarmObject).SetActive(false);
        OnPlayerDataUpdated();

        Manager.GameM.player.OnPlayerDataUpdated = OnPlayerDataUpdated;
        Manager.GameM.player.OnPlayerLevelUp = OnPlayerLevelUp;
        Manager.GameM.player.OnPlayerDamaged = OnDamaged;
        return true;
    }

    public void OnWaveStart(int _currentStageIndex)
    {
        GetText(typeof(Texts), (int)(Texts.WaveValueText)).text = _currentStageIndex.ToString();
    }
    
    public void OnWaveEnd()
    {
        GetObject(gameObjectsType, (int)GameObjects.MonsterAlarmObject).SetActive(false);
    }

    public void OnChangeSecond(int _minute, int _second)
    {
        if (_second == 30 && Manager.GameM.CurrentWaveIndex < 9)
        {
            //TOOD : 알람
            StartCoroutine(SwitchAlarm(AlramType.Wave));
        }

        if(Manager.GameM.CurrentWaveData.BossMonsterID.Count > 0)
        {
            // TODO : 알람
            StartCoroutine(SwitchAlarm(AlramType.Boss));
        }

        GetText(typeof(Texts), (int)Texts.TimeLimitValueText).text = $"{_minute:D2} : {_second:D2}";

        //TODO : 실험 해보기 
        //if (_second == 60)
        //    GetText(typeof(Texts), (int)Texts.TimeLimitValueText).text = "";

    }

    public void OnPlayerDataUpdated()
    {
        GetSlider(typeof(Sliders), (int)Sliders.ExpSliderObject).value = Manager.GameM.player.ExpRatio;
        GetText(typeof(Texts), (int)Texts.KillValueText).text = $"{Manager.GameM.player.KillCount}";
        GetText(typeof(Texts), (int)Texts.CharacterLevelValueText).text = $"{Manager.GameM.player.Level}";
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
        if(_mc.objType == Define.ObjectType.EliteMonster)
        {
            if(_mc.CreatureState != Define.CreatureState.Dead)
            {
                GetObject(gameObjectsType, (int)GameObjects.EliteInfoObject).SetActive(true);
                GetSlider(SlidersType, (int)Sliders.EliteHpSliderObject).value = _mc.Hp / _mc.MaxHp;
                GetText(TextsType, (int)Texts.EliteNameValueText).text = _mc.creatureData.NameKR;
            }
            else
                GetObject(gameObjectsType, (int)GameObjects.EliteInfoObject).SetActive(false);
            
        }
        else if(_mc.objType == Define.ObjectType.Boss)
        {
            if(_mc.CreatureState != Define.CreatureState.Dead)
            {
                GetObject(gameObjectsType, (int)GameObjects.BossInfoObject).SetActive(true);
                GetSlider(SlidersType, (int)Sliders.BossHpSliderObject).value = _mc.Hp / _mc.MaxHp;
                GetText(TextsType, (int)Texts.BossNameValueText).text = _mc.creatureData.NameKR;
            }
            else
                GetObject(gameObjectsType, (int)GameObjects.BossInfoObject).SetActive(false);
        }
    }

 

    void OnClickPauseButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.UiM.ShowPopup<UI_PausePopup>();
    }

    IEnumerator SwitchAlarm(AlramType _type)
    {
        switch (_type)
        {
            case AlramType.Wave:
                //Sound : Wave
                GetObject(gameObjectsType, (int)GameObjects.MonsterAlarmObject).SetActive(true);
                yield return new WaitForSeconds(3f);
                GetObject(gameObjectsType, (int)GameObjects.MonsterAlarmObject).SetActive(false);
                break;

            case AlramType.Boss:
                //Sound : Boss
                GetObject(gameObjectsType, (int)GameObjects.BossAlarmObject).SetActive(true);
                yield return new WaitForSeconds(3f);
                GetObject(gameObjectsType, (int)GameObjects.BossAlarmObject).SetActive(false);
                break;
        }

    }

    public void OnDamaged()
    {
        StartCoroutine(CoBloodScreen());
    }

    public void WhiteFlash()
    {
        StartCoroutine(CoWhiteScreen());
    }

    IEnumerator CoBloodScreen()
    {
        Color color = new Color(1, 0, 0, 0.3f);

        yield return null;

        DOTween.Sequence().
            Append(GetImage(ImagesType, (int)Images.OnDamaged).DOColor(color, 0.3f))
            .Append(GetImage(ImagesType, (int)Images.OnDamaged).DOColor(Color.clear, 0.3f)).OnComplete(() => { });

    }

    IEnumerator CoWhiteScreen()
    {
        Color color = new Color(1, 1, 1, 1f);

        yield return null;

        DOTween.Sequence().
            Append(GetImage(ImagesType, (int)Images.WhiteFlash).DOFade(1, 0.1f))
            .Append(GetImage(ImagesType, (int)Images.WhiteFlash).DOFade(0, 0.2f)).OnComplete(() => { });
    }

}
