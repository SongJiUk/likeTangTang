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

    public void SetInfo(Data.CreatureData _data, RectTransform _targetPos, RectTransform _parentCanvas)
    {
        //TODO : 이미지, 이름, 설명
        GetImage(ImagesType, (int)Images.TargetImage).sprite = Manager.ResourceM.Load<Sprite>(_data.Image_Name);
        GetImage(ImagesType, (int)Images.BackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[Define.EquipmentGrade.Common].BgColor;
        GetText(TextsType, (int)Texts.TargetNameText).text = $"{_data.NameKR}";
        GetText(TextsType, (int)Texts.TargetDescriptionText).text = $"{_data.Description}";
        GetText(TextsType, (int)Texts.TargetDescriptionText).gameObject.SetActive(true);

        ToolTipPosSet(_targetPos, _parentCanvas);
    }

    void ToolTipPosSet(RectTransform _targetPos, RectTransform _parentCanvas)
    {
        gameObject.transform.position = _targetPos.transform.position;

        // 세로 높이 설정
        float sizeY = _targetPos.sizeDelta.y / 2;
        transform.localPosition += new Vector3(0f, sizeY);

        // 가로 높이 설정
        if (_targetPos.transform.localPosition.x > 0) // 오른쪽
        {
            float canvasMaxX = _parentCanvas.sizeDelta.x / 2;
            float targetPosMaxX = transform.localPosition.x + transform.GetComponent<RectTransform>().sizeDelta.x / 2;
            if (canvasMaxX < targetPosMaxX)
            {
                float deltaX = targetPosMaxX - canvasMaxX;
                transform.localPosition = -new Vector3(deltaX + 20, 0f) + transform.localPosition;
            }

        }
        else // 왼쪽
        {
            float canvasMinX = -_parentCanvas.sizeDelta.x / 2;
            float targetPosMinX = transform.localPosition.x - transform.GetComponent<RectTransform>().sizeDelta.x / 2;
            if (canvasMinX > targetPosMinX)
            {
                float deltaX = canvasMinX - targetPosMinX;
                transform.localPosition = new Vector3(deltaX + 20, 0f) + transform.localPosition;
            }

        }
    }

    void OnClickCloseButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.ResourceM.Destory(gameObject);
    }
}
