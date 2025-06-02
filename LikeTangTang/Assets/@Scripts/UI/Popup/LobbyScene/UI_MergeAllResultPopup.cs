using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MergeAllResultPopup : UI_Popup
{
    List<UI_MergeEquipItem> mergeItemPool = new List<UI_MergeEquipItem>();
    enum GameObjects
    {
        ContentObject,
        MergeAlIScrollContentObject
    }

    enum Buttons
    {
        BackgroundButton
    }

    enum Texts
    {

    }

    enum Images
    {

    }

    private void Awake()
    {
        Init();   
    }

    private void OnEnable()
    {
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.ContentObject));
    }
     
    public override bool Init()
    {
        if (!base.Init()) return false;

        gameObjectsType = typeof(GameObjects);
        ButtonsType = typeof(Buttons);
        TextsType = typeof(Texts);
        ImagesType = typeof(Images);

        BindObject(gameObjectsType);
        BindButton(ButtonsType);
        BindText(TextsType);
        BindImage(ImagesType);

        GetButton(ButtonsType, (int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackButton);


        return true;
    }

    public void SetInfo(List<Equipment> _equipments)
    {
        if (_equipments == null) return; 

        Transform parent = GetObject(gameObjectsType, (int)GameObjects.MergeAlIScrollContentObject).transform;

        int index = 0;
        foreach (Equipment mergementItem in _equipments)
        {
            UI_MergeEquipItem item;
            if(index < mergeItemPool.Count)
            {
                item = mergeItemPool[index];
                item.gameObject.SetActive(true);
            }
            else
            {
                item = Manager.UiM.MakeSubItem<UI_MergeEquipItem>(parent);
                mergeItemPool.Add(item);
            }

            item.SetInfo(mergementItem, Define.UI_ItemParentType.EquipInventory);

            index++;
        }

        for (int i = index; i < mergeItemPool.Count; i++)
            mergeItemPool[i].gameObject.SetActive(false);
        
    }

    void Refresh(UI_MergeEquipItem equipment)
    {

    }

    public void OnClickBackButton()
    {
        Manager.SoundM.PlayPopupClose();
        gameObject.SetActive(false);
    }
}
