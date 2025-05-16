using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

/*
 * 플레이어 1~ 9999
 * 몬스터 (일반)	10000 ~ 10999	슬라임, 고블린 같은 일반 몬스터
몬스터 (엘리트)		11000 ~ 11999 강화 몬스터, 엘리트 몬스터
몬스터 (보스)		12000 ~ 12999 스테이지 보스 몬스터
스킬 (플레이어)	20000 ~ 20999	플레이어 전용 스킬
진화 스킬(플레이어) 21000~ 21999 진화에 사용되는 스킬(아이템, 진화 시스템)
스킬(몬스터용) 22000 ~ 22999 몬스터가 사용하는 스킬
진화에 필요한 아이템 23000~ 23999
아이템 (소모품)	30000 ~ 30999	포션, 버프템
아이템 (장비)	31000 ~ 39999	무기, 방어구

스테이지	40000 ~ 40999	스테이지 기본 정보
스테이지 보상	41000 ~ 41999	스테이지 클리어 보상 아이템
기타	50000 ~ 59999특수 데이터(이벤트, 미션 등)
재료값     60000 ~ 69999	머테리얼 값
이펙트, 사운드 등 값 70000~ 79999
 */
namespace Data
{
    #region StageData
    [Serializable]
    public class StageData
    {
        public int StageIndex;
        public string StageName;
        public int StageLevel;
        public string MapName;
        public int StageKill;
        public int ClearGold;
        public int ClearExp;
        public string StageImage;
        public List<int> SpawnMonsterNum;
        public List<WaveData> WaveArray;
    }

    public class StageDataLoader : ILoader<int, StageData>
    {
        public List<StageData> stages = new List<StageData>();
        public Dictionary<int, StageData> MakeDict()
        {
            Dictionary<int, StageData> dic = new Dictionary<int, StageData>();
            foreach (StageData stage in stages)
                dic.Add(stage.StageIndex, stage);

            return dic;
        }
    }
    #endregion

    #region WaveData
    [Serializable]
    public class WaveData
    {
        public int StageIndex;
        public int WaveIndex;
        public int SpawnInterval;
        public int OnceSpawnCount;
        public List<int> MonsterID;
        public List<int> EleteMonsterID;
        public List<int> BossMonsterID;
        public int RemainTime;
        public int WaveType;
        public float FirstMonsterSpawnRate;
        public float HpIncreaseRate;
        public float NonDropRate;
        public float SmallGemDropRate;
        public float GreenGemDropRate;
        public float BlueGemDropRate;
        public float YellowGemDropRate;
        public List<int> EliteDropItemId;
    }
    #endregion


    #region Creature Data
    [Serializable]
    public class CreatureData
    {
        public int DataID;
        public string DescriptionID;
        public string prefabName;
        public Define.ObjectType Type;
        public float MaxHp;
        public float MaxHpUpForIncreasStage;
        public float Attack;
        public float AttackUpForIncreasStage;
        public float Def;
        public float Speed;
        public float HpRate;
        public float AttackRate;
        public float DefRate;
        public float MoveSpeedRate;
        public string Image_Name;
        public string CreatureAnimName;
        public List<int> SkillTypeList;
        public List<int> EvolutionTypeList;

    }

    public class CreatureDataLoader : ILoader<int, CreatureData>
    {
        public List<CreatureData> creatureData = new List<CreatureData>();

        public Dictionary<int, CreatureData> MakeDict()
        {
            Dictionary<int, CreatureData> dic = new Dictionary<int, CreatureData>();
            foreach (CreatureData data in creatureData)
                dic.Add(data.DataID, data);

            return dic;

        }
    }
    #endregion

    #region SkillData

    [Serializable]
    public class SkillData
    {

        public int DataID;
        public string SkillName; //이름
        public string SkillNameE;
        public string SkillDescription; // 설명
        public string SkillIcon; //경로
        public float DamageMultiplier; // 데미지 배율
        public float ScaleMultiplier; //크기배율
        public float CoolTime; //쿨타임
        public float Range; // 범위
        public float Duration; //스킬 지속시간
        public int ProjectileCount; // 발사체 숫자
        public float RoatateSpeed; //회전 속도
        public float AttackInterval; //공격간격
        public int NumBounce; //바운스 횟수
        public float KnockBackPower;
        public float KnockBackInterval;
        public float BounceSpeed; //바운스 스피드 
        public int NumPenerations; // 관통 횟수
        public float Speed;
        public int NumberOfAttacks; // 공격횟수
        public string CastingSoundLabel; //발동 사운드
        public string HitSoundLabel; //맞을때 사운드
        public string CastingEffectID; //캐스팅 이펙트
        public string HitEffectID; // 맞을때 이펙트
        public int CastingEffect;
        public int HitEffect;
        public Define.SkillType SkillType = Define.SkillType.None; //타입
        public bool CanEvolve;
        public int EvolutionItemID;
        public string EvolvedSkillName; //진화스킬 이름
        public string PrefabName; //프리팹 이름
        public int BoundDist;
        public string ExplosionName;
        public float EffectRange;
        public float EffectScaleMultiplier;
        public float SlowRatio;
        public float PullForce;


    }

    [Serializable]
    public class SkillDataLoader : ILoader<int, SkillData>
    {
        //키랑 동일해야함 
        public List<SkillData> skillDatas = new List<SkillData>();

        public Dictionary<int, SkillData> MakeDict()
        {
            Dictionary<int, SkillData> dic = new Dictionary<int, SkillData>();
            foreach (SkillData stat in skillDatas)
            {
                dic.Add(stat.DataID, stat);
            }

            return dic;
        }
    }
    #endregion

    #region SkillEvolutionData
    [Serializable]
    public class SkillEvolutionData
    {
        public int EvolutionItemID;
        public string EvolutionItemName;
        public string EvolutionItemNameE;
        public string EvolutionItemDescription;
        public int EvolutionSkillID;
        public string SkillName;
        public string BeforeSKillName;
        public Define.SkillType Type;
        public string EvolutionItemIcon;
    }

    [Serializable]
    public class SkillEvolutionDataLoader : ILoader<int, SkillEvolutionData>
    {
        public List<SkillEvolutionData> skillEvolutionDatas = new List<SkillEvolutionData>();

        public Dictionary<int, SkillEvolutionData> MakeDict()
        {
            Dictionary<int, SkillEvolutionData> dic = new Dictionary<int, SkillEvolutionData>();
            foreach (SkillEvolutionData data in skillEvolutionDatas)
            {
                dic.Add(data.EvolutionItemID, data);
            }

            return dic;
        }
    }

    #endregion




    #region LevelData
    [Serializable]
    public class LevelData
    {
        public int level;
        public float TotalExp;
    }

    public class LevelDataLoader : ILoader<int, LevelData>
    {
        public List<LevelData> levelData = new List<LevelData>();

        public Dictionary<int, LevelData> MakeDict()
        {
            Dictionary<int, LevelData> dic = new Dictionary<int, LevelData>();
            foreach (LevelData data in levelData)
                dic.Add(data.level, data);

            return dic;
        }
    }
    #endregion

    #region DropItemDatas
    [Serializable]
    public class DropItemData
    {
        public int DataID;
        public string DropItemTypeStr;
        public Define.DropItemType DropItemType;
        public string Grade;
        public string NameTextID;
        public string ItemDescription;
        public string SpriteName;
        public string AnimName;
        public string EffectName;
    }
    [Serializable]
    public class DropItemDataLoader : ILoader<int, DropItemData>
    {
        public List<DropItemData> dropData = new List<DropItemData>();

        public Dictionary<int, DropItemData> MakeDict()
        {
            Dictionary<int, DropItemData> dic = new Dictionary<int, DropItemData>();
            foreach (DropItemData data in dropData)
            {
                dic.Add(data.DataID, data);
            }


            return dic;
        }
    }
    #endregion

    #region SpecialSkillData
    [Serializable]
    public class SpecialSkillData
    {
        public int DataID;
        public Define.SpecialSkillType SkillType;
        public Define.SpecialSkillName SpecialSkillName;
        public Define.SpecialSkillGrade SkillGrade;
        public string Name;
        public string Description;
        public bool IsLocked;
        public bool IsLearned;
        public float MaxHpBonus;
        public float AttackBonus;
        public float CriticalBouns;
        public float MoveSpeedBonus;
        public float ExpBonus;
        public float DamageReductionBonus;
        public float HealingBouns;
        public float HpRegenBonus;
        public float CriticalDamageBouns;
        public float CollectRangeBouns;
        public float Healing;
        public float LevelUpMoveSpeedBonus;
        public float LevelUpDamageReductionBonus;
        public float LevelUpAttackBonus;
        public float LevelUpCriticalBonus;
        public float LevelUpCriticalDamageBonus;
        public float Resurrection;
        public float CoolTime;
        public float RoatateSpeed;
        public float NumBounce;
        public float Speed;
        public float ProjectileCount;
        public float ScaleMultiplier;


    }

    [Serializable]
    public class SpecialSkillDataLoader : ILoader<int, SpecialSkillData>
    {
        public List<SpecialSkillData> speicalskillDatas = new List<SpecialSkillData>();

        public Dictionary<int, SpecialSkillData> MakeDict()
        {
            Dictionary<int, SpecialSkillData> dic = new Dictionary<int, SpecialSkillData>();

            foreach (SpecialSkillData data in speicalskillDatas)
            {
                dic.Add(data.DataID, data);
            }

            return dic;
        }
    }
    #endregion


    #region EquipmentData
    [Serializable]
    public class EquipmentData
    {
        public string DataID;
        public Define.GachaGrade GachaGrade;
        public Define.EquipmentType EquipmentType;
        public Define.EquipmentGrade EquipmentGarde;
        public string NameTextID;
        public string ItemDescription;
        public string SpriteName;
        public float Grade_Hp;
        public float GradeUp_Hp;
        public float Grade_Attack;
        public float GradeUp_Attack;
        public int Grade_MaxLevel;
        public int BaseSkill;
        public int UnCommonGradeAbility;
        public int RareGradeAbility;
        public int EpicGradeAbility;
        public int UniqueGradeAbility;

        public Define.MergeEquipmentType MergeEquipmentType_1;
        public string MergeEquipment_1;
        public Define.MergeEquipmentType MergeEquipmentType_2;
        public string MergeEquipment_2;
        public string MergeItemCode;
        public int LevelUpMaterial;
        public string DownGradeEquipmentCode;
        public string DownGradeMaterialCode;
        public int DownGradeMaterialCount;
    }

    [Serializable]
    public class EquipmentDataLoader : ILoader<string, EquipmentData>
    {
        public List<EquipmentData> equipmentDatas = new List<EquipmentData>();

        public Dictionary<string, EquipmentData> MakeDict()
        {
            Dictionary<string, EquipmentData> dic = new Dictionary<string, EquipmentData>();

            foreach(EquipmentData data in equipmentDatas)
            {
                dic.Add(data.DataID, data);
            }

            return dic;
        }
    }
    #endregion

    #region EquipmentLevelData
    [Serializable]
    public class EquipmentLevelData
    {
        public int Level;
        public int Cost_Gold;
        public int Cost_Material;
    }

    [Serializable]
    public class EquipmentLevelDataLoader : ILoader<int, EquipmentLevelData>
    {
        public List<EquipmentLevelData> levels = new List<EquipmentLevelData>();
        public Dictionary<int, EquipmentLevelData> MakeDict()
        {
            Dictionary<int, EquipmentLevelData> dic = new Dictionary<int, EquipmentLevelData>();

            foreach (EquipmentLevelData levelData in levels)
                dic.Add(levelData.Level, levelData);

            return dic;
        }
    }
    #endregion

    #region MaterialData

    [Serializable]
    public class MaterialData
    {
        public int MaterialID;
        public Define.MaterialType MaterialType;
        public Define.MaterialGrade MaterialGrade;
        public string NameTextID;
        public string Description;
        public string SpriteName;
    }

    [Serializable]
    public class MaterialDataLoader : ILoader<int, MaterialData>
    {
        public List<MaterialData> material = new List<MaterialData>();

        public Dictionary<int, MaterialData> MakeDict()
        {
            Dictionary<int, MaterialData> dic = new Dictionary<int, MaterialData>();

            foreach(MaterialData data in material)
            {
                dic.Add(data.MaterialID, data);
            }

            return dic;
        }
    }
    #endregion

    #region GaChaData
    public class GachaTableData
    {
        public Define.GachaType Type;
        public List<GachaRateData> GachaRateTable = new List<GachaRateData>();
    }

    public class GachaDataLoader : ILoader<Define.GachaType, GachaTableData>
    {
        public List<GachaTableData> list = new List<GachaTableData>();

        public Dictionary<Define.GachaType, GachaTableData> MakeDict()
        {
            Dictionary<Define.GachaType, GachaTableData> dic = new Dictionary<Define.GachaType, GachaTableData>();
            foreach (GachaTableData gacha in list)
                dic.Add(gacha.Type, gacha);

            return dic;
        }
    }




    public class GachaRateData
    {
        public string EquipmentID;
        public float GachaRate;
        public Define.EquipmentGrade EquipGrade;
    }
    #endregion
}