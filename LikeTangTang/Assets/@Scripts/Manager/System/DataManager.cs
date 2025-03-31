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
    public Dictionary<int, Data.PlayerData> PlayerDic { get; private set; }
        = new Dictionary<int, Data.PlayerData>();

    public Dictionary<string, Data.MonsterData> MonsterDic { get; private set; }
        = new Dictionary<string, Data.MonsterData>();

    public Dictionary<int, Data.SkillData> SkillDic {get; private set;}
        = new Dictionary<int, Data.SkillData>();

    public void Init()
    {
        PlayerDic = LoadJson<Data.PlayerDataLoader, int, Data.PlayerData>("PlayerData.json").MakeDict();
        MonsterDic = LoadJson<Data.MonsterDataLoader, string, Data.MonsterData>("MonsterData.json").MakeDict();
        SkillDic = LoadJson<Data.SkillDataLoader, int, Data.SkillData>("SkillData.json").MakeDict();
    }

    Loader LoadJson<Loader, key, value>(string _path) where Loader : ILoader<key, value>
    {
        TextAsset textAsset = Manager.ResourceM.Load<TextAsset>($"{_path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);

        //return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
