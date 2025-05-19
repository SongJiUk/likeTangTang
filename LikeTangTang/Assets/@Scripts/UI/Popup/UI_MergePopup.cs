using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class UI_MergePopup : UI_Popup
{   
    Equipment equipment;
    Equipment equipment_1;
    Equipment equipment_2;
    List<UI_MergeEquipItem> equipItemPool = new();
    Define.EquipmentGrade equipmentGrade;
    Define.EquipmentSortType equipmentSortType;
    string sort_Level = "정렬 : 레벨";
    string sort_Grade = "정렬 : 등급";
    public ScrollRect scrollRect;
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

        ImprovATKObject,
        ImprovHPObject,
        EquipInventoryScrollContentObject
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
        FirstSelectEquipGradeBackgroundImage,
        FirstSelectEquipImage,
        FirstSelectEquipEnforceBackgroundImage,
        FirstSelectEquipTypeBackgroundImage,
        FirstSelectEquipTypeImage,
        DecoImage,
        SecondCostEquipGradeBackgroundImage,
        SecondCostEquipImage,
        SecondCostEquipBackgroundImage,
        SecondSelectEquipGradeBackgroundImage,
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

        //Refresh();
        return true;
    }


    public void SetInfo(Equipment _equipment)
    {
        equipment = _equipment;
        equipment_1 = null;
        equipment_2 = null;

        if(_equipment != null) equipmentGrade = equipment.EquipmentData.EquipmentGarde;
        Refresh();
    }

    public void SetMergeItem(Equipment _equipment, bool _ShowUI = true)
    {
        if (_equipment.IsEquiped) return;
        if (_equipment.Level > 1) return;
        if (_equipment.EquipmentData.MergeEquipmentType_1 == Define.MergeEquipmentType.None) return;
        if(equipment == null)
        {
            equipment = _equipment;
            if(_ShowUI)
            {
                Refresh_SelectEquip();
                SortEquipments();
            }
            return;
        }

        if (equipment == _equipment) return;
        if (equipment.EquipmentData.EquipmentType != _equipment.EquipmentData.EquipmentType) return;
        if (_equipment.Equals(equipment_1)) return;
        if (_equipment.Equals(equipment_2)) return;

        if (equipment_1 == null)
        {
            if (equipment.EquipmentData.MergeEquipmentType_1 == Define.MergeEquipmentType.ItemCode)
            {
                if (_equipment.EquipmentData.DataID != equipment.EquipmentData.MergeEquipment_1) return;
            }
            else if (equipment.EquipmentData.MergeEquipmentType_1 == Define.MergeEquipmentType.Grade)
            {
                if (_equipment.EquipmentData.EquipmentGarde != Define.GetEquipmnetGrade((Define.EquipmentGrade)Enum.Parse(typeof(Define.EquipmentGrade), equipment.EquipmentData.MergeEquipment_1)))
                    return;
            }
            else return;

            equipment_1 = _equipment;
            if (_ShowUI) Refresh_MergeEquip_1();
        }
        else if (equipment_2 == null)
        {
            if (equipment.EquipmentData.MergeEquipmentType_2 == Define.MergeEquipmentType.ItemCode)
            {
                if (_equipment.EquipmentData.DataID != equipment.EquipmentData.MergeEquipment_2) return;
            }
            else if (equipment.EquipmentData.MergeEquipmentType_2 == Define.MergeEquipmentType.Grade)
            {
                if (_equipment.EquipmentData.EquipmentGarde != (Define.EquipmentGrade)Enum.Parse(typeof(Define.EquipmentGrade), equipment.EquipmentData.MergeEquipment_2))
                    return;
            }
            else return;

            equipment_2 = _equipment;
            if (_ShowUI) Refresh_MergeEquip_2();


        }
        else return;

        if (_ShowUI)
            CheckEnableMerge();

        SortEquipments();
    }


    void Refresh()
    {

        //장비가 없다면?
        GetImage(ImagesType, (int)Images.MergePossibleOutlineImage).gameObject.SetActive(false);
        GetButton(ButtonsType, (int)Buttons.SecondCostButton).gameObject.SetActive(true);

        Refresh_SelectEquip();
        Refresh_MergeEquip_1();
        Refresh_MergeEquip_2();
        CheckEnableMerge();
        SortEquipments();
    }


    void Refresh_SelectEquip()
    {
        ///////
        /////// 들어오는 equipment에 맞게 밑에 equipment들 다 비활성화 시켜야됌.
        //////
        if(equipment == null)
        {
            GetObject(gameObjectsType, (int)GameObjects.SelectedEquipObject).SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.OptionResultObject).SetActive(false);
            GetButton(ButtonsType, (int)Buttons.MergeButton).gameObject.SetActive(false);
            return;
        }
        else
        {
            equipmentGrade = equipment.EquipmentData.EquipmentGarde;
            GetImage(ImagesType, (int)Images.SelectedEquipGradeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipmentGrade].BorderColor;
            GetImage(ImagesType, (int)Images.SelectedEquipImage).sprite = Manager.ResourceM.Load<Sprite>(equipment.EquipmentData.SpriteName);
            GetObject(gameObjectsType, (int)GameObjects.SelectedEquipObject).SetActive(true);
            //TODO : 강화가 가능할때만 빛남
            //GetImage(ImagesType, (int)Images.MergePossibleOutlineImage).gameObject.SetActive(true);
            //GetImage(ImagesType, (int)Images.MergePossibleOutlineImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipmentGrade].BgColor;
            GetImage(ImagesType, (int)Images.SelectedEquipTypeImage).sprite = Manager.ResourceM.Load<Sprite>($"{equipment.EquipmentData.EquipmentType}_Icon.sprite");
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
            GetObject(gameObjectsType, (int)GameObjects.SelectMergeCommentText).SetActive(true);


            //TODO : 장비 강화에 필요한 법칙들
            /*
             MergeEquipmentType_1이 Grade면 Equipment2는 필요없음
             */

            if(equipment.EquipmentData.MergeEquipmentType_1 == Define.MergeEquipmentType.None)
            {
                GetButton(ButtonsType, (int)Buttons.FirstCostButton).gameObject.SetActive(false);
                GetButton(ButtonsType, (int)Buttons.SecondCostButton).gameObject.SetActive(false);
            }
            else if(equipment.EquipmentData.MergeEquipmentType_1 == Define.MergeEquipmentType.Grade)
            {
                GetButton(ButtonsType, (int)Buttons.FirstCostButton).gameObject.SetActive(true);
                GetButton(ButtonsType, (int)Buttons.SecondCostButton).gameObject.SetActive(false);
            }
            else
            {
                GetButton(ButtonsType, (int)Buttons.FirstCostButton).gameObject.SetActive(true);
                GetButton(ButtonsType, (int)Buttons.SecondCostButton).gameObject.SetActive(true);
            }
        }
    }

    void Refresh_MergeEquip_1()
    {
        if (equipment_1 == null)
        {
            GetObject(gameObjectsType, (int)GameObjects.FirstCostEquipSelectObject).SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.OptionResultObject).SetActive(false);
        }
        else
        {
            GetImage(ImagesType, (int)Images.FirstSelectEquipImage).sprite = Manager.ResourceM.Load<Sprite>(equipment_1.EquipmentData.SpriteName);
            GetImage(ImagesType, (int)Images.FirstSelectEquipTypeImage).sprite = Manager.ResourceM.Load<Sprite>($"{equipment_1.EquipmentData.EquipmentType}_Icon.sprite");
            GetImage(ImagesType, (int)Images.FirstSelectEquipTypeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipment_1.EquipmentData.EquipmentGarde].BgColor;
            GetImage(ImagesType, (int)Images.FirstSelectEquipGradeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipment_1.EquipmentData.EquipmentGarde].BorderColor;
            GetImage(ImagesType, (int)Images.FirstSelectEquipTypeBackgroundImage).gameObject.SetActive(true);

            int grade = Utils.GetUpgradeNumber(equipment_1.EquipmentData.EquipmentGarde);
            if (grade == 0)
            {
                GetText(TextsType, (int)Texts.FirstSelectEquipEnforceValueText).text = "";
                GetImage(ImagesType, (int)Images.FirstSelectEquipEnforceBackgroundImage).gameObject.SetActive(false);
            }
            else
            {
                GetText(TextsType, (int)Texts.FirstSelectEquipEnforceValueText).text = grade.ToString();
                GetImage(ImagesType, (int)Images.FirstSelectEquipEnforceBackgroundImage).gameObject.SetActive(true);
                GetImage(ImagesType, (int)Images.FirstSelectEquipEnforceBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipment_1.EquipmentData.EquipmentGarde].BgColor;
                
            }


            GetText(TextsType, (int)Texts.FirstSelectEquipLevelValueText).text = $"Lv. {equipment_1.Level}";
            GetObject(gameObjectsType, (int)GameObjects.FirstCostEquipSelectObject).SetActive(true);
        }

    }

    void Refresh_MergeEquip_2()
    {
        if (equipment_2 == null)
        {
            GetObject(gameObjectsType, (int)GameObjects.SecondCostEquipSelectObject).SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.OptionResultObject).SetActive(false);
        }
        else
        {
            GetImage(ImagesType, (int)Images.SecondSelectEquipImage).sprite = Manager.ResourceM.Load<Sprite>(equipment_2.EquipmentData.SpriteName);
            GetImage(ImagesType, (int)Images.SecondSelectEquipTypeImage).sprite = Manager.ResourceM.Load<Sprite>($"{equipment_2.EquipmentData.EquipmentType}_Icon.sprite");
            GetImage(ImagesType, (int)Images.SecondSelectEquipTypeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipment_2.EquipmentData.EquipmentGarde].BgColor;
            GetImage(ImagesType, (int)Images.SecondSelectEquipGradeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipment_2.EquipmentData.EquipmentGarde].BorderColor;
            GetImage(ImagesType, (int)Images.SecondSelectEquipTypeBackgroundImage).gameObject.SetActive(true);
            int grade = Utils.GetUpgradeNumber(equipment_2.EquipmentData.EquipmentGarde);
            if (grade == 0)
            {
                GetText(TextsType, (int)Texts.SecondSelectEquipEnforceValueText).text = "";
                GetImage(ImagesType, (int)Images.SecondSelectEquipTypeBackgroundImage).gameObject.SetActive(false);
            }
            else
            {
                GetText(TextsType, (int)Texts.SecondSelectEquipEnforceValueText).text = grade.ToString();
                GetImage(ImagesType, (int)Images.SecondSelectEquipTypeBackgroundImage).gameObject.SetActive(true);
                GetImage(ImagesType, (int)Images.SecondSelectEquipTypeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipment_2.EquipmentData.EquipmentGarde].BgColor;
            }


            GetText(TextsType, (int)Texts.SecondSelectEquipLevelValueText).text = $"Lv. {equipment_2.Level}";
            GetObject(gameObjectsType, (int)GameObjects.SecondCostEquipSelectObject).SetActive(true);
        }
    }

    bool CheckEnableMerge()
    {
        if(equipment == null)
        {
            GetButton(ButtonsType, (int)Buttons.MergeButton).gameObject.SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.MurgeStartEffect).SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.MurgeFinishEffect).SetActive(false);
            return false;
        }

        if(equipment_2 == null && GetButton(ButtonsType, (int)Buttons.SecondCostButton).gameObject.activeSelf)
        {
            GetButton(ButtonsType, (int)Buttons.MergeButton).gameObject.SetActive(false);
            GetImage(ImagesType, (int)Images.MergePossibleOutlineImage).gameObject.SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.MurgeStartEffect).SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.MurgeFinishEffect).SetActive(false);
            return false;
        }

        if(equipment_1 == null)
        {
            GetButton(ButtonsType, (int)Buttons.MergeButton).gameObject.SetActive(false);
            GetImage(ImagesType, (int)Images.MergePossibleOutlineImage).gameObject.SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.MurgeStartEffect).SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.MurgeFinishEffect).SetActive(false);
            return false;
        }

        GetObject(gameObjectsType, (int)GameObjects.SelectMergeCommentText).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.SelectEquipmentCommentText).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.MurgeStartEffect).SetActive(true);
        GetObject(gameObjectsType, (int)GameObjects.MurgeFinishEffect).SetActive(true);


        //업데이트될 아이템 세팅
        string mergeItemData = equipment.EquipmentData.MergeItemCode;
        equipmentGrade = Manager.DataM.EquipmentDic[mergeItemData].EquipmentGarde;

        GetImage(ImagesType, (int)Images.MergePossibleOutlineImage).gameObject.SetActive(true);
        GetObject(gameObjectsType, (int)GameObjects.OptionResultObject).SetActive(true);


        //장비별 텍스트
        GetText(TextsType, (int)Texts.EquipmentNameText).text = equipment.EquipmentData.NameTextID;
        GetText(TextsType, (int)Texts.BeforeLevelValueText).text = $"{Manager.DataM.EquipmentDic[equipment.EquipmentData.DataID].Grade_MaxLevel}";
        GetText(TextsType, (int)Texts.AfterLevelValueText).text = $"{Manager.DataM.EquipmentDic[mergeItemData].Grade_MaxLevel}";
      
        if(Manager.DataM.EquipmentDic[mergeItemData].Grade_Attack != 0)
        {
            GetObject(gameObjectsType, (int)GameObjects.ImprovATKObject).SetActive(true);
            GetObject(gameObjectsType, (int)GameObjects.ImprovHPObject).SetActive(false);

            GetText(TextsType, (int)Texts.BeforeATKValueText).text = $"{Manager.DataM.EquipmentDic[equipment.EquipmentData.DataID].Grade_Attack}";
            GetText(TextsType, (int)Texts.AfterATKValueText).text = $"{Manager.DataM.EquipmentDic[mergeItemData].Grade_Attack}";
        }
        else
        {
            GetObject(gameObjectsType, (int)GameObjects.ImprovATKObject).SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.ImprovHPObject).SetActive(true);

            GetText(TextsType, (int)Texts.BeforeHPValueText).text = $"{Manager.DataM.EquipmentDic[equipment.EquipmentData.DataID].Grade_Hp}";
            GetText(TextsType, (int)Texts.AfterHPValueText).text = $"{Manager.DataM.EquipmentDic[mergeItemData].Grade_Hp}";
        }


        GetImage(ImagesType, (int)Images.MergePossibleOutlineImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipmentGrade].BorderColor;
        GetImage(ImagesType, (int)Images.SelectedEquipGradeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipmentGrade].BgColor;
        GetImage(ImagesType, (int)Images.SelectedEquipTypeBackgroundImage).color = Define.EquipmentUIColors.EquipGradeStyles[equipmentGrade].BorderColor;

        GetText(TextsType, (int)Texts.BeforeGradeValueText).text = $"{equipmentGrade}";
        GetText(TextsType, (int)Texts.AfterGradeValueText).text = $"{Manager.DataM.EquipmentDic[mergeItemData].EquipmentGarde}";


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


        GetButton(ButtonsType, (int)Buttons.MergeButton).gameObject.SetActive(true);
        return true;
    }


    void SortEquipments()
    {
        if(equipment != null) 
            Manager.GameM.SortEquipment(equipmentSortType, equipment);
        else 
            Manager.GameM.SortEquipment(equipmentSortType);
        var parent = GetObject(gameObjectsType, (int)GameObjects.EquipInventoryScrollContentObject).transform;
        bool isSelected = false;
        int index = 0;

        foreach (Equipment equipmentitem in Manager.GameM.OwnedEquipment)
        {
            //TODO : 이거, 선택한 아이템 위로 올라가기하기 vs 체크표시하기. 뭐할지 고민해보기.
            isSelected = IsSelectedItem(equipmentitem);


            UI_MergeEquipItem item;
            if (index < equipItemPool.Count)
            {
                item = equipItemPool[index];
                item.gameObject.SetActive(true);
            }
            else
            {
                item = Manager.UiM.MakeSubItem<UI_MergeEquipItem>(parent);
                equipItemPool.Add(item);
            }


            item.SetInfo(equipmentitem, Define.UI_ItemParentType.EquipInventory, isSelected, IsLocked(equipmentitem), scrollRect);

            index++;
        }

        for (int i = index; i < equipItemPool.Count; i++)
            equipItemPool[i].gameObject.SetActive(false);
    }
   
    bool IsSelectedItem(Equipment _equipment)
    {
        return _equipment == equipment || _equipment == equipment_1 || _equipment == equipment_2;
    }

    bool IsLocked(Equipment _equipment)
    {
        
        if (_equipment == null) return false;
        if (_equipment.Level > 1 || _equipment.IsEquiped) return true;
        //if (equipment_1 != null && equipment_2 == null) return true;
        /*
         만약, 현재 equipment에 들어가있는 아이템의 MergeEquipmnetType_1이 Item or Grade인데 물론 None일수도 있겠지만

        Grade라면, cost는 하나밖에 필요없음(애초에 두개가 있지도 않을거임)
         */
        if(equipment != null)
        {
            if (equipment.EquipmentData.EquipmentType != _equipment.EquipmentData.EquipmentType) return true;

            Define.MergeEquipmentType mergeEquipType = equipment.EquipmentData.MergeEquipmentType_1;
            
            if (mergeEquipType == Define.MergeEquipmentType.None) 
                return true;
            else if (mergeEquipType == Define.MergeEquipmentType.Grade)
            {
                if (equipment_1 != null) 
                    return true;

                if (!MatchMergeCondition(mergeEquipType, equipment.EquipmentData.MergeEquipment_1, _equipment))
                    return true;
            }
            else //Item Code
            {
                if (equipment_1 != null && equipment_2 != null) 
                    return true;

                if (!MatchMergeCondition(mergeEquipType, equipment.EquipmentData.MergeEquipment_2, _equipment))
                    return true;
            }

        }

        return false;
    }

    bool MatchMergeCondition(Define.MergeEquipmentType _type, string _target, Equipment _equipment)
    {
        if (_type == Define.MergeEquipmentType.ItemCode)
            return _equipment.EquipmentData.DataID == _target;
        else if(_type == Define.MergeEquipmentType.Grade)
            return _equipment.EquipmentData.EquipmentGarde == (Define.EquipmentGrade)Enum.Parse(typeof(Define.EquipmentGrade), _target);

        return false;
    }

    void OnClickEquipmentResultButton()
    {
        Manager.SoundM.PlayButtonClick();
        equipment = null;
        equipment_1 = null;
        equipment_2 = null;
        Refresh();
    }
    void OnClickFirstCostButton()
    {
        Manager.SoundM.PlayButtonClick();
        equipment_1 = null;
        Refresh();
    }

    void OnClickSecondCostButton()
    {
        Manager.SoundM.PlayButtonClick();
        equipment_2 = null;
        Refresh();
    }

    void OnclickSortButton()
    {
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

    void OnClickMergeAllButtion()
    {
        Manager.SoundM.PlayButtonClick();
        StartCoroutine(CoMergeAll());
    }

    IEnumerator CoMergeAll()
    {
        Manager.GameM.SortEquipment(Define.EquipmentSortType.Grade);

        List<Equipment> candidateList = Manager.GameM.OwnedEquipment
            .Where(e => e != null && !e.IsEquiped && e.EquipmentData.EquipmentGarde <= Define.EquipmentGrade.Epic).ToList();

        List<Equipment> newEquipments = new List<Equipment>();

        int i = 0;

        while (i < candidateList.Count)
        {
            Equipment baseEquip = candidateList[i];
            bool isMerged = false;

            equipment = baseEquip;

            Equipment mergeEquip_1 = candidateList.
                FirstOrDefault(e => e != baseEquip && MatchMergeCondition(baseEquip.EquipmentData.MergeEquipmentType_1,
                baseEquip.EquipmentData.MergeEquipment_1, e));

            Equipment mergeEquip_2 = candidateList.
                FirstOrDefault(e => e != baseEquip && MatchMergeCondition(baseEquip.EquipmentData.MergeEquipmentType_2,
                baseEquip.EquipmentData.MergeEquipment_2, e));


            if(mergeEquip_1 != null && mergeEquip_2 != null)
            {
                equipment_1 = mergeEquip_1;
                equipment_2 = mergeEquip_2;

                Equipment newItem = Manager.GameM.MergeEquipment(equipment, mergeEquip_1, mergeEquip_2, true);
                if(newItem != null)
                {
                    newEquipments.Add(newItem);
                    candidateList.Remove(baseEquip);
                    candidateList.Remove(mergeEquip_1);
                    candidateList.Remove(mergeEquip_2);
                }
            }

            if (!isMerged) i++;
            if (i % 5 == 0)
                yield return new WaitForEndOfFrame();
        }

        equipment = null;
        equipment_1 = null;
        equipment_2 = null;

        SortEquipments();

        if (newEquipments.Count > 0)
        {
            UI_MergeAllResultPopup mergeAllPopup = (Manager.UiM.SceneUI as UI_LobbyScene).Ui_MergeAllResultPopup;
            mergeAllPopup.SetInfo(newEquipments);
            mergeAllPopup.gameObject.SetActive(true);
        }
        else
            Manager.UiM.ShowToast("합성할 수 있는 장비가 없습니다.");
            

        Manager.GameM.SaveGame();
    }

    void OnClickMergeButton()
    {
        Manager.SoundM.PlayButtonClick();
        Equipment beforeEquipment = equipment;
        Equipment newItem = Manager.GameM.MergeEquipment(equipment, equipment_1, equipment_2);

        UI_MergeResultPopup resultPopup = (Manager.UiM.SceneUI as UI_LobbyScene).Ui_MergeResultPopup;
        resultPopup.SetInfo(beforeEquipment, newItem, OnClosedMergeResultPopup);
        resultPopup.gameObject.SetActive(true);

        SortEquipments();
    }

    void OnClickBackButton()
    {
        this.gameObject.SetActive(false);
        (Manager.UiM.SceneUI as UI_LobbyScene).Ui_EquipmentPopup.SetInfo();
    }

    void OnClosedMergeResultPopup()
    {
        OnClickEquipmentResultButton();
    }
}
