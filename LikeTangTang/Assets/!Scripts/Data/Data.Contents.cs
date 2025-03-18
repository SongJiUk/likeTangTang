using System;
using System.Collections;
using System.Collections.Generic;
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

    /*TODO : 드랍아이템 계층구조. 
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
}
