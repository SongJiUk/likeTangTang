using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MaterialItem : UI_Base
{

    enum GameObjects
    {
        GetEffectObject
    }

    enum Buttons
    {
        MaterialInfoButton
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
        ImagesType = typeof(Images);

        BindObject(gameObjectsType);
        BindButton(ButtonsType);
        BindText(TextsType);
        BindImage(ImagesType);

        GetObject(gameObjectsType, (int)GameObjects.GetEffectObject).SetActive(false);
        gameObject.BindEvent(OnClickMaterialInfoButton);
        GetButton(ButtonsType, (int)Buttons.MaterialInfoButton).gameObject.BindEvent(OnClickMaterialInfoButton);


        return true;
    }

    public void SetInfo(string _name, int _count)
    {
        transform.localScale = Vector3.one;
        GetImage(ImagesType, (int)Images.MaterialItemImage).sprite = Manager.ResourceM.Load<Sprite>(name);
        GetImage(ImagesType, (int)Images.MaterialItemBackgroundImage).color = Define.EquipmentUIColors.MaterialGradeStyles[Define.MaterialGrade.Epic].BorderColor;
        GetText(TextsType, (int)Texts.ItemCountValueText).text = $"{_count}";
        GetObject(gameObjectsType, (int)GameObjects.GetEffectObject).SetActive(true);
    }


    public void SetInfo(Data.MaterialData _data, Transform _parent, int _count)
    {
        transform.localScale = Vector3.one;
        materialData = _data;
        parent = _parent;

        GetImage(ImagesType, (int)Images.MaterialItemImage).sprite = Manager.ResourceM.Load<Sprite>(materialData.SpriteName);
        GetText(TextsType, (int)Texts.ItemCountValueText).text = $"{_count}";

        var style = Define.EquipmentUIColors.MaterialGradeStyles[materialData.MaterialGrade];
        GetImage(ImagesType, (int)Images.MaterialItemBackgroundImage).color = style.BorderColor;
    }

    void OnClickMaterialInfoButton()
    {
        Manager.SoundM.PlayButtonClick();
        //TODO : Info 생성해서, 띄워줌 
    }
}
