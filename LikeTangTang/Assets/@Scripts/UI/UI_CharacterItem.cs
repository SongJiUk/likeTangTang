using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CharacterItem : UI_Base
{
    enum GameObjects
    {
        SelectObject,
        EquipedObject,
        LockObject
    }
    enum Images
    {
        BackgroundImage,
        CharacterImage,

    }
    enum Texts
    {
        CharacterLevelValueText,

    }
    enum Buttons
    {
        UI_CharacterItem
    }
    Character character;
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

        GetButton(ButtonsType, (int)Buttons.UI_CharacterItem).gameObject.BindEvent(OnClickCharacter);

        GetObject(gameObjectsType, (int)GameObjects.SelectObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.EquipedObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.LockObject).SetActive(false);

        return true;
    }

    public void SetInfo(Character _character)
    {
        character = _character;

        GetObject(gameObjectsType, (int)GameObjects.SelectObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.EquipedObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.LockObject).SetActive(false);

        Refresh();
    }

    void Refresh()
    {
        if (!Manager.DataM.CreatureDic.TryGetValue(character.DataId, out var creatureData)) return;
        GetImage(ImagesType, (int)Images.CharacterImage).sprite = Manager.ResourceM.Load<Sprite>(creatureData.Image_Name);
        GetText(TextsType, (int)Texts.CharacterLevelValueText).text = $"Lv. {character.Level}";

        if (character.isCurrentCharacter)
            GetObject(gameObjectsType, (int)GameObjects.EquipedObject).SetActive(true);


        if (creatureData.UnLockStage >= Manager.GameM.CurrentStageData.StageIndex)
            GetObject(gameObjectsType, (int)GameObjects.LockObject).SetActive(true);
        
    }

    void OnClickCharacter()
    {
        GetObject(gameObjectsType, (int)GameObjects.SelectObject).SetActive(true);
    }
}
