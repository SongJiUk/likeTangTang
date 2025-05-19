using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UI_GachaListPopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,
        GachaInfoContentObject,
        CommonGachaGradeRateItem,
        UncommonGachaGradeRateItem,
        RareGachaGradeRateItem,
        EpicGachaGradeRateItem,
        CommonGachaRateListObject,
        UncommonGachaRateListObject,
        RareGachaRateListObject,
        EpicGachaRateListObject
    }

    enum Texts
    {
        EpicGradeRateValueText,
        RareGradeRateValueText,
        UncommonGradeRateValueText,
        CommonGradeRateValueText,
    }

    enum Images
    {
        CommonGradeTitle,
        UncommonGradeTitle,
        RareGradeTitle,
        EpicGradeTitle,
        
    }



    enum Buttons
    {
        BackgroundButton,

    }

    Define.GachaType gachaType;
    Dictionary<Transform, List<UI_GachaRateItem>> rateItemPool = new();
    void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.ContentObject));
    }


    public override bool Init()
    {
        if(!base.Init()) return false;
        gameObjectsType = typeof(GameObjects);
        ButtonsType = typeof(Buttons);
        TextsType = typeof(Texts);
        ImagesType = typeof(Images);

        BindObject(gameObjectsType);
        BindButton(ButtonsType);
        BindText(TextsType);
        BindImage(ImagesType);

        GetButton(ButtonsType, (int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBgButton);

        return true;
    }

    public void SetInfo(Define.GachaType _gachaType)
    {
        gachaType = _gachaType;
        Refresh();
    }

    private void OnDisable()
    {
        DeactivateAllItem();
    }

    void Refresh()
    {
        if (gachaType == Define.GachaType.None) return;

        List<Data.GachaRateData> list = Manager.DataM.GachaTableDataDic[gachaType].GachaRateTable.ToList();
        list.Reverse();

        float commonRate = 0f;
        float unCommonRate = 0f;
        float rareRate = 0f;
        float uniqueRate = 0f;

        foreach(Data.GachaRateData item in list)
        {
            switch (item.EquipGrade)
            {
                case Define.EquipmentGrade.Common:
                    GetImage(ImagesType, (int)Images.CommonGradeTitle).color = Define.EquipmentUIColors.EquipGradeStyles[item.EquipGrade].BgColor;
                    commonRate += item.GachaRate;
                    var commonItem = GetRateItem(GetObject(gameObjectsType, (int)GameObjects.CommonGachaRateListObject).transform);
                    commonItem.SetInfo(item);
                    break;

                case Define.EquipmentGrade.UnCommon:
                    GetImage(ImagesType, (int)Images.UncommonGradeTitle).color = Define.EquipmentUIColors.EquipGradeStyles[item.EquipGrade].BgColor;
                    unCommonRate += item.GachaRate;
                    var uncommonItem = GetRateItem(GetObject(gameObjectsType, (int)GameObjects.UncommonGachaRateListObject).transform);
                    uncommonItem.SetInfo(item);
                    break;

                case Define.EquipmentGrade.Rare:
                    GetImage(ImagesType, (int)Images.RareGradeTitle).color = Define.EquipmentUIColors.EquipGradeStyles[item.EquipGrade].BgColor;
                    rareRate += item.GachaRate;
                    var rareItem = GetRateItem(GetObject(gameObjectsType, (int)GameObjects.RareGachaRateListObject).transform);
                    rareItem.SetInfo(item);
                    break;

                case Define.EquipmentGrade.Epic:
                    GetImage(ImagesType, (int)Images.EpicGradeTitle).color = Define.EquipmentUIColors.EquipGradeStyles[item.EquipGrade].BgColor;
                    uniqueRate += item.GachaRate;
                    var epicItem = GetRateItem(GetObject(gameObjectsType, (int)GameObjects.EpicGachaRateListObject).transform);
                    epicItem.SetInfo(item);
                    break;
            }

        }


        GetText(TextsType, (int)Texts.CommonGradeRateValueText).text = commonRate.ToString("P2");
        GetText(TextsType, (int)Texts.UncommonGradeRateValueText).text = unCommonRate.ToString("P2");
        GetText(TextsType, (int)Texts.RareGradeRateValueText).text = rareRate.ToString("P2");
        GetText(TextsType, (int)Texts.EpicGradeRateValueText).text = uniqueRate.ToString("P2");

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject(gameObjectsType, (int)GameObjects.CommonGachaRateListObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject(gameObjectsType, (int)GameObjects.UncommonGachaRateListObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject(gameObjectsType, (int)GameObjects.RareGachaRateListObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject(gameObjectsType, (int)GameObjects.EpicGachaRateListObject).GetComponent<RectTransform>());
        StartCoroutine(CoResetScrollPosition());
    }

    UI_GachaRateItem GetRateItem(Transform _parent)
    {
        if (!rateItemPool.ContainsKey(_parent)) rateItemPool[_parent] = new();

        var pool = rateItemPool[_parent];

        var item = pool.FirstOrDefault(i => !i.gameObject.activeSelf);
        if(item == null)
        {
            item = Manager.UiM.MakeSubItem<UI_GachaRateItem>(_parent);
            pool.Add(item);
        }

        item.gameObject.SetActive(true);
        return item;
    }

    void DeactivateAllItem()
    {
        foreach(Transform child in GetObject(gameObjectsType, (int)GameObjects.CommonGachaRateListObject).transform)
            child.gameObject.SetActive(false);
        
        foreach (Transform child in GetObject(gameObjectsType, (int)GameObjects.UncommonGachaRateListObject).transform)
            child.gameObject.SetActive(false);
        
        foreach (Transform child in GetObject(gameObjectsType, (int)GameObjects.RareGachaRateListObject).transform)
            child.gameObject.SetActive(false);
        
        foreach (Transform child in GetObject(gameObjectsType, (int)GameObjects.EpicGachaRateListObject).transform)
            child.gameObject.SetActive(false);
        
    }

    IEnumerator CoResetScrollPosition()
    {
        yield return null;
        GetObject(gameObjectsType, (int)GameObjects.ContentObject).GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
    }

    void OnClickBgButton()
    {
        Manager.SoundM.PlayPopupClose();

        Manager.ResourceM.Destory(gameObject);
    }
}
