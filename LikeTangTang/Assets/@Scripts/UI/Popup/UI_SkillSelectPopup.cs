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
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindSlider(typeof(Sliders));
        BindImage(typeof(Images));
        

        GetButton(typeof(Buttons), (int)Buttons.ADRefreshButton).gameObject.BindEvent(OnClickAdRefreshButton);
        GetButton(typeof(Buttons), (int)Buttons.CardRefreshButton).gameObject.BindEvent(OnClickCardRefreshButton);

        GetObject(typeof(GameObjects), (int)GameObjects.ADRefreshDisabledObject).gameObject.SetActive(false);
        GetObject(typeof(GameObjects), (int)GameObjects.CardRefreshDisabledObject).gameObject.SetActive(false);


        RefreshUI();

        PopulateCardItem();
        List<SkillBase> activeSkills = gm.player.Skills.skillList.Where(skill => skill.isLearnSkill).ToList();

        for(int i = 0; i<activeSkills.Count; i++)
        {
            SetCurrentSkill(i, activeSkills[i]);
        }

        //TODO : Sound
        return true;


    }

    protected override void RefreshUI()
    {   
        GetText(typeof(Texts), (int)Texts.CharacterLevelValueText).text = $"{gm.player.Level}";
        GetText(typeof(Texts), (int)Texts.BeforeLevelValueText).text = $"LV. {gm.player.Level - 1}";
        GetText(typeof(Texts), (int)Texts.AfterLevelValueText).text = $"LV. {gm.player.Level}";

        if(gm.player.SkillRefreshCount > 0 )
        {
            GetText(typeof(Texts), (int)Texts.CardRefreshText).text = $"<color=white>새로고침</color>";
            GetText(typeof(Texts), (int)Texts.CardRefreshCountValueText).text = $"{gm.player.SkillRefreshCount} / 3";
        }
       
        else
        {
            GetText(typeof(Texts), (int)Texts.CardRefreshText).text = $"<color=red>새로고침</color>";
            GetText(typeof(Texts), (int)Texts.CardRefreshCountValueText).text = $"<color=red>{gm.player.SkillRefreshCount}</color>";
            GetObject(typeof(GameObjects), (int)GameObjects.ADRefreshDisabledObject).gameObject.SetActive(true);
        }
            
    }
    void PopulateCardItem()
    {
        GameObject cont = GetObject(typeof(GameObjects), (int)GameObjects.SkillCardSelectListObject);
        cont.DestoryChilds();
        List<SkillBase> skillList = gm.player.Skills.RecommendSkills();

        foreach(SkillBase skill in skillList)
        {
            UI_SkillCardItem item = Manager.UiM.MakeSubItem<UI_SkillCardItem>(cont.transform);
            item = item.GetComponent<UI_SkillCardItem>();
            item.Init();
            item.SetInfo(skill);
        }

        Manager.TimeM.TimeStop();
    } 

    void SetCurrentSkill(int _index, SkillBase _skill)
    {
        GetImage(typeof(Images), _index).sprite = Manager.ResourceM.Load<Sprite>(_skill.SkillDatas.SkillIcon);
        GetImage(typeof(Images), _index).enabled = true;
    }

    public void OnClickAdRefreshButton()
    {   
        //TODO : Sound
    }

    public void OnClickCardRefreshButton()
    {
        if(gm.player.SkillRefreshCount > 0)
        {
            PopulateCardItem();
            gm.player.SkillRefreshCount--;
        }

        RefreshUI();
    }
}
