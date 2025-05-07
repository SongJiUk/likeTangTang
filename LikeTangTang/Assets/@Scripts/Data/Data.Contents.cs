using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

/*
 * 몬스터 (일반)	1000 ~ 1099	슬라임, 고블린 같은 일반 몬스터
몬스터 (엘리트)	1100 ~ 1199	강화 몬스터, 엘리트 몬스터
몬스터 (보스)	1200 ~ 1299	스테이지 보스 몬스터
스킬 (플레이어)	2000 ~ 2099	플레이어 전용 스킬
스킬(몬스터용) 2200 ~ 2299 몬스터가 사용하는 스킬
진화에 필요한 아이템 2300~2399
아이템 (장비)	3000 ~ 3099	무기, 방어구
아이템 (소모품)	3100 ~ 3199	포션, 버프템
스테이지	4000 ~ 4099	스테이지 기본 정보
스테이지 보상	4100 ~ 4199	스테이지 클리어 보상 아이템
기타	5000 ~ 5999특수 데이터(이벤트, 미션 등)
머테리얼값     6000 ~ 6099	머테리얼 값
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
            foreach(StageData stage in stages)
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
            foreach(CreatureData data in creatureData)
                dic.Add(data.DataID, data);
            
            return dic;

        }
    }
    #endregion

    #region Json MonsterData

    [Serializable]
    public class MonsterData
    {
        public string name; //key
        public int maxHp;
        public int attck;
        public int giveExp;
        public string prefab;
        public List<int> rare;
    }

    [Serializable]
    public class MonsterDataLoader : ILoader<string, MonsterData>
    {
        public List<MonsterData> stats = new List<MonsterData>();

        public Dictionary<string, MonsterData> MakeDict()
        {
            Dictionary<string, MonsterData> dic = new Dictionary<string, MonsterData>();
            foreach (MonsterData stat in stats)
                dic.Add(stat.name, stat);

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
        public string SkillTypeStr; //타입
        public Define.SkillType Type = Define.SkillType.None; //타입
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
            foreach(SkillData stat in skillDatas)
            {
                if(stat.Type == Define.SkillType.None)
                {
                    if(Enum.TryParse(stat.SkillTypeStr, out Define.SkillType skillType))
                        stat.Type = skillType;
                    else 
                        Debug.LogError("SkillData Type Match Error!!, Data.Contents 98Line");
                }
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
            foreach(SkillEvolutionData data in skillEvolutionDatas)
            {
                if(data.Type == Define.SkillType.None)
                {
                    if (Enum.TryParse(data.BeforeSKillName, out Define.SkillType skillType))
                    {
                        data.Type = skillType;
                    }
                    else
                        Debug.LogError("SKillData 틀림");
                }
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
            foreach(LevelData data in levelData)
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

    public class DropItemDataLoader : ILoader<int, DropItemData>
    {
        public List<DropItemData> dropData = new List<DropItemData>();

        public Dictionary<int, DropItemData> MakeDict()
        {
            Dictionary<int, DropItemData> dic = new Dictionary<int, DropItemData>();
            foreach(DropItemData data in dropData)
            {
                if(data.DropItemType == Define.DropItemType.None)
                {
                    if(Enum.TryParse(data.DropItemTypeStr, out Define.DropItemType dropitemType))
                    {
                        data.DropItemType = dropitemType;
                    }
                    else
                    {
                        Debug.LogError("DropItemData Type Match Error!!, DropItemDataLoader MakeDict()!!");
                    }
                }

                dic.Add(data.DataID, data);
            }
             

            return dic;
        }
    }
    #endregion
}