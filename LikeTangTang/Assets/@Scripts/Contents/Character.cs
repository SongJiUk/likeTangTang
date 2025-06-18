using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Character
{
    public Data.CreatureData Data;
    public Data.CharacterLevelData CharacterLevelData;
    public Dictionary<int, Data.EvolutionData> evolutionData = new();
    public Dictionary<int, bool> isLearnEvloution = new();

    public int DataId { get; set; } = 1;
    public int Level { get; set; } = 1;
    public int UseCoupon { get; set; }
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
    public float CoolTimeBouns { get; set; } = 0;
    public float DiaBouns { get; set; } = 0;
    public float GoldBonus { get; set; } = 0;
    public float HealingBouns { get; set; } = 0;

    public float Evol_AttackRate  { get; set; } = 0;
    public float Evol_MaxHpRate { get; set; } = 0;
    public float Evol_CoolTimeBouns{ get; set; } = 0;
    public float Evol_DiaBouns { get; set; } = 1;
    public float Evol_GoldBonus { get; set; } = 1;
    public float Evol_DefRate { get; set; } = 0;
    public float Evol_HealingBouns{ get; set; } = 0;
    public float Evol_CriticalRate { get; set; } = 0;
    public float Evol_CriticalDamage{ get; set; } = 0;
    public float Evol_SpeedRate { get; set; } = 0;


    public bool isCurrentCharacter = false;

    public void Init(int _key)
    {
        DataId = _key;
        Level = 1;
       

        for (int i =3; i<=Define.CHARACTER_MAX_LEVEL; i+= 3)
        {
            if(Manager.DataM.EvolutionDataDic.TryGetValue(i, out var data))
            {
                evolutionData[i] = data;
                isLearnEvloution[i] = false;
            }
        }

        UpdateStats();
    }


    private void UpdateStats()
    {
        Data = Manager.DataM.CreatureDic[DataId];
        CharacterLevelData = Manager.DataM.CharacterLevelDataDic[Level];

        MaxHp = Data.MaxHp + CharacterLevelData.HpUp;
        MaxHpRate = Data.HpRate + Evol_MaxHpRate;
        Attack = Data.Attack + CharacterLevelData.AttackUp;
        AttackRate = Data.AttackRate + Evol_AttackRate; ;
        Def = Data.Def + CharacterLevelData.DefUp;
        DefRate = Data.AttackRate + Evol_DefRate;
        MoveSpeed = Data.Speed + CharacterLevelData.SpeedUp;
        SpeedRate = Data.MoveSpeedRate + Evol_SpeedRate;
        CriticalRate = CharacterLevelData.CriticalUp + Evol_CriticalRate;
        CriticalDamage = CharacterLevelData.CriticalDamageUp + Evol_CriticalDamage;

        int index = Manager.GameM.Characters.IndexOf(this);
        if(index >= 0)
            Manager.GameM.UpdateCharacter(index, this);
    }
    public void SetInfo(int _key)
    {
        DataId = _key;
        UpdateStats();
    }

    public void LevelUp()
    {
        if (++Level > Define.CHARACTER_MAX_LEVEL)
            Level = Define.CHARACTER_MAX_LEVEL;

        UpdateStats();
    }

    public void ChangeCharacter(int _id)
    {
        DataId = _id;
        UpdateStats();
    }

    public void SetEvolution()
    {
        UpdateStats();
    }

    public void Evolution(int _level)
    {
        if (!evolutionData.TryGetValue(_level, out var data) || isLearnEvloution[_level]) return;

        isLearnEvloution[_level] = true;

        var spec = Manager.DataM.SpecialSkillDic[data.EvolutionAbilityNum];


        switch (data.EvolutionAbility)
        {
            case Define.EvolutionAbility.AttackBonus:

                Evol_AttackRate += spec.AttackBonus;
                break;
            case Define.EvolutionAbility.MaxHpBonus:
                Evol_MaxHpRate += spec.MaxHpBonus;
                break;
            case Define.EvolutionAbility.GoldBonus:
                Evol_GoldBonus = spec.GoldBouns;
                break;
            case Define.EvolutionAbility.CriticalBonus:
                Evol_CriticalRate += spec.GoldBouns;
                break;
            case Define.EvolutionAbility.CoolTimeBonus:
                Evol_CoolTimeBouns += spec.CoolTime;
                break;
            case Define.EvolutionAbility.DefBonus:
                Evol_DefRate += spec.DefBouns;
                break;
            case Define.EvolutionAbility.DiaBonus:
                Evol_DiaBouns = spec.DiaBouns;
                break;
            case Define.EvolutionAbility.HealingBonus:
                Evol_HealingBouns += spec.HealingBouns;
                break;
            case Define.EvolutionAbility.CriticalDamageBonus:
                Evol_CriticalDamage += spec.CriticalDamageBouns;
                break;
            case Define.EvolutionAbility.MoveSpeedBonus:
                Evol_SpeedRate += spec.MoveSpeedBonus;
                break;
        }


        SetEvolution();
    }
}
