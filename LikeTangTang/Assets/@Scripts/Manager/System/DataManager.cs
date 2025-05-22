using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;


public interface ILoader<key, value>
{
    Dictionary<key, value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.SkillData> SkillDic {get; private set;} = new Dictionary<int, Data.SkillData>();
    public Dictionary<int, Data.SkillEvolutionData> SkillEvolutionDic { get; private set; } = new Dictionary<int, Data.SkillEvolutionData>();
    public Dictionary<int, Data.StageData> StageDic { get; private set; } = new Dictionary<int, Data.StageData>();
    public Dictionary<int, Data.CreatureData> CreatureDic { get; private set; } = new Dictionary<int, Data.CreatureData>();
    public Dictionary<int, Data.LevelData> LevelDic {get; private set; } = new Dictionary<int, Data.LevelData>();
    public Dictionary<int, Data.EquipmentLevelData> EquipmentLevelDic { get; private set; } = new Dictionary<int, Data.EquipmentLevelData>();
    public Dictionary<string, Data.EquipmentData> EquipmentDic { get; private set; } = new Dictionary<string, Data.EquipmentData>();
    public Dictionary<int, Data.MaterialData> MaterialDic { get; private set; } = new Dictionary<int, Data.MaterialData>();
    public Dictionary<int, Data.SpecialSkillData> SpecialSkillDic { get; private set; } = new Dictionary<int, Data.SpecialSkillData>();
    public Dictionary<int, Data.DropItemData> DropItemDic {get; private set;} = new Dictionary<int, Data.DropItemData>();
    public Dictionary<Define.GachaType, Data.GachaTableData> GachaTableDataDic { get; private set; } = new Dictionary<Define.GachaType, Data.GachaTableData>();
    public Dictionary<int, Data.AttendanceCheckData> AttendanceCheckDataDic { get; private set; } = new Dictionary<int, Data.AttendanceCheckData>();
    public Dictionary<int, Data.MissionData> MissionDataDic { get; private set; } = new Dictionary<int, Data.MissionData>();
    public Dictionary<int, Data.AchievementData> AchievementDataDic { get; private set; } = new Dictionary<int, Data.AchievementData>();
    


    
    public void Init()
    {
        SkillDic = LoadJson<Data.SkillDataLoader, int, Data.SkillData>("SkillData.json").MakeDict();
        SkillEvolutionDic = LoadJson<Data.SkillEvolutionDataLoader, int, Data.SkillEvolutionData>("SkillEvolutionData.json").MakeDict();
        StageDic = LoadJson<Data.StageDataLoader, int, Data.StageData>("StageData.json").MakeDict();
        CreatureDic = LoadJson<Data.CreatureDataLoader, int, Data.CreatureData>("CreatureData.json").MakeDict();
        LevelDic = LoadJson<Data.LevelDataLoader, int, Data.LevelData>("LevelData.json").MakeDict();
        EquipmentLevelDic = LoadJson<Data.EquipmentLevelDataLoader, int, Data.EquipmentLevelData>("EquipmentLevelData.json").MakeDict();
        EquipmentDic = LoadJson<Data.EquipmentDataLoader, string, Data.EquipmentData>("EquipmentData.json").MakeDict();
        MaterialDic = LoadJson<Data.MaterialDataLoader, int, Data.MaterialData>("MaterialData.json").MakeDict();
        SpecialSkillDic = LoadJson<Data.SpecialSkillDataLoader, int, Data.SpecialSkillData>("SpecialSkillData.json").MakeDict();
        DropItemDic = LoadJson<Data.DropItemDataLoader, int, Data.DropItemData>("DropItemData.json").MakeDict();
        GachaTableDataDic = LoadJson<Data.GachaDataLoader, Define.GachaType, Data.GachaTableData>("GachaData.json").MakeDict();
        AttendanceCheckDataDic = LoadJson<Data.AttendanceCheckDataLoader, int, Data.AttendanceCheckData>("AttendanceCheckData.json").MakeDict();
        MissionDataDic = LoadJson<Data.MissionDataLoader, int, Data.MissionData>("MissionData.json").MakeDict();
        AchievementDataDic = LoadJson<Data.AchievementDataLoader, int, Data.AchievementData>("AchievementData.json").MakeDict();
    }

    Loader LoadJson<Loader, key, value>(string _path) where Loader : ILoader<key, value>
    {
        TextAsset textAsset = Manager.ResourceM.Load<TextAsset>($"{_path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }
}
