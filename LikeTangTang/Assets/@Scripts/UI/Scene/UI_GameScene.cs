using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using DG.Tweening;
using System.Linq;


public class UI_GameScene : UI_Scene
{
    enum GameObjects
    {
        WaveObject,
        BossInfoObject,
        EliteInfoObject,
        MonsterAlarmObject,
        BossAlarmObject,
    }

    enum Images
    {
        WhiteFlash,
        OnDamaged,
        BattleSkilI_Icon_1,
        BattleSkilI_Icon_2,
        BattleSkilI_Icon_3,
        BattleSkilI_Icon_4,
        BattleSkilI_Icon_5,
        BattleSkilI_Icon_6,
        EvolutionItem_Icon_1,
        EvolutionItem_Icon_2,
        EvolutionItem_Icon_3,
        EvolutionItem_Icon_4,
        EvolutionItem_Icon_5,
        EvolutionItem_Icon_6,
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
        PauseButton,
        HealButton
    }

    enum AlramType
    {
        Wave,
        Boss
    }

    private void Awake()
    {
        Init();
        Manager.GameM.player.Skills.UpdateSkillUI += OnLevelUpSkillUI;
    }

    private void OnDestroy()
    {
        Manager.GameM.player.Skills.UpdateSkillUI -= OnLevelUpSkillUI;
    }

    public override bool Init()
    {
        if (!base.Init()) return false;

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
        GetButton(ButtonsType, (int)Buttons.HealButton).gameObject.BindEvent(OnClickHealButton);

        GetObject(gameObjectsType, (int)GameObjects.BossInfoObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.EliteInfoObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.MonsterAlarmObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.BossAlarmObject).SetActive(false);
        OnPlayerDataUpdated();

        Manager.GameM.player.OnPlayerDataUpdated = OnPlayerDataUpdated;
        Manager.GameM.player.OnPlayerLevelUp = OnPlayerLevelUp;
        Manager.GameM.player.OnPlayerDamaged = OnDamaged;

        //TODO : 힐 횟수 확인 후 버튼 활성화 버튼 지우기
        GetButton(ButtonsType, (int)Buttons.HealButton).interactable = false;
        Refresh();

        return true;
    }

    void Refresh()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject(gameObjectsType, (int)GameObjects.WaveObject).GetComponent<RectTransform>());
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
        if (_second == 45 && Manager.GameM.CurrentWaveIndex < 9)
        {
            //TOOD : 알람
            StartCoroutine(SwitchAlarm(AlramType.Wave));
        }

        if(Manager.GameM.CurrentWaveData.BossMonsterID.Count > 0)
        {
            int bossGenTime = 55;
            if(_second == bossGenTime)
                StartCoroutine(SwitchAlarm(AlramType.Boss));
        }

        GetText(typeof(Texts), (int)Texts.TimeLimitValueText).text = $"{_minute:D2} : {_second:D2}";

        Refresh();

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

    void OnClickHealButton()
    {

    }

    void OnLevelUpSkillUI()
    {
        List<SkillBase> activeSkills = Manager.GameM.player.Skills.skillList.Where(skill => skill.isLearnSkill).ToList();

        for (int i = 0; i < activeSkills.Count; i++)
        {
            SetCurrentSkill(i, activeSkills[i]);
        }

        List<int> GetEvoloutionItems = Manager.GameM.player.Skills.evolutionItemList.ToList();
        for (int i = 0; i < GetEvoloutionItems.Count; i++)
        {
            SetEvolutionItem(i, GetEvoloutionItems[i]);
        }
    }

    void SetCurrentSkill(int _num, SkillBase _skill)
    {
        GetImage(ImagesType, (int)Images.BattleSkilI_Icon_1 + _num).sprite = Manager.ResourceM.Load<Sprite>(_skill.SkillDatas.SkillIcon);
        GetImage(ImagesType, (int)Images.BattleSkilI_Icon_1 + _num).enabled = true;
    }

    void SetEvolutionItem(int _num, int _evolutionItemID)
    {
        GetImage(ImagesType, (int)Images.EvolutionItem_Icon_1 + _num).sprite = Manager.ResourceM.Load<Sprite>(Manager.DataM.SkillEvolutionDic[_evolutionItemID].EvolutionItemIcon);
        GetImage(ImagesType, (int)Images.EvolutionItem_Icon_1 + _num).enabled = true;
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
            Append(GetImage(ImagesType, (int)Images.WhiteFlash).DOFade(1, 0.15f))
            .Append(GetImage(ImagesType, (int)Images.WhiteFlash).DOFade(0, 0.3f)).OnComplete(() => { });
    }
    

}
