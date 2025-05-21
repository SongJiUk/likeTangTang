using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillCardItem : UI_Base
{

    /*[x] : 어떤 스킬?, 몇 레벨?, 데이트 시트, Set, ClickItem
    */

    enum Images
    {
        EvoSkillINeedITemImage,
        SkillImage,

    }

    enum Texts
    {
        SkillDescriptionText,
        CardNameText
    }

    enum GameObjects
    {
        StarOn_0,
        StarOn_1,
        StarOn_2,
        StarOn_3,
        StarOn_4,
        StarOff_0,
        StarOff_1,
        StarOff_2,
        StarOff_3,
        StarOff_4,
        NewIImageObject,
        EvoSkillInfoObject
    }

    enum Buttons
    {
        SkillCardBackgroundImage
    }

    public int templateID;
    public Data.SkillData skillData;
    GameManager gm;
    public override bool Init()
    {
        //TODO : 새로고침하면,  new, star 초기화가 안되어있음.
        if(!base.Init()) return false;
        gm = Manager.GameM;
        gameObjectsType = typeof(GameObjects);
        ButtonsType = typeof(Buttons);
        TextsType = typeof(Texts);
        ImagesType = typeof(Images);


        BindObject(gameObjectsType);
        BindButton(ButtonsType);
        BindText(TextsType);
        BindImage(ImagesType);

        GetObject(gameObjectsType, (int)GameObjects.EvoSkillInfoObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.NewIImageObject).gameObject.SetActive(false);

        for(int i =0; i<Define.MAX_SKILL_LEVEL; i++)
        {
            GetObject(gameObjectsType, (int)GameObjects.StarOn_0 + i).SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.StarOff_0 + i).SetActive(true);
        }
        
        GetButton(ButtonsType, (int)Buttons.SkillCardBackgroundImage).gameObject.BindEvent(OnClickItem);

        transform.localScale = Vector3.one;

        return true;
    }


    //NOTE : 버튼을 찾아서 동적으로 클릭함수를 넣어주는 코드(인자가 있으면 람다로 해줘야함)
    public void Click()
    {

    }
    public void Click(int a)
    {

    }
    SkillBase skill;
    int evolutionItemID;
    public void SetInfo(SkillBase _skill = null, int _evolutionItemID = 0)
    {
        if (_skill != null)
        {
            skill = _skill;

            //초기화
            GetObject(gameObjectsType, (int)GameObjects.NewIImageObject).gameObject.SetActive(false);
            for(int i =0; i<Define.MAX_SKILL_LEVEL; i++)
            {
                GetObject(gameObjectsType, (int)GameObjects.StarOn_0 + i).SetActive(false);
            }


            if (skill.SkillLevel == 0)
            {
                GetObject(gameObjectsType, (int)GameObjects.NewIImageObject).gameObject.SetActive(true);
            }

            //TODO : 스킬 아이템 세팅 
            if (skill.SkillLevel == 5)
            {
                GetObject(gameObjectsType, (int)GameObjects.EvoSkillInfoObject).gameObject.SetActive(true);
                GetImage(ImagesType, (int)Images.EvoSkillINeedITemImage).sprite = Manager.ResourceM.Load<Sprite>(Manager.DataM.SkillEvolutionDic[skill.SkillDatas.EvolutionItemID].EvolutionItemIcon);
            }


            GetText(TextsType, (int)Texts.CardNameText).text = $"{skill.SkillDatas.SkillName}";
            GetText(TextsType, (int)Texts.SkillDescriptionText).text = $"{skill.SkillDatas.SkillDescription}";
            GetImage(ImagesType, (int)Images.SkillImage).sprite = Manager.ResourceM.Load<Sprite>(skill.SkillDatas.SkillIcon);

            for(int i =0; i< Define.MAX_SKILL_LEVEL; i++)
            {
                GetObject(gameObjectsType, (int)GameObjects.StarOff_0 + i).SetActive(true);
            }
            for (int i = 0; i < skill.SkillLevel; i++)
            {
                GetObject(gameObjectsType, (int)GameObjects.StarOn_0 + i).SetActive(true);
            }
        }
        else if(_evolutionItemID != 0)
        {


            evolutionItemID = _evolutionItemID;
            Data.SkillEvolutionData evoData = Manager.DataM.SkillEvolutionDic[_evolutionItemID];



            GetObject(gameObjectsType, (int)GameObjects.NewIImageObject).gameObject.SetActive(true);
            GetImage(ImagesType, (int)Images.SkillImage).sprite = Manager.ResourceM.Load<Sprite>(Manager.DataM.SkillEvolutionDic[_evolutionItemID].EvolutionItemIcon);
            GetText(TextsType, (int)Texts.CardNameText).text = $"{evoData.EvolutionItemName}";
            GetText(TextsType, (int)Texts.SkillDescriptionText).text = $"{evoData.EvolutionItemDescription}";


            for(int i =0; i< Define.MAX_SKILL_LEVEL; i++)
            {
                GetObject(gameObjectsType, (int)GameObjects.StarOn_0 + i).SetActive(false);
                GetObject(gameObjectsType, (int)GameObjects.StarOff_0 + i).SetActive(false);
            }
        }
    }

    //지워라 이거: 스킬 어디서 배우는지 확인, 어떻게 추가되는지도 확인 그거 확인해서 지우면 됌
    public void OnClickItem()
    {
        //TODO : 스킬 고를때 선택하면 잘못 선택되는 경우가 있음(테스트 좀 많이 해봐야 될듯)
        if(skill != null)
        {
           Manager.GameM.player.Skills.LevelUpSkill(skill.Skilltype);
        }
        else if(evolutionItemID != 0)
        {
            Manager.GameM.player.Skills.TryEvolveSkill(evolutionItemID);
        }

        Manager.TimeM.TimeReStart();
        Manager.UiM.ClosePopup();
    }

}