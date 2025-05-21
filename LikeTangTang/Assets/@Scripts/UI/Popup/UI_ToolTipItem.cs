using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ToolTipItem : UI_Base
{
    RectTransform rectTransform;
    enum Buttons
    {
        CloseButton
    }

    enum Texts
    {
        TargetNameText,
        TargetDescriptionText
    }

    enum Images
    {
        BackgroundImage,
        TargetImage
    }

    void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if(!base.Init()) return false;

        ButtonsType = typeof(Buttons);
        TextsType = typeof(Texts);
        ImagesType = typeof(Images);
        BindButton(ButtonsType);
        BindText(TextsType);
        BindImage(ImagesType);
        GetButton(ButtonsType, (int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);
        rectTransform = GetComponent<RectTransform>();
        Refresh();

        return true;
    }


    void Refresh()
    {

    }

    public void SetInfo(Data.MaterialData _data, Vector2 _rt)
    {
        transform.localScale = Vector3.one;
        //TODO : 이미지, 이름, 설명
        GetImage(ImagesType, (int)Images.TargetImage).sprite = Manager.ResourceM.Load<Sprite>(_data.SpriteName);
        GetImage(ImagesType, (int)Images.BackgroundImage).color = Define.EquipmentUIColors.MaterialGradeStyles[_data.MaterialGrade].BgColor;
        GetText(TextsType, (int)Texts.TargetNameText).text = $"{_data.NameTextID}";
        GetText(TextsType, (int)Texts.TargetDescriptionText).text =$"{_data.Description}";

        rectTransform.anchoredPosition = _rt;
    }

    void OnClickCloseButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.ResourceM.Destory(gameObject);
    }
}
