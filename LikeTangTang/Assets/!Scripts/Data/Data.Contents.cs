using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Data
{
    #region Json PlayerData
    [Serializable]
    public class PlayerData
    {
        public int level; //key
        public int maxHp;
        public int attack;
        public int totalExp;
    }

    [Serializable]
    public class PlayerDataLoader : ILoader<int, PlayerData>
    {
        
        public List<PlayerData> stats = new List<PlayerData>();

        public Dictionary<int, PlayerData> MakeDict()
        {
            Dictionary<int, PlayerData> dic = new Dictionary<int, PlayerData>();
            foreach (PlayerData stat in stats)
                dic.Add(stat.level, stat);

            return dic;
        }
    }
    #endregion

    /*TODO : 드랍아이템 계층구조 설정 해야됌.
     * 
     */

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
        public int templateID;
        public string name;
        public string skillTypeStr;
        public Define.SkillType type = Define.SkillType.None;
        public string prefab;
        public float damage;
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
                if(stat.type == Define.SkillType.None)
                {
                    if(Enum.TryParse(stat.skillTypeStr, out Define.SkillType skillType))
                        stat.type = skillType;
                    else 
                        Debug.LogError("SkillData Type Match Error!!");
                }
                dic.Add(stat.templateID, stat);
            }
            
            return dic;
        }
    }
    #endregion

    #region 조이스틱이나, 맵, 하드코딩되는것들 데이터로 정리해서 가져와서 사용하자.
    #endregion
}
