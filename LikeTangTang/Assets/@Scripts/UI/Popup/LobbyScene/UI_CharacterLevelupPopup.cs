using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CharacterLevelupPopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject
    }

    enum Texts
    {

        EquipmentNameValueText,
        CharacterLevelValueText,
        BeforeLevelValueText,
        AfterLevelValueText,
        BeforeATKValueText,
        AfterATKValueText,
        BeforeHPValueText,
        AfterHPValueText,
        BeforeDefValueText,
        AfterDefValueText,
        BeforeSpeedValueText,
        AfterSpeedValueText,
        BeforeCriticalValueText,
        AfterCriticalValueText,
        BeforeCriticalDamageValueText,
        AfterCriticalDamageValueText

    }

    enum Buttons
    {
        BackgroundButton
    }

    enum Images
    {
        CharacterImage,

    }

    Character character;
    void Awake()
    {
        Init();
    }

    void OnEnable()
    {
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.ContentObject));
    }

    public override bool Init()
    {
        if (!base.Init()) return false;

        gameObjectsType = typeof(GameObjects);
        TextsType = typeof(Texts);
        ButtonsType = typeof(Buttons);
        ImagesType = typeof(Images);

        BindObject(gameObjectsType);
        BindText(TextsType);
        BindButton(ButtonsType);
        BindImage(ImagesType);

        GetButton(ButtonsType, (int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBgButton);
        return true;
    }

    public void SetInfo(Character _character)
    {
        character = _character;
        Refresh();
    }

    void Refresh()
    {
        //TODO : 30에서 31 넘어갈때 막아야됌.

        if (!Manager.DataM.CreatureDic.TryGetValue(character.DataId, out var creatureData)) return;

        Data.CharacterLevelData characterNowLevelData = Manager.DataM.CharacterLevelDataDic[character.Level];
        Data.CharacterLevelData characterLevelUpData = Manager.DataM.CharacterLevelDataDic[character.Level + 1];
        GetText(TextsType, (int)Texts.EquipmentNameValueText).text = $"{creatureData.NameKR}";

        GetImage(ImagesType, (int)Images.CharacterImage).sprite = Manager.ResourceM.Load<Sprite>(creatureData.Image_Name);
        GetText(TextsType, (int)Texts.CharacterLevelValueText).text = $"Lv. {character.Level + 1}";

        GetText(TextsType, (int)Texts.BeforeLevelValueText).text = $"{characterNowLevelData.Level}";
        GetText(TextsType, (int)Texts.AfterLevelValueText).text = $"{characterLevelUpData.Level}";

        GetText(TextsType, (int)Texts.BeforeATKValueText).text = $"{characterNowLevelData.AttackUp}";
        GetText(TextsType, (int)Texts.AfterATKValueText).text = $"{characterLevelUpData.AttackUp}";

        GetText(TextsType, (int)Texts.BeforeHPValueText).text = $"{characterNowLevelData.HpUp}";
        GetText(TextsType, (int)Texts.AfterHPValueText).text = $"{characterLevelUpData.HpUp}";

        GetText(TextsType, (int)Texts.BeforeDefValueText).text = $"{characterNowLevelData.DefUp}";
        GetText(TextsType, (int)Texts.AfterDefValueText).text = $"{characterLevelUpData.DefUp}";

        GetText(TextsType, (int)Texts.BeforeSpeedValueText).text = $"{characterNowLevelData.SpeedUp}";
        GetText(TextsType, (int)Texts.AfterSpeedValueText).text = $"{characterLevelUpData.SpeedUp}";

        GetText(TextsType, (int)Texts.BeforeCriticalValueText).text = $"{characterNowLevelData.CriticalUp}";
        GetText(TextsType, (int)Texts.AfterCriticalValueText).text = $"{characterLevelUpData.CriticalUp}";

        GetText(TextsType, (int)Texts.BeforeCriticalDamageValueText).text = $"{characterNowLevelData.CriticalDamageUp}";
        GetText(TextsType, (int)Texts.AfterCriticalDamageValueText).text = $"{characterLevelUpData.CriticalDamageUp}";


    }

    

    void OnClickBgButton()
    {
        Manager.SoundM.PlayPopupClose();
        Manager.UiM.ClosePopup(this);
    }
}
