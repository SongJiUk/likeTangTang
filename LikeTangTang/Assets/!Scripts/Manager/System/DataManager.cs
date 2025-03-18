using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public interface ILoader<key, value>
{
    Dictionary<key, value> MakeDict();
}

public class DataManager
{
    /*TODO : Json vs Xml (Json 코드 파악해보기)
        난 Json
     
     */

    public Dictionary<int, Data.PlayerData> PlayerDic { get; private set; } = new Dictionary<int, Data.PlayerData>();
    public void Init()
    {
        PlayerDic = LoadJson<Data.PlayerDataLoader, int, Data.PlayerData>("PlayerData.json").MakeDict();
    }

    Loader LoadJson<Loader, key, value>(string _path) where Loader : ILoader<key, value>
    {
        TextAsset textAsset = Manager.ResourceM.Load<TextAsset>($"{_path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
