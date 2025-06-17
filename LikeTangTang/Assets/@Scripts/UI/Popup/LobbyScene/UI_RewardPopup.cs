using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RewardPopup : UI_Popup
{

    List<UI_MaterialItem> materialPool = new List<UI_MaterialItem>();
    List<UI_MergeEquipItem> itemPool = new List<UI_MergeEquipItem>();
    enum GameObjects
    {
        Content,
        RewardItemScrollContentObject,

    }

    enum Buttons
    {
        BackgroundButton,

    }   
    enum Texts
    {
        RewardPopupTitleText,

    }

    Queue<string> spriteNames;
    Queue<int> counts;
    Equipment equipment;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.Content));
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

        GetButton(ButtonsType, (int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBgButton);

        Manager.SoundM.Play(Define.Sound.Effect, "PopupOpen_Reward");
        return true;
    }


    public void SetInfo(Queue<string> _spriteNames, Queue<int> _counts, Equipment _equipment = null)
    {
        spriteNames = _spriteNames;
        counts = _counts;
        if(_equipment != null ) equipment = _equipment;
        SetItem();
    }

    void SetItem()
    {
        //첫번째 - 받는 아이템들
        Transform parent = GetObject(gameObjectsType, (int)GameObjects.RewardItemScrollContentObject).transform;
        
        if (equipment != null)
        {
            string name = spriteNames.Dequeue();
            int count = counts.Dequeue();

            while(itemPool.Count < 1)
            {
                UI_MergeEquipItem item =  Manager.UiM.MakeSubItem<UI_MergeEquipItem>(parent);
                itemPool.Add(item);
            }

            itemPool[0].SetInfo(equipment, count);
            itemPool[0].gameObject.SetActive(true);
        }
        

        //나머지 - 골드 및 재료
        
        int needItems = spriteNames.Count;

        while(materialPool.Count < needItems)
        {
            UI_MaterialItem item = Manager.UiM.MakeSubItem<UI_MaterialItem>(parent);
            materialPool.Add(item);
        }

        foreach (var slot in materialPool)
            slot.gameObject.SetActive(false);


        int index = 0;
        while(spriteNames.Count > 0)
        {
            materialPool[index].SetInfo(spriteNames.Dequeue(), counts.Dequeue());
            materialPool[index].gameObject.SetActive(true);
            index++;
        }
    }

    void OnClickBgButton()
    {
        Manager.SoundM.PlayPopupClose();
        gameObject.SetActive(false);

    }
}
