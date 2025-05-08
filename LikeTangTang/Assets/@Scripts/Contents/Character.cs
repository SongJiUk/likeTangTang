using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public Data.CreatureData Data;
    public int DataId { get; set; } = 1;
    public int Level { get; set; } = 1;
    public float MaxHp { get; set; } = 1;
    public float Attack { get; set; } = 1;
    public float Def { get; set; } = 1;
    public int TotalExp { get; set; } = 1;
    public float MoveSpeed { get; set; } = 1;
    public bool isCurrentCharacter = false;

    public void SetInfo(int _key)
    {
        DataId = _key;
        Data = Manager.DataM.CreatureDic[DataId];
        MaxHp = Data.MaxHp + (Level * Data.MaxHpUpForIncreasStage) * Data.HpRate;
        Attack= Data.Attack + (Level * Data.AttackUpForIncreasStage) * Data.AttackRate;
        Def = Data.Def * Data.DefRate;
        MoveSpeed = Data.Speed * Data.MoveSpeedRate;
    }

    public void LevelUp()
    {
        Level++;
        Data = Manager.DataM.CreatureDic[DataId];
        MaxHp = Data.MaxHp + (Level * Data.MaxHpUpForIncreasStage) * Data.HpRate;
        Attack = Data.Attack + (Level * Data.AttackUpForIncreasStage) * Data.AttackRate;
        Def = Data.Def * Data.DefRate;
        MoveSpeed = Data.Speed * Data.MoveSpeedRate;
    }
}
