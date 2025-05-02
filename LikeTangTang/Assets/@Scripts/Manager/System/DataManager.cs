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
    public Dictionary<int, Data.CreatureData> CreatureDic {get; private set;} = new Dictionary<int, Data.CreatureData>();

    public Dictionary<int, Data.SkillData> SkillDic {get; private set;} = new Dictionary<int, Data.SkillData>();
    public Dictionary<int, Data.SkillEvolutionData> SkillEvolutionDic { get; private set; } = new Dictionary<int, Data.SkillEvolutionData>();
    public Dictionary<int, Data.LevelData> LevelDic {get; private set; } = new Dictionary<int, Data.LevelData>();

    public Dictionary<int, Data.StageData> StageDic {get; private set;} = new Dictionary<int, Data.StageData>();
    public Dictionary<int, Data.DropItemData> DropItemDic {get; private set;} = new Dictionary<int, Data.DropItemData>();
    
    public void Init()
    {
        CreatureDic = LoadJson<Data.CreatureDataLoader, int, Data.CreatureData>("CreatureData.json").MakeDict();
        SkillDic = LoadJson<Data.SkillDataLoader, int, Data.SkillData>("SkillData.json").MakeDict();
        SkillEvolutionDic = LoadJson<Data.SkillEvolutionDataLoader, int, Data.SkillEvolutionData>("SkillEvolutionData.json").MakeDict();
        StageDic = LoadJson<Data.StageDataLoader, int, Data.StageData>("StageData.json").MakeDict();
        LevelDic = LoadJson<Data.LevelDataLoader, int, Data.LevelData>("LevelData.json").MakeDict();
        DropItemDic = LoadJson<Data.DropItemDataLoader, int, Data.DropItemData>("DropItemData.json").MakeDict();

    }

    Loader LoadJson<Loader, key, value>(string _path) where Loader : ILoader<key, value>
    {
        TextAsset textAsset = Manager.ResourceM.Load<TextAsset>($"{_path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);

        //return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
