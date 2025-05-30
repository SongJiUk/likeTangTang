using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Character
{
    public Data.CreatureData Data;
    public Data.CharacterLevelData CharacterLevelData;
    public int DataId { get; set; } = 1;
    public int Level { get; set; } = 1;
    public float MaxHp { get; set; } = 1;
    public float MaxHpRate { get; set; } = 1;
    public float Attack { get; set; } = 1;

    public float AttackRate { get; set; } = 1;
    public float Def { get; set; } = 1;
    public float DefRate { get; set; } = 1;
    public int TotalExp { get; set; } = 1;
    public float MoveSpeed { get; set; } = 1;
    public float SpeedRate { get; set; } = 1;
    public float CriticalRate { get; set; } = 0;
    public float CriticalDamage { get; set; } = 0;
    public bool isCurrentCharacter = false;

    public void SetInfo(int _key)
    {
        DataId = _key;
        Data = Manager.DataM.CreatureDic[DataId];
        CharacterLevelData = Manager.DataM.CharacterLevelDataDic[Level];
        MaxHp = Data.MaxHp + CharacterLevelData.HpUp;
        MaxHpRate = Data.HpRate;
        Attack = Data.Attack + CharacterLevelData.AttackUp;
        AttackRate = Data.AttackRate;
        Def = Data.Def + CharacterLevelData.DefUp;
        DefRate = Data.AttackRate;
        MoveSpeed = Data.Speed + CharacterLevelData.SpeedUp;
        SpeedRate = Data.MoveSpeedRate;
        CriticalRate = CharacterLevelData.CriticalUp;
        CriticalDamage = CharacterLevelData.CriticalDamageUp;

    }

    public void LevelUp()
    {
        Level++;
        Data = Manager.DataM.CreatureDic[DataId];
        CharacterLevelData = Manager.DataM.CharacterLevelDataDic[Level];
        MaxHp = Data.MaxHp + CharacterLevelData.HpUp;
        MaxHpRate = Data.HpRate;
        Attack = Data.Attack + CharacterLevelData.AttackUp;
        AttackRate = Data.AttackRate;
        Def = Data.Def + CharacterLevelData.DefUp;
        DefRate = Data.AttackRate;
        MoveSpeed = Data.Speed + CharacterLevelData.SpeedUp;
        SpeedRate = Data.MoveSpeedRate;
        CriticalRate = CharacterLevelData.CriticalUp;
        CriticalDamage = CharacterLevelData.CriticalDamageUp;
    }

    public void ChangeCharacter(int _id)
    {
        //TODO : isCurrentCharacter = false 해줘야됌 (기존 캐릭터)
        DataId = _id;
        Data = Manager.DataM.CreatureDic[DataId];
        CharacterLevelData = Manager.DataM.CharacterLevelDataDic[Level];
        MaxHp = Data.MaxHp + CharacterLevelData.HpUp;
        MaxHpRate = Data.HpRate;
        Attack = Data.Attack + CharacterLevelData.AttackUp;
        AttackRate = Data.AttackRate;
        Def = Data.Def + CharacterLevelData.DefUp;
        DefRate = Data.AttackRate;
        MoveSpeed = Data.Speed + CharacterLevelData.SpeedUp;
        SpeedRate = Data.MoveSpeedRate;
        CriticalRate = CharacterLevelData.CriticalUp;
        CriticalDamage = CharacterLevelData.CriticalDamageUp;
    }
}
