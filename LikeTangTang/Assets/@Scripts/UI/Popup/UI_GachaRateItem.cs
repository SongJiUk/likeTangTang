using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GachaRateItem : UI_Base
{
    enum Images
    {
        BackgroundImage
    }

    enum Texts
    {
        EquipmentNameValueText,
        EquipmentReteValueText
    }

    Data.GachaRateData gachaData;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;

        TextsType = typeof(Texts);
        ImagesType = typeof(Images);
        BindText(TextsType);
        BindImage(ImagesType);

        return true;
    }

    public void SetInfo(Data.GachaRateData _data)
    {
        gachaData = _data;

        transform.localScale = Vector3.one;
        Refresh();
    }


    void Refresh()
    {
        string itemName = Manager.DataM.EquipmentDic[gachaData.EquipmentID].NameTextID;
        GetText(TextsType, (int)Texts.EquipmentNameValueText).text = itemName;
        GetText(TextsType, (int)Texts.EquipmentReteValueText).text = gachaData.GachaRate.ToString("P2");
        GetImage(ImagesType, (int)Images.BackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[gachaData.EquipGrade].BgColor;

    }
}
