using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Data;
public class AchievementManager 
{
    public List<Data.AchievementData> achievements;
    private Dictionary<Define.MissionTarget, List<AchievementData>> byTarget = new Dictionary<Define.MissionTarget, List<AchievementData>>();

    
    
    public void Init()
    {
        achievements = new List<AchievementData>(Manager.DataM.AchievementDataDic.Values);

        foreach(var achievement in achievements)
        {
            if (!byTarget.TryGetValue(achievement.MissionTarget, out var list))
            {
                list = new List<AchievementData>();
                byTarget[achievement.MissionTarget] = list;
            }
            list.Add(achievement);
        }

        Manager.GameM.Achievements = achievements;
    }


    private void Complete(AchievementData _data)
    {
        if(!_data.IsRewarded)
        {
            _data.IsCompleted = true;
            Manager.GameM.SaveGame();
        }
    }

    private void Reward(AchievementData _data)
    {
        if(!_data.IsRewarded)
        {
            _data.IsRewarded = true;
            _data.IsCompleted = true;
            Manager.GameM.SaveGame();
        }
    }

    //업적 완료 처리
    public void CompleteAchievement(int _dataID)
    {
        AchievementData achievement = achievements.Find(a => a.AchievementID == _dataID);
        if (achievement != null) Complete(achievement);
    }


    //보상받기 완료 처리
    public void RewardedAchievement(int _dataID)
    {
        AchievementData achievement = achievements.Find(a => a.AchievementID == _dataID);
        if (achievement != null) Reward(achievement);
    }


    //미션 타겟별 진행 체크 헬퍼
    private void ProcessTarget(Define.MissionTarget _target, Func<int> _getValue, bool _requireExact = true)
    {
        if (!byTarget.TryGetValue(_target, out var list)) return;

        int progress = _getValue();
        foreach(var data in list)
        {
            if (data.IsCompleted && data.IsRewarded) continue;

            if(_requireExact ? data.MissionTargetValue == progress
                             : data.MissionTargetValue <= progress)
            {
                Complete(data);
            }
        }
        Manager.UiM.CheckRedDotObject(Define.RedDotObjectType.AchievementPopup);
    }

    public List<AchievementData> GetAchievements()
    {
        List<AchievementData> result = new List<AchievementData>();

        foreach (Define.MissionTarget missionTarget in Enum.GetValues(typeof(Define.MissionTarget)))
        {
            if (!byTarget.TryGetValue(missionTarget, out var list) || list.Count == 0) continue;

            AchievementData pick = null;

            foreach (var data in list)
            {
                if (!data.IsCompleted || !data.IsRewarded)
                {
                    pick = data;
                    break;
                }
            }

            result.Add(pick ?? list[^1]);
        }
        return result;
    }

    public List<AchievementData> CheckAchievements()
    {
        List<AchievementData> result = new List<AchievementData>();

        foreach (var achievement in achievements)
        {
            if (achievement.IsCompleted && !achievement.IsRewarded)
                result.Add(achievement);
        }

        return result;
    }
    public int GetProgressValue(Define.MissionTarget _missionTarget)
    {

        return _missionTarget switch
        { 
            Define.MissionTarget.StageEnter => Manager.GameM.MissionDic[_missionTarget].Progress,
            Define.MissionTarget.EquipmentLevelUp => Manager.GameM.MissionDic[_missionTarget].Progress,
            Define.MissionTarget.EquipmentMerge => Manager.GameM.MissionDic[_missionTarget].Progress,
            Define.MissionTarget.ADWatchIng => Manager.GameM.MissionDic[_missionTarget].Progress,
            
            Define.MissionTarget.OfflineRewardGet => Manager.GameM.OfflineRewardGetCount,
            Define.MissionTarget.FastOfflineRewardGet => Manager.GameM.FastOfflineRewardGetCount,

            Define.MissionTarget.MonsterKill => Manager.GameM.TotalMonsterKillCount,
            Define.MissionTarget.EliteMonsterKill => Manager.GameM.TotalEliteMonsterKillCount,
            Define.MissionTarget.BossKill => Manager.GameM.TotalBossKillCount,

            Define.MissionTarget.StageClear => Manager.GameM.GetMaxStageClearIndex(),
            Define.MissionTarget.Login => Manager.TimeM.AttendanceDay,
            Define.MissionTarget.CommonGachaOpen => Manager.GameM.CommonGachaOpenCount,
            Define.MissionTarget.AdvancedGachaOpen => Manager.GameM.AdvancedGachaOpenCount,

            _ => 0
        };
    }

    public AchievementData GetNextAchievement(int _dataID)
    {
        return achievements.Find(a => a.AchievementID == _dataID + 1 && !a.IsRewarded);
    }

    public void Attendance() => ProcessTarget(Define.MissionTarget.Login, () => Manager.TimeM.AttendanceDay);
    public void StageClear() => ProcessTarget(Define.MissionTarget.StageClear, () => Manager.GameM.GetMaxStageClearIndex());
    public void CommonBoxOpen() => ProcessTarget(Define.MissionTarget.CommonGachaOpen, () => Manager.GameM.CommonGachaOpenCount, _requireExact: false);
    public void AdvancedBoxOpen() => ProcessTarget(Define.MissionTarget.AdvancedGachaOpen, () => Manager.GameM.AdvancedGachaOpenCount, _requireExact : false);
    public void OfflineReward() => ProcessTarget(Define.MissionTarget.OfflineRewardGet, () => Manager.GameM.OfflineRewardGetCount);
    public void FastReward() => ProcessTarget(Define.MissionTarget.FastOfflineRewardGet, () => Manager.GameM.FastOfflineRewardGetCount);
    public void MonsterKill() => ProcessTarget(Define.MissionTarget.MonsterKill, () => Manager.GameM.TotalMonsterKillCount);
    public void EliteMonsterKill() => ProcessTarget(Define.MissionTarget.EliteMonsterKill, () => Manager.GameM.TotalEliteMonsterKillCount);
    public void BossKill() => ProcessTarget(Define.MissionTarget.BossKill, () => Manager.GameM.TotalBossKillCount);
}
