using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class UI_CharacterSelectPopup : UI_Popup
{
    //TODO : 캐릭터 정보 등등(캐릭터 레벨업은 직접 하는걸로 하자 그럼)
    enum GameObjects
    {
        LevelUpToggleCheckmark,
        CharacterToggleCheckmark,
        CharacterLevelUpContentObject,
        CharacterSelectContentObject,
        CharacterSelectContent,
        CharacterImage,
        CharacterLevelObject,
        AttackPointObject,
        HealthPointObject
    }

    enum Texts
    {
        CharacterLevelValueText,
        CharacterNameValueText,
        AttackValueText,
        AttackBonusValueText,
        HealthValueText,
        HealthBonusValueText,
        EnhanceCostMaterialValueText,
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

    enum Toggles
    {
        LevelUpToggle,
        CharacterToggle
    }

    public Action OnChangeCharacter;
    Character character;
    Data.CreatureData creatureData;
    Data.CharacterLevelData characterNowLevelData;
    Data.CharacterLevelData characterLevelUpData;

    private UI_CharacterItem selectedItem;

    List<UI_CharacterItem> characterItemPool = new List<UI_CharacterItem>();
    private void Awake()
    {
        Init();
    }

    void OnEnable()
    {
        Events.OnCharacterSelected += HandleCharacterSeleected;
    }

    void OnDisable()
    {
        Events.OnCharacterSelected -= HandleCharacterSeleected;
    }

    void HandleCharacterSeleected(UI_CharacterItem _item)
    {
        selectedItem = _item;
    }

    public override bool Init()
    {
        if (!base.Init()) return false;

        gameObjectsType = typeof(GameObjects);
        TextsType = typeof(Texts);
        ButtonsType = typeof(Buttons);
        TogglesType = typeof(Toggles);

        BindObject(gameObjectsType);
        BindText(TextsType);
        BindButton(ButtonsType);
        BindToggle(TogglesType);

        GetObject(gameObjectsType, (int)GameObjects.CharacterImage).GetComponent<RawImage>().texture = Manager.SceneM.cam_target;


        GetButton(ButtonsType, (int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);
        GetButton(ButtonsType, (int)Buttons.EquipButton).gameObject.BindEvent(OnClickEquipButton);
        GetButton(ButtonsType, (int)Buttons.LevelUpButton).gameObject.BindEvent(OnClickLevelUpButton);

        GetToggle(TogglesType, (int)Toggles.LevelUpToggle).gameObject.BindEvent(OnClickLevelUpToggle);
        GetToggle(TogglesType, (int)Toggles.CharacterToggle).gameObject.BindEvent(OnClickCharacterToggle);



        return true;
    }

    public void SetInfo()
    {
        GetText(TextsType, (int)Texts.AttackUpAfterValueText).gameObject.SetActive(true);
        GetText(TextsType, (int)Texts.HpUpAfterValueText).gameObject.SetActive(true);
        GetText(TextsType, (int)Texts.DefUpAfterValueText).gameObject.SetActive(true);
        GetText(TextsType, (int)Texts.SpeedUpAfterValueText).gameObject.SetActive(true);
        GetText(TextsType, (int)Texts.CriticalUpAfterValueText).gameObject.SetActive(true);
        GetText(TextsType, (int)Texts.CiriticalDamageUpAfterValueText).gameObject.SetActive(true);
        GetText(TextsType, (int)Texts.EnhanceCostMaterialValueText).gameObject.SetActive(true);
        Refresh();
    }

    void Refresh()
    {

        if (!Manager.DataM.CreatureDic.TryGetValue(Manager.GameM.CurrentCharacter.DataId, out creatureData)) return;
        character = Manager.GameM.CurrentCharacter;
        if (character.Level == Manager.DataM.CharacterLevelDataDic.Count)
        {
            characterNowLevelData = Manager.DataM.CharacterLevelDataDic[character.Level];
        }
        else
        {
            characterNowLevelData = Manager.DataM.CharacterLevelDataDic[character.Level];
            characterLevelUpData = Manager.DataM.CharacterLevelDataDic[character.Level + 1];
        }
        

        GetText(TextsType, (int)Texts.CharacterLevelValueText).text = $"{Manager.GameM.CurrentCharacter.Level} / 30";
        GetText(TextsType, (int)Texts.CharacterNameValueText).text = $"{creatureData.NameKR}";
        float DefaultAttack = creatureData.Attack * creatureData.AttackRate;
        float AllAttack = DefaultAttack + characterNowLevelData.AttackUp;

        float Defaulthp = creatureData.MaxHp * creatureData.HpRate;
        float AllHp = Defaulthp + characterNowLevelData.HpUp;

        GetText(TextsType, (int)Texts.AttackValueText).text = $"{AllAttack}";
        GetText(TextsType, (int)Texts.AttackBonusValueText).text = $"(+{AllAttack - DefaultAttack})";

        GetText(TextsType, (int)Texts.HealthValueText).text = $"{AllHp}";
        GetText(TextsType, (int)Texts.HealthBonusValueText).text = $"(+{AllHp - Defaulthp})";

        OnClickLevelUpToggle();
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject(gameObjectsType, (int)GameObjects.CharacterLevelObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject(gameObjectsType, (int)GameObjects.AttackPointObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject(gameObjectsType, (int)GameObjects.HealthPointObject).GetComponent<RectTransform>());

    }

    void SetupLevelUp()
    {
         if (character.Level == Manager.DataM.CharacterLevelDataDic.Count)
        {
            MaxLevel();
            return;
        }

        //TODO : 레벨업 정보 초기화
        GetText(TextsType, (int)Texts.AttackUpNowValueText).text = $"{characterNowLevelData.AttackUp}";
        GetText(TextsType, (int)Texts.AttackUpAfterValueText).text = $"{characterLevelUpData.AttackUp}";
        GetText(TextsType, (int)Texts.HpUpNowValueText).text = $"{characterNowLevelData.HpUp}";
        GetText(TextsType, (int)Texts.HpUpAfterValueText).text = $"{characterLevelUpData.HpUp}";
        GetText(TextsType, (int)Texts.DefUpNowValueText).text = $"{characterNowLevelData.DefUp}";
        GetText(TextsType, (int)Texts.DefUpAfterValueText).text = $"{characterLevelUpData.DefUp}";
        GetText(TextsType, (int)Texts.SpeedUpNowValueText).text = $"{characterNowLevelData.SpeedUp}";
        GetText(TextsType, (int)Texts.SpeedUpAfterValueText).text = $"{characterLevelUpData.SpeedUp}";
        GetText(TextsType, (int)Texts.CriticalUpNowValueText).text = $"{characterNowLevelData.CriticalUp}";
        GetText(TextsType, (int)Texts.CriticalUpAfterValueText).text = $"{characterLevelUpData.CriticalUp}";
        GetText(TextsType, (int)Texts.CiriticalDamageUpNowValueText).text = $"{characterNowLevelData.CriticalDamageUp}";
        GetText(TextsType, (int)Texts.CiriticalDamageUpAfterValueText).text = $"{characterLevelUpData.CriticalDamageUp}";

        GetText(TextsType, (int)Texts.EnhanceCostMaterialValueText).text = $"{characterLevelUpData.NeedCouponCount}";
    }


    void MaxLevel()
    {
        Data.CharacterLevelData characterNowLevelData = Manager.DataM.CharacterLevelDataDic[character.Level];

        GetText(TextsType, (int)Texts.AttackUpNowValueText).text = $"{characterNowLevelData.AttackUp}";
        GetText(TextsType, (int)Texts.AttackUpAfterValueText).gameObject.SetActive(false);

        GetText(TextsType, (int)Texts.HpUpNowValueText).text = $"{characterNowLevelData.HpUp}";
        GetText(TextsType, (int)Texts.HpUpAfterValueText).gameObject.SetActive(false);

        GetText(TextsType, (int)Texts.DefUpNowValueText).text = $"{characterNowLevelData.DefUp}";
        GetText(TextsType, (int)Texts.DefUpAfterValueText).gameObject.SetActive(false);

        GetText(TextsType, (int)Texts.SpeedUpNowValueText).text = $"{characterNowLevelData.SpeedUp}";
        GetText(TextsType, (int)Texts.SpeedUpAfterValueText).gameObject.SetActive(false);

        GetText(TextsType, (int)Texts.CriticalUpNowValueText).text = $"{characterNowLevelData.CriticalUp}";
        GetText(TextsType, (int)Texts.CriticalUpAfterValueText).gameObject.SetActive(false);

        GetText(TextsType, (int)Texts.CiriticalDamageUpNowValueText).text = $"{characterNowLevelData.CriticalDamageUp}";
        GetText(TextsType, (int)Texts.CiriticalDamageUpAfterValueText).gameObject.SetActive(false);
        GetText(TextsType, (int)Texts.EnhanceCostMaterialValueText).gameObject.SetActive(false);
        GetButton(ButtonsType, (int)Buttons.LevelUpButton).gameObject.SetActive(false);
    }

    void SetupCharacterSelect()
    {
        //TODO : 캐릭터 선택 초기화
        Transform cont = GetObject(gameObjectsType, (int)GameObjects.CharacterSelectContent).transform;
        int needCount = Manager.GameM.Characters.Count;

        while (characterItemPool.Count < needCount)
        {
            UI_CharacterItem item = Manager.UiM.MakeSubItem<UI_CharacterItem>(cont);
            characterItemPool.Add(item);
        }

        foreach (var slot in characterItemPool)
            slot.gameObject.SetActive(false);

        int index = 0;

        for (int i = 0; i < Manager.GameM.Characters.Count; i++)
        {
            UI_CharacterItem item = characterItemPool[index++];
            item.gameObject.SetActive(true);

            item.SetInfo(Manager.GameM.Characters[i]);
        }

    }

    void OnClickLevelUpToggle()
    {
        GetObject(gameObjectsType, (int)GameObjects.CharacterLevelUpContentObject).SetActive(true);
        GetObject(gameObjectsType, (int)GameObjects.CharacterSelectContentObject).SetActive(false);

        GetButton(ButtonsType, (int)Buttons.LevelUpButton).gameObject.SetActive(true);
        GetButton(ButtonsType, (int)Buttons.EquipButton).gameObject.SetActive(false);

        GetObject(gameObjectsType, (int)GameObjects.LevelUpToggleCheckmark).SetActive(true);
        GetObject(gameObjectsType, (int)GameObjects.CharacterToggleCheckmark).SetActive(false);

        SetupLevelUp();
    }

    void OnClickCharacterToggle()
    {
        GetObject(gameObjectsType, (int)GameObjects.CharacterLevelUpContentObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.CharacterSelectContentObject).SetActive(true);

        GetButton(ButtonsType, (int)Buttons.LevelUpButton).gameObject.SetActive(false);
        GetButton(ButtonsType, (int)Buttons.EquipButton).gameObject.SetActive(true);

        GetObject(gameObjectsType, (int)GameObjects.LevelUpToggleCheckmark).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.CharacterToggleCheckmark).SetActive(true);

        SetupCharacterSelect();
    }

    void OnClickBackButton()
    {
        (Manager.UiM.SceneUI as UI_LobbyScene).Ui_EquipmentPopup.RefreshCharacterInfo();
        Manager.UiM.ClosePopup(this);
    }

    void OnClickEquipButton()
    {
        //TODO : 캐릭터 변경하면서 isCurrent어쩌고 바꿔주기

        if (selectedItem == null) return;

        Manager.GameM.CurrentCharacter.isCurrentCharacter = false;
        Character character = selectedItem.character;
        character.isCurrentCharacter = true;
        character.ChangeCharacter(character.DataId);

        Manager.SceneM.lobbyScene.ChangeCharacter();
        Refresh();

    }

    void OnClickLevelUpButton()
    {
        if (Manager.GameM.ItemDic[Define.ID_LevelUpCoupon] >= characterLevelUpData.NeedCouponCount)
        {
            UI_CharacterLevelupPopup popup = Manager.UiM.ShowPopup<UI_CharacterLevelupPopup>();
            popup.SetInfo(character);

            Manager.GameM.RemoveMaterialItem(Define.ID_LevelUpCoupon, characterLevelUpData.NeedCouponCount);
            Manager.GameM.CurrentCharacter.LevelUp();
            Refresh();

        }
        else
        {
            Manager.UiM.ShowToast("레벨업 쿠폰이 부족합니다.");
        }
    }
}
