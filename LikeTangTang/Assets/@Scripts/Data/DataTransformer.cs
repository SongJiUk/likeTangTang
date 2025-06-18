using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using System.Linq;
using UnityEditor.AddressableAssets;
using Newtonsoft.Json;
using Data;
using System.ComponentModel;
using static Define;
using UnityEngine.Analytics;


public class DataTransformer : EditorWindow
{
#if UNITY_EDITOR
    [MenuItem("Tools/DeleteGameData ")]
    public static void DeleteGameData()
    {
        PlayerPrefs.DeleteAll();
        string path = Application.persistentDataPath + "/SaveData.json";
        if (File.Exists(path))
            File.Delete(path);

        Debug.Log("PlayerPrefs + SaveData 삭제 완료: " + path);
    }
#endif

    [MenuItem("Tools/ParseExcel %#K")]
    public static void ParseExcel()
    {
        ParseSkillData("SkillData");
        ParseSkillEvolutionData("SkillEvolutionData");
        ParseStageData("StageData");
        ParseCreatureData("CreatureData");
        ParseLevelData("LevelData");
        ParseEquipmentLevelData("EquipmentLevelData");
        ParseEquipmentData("EquipmentData");
        ParseMaterialData("MaterialData");
        ParseSupportSkillData("SpecialSkillData");
        ParseDropItemData("DropItemData");
        ParseGachaData("GachaData");
        ParseEvolutionData("EvolutionData");
        ParseMissionData("MissionData");
        ParseAchievementData("AchievementData");
        ParseCheckOutData("AttendanceCheckData");
        ParseOfflineRewardData("OfflineRewardData");
        ParseCharacterLevelData("CharacterLevelData");
        Debug.Log("Complete DataTransformer");
    }

    static void ParseSkillData(string filename)
    {
        SkillDataLoader loader = new SkillDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");


        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            SkillData skillData = new SkillData();
            skillData.DataID = ConvertValue<int>(row[i++]);
            skillData.SkillName = ConvertValue<string>(row[i++]);
            skillData.SkillNameE = ConvertValue<string>(row[i++]);
            skillData.SkillDescription = ConvertValue<string>(row[i++]);
            skillData.SkillIcon = ConvertValue<string>(row[i++]);
            skillData.DamageMultiplier = ConvertValue<float>(row[i++]);
            skillData.ScaleMultiplier = ConvertValue<float>(row[i++]);
            skillData.CoolTime = ConvertValue<float>(row[i++]);
            skillData.Range = ConvertValue<float>(row[i++]);
            skillData.Duration = ConvertValue<float>(row[i++]);
            skillData.ProjectileCount = ConvertValue<int>(row[i++]);
            skillData.RoatateSpeed = ConvertValue<float>(row[i++]);
            skillData.AttackInterval = ConvertValue<float>(row[i++]);
            skillData.NumBounce = ConvertValue<int>(row[i++]);
            skillData.KnockBackPower = ConvertValue<float>(row[i++]);
            skillData.KnockBackInterval = ConvertValue<float>(row[i++]);
            skillData.BounceSpeed = ConvertValue<float>(row[i++]);
            skillData.NumPenerations = ConvertValue<int>(row[i++]);
            skillData.Speed = ConvertValue<float>(row[i++]);
            skillData.NumberOfAttacks = ConvertValue<int>(row[i++]);
            skillData.CastingSoundLabel = ConvertValue<string>(row[i++]);
            skillData.HitSoundLabel = ConvertValue<string>(row[i++]);
            skillData.CastingEffectID = ConvertValue<string>(row[i++]);
            skillData.HitEffectID = ConvertValue<string>(row[i++]);
            skillData.CastingEffect = ConvertValue<int>(row[i++]);
            skillData.HitEffect = ConvertValue<int>(row[i++]);
            skillData.SkillType = ConvertValue<SkillType>(row[i++]);
            skillData.CanEvolve = ConvertValue<bool>(row[i++]);
            skillData.EvolutionItemID = ConvertValue<int>(row[i++]);
            skillData.EvolvedSkillName = ConvertValue<string>(row[i++]);
            skillData.PrefabName = ConvertValue<string>(row[i++]);
            skillData.BoundDist = ConvertValue<int>(row[i++]);
            skillData.ExplosionName = ConvertValue<string>(row[i++]);
            skillData.EffectRange = ConvertValue<float>(row[i++]);
            skillData.EffectScaleMultiplier = ConvertValue<float>(row[i++]);
            skillData.SlowRatio = ConvertValue<float>(row[i++]);
            skillData.PullForce = ConvertValue<float>(row[i++]);
            


            loader.skillDatas.Add(skillData);
        }
        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/Json/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }

    static void ParseSkillEvolutionData(string filename)
    {
        SkillEvolutionDataLoader loader = new SkillEvolutionDataLoader();
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            SkillEvolutionData skillEvolutionData = new SkillEvolutionData();
            skillEvolutionData.EvolutionItemID = ConvertValue<int>(row[i++]);
            skillEvolutionData.EvolutionItemName = ConvertValue<string>(row[i++]);
            skillEvolutionData.EvolutionItemNameE = ConvertValue<string>(row[i++]);
            skillEvolutionData.EvolutionItemDescription = ConvertValue<string>(row[i++]);
            skillEvolutionData.EvolutionSkillID = ConvertValue<int>(row[i++]);
            skillEvolutionData.SkillName = ConvertValue<string>(row[i++]);
            skillEvolutionData.Type = ConvertValue<SkillType>(row[i++]);
            skillEvolutionData.EvolutionItemIcon = ConvertValue<string>(row[i++]);
            
            loader.skillEvolutionDatas.Add(skillEvolutionData);
        }

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/Json/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }

    static void ParseSupportSkillData(string filename)
    {
        SpecialSkillDataLoader loader = new SpecialSkillDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            SpecialSkillData skillData = new SpecialSkillData();
            skillData.DataID = ConvertValue<int>(row[i++]);
            skillData.SkillType = ConvertValue<SpecialSkillType>(row[i++]);
            skillData.SpecialSkillName = ConvertValue<SpecialSkillName>(row[i++]);
            skillData.SkillGrade = ConvertValue<SpecialSkillGrade>(row[i++]);
            skillData.Name = ConvertValue<string>(row[i++]);
            skillData.Description = ConvertValue<string>(row[i++]);
            skillData.IsLocked = ConvertValue<bool>(row[i++]);
            skillData.IsLearned = ConvertValue<bool>(row[i++]);
            skillData.MaxHpBonus = ConvertValue<float>(row[i++]);
            skillData.AttackBonus = ConvertValue<float>(row[i++]);
            skillData.CriticalBouns = ConvertValue<float>(row[i++]);
            skillData.MoveSpeedBonus = ConvertValue<float>(row[i++]);
            skillData.ExpBonus = ConvertValue<float>(row[i++]);
            skillData.DamageReductionBonus = ConvertValue<float>(row[i++]);
            skillData.HealingBouns = ConvertValue<float>(row[i++]);
            skillData.HpRegenBonus = ConvertValue<float>(row[i++]);
            skillData.CriticalDamageBouns = ConvertValue<float>(row[i++]);
            skillData.CollectRangeBouns = ConvertValue<float>(row[i++]);
            skillData.Healing = ConvertValue<float>(row[i++]);
            skillData.LevelUpMoveSpeedBonus = ConvertValue<float>(row[i++]);
            skillData.LevelUpDamageReductionBonus = ConvertValue<float>(row[i++]);
            skillData.LevelUpAttackBonus = ConvertValue<float>(row[i++]);
            skillData.LevelUpCriticalBonus = ConvertValue<float>(row[i++]);
            skillData.LevelUpCriticalDamageBonus = ConvertValue<float>(row[i++]);
            skillData.Resurrection = ConvertValue<float>(row[i++]);
            skillData.CoolTime = ConvertValue<float>(row[i++]);
            skillData.RoatateSpeed = ConvertValue<float>(row[i++]);
            skillData.NumBounce = ConvertValue<float>(row[i++]);
            skillData.Speed = ConvertValue<float>(row[i++]);
            skillData.ProjectileCount = ConvertValue<float>(row[i++]);
            skillData.ScaleMultiplier = ConvertValue<float>(row[i++]);
            skillData.EffectScaleMultiplier = ConvertValue<float>(row[i++]);
            skillData.DefBouns = ConvertValue<float>(row[i++]);
            skillData.GoldBouns = ConvertValue<float>(row[i++]);
            skillData.DiaBouns = ConvertValue<float>(row[i++]);

            loader.speicalskillDatas.Add(skillData);
        }
        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/Json/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }

    static void ParseStageData(string filename)
    {
        Dictionary<int, List<WaveData>> waveTable = ParseWaveData("WaveData");
        StageDataLoader loader = new StageDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;

            StageData stageData = new StageData();
            stageData.StageIndex = ConvertValue<int>(row[i++]);
            stageData.StageName = ConvertValue<string>(row[i++]);
            stageData.StageLevel = ConvertValue<int>(row[i++]);
            stageData.MapName = ConvertValue<string>(row[i++]);
            stageData.StageKill = ConvertValue<int>(row[i++]);
            stageData.FirstWaveCountValue = ConvertValue<int>(row[i++]);
            stageData.FirstWaveClearRewardItemID = ConvertValue<int>(row[i++]);
            stageData.FirstWaveClearRewardItemValue = ConvertValue<int>(row[i++]);
            stageData.SecondWaveCountValue = ConvertValue<int>(row[i++]);
            stageData.SecondWaveClearRewardItemID = ConvertValue<int>(row[i++]);
            stageData.SecondWaveClearRewardItemValue = ConvertValue<int>(row[i++]);
            stageData.ThirdWaveCountValue = ConvertValue<int>(row[i++]);
            stageData.ThirdWaveClearRewardItemID = ConvertValue<int>(row[i++]);
            stageData.ThirdWaveClearRewardItemValue = ConvertValue<int>(row[i++]);
            stageData.ClearGold = ConvertValue<int>(row[i++]);
            stageData.ClearExp = ConvertValue<int>(row[i++]);
            stageData.StageImage = ConvertValue<string>(row[i++]);
            stageData.SpawnMonsterNum = ConvertList<int>(row[i++]);
            waveTable.TryGetValue(stageData.StageIndex, out stageData.WaveArray);

            loader.stages.Add(stageData);
        }
        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/Json/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }

    static Dictionary<int, List<WaveData>> ParseWaveData(string filename)
    {
        Dictionary<int, List<WaveData>> waveTable = new Dictionary<int, List<WaveData>>();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;

            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            //int respawnID = ConvertValue<int>(row[i++]);
            WaveData waveData = new WaveData();
            waveData.StageIndex = ConvertValue<int>(row[i++]);
            waveData.WaveIndex = ConvertValue<int>(row[i++]);
            waveData.SpawnInterval = ConvertValue<int>(row[i++]);
            waveData.OnceSpawnCount = ConvertValue<int>(row[i++]);
            waveData.MonsterID = ConvertList<int>(row[i++]);
            waveData.EleteMonsterID = ConvertList<int>(row[i++]);
            waveData.BossMonsterID = ConvertList<int>(row[i++]);
            waveData.RemainsTime = ConvertValue<int>(row[i++]);
            waveData.WaveType = ConvertValue<int>(row[i++]);
            waveData.FirstMonsterSpawnRate = ConvertValue<float>(row[i++]);
            waveData.HpIncreaseRate = ConvertValue<float>(row[i++]);
            waveData.NonDropRate = ConvertValue<float>(row[i++]);
            waveData.SmallGemDropRate = ConvertValue<float>(row[i++]);
            waveData.GreenGemDropRate = ConvertValue<float>(row[i++]);
            waveData.BlueGemDropRate = ConvertValue<float>(row[i++]);
            waveData.YellowGemDropRate = ConvertValue<float>(row[i++]);
            waveData.EliteDropItemId = ConvertList<int>(row[i++]);

            if (waveTable.ContainsKey(waveData.StageIndex) == false)
                waveTable.Add(waveData.StageIndex, new List<WaveData>());

            waveTable[waveData.StageIndex].Add(waveData);
        }
        #endregion

        return waveTable;
    }
    static void ParseCreatureData(string filename)
    {
        CreatureDataLoader loader = new CreatureDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            CreatureData cd = new CreatureData();
            cd.DataID = ConvertValue<int>(row[i++]);
            cd.Name = ConvertValue<string>(row[i++]);
            cd.NameKR = ConvertValue<string>(row[i++]);
            cd.Description = ConvertValue<string>(row[i++]);
            cd.prefabName = ConvertValue<string>(row[i++]);
            cd.Type = ConvertValue<ObjectType>(row[i++]);
            cd.MaxHp = ConvertValue<float>(row[i++]);
            cd.MaxHpUpForIncreasStage = ConvertValue<float>(row[i++]);
            cd.Attack = ConvertValue<float>(row[i++]);
            cd.AttackUpForIncreasStage = ConvertValue<float>(row[i++]);
            cd.Def = ConvertValue<float>(row[i++]);
            cd.Speed = ConvertValue<float>(row[i++]);
            cd.HpRate = ConvertValue<float>(row[i++]);
            cd.AttackRate = ConvertValue<float>(row[i++]);
            cd.DefRate = ConvertValue<float>(row[i++]);
            cd.MoveSpeedRate = ConvertValue<float>(row[i++]);
            cd.Image_Name = ConvertValue<string>(row[i++]);
            cd.CreatureAnimName = ConvertValue<string>(row[i++]);
            cd.CharacterAnimName = ConvertValue<string>(row[i++]);
            cd.UnLockStage = ConvertValue<int>(row[i++]);
            cd.SkillTypeList = ConvertList<int>(row[i++]);
            cd.EvolutionTypeList = ConvertList<int>(row[i++]);
            loader.creatureData.Add(cd);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/Json/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }

    static void ParseLevelData(string filename)
    {
        LevelDataLoader loader = new LevelDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            LevelData data = new LevelData();
            data.level = ConvertValue<int>(row[i++]);
            data.TotalExp = ConvertValue<float>(row[i++]);
            loader.levelData.Add(data);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/Json/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }

    static void ParseEquipmentLevelData(string filename)
    {
        EquipmentLevelDataLoader loader = new EquipmentLevelDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            EquipmentLevelData data = new EquipmentLevelData();
            data.Level = ConvertValue<int>(row[i++]);
            data.Cost_Gold = ConvertValue<int>(row[i++]);
            data.Cost_Material = ConvertValue<int>(row[i++]);

            loader.levels.Add(data);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/Json/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }
    static void ParseEquipmentData(string filename)
    {
        EquipmentDataLoader loader = new EquipmentDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            EquipmentData ed = new EquipmentData();
            ed.DataID = ConvertValue<string>(row[i++]);
            ed.GachaGrade = ConvertValue<GachaGrade>(row[i++]);
            ed.EquipmentType = ConvertValue<EquipmentType>(row[i++]);
            ed.EquipmentGarde = ConvertValue<EquipmentGrade>(row[i++]);
            ed.NameTextID = ConvertValue<string>(row[i++]);
            ed.ItemDescription = ConvertValue<string>(row[i++]);
            ed.SpriteName = ConvertValue<string>(row[i++]);
            ed.Grade_Hp = ConvertValue<float>(row[i++]);
            ed.GradeUp_Hp = ConvertValue<float>(row[i++]);
            ed.Grade_Attack = ConvertValue<float>(row[i++]);
            ed.GradeUp_Attack = ConvertValue<float>(row[i++]);
            ed.Grade_MaxLevel = ConvertValue<int>(row[i++]);
            ed.BaseSkill = ConvertValue<int>(row[i++]);
            ed.UnCommonGradeAbility = ConvertValue<int>(row[i++]);
            ed.RareGradeAbility = ConvertValue<int>(row[i++]);
            ed.EpicGradeAbility = ConvertValue<int>(row[i++]);
            ed.UniqueGradeAbility = ConvertValue<int>(row[i++]);
            ed.MergeEquipmentType_1 = ConvertValue<MergeEquipmentType>(row[i++]);
            ed.MergeEquipment_1 = ConvertValue<string>(row[i++]);
            ed.MergeEquipmentType_2 = ConvertValue<MergeEquipmentType>(row[i++]);
            ed.MergeEquipment_2 = ConvertValue<string>(row[i++]);
            ed.MergeItemCode = ConvertValue<string>(row[i++]);
            ed.LevelUpMaterial = ConvertValue<int>(row[i++]);
            ed.DownGradeEquipmentCode = ConvertValue<string>(row[i++]);
            ed. DownGradeMaterialCode = ConvertValue<string>(row[i++]);
            ed.DownGradeMaterialCount = ConvertValue<int>(row[i++]);
            loader.equipmentDatas.Add(ed);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/Json/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }

    static void ParseMaterialData(string filename)
    {
        MaterialDataLoader loader = new MaterialDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;

            MaterialData material = new MaterialData();
            material.MaterialID = ConvertValue<int>(row[i++]);
            material.MaterialType = ConvertValue<Define.MaterialType>(row[i++]);
            material.MaterialGrade = ConvertValue<Define.MaterialGrade>(row[i++]);
            material.NameTextID = ConvertValue<string>(row[i++]);
            material.Description = ConvertValue<string>(row[i++]);
            material.SpriteName = ConvertValue<string>(row[i++]);

            loader.material.Add(material);
        }
        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/Json/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }

    static void ParseDropItemData(string filename)
    {
        DropItemDataLoader loader = new DropItemDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;

            DropItemData dropItem = new DropItemData();
            dropItem.DataID = ConvertValue<int>(row[i++]);
            dropItem.DropItemType = ConvertValue<DropItemType>(row[i++]);
            dropItem.Grade = ConvertValue<ItemGrade>(row[i++]);
            dropItem.NameTextID = ConvertValue<string>(row[i++]);
            dropItem.ItemDescription = ConvertValue<string>(row[i++]);
            dropItem.SpriteName = ConvertValue<string>(row[i++]);
            dropItem.AnimName = ConvertValue<string>(row[i++]);
            dropItem.EffectName = ConvertValue<string>(row[i++]);

            loader.dropData.Add(dropItem);
        }
        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/Json/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }

    static void ParseGachaData(string filename)
    {
        Dictionary<GachaType, List<GachaRateData>> gachaTable = ParseGachaRateData("GachaData");
        GachaDataLoader loader = new GachaDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int i = 0; i < gachaTable.Count + 1; i++)
        {
            GachaTableData gachaData = new GachaTableData()
            {
                Type = (GachaType)i,
            };
            if (gachaTable.TryGetValue(gachaData.Type, out List<GachaRateData> gachaRate))
                gachaData.GachaRateTable.AddRange(gachaRate);

            loader.list.Add(gachaData);
        }
        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/Json/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }

    static void ParseEvolutionData(string filename)
    {
        EvolutionDataLoader loader = new EvolutionDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;

            EvolutionData evolution = new EvolutionData();
            evolution.Level = ConvertValue<int>(row[i++]);
            evolution.EvolutionAbility = ConvertValue<Define.EvolutionAbility>(row[i++]);
            evolution.EvolutionAbilityNum = ConvertValue<int>(row[i++]);
            evolution.NeedGold = ConvertValue<int>(row[i++]);
            evolution.SpriteName = ConvertValue<string>(row[i++]);

            loader.list.Add(evolution);
        }
        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/Json/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }

    static Dictionary<GachaType, List<GachaRateData>> ParseGachaRateData(string filename)
    {
        Dictionary<GachaType, List<GachaRateData>> gachaTable = new Dictionary<GachaType, List<GachaRateData>>();

     
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0)
                continue;

            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            GachaType dropType = (GachaType)Enum.Parse(typeof(GachaType), row[i++]);
            GachaRateData rateData = new GachaRateData()
            {
                EquipmentID = ConvertValue<string>(row[i++]),
                GachaRate = float.Parse(row[i++]),
                EquipGrade = ConvertValue<EquipmentGrade>(row[i++]),
            };

            if (gachaTable.ContainsKey(dropType) == false)
                gachaTable.Add(dropType, new List<GachaRateData>());

            gachaTable[dropType].Add(rateData);
        }
        

        return gachaTable;
    }

    static void ParseMissionData(string filename)
    {
        MissionDataLoader loader = new MissionDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            MissionData stp = new MissionData();
            stp.MissionID = ConvertValue<int>(row[i++]);
            stp.MissionType = ConvertValue<Define.MissionType>(row[i++]);
            stp.DescriptionTextID = ConvertValue<string>(row[i++]);
            stp.MissionTarget = ConvertValue<Define.MissionTarget>(row[i++]);
            stp.MissionTargetValue = ConvertValue<int>(row[i++]);
            stp.ClearRewardItmeID = ConvertValue<int>(row[i++]);
            stp.RewardValue = ConvertValue<int>(row[i++]);

            loader.list.Add(stp);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/Json/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }

    static void ParseAchievementData(string filename)
    {
        AchievementDataLoader loader = new AchievementDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            AchievementData ach = new AchievementData();
            ach.AchievementID = ConvertValue<int>(row[i++]);
            ach.DescriptionTextID = ConvertValue<string>(row[i++]);
            ach.MissionTarget = ConvertValue<Define.MissionTarget>(row[i++]);
            ach.MissionTargetValue = ConvertValue<int>(row[i++]);
            ach.ClearRewardItemID = ConvertValue<int>(row[i++]);
            ach.RewardValue = ConvertValue<int>(row[i++]);
            ach.IsCompleted = ConvertValue<bool>(row[i++]);
            ach.IsRewarded = ConvertValue<bool>(row[i++]);
            loader.list.Add(ach);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/Json/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }

    static void ParseCheckOutData(string filename)
    {
        AttendanceCheckDataLoader loader = new AttendanceCheckDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            AttendanceCheckData acd = new AttendanceCheckData();
            acd.Day = ConvertValue<int>(row[i++]);
            acd.RewardItemId = ConvertValue<int>(row[i++]);
            acd.RewardItemValue = ConvertValue<int>(row[i++]);

            loader.list.Add(acd);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/Json/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }

    static void ParseOfflineRewardData(string filename)
    {
        OfflineRewardDataLoader loader = new OfflineRewardDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            OfflineRewardData ofr = new OfflineRewardData();
            ofr.StageIndex = ConvertValue<int>(row[i++]);
            ofr.Reward_Gold = ConvertValue<int>(row[i++]);
            ofr.Reward_LevelUpCoupon = ConvertValue<int>(row[i++]);
            ofr.FastReward_Scroll = ConvertValue<int>(row[i++]);
            ofr.FastReward_LevelUpCoupon = ConvertValue<int>(row[i++]);


            loader.list.Add(ofr);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/Json/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }
    static void ParseCharacterLevelData(string filename)
    {
        CharacterLevelDataLoader loader = new CharacterLevelDataLoader();

        #region ExcelData
        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/CSV/{filename}.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++)
        {
            string[] row = lines[y].Replace("\r", "").Split(',');

            if (row.Length == 0)
                continue;
            if (string.IsNullOrEmpty(row[0]))
                continue;

            int i = 0;
            CharacterLevelData chd = new CharacterLevelData();
            chd.Level = ConvertValue<int>(row[i++]);
            chd.NeedCouponCount = ConvertValue<int>(row[i++]);
            chd.AttackUp = ConvertValue<float>(row[i++]);
            chd.HpUp = ConvertValue<float>(row[i++]);
            chd.SpeedUp = ConvertValue<float>(row[i++]);
            chd.CriticalUp = ConvertValue<float>(row[i++]);
            chd.CriticalDamageUp = ConvertValue<float>(row[i++]);
            chd.DefUp = ConvertValue<float>(row[i++]);
            


            loader.list.Add(chd);
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/Json/{filename}.json", jsonStr);
        AssetDatabase.Refresh();
    }

   
    public static T ConvertValue<T>(string value)
    {
        if (string.IsNullOrEmpty(value))
            return default(T);

        if(typeof(T) == typeof(bool))
        {
            value = value.ToLower().Trim();
            if (value == "true")
                return (T)(object)true;
            if (value == "false")
                return (T)(object)false;

            return (T)(object)false;
        }
        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
        return (T)converter.ConvertFromString(value);
    }

    public static List<T> ConvertList<T>(string value)
    {
        if (string.IsNullOrEmpty(value))
            return new List<T>();

        return value.Split('/').Select(x => ConvertValue<T>(x)).ToList();
    }
// #endif

}