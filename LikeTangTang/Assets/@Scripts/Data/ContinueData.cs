using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ContinueData
{
    public bool isContinue { get { return SavedBattleSkill.Count > 0; } }
    public string PrefabName;
    public string PlayerName;
    public bool IsDead;
    public int PlayerDataID;
    public int CurrentWaveIndex;
    public float Hp;
    public float MaxHp;
    public float MaxHpBonusRate = 1;
    public float HealBonusRate = 1;
    public float HpRegen;
    public float Attack;
    public float AttackRate = 1;
    public float Def;
    public float DefRate = 1f;
    public float MoveSpeed;
    public float MoveSpeedRate = 1;
    public float TotalExp;
    public int Level = 1;
    public float Exp;
    public float CriticalRate;
    public float CriticalDamage = 1.5f;
    public float DamageReduction;
    public float ExpBonusRate = 1;
    public float CollectDistBonus = 1;
    public int KillCount;
    public int SkillRefreshCountAD = 3;
    public int SkillRefreshCount = 3;
    public int SpecialSkillHealCount = 0;
    public Dictionary<Define.SkillType, int> SavedBattleSkill = new Dictionary<Define.SkillType, int>();
    public Dictionary<Define.SkillType, int> SavedEvolutionSkill = new Dictionary<Define.SkillType, int>();
    public List<Data.SpecialSkillData> SavedSpecialSkill = new List<Data.SpecialSkillData>();
    public void Clear()
    {
        IsDead = false;
        PlayerDataID = 0;
        PrefabName = string.Empty;
        PlayerName = string.Empty;
        CurrentWaveIndex = 1;
        Hp = 0;
        MaxHp = 0;
        MaxHpBonusRate = 1f;
        HealBonusRate = 1f;
        HpRegen = 0f;
        Attack = 0;
        AttackRate = 1f;
        Def = 0;
        DefRate = 0;
        MoveSpeed = 0;
        MoveSpeedRate = 1f;
        TotalExp = 0f;
        Level = 1;
        Exp = 0f;
        CriticalRate = 0f;
        CriticalDamage = 1.5f;
        DamageReduction = 0f;
        ExpBonusRate = 1f;
        CollectDistBonus = 1f;
        KillCount = 0;
        SkillRefreshCountAD = 3;
        SkillRefreshCount = 3;
        SpecialSkillHealCount = 0;
        SavedBattleSkill.Clear();
        SavedEvolutionSkill.Clear();
        SavedSpecialSkill.Clear();
    }
}
