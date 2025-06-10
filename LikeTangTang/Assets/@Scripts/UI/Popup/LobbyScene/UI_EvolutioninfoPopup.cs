using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_EvolutioninfoPopup : UI_Popup
{
    enum GameObjects
    {
        Content
    }

    enum Images
    {
        TargetImage,
    }

    enum Texts
    {
        TargetNameText,
        TargetDescriptionText,
        GoldNameText
    }

    enum Buttons
    {
        BackgroundButton,
        LearnButton,
        QuitButton
    }

    public Action OnLearnCallBack;
    Data.EvolutionData data;
    int level;
    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;

        gameObjectsType = typeof(GameObjects);
        ImagesType = typeof(Images);
        TextsType = typeof(Texts);
        ButtonsType = typeof(Buttons);

        BindObject(gameObjectsType);
        BindImage(ImagesType);
        BindText(TextsType);
        BindButton(ButtonsType);

        GetButton(ButtonsType, (int)Buttons.LearnButton).gameObject.BindEvent(OnClickLearnButton);
        GetButton(ButtonsType, (int)Buttons.QuitButton).gameObject.BindEvent(OnClickQuitButton);
        GetButton(ButtonsType, (int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickQuitButton);


        return true;
    }


    public void SetInfo(Data.EvolutionData _data, int _level)
    {
        data = _data;
        level = _level;
        Refresh();
    }

    void Refresh()
    {
        GetImage(ImagesType, (int)Images.TargetImage).sprite = Manager.ResourceM.Load<Sprite>(data.SpriteName);
        GetText(TextsType, (int)Texts.TargetNameText).text = Manager.DataM.SpecialSkillDic[data.EvolutionAbilityNum].Name;
        GetText(TextsType, (int)Texts.TargetDescriptionText).text = Manager.DataM.SpecialSkillDic[data.EvolutionAbilityNum].Description;
        GetText(TextsType, (int)Texts.GoldNameText).text = $"{data.NeedGold}골드가 필요합니다.";

    }

    void OnClickLearnButton()
    {
        if(Manager.GameM.Gold >= data.NeedGold)
        {
            //배우기
            Manager.GameM.Gold -= data.NeedGold;
            Manager.GameM.CurrentCharacter.Evolution(level);
            OnLearnCallBack?.Invoke();
            Manager.UiM.ClosePopup(this);
        }
        else
        {
            Manager.UiM.ShowToast("골드가 부족합니다.");
        }
    }

    void OnClickQuitButton()
    {
        Manager.UiM.ClosePopup(this);
    }
}
