using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{
    public string key;
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


    public Equipment(string _key)
    {
        key = _key;

        EquipmentData = Manager.DataM.EquipmentDic[key];
    }

}
