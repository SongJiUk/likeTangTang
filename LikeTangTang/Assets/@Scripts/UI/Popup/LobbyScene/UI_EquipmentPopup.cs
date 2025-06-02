using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class UI_EquipmentPopup : UI_Popup
{
    List<UI_EquipItem> slotPool = new List<UI_EquipItem>();
    List<UI_MaterialItem> itemPool = new List<UI_MaterialItem>();
    Dictionary<Define.EquipmentType, UI_EquipItem> equipedSlotPool = new Dictionary<Define.EquipmentType, UI_EquipItem>();
    Define.EquipmentSortType equipmentSortType;
    string sort_Level = "정렬 : 레벨";
    string sort_Grade = "정렬 : 등급";
    public ScrollRect scrollRect;
    public bool isOpen = false;
    enum GameObjects
    {
        CharacterImage,
        ContentObject,
        WeaponEquipObject,
        GlovesEquipObject,
        RingEquipObject,
        ArmorEquipObject,
        HelmetEquipObject,
        BootsEquipObject,
        CharacterRedDotObject,
        MergeButtonRedDotObject,
        EquipInventoryObject,
        ItemInventoryObject,
        EquipInventoryGroupObject,
        ItemInventoryGroupObject
    }

    enum Buttons
    {
        CharacterButton,
        SortButton,
        MergeButton
    }

    enum Texts
    {
        AttackValueText,
        HealthValueText,
        SortButtonText,
        MergeButtonText,
        EquipInventoryTlileText,
        ItemInventoryTlileText
    }

    private void OnEnable()
    {
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.ContentObject));
    }
    private void Awake()
    {
        Init();
    }
    public override bool Init()
    {
        if (!base.Init()) return false;

        gameObjectsType = typeof(GameObjects);
        ButtonsType = typeof(Buttons);
        TextsType = typeof(Texts);

        BindObject(gameObjectsType);
        BindButton(ButtonsType);
        BindText(TextsType);

        GetObject(gameObjectsType, (int)GameObjects.CharacterRedDotObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.MergeButtonRedDotObject).SetActive(false);

        GetButton(ButtonsType, (int)Buttons.CharacterButton).gameObject.BindEvent(OnClickCharacterButton);
        GetButton(ButtonsType, (int)Buttons.MergeButton).gameObject.BindEvent(OnClickMergeButton);
        GetButton(ButtonsType, (int)Buttons.SortButton).gameObject.BindEvent(OnClickSortButton);

        equipmentSortType = Define.EquipmentSortType.Level;
        GetText(TextsType, (int)Texts.SortButtonText).text = sort_Level;

        GameObject WeaponCont = GetObject(gameObjectsType, (int)GameObjects.WeaponEquipObject);
        GameObject HelmetCont = GetObject(gameObjectsType, (int)GameObjects.HelmetEquipObject);
        GameObject GlovesCont = GetObject(gameObjectsType, (int)GameObjects.GlovesEquipObject);
        GameObject ArmorCont = GetObject(gameObjectsType, (int)GameObjects.ArmorEquipObject);
        GameObject BootsCont = GetObject(gameObjectsType, (int)GameObjects.BootsEquipObject);
        GameObject RingCont = GetObject(gameObjectsType, (int)GameObjects.RingEquipObject);


        if (equipedSlotPool.Count == 0)
        {
            equipedSlotPool[Define.EquipmentType.Weapon] = Manager.UiM.MakeSubItem<UI_EquipItem>(WeaponCont.transform);
            equipedSlotPool[Define.EquipmentType.Helmet] = Manager.UiM.MakeSubItem<UI_EquipItem>(HelmetCont.transform);
            equipedSlotPool[Define.EquipmentType.Glove] = Manager.UiM.MakeSubItem<UI_EquipItem>(GlovesCont.transform);
            equipedSlotPool[Define.EquipmentType.Armor] = Manager.UiM.MakeSubItem<UI_EquipItem>(ArmorCont.transform);
            equipedSlotPool[Define.EquipmentType.Boots] = Manager.UiM.MakeSubItem<UI_EquipItem>(BootsCont.transform);
            equipedSlotPool[Define.EquipmentType.Ring] = Manager.UiM.MakeSubItem<UI_EquipItem>(RingCont.transform);
        }

        GetObject(gameObjectsType, (int)GameObjects.CharacterImage).GetComponent<RawImage>().texture = Manager.SceneM.cam_target;
        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {

        foreach (var slot in equipedSlotPool.Values)
            slot.gameObject.SetActive(false);

        foreach (Equipment item in Manager.GameM.OwnedEquipment)
        {
            if (item.IsEquiped)
            {
                if (equipedSlotPool.TryGetValue(item.EquipmentData.EquipmentType, out var slot))
                {
                    slot.gameObject.SetActive(true);
                    slot.SetInfo(item, Define.UI_ItemParentType.CharacterEquipment);
                }
            }
        }

        SortEquipments();


        var (hp, attack) = Manager.GameM.GetCurrentCharacterStat();
        //TODO : Player를 먼저 Setting해줘야 될것 같음
        GetText(TextsType, (int)Texts.AttackValueText).text = (Manager.GameM.CurrentCharacter.Attack + attack).ToString();
        GetText(TextsType, (int)Texts.HealthValueText).text = (Manager.GameM.CurrentCharacter.MaxHp + hp).ToString();

        SetItem();

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject(gameObjectsType, (int)GameObjects.EquipInventoryObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject(gameObjectsType, (int)GameObjects.ItemInventoryObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject(gameObjectsType, (int)GameObjects.EquipInventoryGroupObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject(gameObjectsType, (int)GameObjects.ItemInventoryGroupObject).GetComponent<RectTransform>());
    }

    void SortEquipments()
    {
        Manager.GameM.SortEquipment(equipmentSortType);

        Transform parent = GetObject(gameObjectsType, (int)GameObjects.EquipInventoryObject).transform;

        int needCount = Manager.GameM.OwnedEquipment.Count(item => !item.IsEquiped);

        while (slotPool.Count < needCount)
        {
            UI_EquipItem popup = Manager.ResourceM.Instantiate("UI_EquipItem", parent).GetOrAddComponent<UI_EquipItem>();
            slotPool.Add(popup);
        }

        foreach (var slot in slotPool)
            slot.gameObject.SetActive(false);

        int index = 0;
        foreach (Equipment item in Manager.GameM.OwnedEquipment)
        {
            if (item.IsEquiped) continue;

            var slot = slotPool[index++];
            slot.gameObject.SetActive(true);
            slot.SetInfo(item, Define.UI_ItemParentType.EquipInventory, scrollRect);
        }
    }

    void SetItem()
    {

        Transform parent = GetObject(gameObjectsType, (int)GameObjects.ItemInventoryObject).transform;

        int needCount = Manager.GameM.ItemDic.Count;

        while (itemPool.Count < needCount)
        {
            UI_MaterialItem item = Manager.UiM.MakeSubItem<UI_MaterialItem>(parent);
            itemPool.Add(item);
        }

        foreach (var slot in itemPool)
            slot.gameObject.SetActive(false);

        int index = 0;

        foreach (int id in Manager.GameM.ItemDic.Keys)
        {
            if (Manager.DataM.MaterialDic.TryGetValue(id, out Data.MaterialData data))
            {
                UI_MaterialItem slot = itemPool[index++];
                slot.gameObject.SetActive(true);

                int count = Manager.GameM.ItemDic[id];
                slot.SetInfo(data, parent, count, scrollRect);
            }
        }


    }

    void OnClickSortButton()
    {
        //TODO : 정렬
        if (equipmentSortType == Define.EquipmentSortType.Level)
        {
            equipmentSortType = Define.EquipmentSortType.Grade;
            GetText(TextsType, (int)Texts.SortButtonText).text = sort_Grade;
        }
        else if (equipmentSortType == Define.EquipmentSortType.Grade)
        {
            equipmentSortType = Define.EquipmentSortType.Level;
            GetText(TextsType, (int)Texts.SortButtonText).text = sort_Level;
        }

        SortEquipments();
    }

    void OnClickMergeButton()
    {
        //TODO : 합성
        UI_MergePopup mergePopup = (Manager.UiM.SceneUI as UI_LobbyScene).Ui_MergePopup;

        if (mergePopup != null)
        {
            mergePopup.SetInfo(null);
            mergePopup.gameObject.SetActive(true);
        }

    }

    void OnClickCharacterButton()
    {
        //TODO : 캐릭터 변경
        UI_CharacterSelectPopup characterSelectPopup = Manager.UiM.ShowPopup<UI_CharacterSelectPopup>();
        characterSelectPopup.SetInfo();
    }

    public void RefreshCharacterInfo()
    {

        var (hp, attack) = Manager.GameM.GetCurrentCharacterStat();
        GetText(TextsType, (int)Texts.AttackValueText).text = (Manager.GameM.CurrentCharacter.Attack + attack).ToString();
        GetText(TextsType, (int)Texts.HealthValueText).text = (Manager.GameM.CurrentCharacter.MaxHp + hp).ToString();
    }

}
