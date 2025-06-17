using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;


public class UI_SkillSelectPopup : UI_Popup
{

    GameManager gm;
    #region 

    enum GameObjects
    {
        Content,
        ADRefreshDisabledObject,
        CardRefreshDisabledObject,
        SkillCardSelectListObject
    }
    enum Texts
    {
        CharacterLevelValueText,
        BeforeLevelValueText,
        AfterLevelValueText,
        CardRefreshText,
        CardRefreshCountValueText,

    }

    enum Sliders
    {
        ExpSliderObject,

    }
    
    enum Buttons
    {
        ADRefreshButton,
        CardRefreshButton
    }

    enum Images
    {
        BattleSkilI_Icon_0,
        BattleSkilI_Icon_1,
        BattleSkilI_Icon_2,
        BattleSkilI_Icon_3,
        BattleSkilI_Icon_4,
        BattleSkilI_Icon_5,

        EvolutionItem_Icon_0,
        EvolutionItem_Icon_1,
        EvolutionItem_Icon_2,
        EvolutionItem_Icon_3,
        EvolutionItem_Icon_4,
        EvolutionItem_Icon_5
    }

    #endregion

    // [x] 스킬 팝업 그리드를 찾아서, 프리팹을 만들어 채워줘야함
    [SerializeField]
    Transform _grid;

    List<UI_Base> _items = new List<UI_Base>();

    void OnEnable()
    {
        Init();
        PopupOpenAnim(GetObject(typeof(GameObjects), (int)GameObjects.Content));
    }

    public override bool Init()
    {
        if(!base.Init()) return false;

        gm = Manager.GameM;
        gameObjectsType = typeof(GameObjects);
        ButtonsType = typeof(Buttons);
        TextsType = typeof(Texts);
        SlidersType = typeof(Sliders);
        ImagesType = typeof(Images);

        BindObject(gameObjectsType);
        BindButton(ButtonsType);
        BindText(TextsType);
        BindSlider(SlidersType);
        BindImage(ImagesType);
        

        GetButton(ButtonsType, (int)Buttons.ADRefreshButton).gameObject.BindEvent(OnClickAdRefreshButton);
        GetButton(ButtonsType, (int)Buttons.CardRefreshButton).gameObject.BindEvent(OnClickCardRefreshButton);

        GetObject(gameObjectsType, (int)GameObjects.ADRefreshDisabledObject).gameObject.SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.CardRefreshDisabledObject).gameObject.SetActive(false);


        RefreshUI();

        PopulateCardItem();
        List<SkillBase> activeSkills = gm.player.Skills.skillList.Where(skill => skill.isLearnSkill).ToList();

        for (int i = 0; i < activeSkills.Count; i++)
        {
            SetCurrentSkill(i, activeSkills[i]);
        }

        List<int> GetEvoloutionItems = gm.player.Skills.evolutionItemList.ToList();
        for(int i =0; i<GetEvoloutionItems.Count; i++)
        {
            SetEvolutionItem(i, GetEvoloutionItems[i]);
        }


        Manager.SoundM.Play(Define.Sound.Effect, "PopupOpen_SkillSelect");
        return true;


    }

    protected override void RefreshUI()
    {
        GetSlider(SlidersType, (int)Sliders.ExpSliderObject).value = Manager.GameM.player.ExpRatio;
        GetText(TextsType, (int)Texts.CharacterLevelValueText).text = $"{gm.player.Level}";
        GetText(TextsType, (int)Texts.BeforeLevelValueText).text = $"LV. {gm.player.Level - 1}";
        GetText(TextsType, (int)Texts.AfterLevelValueText).text = $"LV. {gm.player.Level}";

        if(gm.player.SkillRefreshCount > 0 )
        {
            GetText(TextsType, (int)Texts.CardRefreshText).text = $"<color=white>새로고침</color>";
            GetText(TextsType, (int)Texts.CardRefreshCountValueText).text = $"<color=white>{gm.player.SkillRefreshCount} / 3</color>";
        }
       
        else
        {
            GetText(TextsType, (int)Texts.CardRefreshText).text = $"<color=red>새로고침</color>";
            GetText(TextsType, (int)Texts.CardRefreshCountValueText).text = $"<color=red>{gm.player.SkillRefreshCount}</color>";
            GetObject(gameObjectsType, (int)GameObjects.ADRefreshDisabledObject).gameObject.SetActive(true);
        }
            
    }
    void PopulateCardItem()
    {
        GameObject cont = GetObject(typeof(GameObjects), (int)GameObjects.SkillCardSelectListObject);
        cont.DestroyChilds();
        //ist<SkillBase> skillList = gm.player.Skills.Test();
        List<object> skillList = gm.player.Skills.GetSkills();

        foreach (var candidate in skillList)
        {
            UI_SkillCardItem item = Manager.UiM.MakeSubItem<UI_SkillCardItem>(cont.transform);
            item = item.GetComponent<UI_SkillCardItem>();
            item.Init();

            if (candidate is SkillBase skill)
            {
                item.SetInfo(skill);
            }
            else if (candidate is int evolutionItemID)
            {
                item.SetInfo(_evolutionItemID: evolutionItemID);
            }
        }

        Manager.TimeM.TimeStop();
    }


    void SetCurrentSkill(int _index, SkillBase _skill)
    {
        GetImage(ImagesType, _index).sprite = Manager.ResourceM.Load<Sprite>(_skill.SkillDatas.SkillIcon);
        GetImage(ImagesType, _index).enabled = true;
    }

    void SetEvolutionItem(int _index, int _dataID)
    {
        GetImage(ImagesType, (int)Images.EvolutionItem_Icon_0 + _index).sprite = Manager.ResourceM.Load<Sprite>(Manager.DataM.SkillEvolutionDic[_dataID].EvolutionItemIcon);
        GetImage(ImagesType, (int)Images.EvolutionItem_Icon_0 + _index).enabled = true;
    }


    public void OnClickAdRefreshButton()
    {
        Manager.SoundM.PlayButtonClick();
        PopulateCardItem();
        RefreshUI();
    }

    public void OnClickCardRefreshButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (gm.player.SkillRefreshCount > 0)
        {
            PopulateCardItem();
            gm.player.SkillRefreshCount--;
        }

        RefreshUI();
    }
}
