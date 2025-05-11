using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_MergePopup : UI_Popup
{   
    Equipment equipment;
    Equipment equipment_1;
    Equipment equipment_2;
    Define.EquipmentGrade equipmentGrade;
     Define.EquipmentSortType equipmentSortType;
    string sort_Level = "정렬 : 레벨";
    string sort_Grade = "정렬 : 등급";
    enum GameObjects
    {
        ContentObject,
        SelectedEquipObject,
        MurgeStartEffect,
        MurgeFinishEffect,
        OptionResultObject,
        SelectEquipmentCommentText,
        SelectMergeCommentText,
        FirstCostEquipNeedObject,
        FirstCostEquipSelectObject,

        SecondCostButton,
        SecondCostEquipNeedObject,
        SecondCostEquipSelectObject,
        MergeAllButtonRedDotObject,


    }

    enum Images
    {
        MergePossibleOutlineImage,
        SelectedEquipGradeBackgroundImage, //배경 이미지(색 변경)
        SelectedEquipImage, //장비 이미지	
        SelectedEquipTypeBackgroundImage, //장비 타입 배경 이미지(색 변경)
        SelectedEquipTypeImage, //장비 타입 이미지
        SelectedEquipEnforceBackgroundImage, //장비 강화 배경 이미지(색 변경)
        FirstCostEquipGradeBackgroundImage,
        FirstCostEquipImage,
        FirstCostEquipBackgroundImage,
        FirstCostEquipShadowImage,
        FirstSelectEquipGradeBackgroundImage,
        FirstSelectEquipImage,
        FirstSelectEquipEnforceBackgroundImage,
        FirstSelectEquipTypeBackgroundImage,
        FirstSelectEquipTypeImage,
        DecoImage,
        SecondCostEquipGradeBackgroundImage,
        SecondCostEquipImage,
        SecondCostEquipBackgroundImage,
        SecondCostEquipShadowImage,
        SecondSelectEquipImage,
        SecondSelectEquipEnforceBackgroundImage,
        SecondSelectEquipTypeBackgroundImage,
        SecondSelectEquipTypeImage,




    }

    enum Buttons
    {
        EquipResultButton,

        FirstCostButton,
        SecondCostButton,

        SortButton,
        MergeAllButton,
        MergeButton,
        BackButton,
    }

    enum Texts
    {
        SelectedEquipLevelValueText,
        SelectedEquipEnforceValueText,
        EquipmentNameText,
        BeforeGradeValueText,
        AfterGradeValueText,
        BeforeLevelValueText,
        AfterLevelValueText,
        BeforeATKValueText,
        AfterATKValueText,
        BeforeHPValueText,
        AfterHPValueText,
        FirstCostEquipEnforceValueText,
        FirstSelectEquipLevelValueText,
        FirstSelectEquipEnforceValueText,

        SecondCostEquipEnforceValueText,
        SecondSelectEquipLevelValueText,
        SecondSelectEquipEnforceValueText,
        SortButtonText


    }
    void OnEnable()
    {
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.ContentObject));
    }

    void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if(!base.Init()) return false;
        gameObjectsType = typeof(GameObjects);
        TextsType = typeof(Texts);
        ImagesType = typeof(Images);
        ButtonsType = typeof(Buttons);

        BindObject(gameObjectsType);
        BindImage(ImagesType);
        BindText(TextsType);
        BindButton(ButtonsType);
        
        //최상단 장비
        GetImage(ImagesType, (int)Images.MergePossibleOutlineImage).gameObject.SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.SelectedEquipObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.MurgeStartEffect).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.MurgeFinishEffect).SetActive(false);
        GetImage(ImagesType, (int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(false);

        //장비 설명 텍스트
        GetObject(gameObjectsType, (int)GameObjects.OptionResultObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.SelectEquipmentCommentText).SetActive(true);
        GetObject(gameObjectsType, (int)GameObjects.SelectMergeCommentText).SetActive(false);

        //합성에 필요한 장비
        GetObject(gameObjectsType, (int)GameObjects.FirstCostEquipNeedObject).SetActive(false);
        GetImage(ImagesType, (int)Images.FirstCostEquipBackgroundImage).gameObject.SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.FirstCostEquipSelectObject).SetActive(false);
        GetImage(ImagesType, (int)Images.FirstSelectEquipEnforceBackgroundImage).gameObject.SetActive(false);
        GetImage(ImagesType, (int)Images.FirstSelectEquipTypeBackgroundImage).gameObject.SetActive(false);

        GetObject(gameObjectsType, (int)GameObjects.SecondCostEquipNeedObject).SetActive(false);
        GetImage(ImagesType, (int)Images.SecondCostEquipBackgroundImage).gameObject.SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.SecondCostEquipSelectObject).SetActive(false);
        GetImage(ImagesType, (int)Images.SecondCostEquipBackgroundImage).gameObject.SetActive(false);
        GetImage(ImagesType, (int)Images.SecondSelectEquipEnforceBackgroundImage).gameObject.SetActive(false);
        GetImage(ImagesType, (int)Images.SecondSelectEquipTypeBackgroundImage).gameObject.SetActive(false);
        
        //내 장비 부분
        GetObject(gameObjectsType, (int)GameObjects.MergeAllButtonRedDotObject).SetActive(false);

        equipmentSortType = Define.EquipmentSortType.Level;
        GetText(TextsType, (int)Texts.SortButtonText).text = sort_Level;


        GetButton(ButtonsType, (int)Buttons.EquipResultButton).gameObject.BindEvent(OnClickEquipmentResultButton);
        GetButton(ButtonsType, (int)Buttons.FirstCostButton).gameObject.BindEvent(OnClickFirstCostButton);
        GetButton(ButtonsType, (int)Buttons.SecondCostButton).gameObject.BindEvent(OnClickSecondCostButton);
        GetButton(ButtonsType, (int)Buttons.SortButton).gameObject.BindEvent(OnclickSortButton);
        GetButton(ButtonsType, (int)Buttons.MergeAllButton).gameObject.BindEvent(OnClickMergeAllButtion);
        GetButton(ButtonsType, (int)Buttons.MergeButton).gameObject.BindEvent(OnClickMergeButton);
        GetButton(ButtonsType, (int)Buttons.MergeButton).gameObject.SetActive(false);
        GetButton(ButtonsType, (int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);

        Refresh();
        return true;
    }


    public void SetInfo(Equipment _equipment)
    {
        equipment = _equipment;
        equipment_1 = null;
        equipment_2 = null;
        equipmentGrade = equipment.EquipmentData.EquipmentGarde;
        Refresh();
    }


    void Refresh()
    {
        //장비가 없다면?
        SelectEquip();
    }


    void SelectEquip()
    {
        if(equipment == null)
        {
            GetObject(gameObjectsType, (int)GameObjects.SelectedEquipObject).SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.OptionResultObject).SetActive(false);
            GetButton(ButtonsType, (int)Buttons.MergeButton).gameObject.SetActive(false);
            return;
        }
        else
        {
            GetImage(ImagesType, (int)Images.SelectedEquipGradeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipmentGrade].BorderColor;
            GetImage(ImagesType, (int)Images.SelectedEquipImage).sprite = Resources.Load<Sprite>(equipment.EquipmentData.SpriteName);
            GetObject(gameObjectsType, (int)GameObjects.SelectedEquipObject).SetActive(true);
            //TODO : 강화가 가능할때만 빛나는거같음
            //GetImage(ImagesType, (int)Images.MergePossibleOutlineImage).gameObject.SetActive(true);
            //GetImage(ImagesType, (int)Images.MergePossibleOutlineImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipmentGrade].BgColor;
            GetImage(ImagesType, (int)Images.SelectedEquipTypeImage).sprite = Resources.Load<Sprite>($"{equipment.EquipmentData.EquipmentType}_Icon.sprite");
            GetImage(ImagesType, (int)Images.SelectedEquipTypeBackgroundImage).gameObject.SetActive(true);
            GetImage(ImagesType, (int)Images.SelectedEquipTypeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipmentGrade].BgColor;
            GetText(TextsType, (int)Texts.SelectedEquipLevelValueText).text = $"LV. {equipment.Level}";


            int grade = Utils.GetUpgradeNumber(equipmentGrade);
            if (grade == 0)
            {
                GetText(TextsType, (int)Texts.SelectedEquipEnforceValueText).text = "";
                GetImage(ImagesType, (int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(false);
            }
            else
            {
                GetText(TextsType, (int)Texts.SelectedEquipEnforceValueText).text = grade.ToString();
                GetImage(ImagesType, (int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
                GetImage(ImagesType, (int)Images.SelectedEquipEnforceBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipmentGrade].BgColor;
            }

            GetObject(gameObjectsType, (int)GameObjects.SelectEquipmentCommentText).SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.SelectMergeCommentText).SetActive(false);


            //TODO : 장비 강화에 필요한 법칙들
            
        }
    }


    void OnClickEquipmentResultButton()
    {
        equipment = null;
        Refresh();
    }
    void OnClickFirstCostButton()
    {

    }

    void OnClickSecondCostButton()
    {

    }

    void OnclickSortButton()
    {

    }

    void OnClickMergeAllButtion()
    {

    }

    void OnClickMergeButton()
    {

    }

    void OnClickBackButton()
    {

    }

}
