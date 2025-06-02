using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterSelectPopup : UI_Popup
{
    //TODO : 캐릭터 정보 등등(캐릭터 레벨업은 직접 하는걸로 하자 그럼)
    enum GameObjects
    {
        LevelUpCheckmark,
        CharacterToggleCheckmark,
        CharacterLevelUpContentObject,
        CharacterSelectContentObject,
        CharacterInventoryContent,
        CharacterImage
    }

    enum Texts
    {
        CharacterLevelValueText,
        CharacterNameValueText,
        AttackValueText,
        AttackBonusValueText,
        HealthValueText,
        HealthBonusValueText,
        CharacterLevelUpCostMaterialText,
        AttackUpNowValueText,
        AttackUpAfterValueText,
        HpUpNowValueText,
        HpUpAfterValueText,
        DefUpNowValueText,
        DefUpAfterValueText,
        SpeedUpNowValueText,
        SpeedUpAfterValueText,
        CriticalUpNowValueText,
        CriticalUpAfterValueText,
        CiriticalDamageUpNowValueText,
        CiriticalDamageUpAfterValueText

    }

    enum Buttons
    {
        LevelUpButton,
        EquipButton,
        BackButton,

    }

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;

        gameObjectsType = typeof(GameObjects);
        TextsType = typeof(Texts);
        ButtonsType = typeof(Buttons);

        BindObject(gameObjectsType);
        BindText(TextsType);
        BindButton(ButtonsType);

        GetButton(ButtonsType, (int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);
        GetButton(ButtonsType, (int)Buttons.EquipButton).gameObject.BindEvent(OnClickEquipButton);
        GetButton(ButtonsType, (int)Buttons.LevelUpButton).gameObject.BindEvent(OnClickLevelUpButton);
        return true;
    }

    public void SetInfo()
    {

    }

    void Refresh()
    {
        GetObject(gameObjectsType, (int)GameObjects.CharacterImage).GetComponent<RawImage>().texture = Manager.SceneM.cam_target;
    }


    void OnClickBackButton()
    {

    }

    void OnClickEquipButton()
    {

    }

    void OnClickLevelUpButton()
    {

    }
}
