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
    public Dictionary<int, Data.OfflineRewardData> OfflineRewardDataDic { get; private set; } = new Dictionary<int, Data.OfflineRewardData>();
    public Dictionary<int, Data.CharacterLevelData> CharacterLevelDataDic { get; private set; } = new Dictionary<int, Data.CharacterLevelData>();

    public Dictionary<int, Data.EvolutionData> EvolutionDataDic { get; private set; } = new Dictionary<int, Data.EvolutionData>();




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
        OfflineRewardDataDic = LoadJson<Data.OfflineRewardDataLoader, int, Data.OfflineRewardData>("OfflineRewardData.json").MakeDict();
        CharacterLevelDataDic = LoadJson<Data.CharacterLevelDataLoader, int, Data.CharacterLevelData>("CharacterLevelData.json").MakeDict();
        EvolutionDataDic = LoadJson<Data.EvolutionDataLoader, int, Data.EvolutionData>("EvolutionData.json").MakeDict();
    }

    Loader LoadJson<Loader, key, value>(string _path) where Loader : ILoader<key, value>
    {
        TextAsset textAsset = Manager.ResourceM.Load<TextAsset>($"{_path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }
}


/* NOTE
 LoadJson에서 텍스트 파일 형태로 불러온다. 모든 텍스트 내용을 문자열로 불러옴,
 컨버트 라이브러리를 사용해서 문자열을 C#객체로 변환시킨다. 이때, Loader타입으로 변환, 예를 들면 SkillDic은 Data.SkillDataLoader로 변환된다.

 Json파일은 보통 리스트 형태로 되어있다. 하지만 딕셔너리가 훨씬 좋음.
그래서 딕셔너리로 변경하는 기능(MakeDict)을 반드시 가지고있어야한다는 규칙을 정의한다.
그래서 Loader들은 인터페이스를 상속해야 한다.

메서드 체이닝
1. 게임 시작 시 어딘가에서 DataManager.Init()을 호출합니다.
   2. Init() 메소드 안에서 다음 코드가 실행됩니다.
      SkillDic = LoadJson<Data.SkillDataLoader, int, Data.SkillData>("SkillData.json").MakeDict();
   3. `LoadJson` 호출:
       - Unity의 Resources 폴더에서 SkillData.json 파일을 찾아 텍스트를 읽습니다.
       - Newtonsoft.Json이 이 텍스트를 Data.SkillDataLoader 객체로 변환합니다. 이 객체 안에는 SkillData의 리스트가 채워져 있습니다.
       - LoadJson은 이 Data.SkillDataLoader 객체를 반환합니다.
   4. `MakeDict()` 호출:
       - LoadJson이 반환한 Data.SkillDataLoader 객체에 이어서 .MakeDict()가 호출됩니다.
       - SkillDataLoader 객체는 자기가 가지고 있는 SkillData 리스트를 순회하면서, 각 스킬의 ID를 key로, SkillData 객체를 value로 하는 Dictionary<int, Data.SkillData>를 생성하여 반환합니다.
   5. 할당:
       - MakeDict()가 반환한 완성된 딕셔너리가 DataManager의 SkillDic 프로퍼티에 최종적으로 할당됩니다.
   6. 이 과정이 Init()에 있는 모든 데이터 종류에 대해 반복됩니다.
*/
