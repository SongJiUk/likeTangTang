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
    public int stamina = 0;

    #region 업적
    public int TotalStageClear = 0;
    public int CommonGachaOpenCount = 0;
    public int AdvancedGachaOpenCount = 0;
    public int OfflineRewardGetCount = 0;
    public int FastOfflineRewardGetCount = 0;
    public int TotalMonsterKillCount = 0;
    public int TotalEliteMonsterKillCount = 0;
    public int TotalBossKillCount = 0;
    public List<Data.AchievementData> Achievements = new List<AchievementData>();
    public bool isAchievementAcceptItem = false;
    #endregion
    public bool isMissionPossibleAcceptItem = false;
    //하루에 한번 초기화
    public int GacahCountAdsAdvanced = 1;
    public int GacahCountAdsCommon = 1;
    public int GoldCountAds = 1;
    public int SilverKeyCountAds = 3;
    public int DiaCountAds = 3;
    public int StaminaCountAds = 2;
    public int RemainBuyStaminaForDia = 3;
    public int FastRewardCountAd = 1;
    public int FastRewardCountStamina = 3;
    public int RebirthCountAds = 3;

    public bool BGMOn = true;
    public bool EffectSoundOn = true;
    public Define.JoyStickType JoyStickType = Define.JoyStickType.Flexible;

    public bool[] AttendanceReceived = new bool[30];

    // TODO : 모든 정보가 다 들어간다고 생각하면 됌. 
    public ContinueData ContinueDatas = new ContinueData();
    public StageData CurrentStageData = new StageData();
    public List<Character> Characters = new List<Character>();

    public List<Equipment> OwnedEquipments = new List<Equipment>();
    public Dictionary<Define.EquipmentType, Equipment> EquipedEquipments = new Dictionary<Define.EquipmentType, Equipment>();
    public Dictionary<int, int> ItemDictionary = new Dictionary<int, int>();
    public Dictionary<int, StageClearInfoData> StageClearInfoDic = new Dictionary<int, StageClearInfoData>();
    public Dictionary<Define.MissionTarget, MissionInfo> MissionDic = new Dictionary<Define.MissionTarget, MissionInfo>()
    {
        {Define.MissionTarget.StageEnter, new MissionInfo() { Progress = 0, isRewarded = false} },
        {Define.MissionTarget.EquipmentLevelUp, new MissionInfo() { Progress = 0, isRewarded = false} },
        {Define.MissionTarget.EquipmentMerge, new MissionInfo() { Progress = 0, isRewarded = false} },
        {Define.MissionTarget.GachaOpen, new MissionInfo() { Progress = 0, isRewarded = false} },
        {Define.MissionTarget.OfflineRewardGet, new MissionInfo() { Progress = 0, isRewarded = false} },
        {Define.MissionTarget.MonsterKill, new MissionInfo() { Progress = 0, isRewarded = false} },
        {Define.MissionTarget.EliteMonsterKill, new MissionInfo() { Progress = 0, isRewarded = false} },
        {Define.MissionTarget.StageClear, new MissionInfo() { Progress = 0, isRewarded = false} },
        {Define.MissionTarget.ADWatchIng, new MissionInfo() { Progress = 0, isRewarded = false} },
    };


    public void Init()
    {
        foreach (var e in OwnedEquipments)
            e?.Init();

        foreach(var e in EquipedEquipments)
            e.Value?.Init();
        
    }
}
