using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{
    public string key = "";
    public Data.EquipmentData EquipmentData;

    public int Level { get; set; } = 1;
    public float AttackBonus { get; set; } = 0;
    public float MaxHpBonus { get; set; } = 0;

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


    //public Equipment(string _key)
    //{
    //    key = _key;

    //    if (string.IsNullOrEmpty(key))
    //    {
    //        Debug.Log("키가 비어있음(아마 장착한 장비가 없어서 그럴 확률이 높음");
    //        return;
    //    }

    //    EquipmentData = Manager.DataM.EquipmentDic[key];
    //    SetInfo(Level);
    //    IsOwned = true;
    //}

    //NOTE : 생성자 안쓰는 Json에서 값을 가져올경우 GameData에 자동으로 매핑이 되는데,
    //거기서 Equipment안에 들어가는 값들을 자동으로 매핑해줌, 근데 생성자가 정의되어있으면
    //그 생성자 안으로 들어가서 안되는거임.
    public void Init(string _key = null)
    {
        if (_key != null) key = _key;

        if(string.IsNullOrEmpty(key) || !Manager.DataM.EquipmentDic.ContainsKey(key))
        {
            Debug.LogWarning($"[Equipment.Init] 잘못된 키: {key}");
            return;
        }

        EquipmentData = Manager.DataM.EquipmentDic[key];
        SetInfo(Level);
        IsOwned = true;
    }

    public void SetInfo(int _level)
    {
        Level = _level;
        AttackBonus = EquipmentData.Grade_Attack + (Level - 1) * EquipmentData.GradeUp_Attack;
        MaxHpBonus = EquipmentData.Grade_Hp + (Level - 1) * EquipmentData.GradeUp_Hp;
    }

    public void LevelUp()
    {
        Level++;
        EquipmentData = Manager.DataM.EquipmentDic[key];
        AttackBonus = EquipmentData.Grade_Attack + (Level - 1) * EquipmentData.GradeUp_Attack;
        MaxHpBonus = EquipmentData.Grade_Hp + (Level - 1) * EquipmentData.GradeUp_Hp;

    }
}
