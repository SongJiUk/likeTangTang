using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{
    public int key;
    public Data.EquipmentData EquipmentData;

    public int Level { get; set; } = 1;
    public int AttackBonus { get; set; } = 0;
    public int MaxHpBonus { get; set; } = 0;

    bool isEquiped = false;
    public bool IsEquiped
    {
        get { return isEquiped; }
        set { isEquiped = value; }
    }

    public bool IsOwned { get; set; } = false; //가졌는지

    public bool IsUpgradeable { get; set; } = false; //장비 업그레이드 가능한지

    public bool IsConfirmed { get; set; } = false; //장비 획득 확인
    public bool IsEquipmentSynthesizable { get; set; } = false; //합성 가능한지 
    public bool IsSelect { get; set; } = false; //선택 가능한지
    public bool IsUnvailable { get; set; } = false; //합성에서 선택 불가능한지


    public Equipment(int _key)
    {
        key = _key;

        if (key == 0 || !Manager.DataM.EquipmentDic.ContainsKey(key))
        {
            Debug.Log($"Equipment 생성자 잘못된 키 입력 : {key} - Equipment 할당 안됨(아마, 장착된장비가 없어서 뜨는것");
            return;
        }

        EquipmentData = Manager.DataM.EquipmentDic[key];
    }

}
