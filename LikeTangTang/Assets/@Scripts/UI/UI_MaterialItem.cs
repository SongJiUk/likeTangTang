using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MaterialItem : UI_Base
{

    enum GameObjects
    {
        GetEffectObject
    }

    enum Texts
    {
        ItemCountValueText
    }

    enum Images
    {
        MaterialItemImage,
        MaterialItemBackgroundImage
    }




    Data.MaterialData materialData;
    Transform parent;
    ScrollRect scrollRect;
    bool isDrag = false;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;

        gameObjectsType = typeof(GameObjects);
        TextsType = typeof(Texts);
        ImagesType = typeof(Images);

        BindObject(gameObjectsType);
        BindText(TextsType);
        BindImage(ImagesType);

        GetObject(gameObjectsType, (int)GameObjects.GetEffectObject).SetActive(false);
        

        gameObject.BindEvent(null, OnDrag, Define.UIEvent.Drag);
        gameObject.BindEvent(null, OnBeginDrag, Define.UIEvent.BeginDrag);
        gameObject.BindEvent(null, OnEndDrag, Define.UIEvent.EndDrag);

        gameObject.BindEvent(OnClickMaterialInfoButton);
        //GetButton(ButtonsType, (int)Buttons.MaterialInfoButton).gameObject.BindEvent(OnClickMaterialInfoButton);


        return true;
    }

    public void SetInfo(string _name, int _count)
    {
        transform.localScale = Vector3.one;
        GetImage(ImagesType, (int)Images.MaterialItemImage).sprite = Manager.ResourceM.Load<Sprite>(_name);
        GetImage(ImagesType, (int)Images.MaterialItemBackgroundImage).color = Define.EquipmentUIColors.MaterialGradeStyles[Define.MaterialGrade.Epic].BorderColor;
        GetText(TextsType, (int)Texts.ItemCountValueText).text = $"{_count}";
        GetObject(gameObjectsType, (int)GameObjects.GetEffectObject).SetActive(true);
    }

    public void SetInfo(Data.MaterialData _data, Transform _parent, int _count, ScrollRect _scrollRect = null)
    {
        transform.localScale = Vector3.one;
        materialData = _data;
        parent = _parent;
        scrollRect = _scrollRect;

        GetImage(ImagesType, (int)Images.MaterialItemImage).sprite = Manager.ResourceM.Load<Sprite>(materialData.SpriteName);
        GetText(TextsType, (int)Texts.ItemCountValueText).text = $"{_count}";

        var style = Define.EquipmentUIColors.MaterialGradeStyles[materialData.MaterialGrade];
        GetImage(ImagesType, (int)Images.MaterialItemBackgroundImage).color = style.BorderColor;
    }

    void OnClickMaterialInfoButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (isDrag) return;
        //TODO : Info 생성해서, 띄워줌 
        Debug.Log("가보자");
    }

    public void OnDrag(BaseEventData baseEventData)
    {
        if (scrollRect == null) return;
        isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        scrollRect.OnDrag(pointerEventData);
    }

    public void OnBeginDrag(BaseEventData baseEventData)
    {
        if (scrollRect == null) return;
        isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        scrollRect.OnBeginDrag(pointerEventData);
    }

    public void OnEndDrag(BaseEventData baseEventData)
    {
        if (scrollRect == null) return;
        isDrag = false;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        scrollRect.OnEndDrag(pointerEventData);
    }
}
