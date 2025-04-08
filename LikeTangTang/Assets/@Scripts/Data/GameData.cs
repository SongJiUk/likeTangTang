using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

[Serializable]
public class GameData
{
    public int userID = Define.DEFAULT_PLAYER_ID;
    public int userLevel = 1;
    public string userName = "Player";
    public int gold = 0;
    public int dia = 0;

    // TODO : 모든 정보가 다 들어간다고 생각하면 됌. 
    public ContinueData ContinueDatas = new ContinueData();
    public StageData CurrentStageData = new StageData();

    public Dictionary<int, StageClearInfoData> StageClearInfoDic = new Dictionary<int, StageClearInfoData>();
}
