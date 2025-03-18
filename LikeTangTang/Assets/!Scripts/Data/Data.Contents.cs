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
        public int level;
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

    /*TODO : 몬스터 데이터 생성하기.(Json 파일도 같이)
     * 
     * 
     * 드랍 아이템도 정해줘야함
     */
}
