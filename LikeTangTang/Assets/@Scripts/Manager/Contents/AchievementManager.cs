using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Data;
public class AchievementManager 
{
   public List<Data.AchievementData> achievements
    {
        get { return Manager.GameM.Achievements; }
        set { Manager.GameM.Achievements = value; }
    }

    public event Action<Data.AchievementData> OnAchievementCompleted;

    public void Init()
    {
        achievements = Manager.DataM.AchievementDataDic.Values.ToList();
    }

    //업적 완료 처리
    public void CompleteAchievement(int _dataID)
    {
        AchievementData achievement = achievements.Find(a => a.AchievementID == _dataID);

        if(achievement != null && !achievement.IsRewarded)
        {
            achievement.IsCompleted = true;
            Manager.GameM.SaveGame();
        }
    }


    //보상받기 완료 처리
    public void RewardedAchievement(int _dataID)
    {
        AchievementData achievement = achievements.Find(a => a.AchievementID == _dataID);
        if(achievement != null && !achievement.IsRewarded)
        {
            achievement.IsRewarded = true;
            achievement.IsCompleted = true;
            Manager.GameM.SaveGame();
        }
    }


    public List<AchievementData> GetAchievements()
    {
        List<AchievementData> achievement = new List<AchievementData>();

        foreach(Define.MissionTarget missionTarget in Enum.GetValues(typeof(Define.MissionTarget)))
        {
            List<AchievementData> list = achievements.Where(a => a.MissionTarget == missionTarget).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                if(!list[i].IsCompleted)
                {
                    achievement.Add(list[i]);
                    break;
                }
                else
                {
                    if(!list[i].IsRewarded)
                    {
                        achievement.Add(list[i]);
                        break;
                    }

                    if (i == list.Count - 1)
                        achievement.Add(list[i]);
                }
            }
        }

        return achievement;
    }

    public int GetProgressValue(Define.MissionTarget _missionTarget)
    {
        switch (_missionTarget)
        {
            case Define.MissionTarget.StageEnter:
                return Manager.GameM.MissionDic[_missionTarget].Progress;

            case Define.MissionTarget.EquipmentLevelUp:
                return Manager.GameM.MissionDic[_missionTarget].Progress;


            case Define.MissionTarget.EquipmentMerge:
                return Manager.GameM.MissionDic[_missionTarget].Progress;

            case Define.MissionTarget.OfflineRewardGet:
                return Manager.GameM.OfflineRewardGetCount;

            case Define.MissionTarget.FastOfflineRewardGet:
                return Manager.GameM.FastOfflineRewardGetCount;

            case Define.MissionTarget.MonsterKill:
                return Manager.GameM.TotalMonsterKillCount;

            case Define.MissionTarget.EliteMonsterKill:
                return Manager.GameM.TotalEliteMonsterKillCount;

            case Define.MissionTarget.BossKill:
                return Manager.GameM.TotalBossKillCount;

            case Define.MissionTarget.StageClear:
                return Manager.GameM.GetMaxStageClearIndex();

            case Define.MissionTarget.ADWatchIng:
                return Manager.GameM.MissionDic[_missionTarget].Progress;

            case Define.MissionTarget.Login:
                return Manager.TimeM.AttendanceDay;
        }
        return 0;
    }

    public AchievementData GetNextAchievement(int _dataID)
    {
        AchievementData achievement = achievements.Find(a => a.AchievementID == _dataID + 1);
        if(achievement != null && !achievement.IsRewarded)
        {
            return achievement;
        }
        return null;
    }

    //출석
    public void Attendance()
    {
        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.Login).ToList();

        foreach(AchievementData achievement in list)
        {
            if (!achievement.IsCompleted && achievement.MissionTargetValue == Manager.TimeM.AttendanceDay)
            {
                CompleteAchievement(achievement.AchievementID);
            }
        }
    }

    //스테이지 클리어
    public void StageClear()
    {
        int MaxStageClearIndex = Manager.GameM.GetMaxStageClearIndex();

        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.StageClear).ToList();
        foreach(AchievementData achievement in list)
        {
            if(!achievement.IsCompleted && achievement.MissionTargetValue == MaxStageClearIndex)
            {
                CompleteAchievement(achievement.AchievementID);
            }
        }
    }

    //일반 장비상자
    public void CommonBoxOpen()
    {
        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.CommonGachaOpen).ToList();

        foreach(AchievementData achievement in list)
        {
            if(!achievement.IsCompleted && achievement.MissionTargetValue == Manager.GameM.CommonGachaOpenCount)
            {
                CompleteAchievement(achievement.AchievementID);
            }
        }
    }


    //고급 장비상자
    public void AdvancedBoxOpen()
    {
        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.AdvancedGachaOpen).ToList();

        foreach (AchievementData achievement in list)
        {
            if (!achievement.IsCompleted && achievement.MissionTargetValue == Manager.GameM.AdvancedGachaOpenCount)
            {
                CompleteAchievement(achievement.AchievementID);
            }
        }
    }
    
    //정찰
    public void OfflineReward()
    {
        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.OfflineRewardGet).ToList();

        foreach (AchievementData achievement in list)
        {
            if (!achievement.IsCompleted && achievement.MissionTargetValue == Manager.GameM.OfflineRewardGetCount)
            {
                CompleteAchievement(achievement.AchievementID);
            }
        }
    }

    //빠른정찰
    public void FastReward()
    {
        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.FastOfflineRewardGet).ToList();

        foreach (AchievementData achievement in list)
        {
            if (!achievement.IsCompleted && achievement.MissionTargetValue == Manager.GameM.FastOfflineRewardGetCount)
            {
                CompleteAchievement(achievement.AchievementID);
            }
        }
    }

    //몬스터 처치

    public void MonsterKill()
    {
        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.MonsterKill).ToList();

        foreach (AchievementData achievement in list)
        {
            if (!achievement.IsCompleted && achievement.MissionTargetValue == Manager.GameM.TotalMonsterKillCount)
            {
                CompleteAchievement(achievement.AchievementID);
            }
        }
    }

    // 엘리트 처치
    public void EliteMonsterKill()
    {
        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.EliteMonsterKill).ToList();

        foreach (AchievementData achievement in list)
        {
            if (!achievement.IsCompleted && achievement.MissionTargetValue == Manager.GameM.TotalEliteMonsterKillCount)
            {
                CompleteAchievement(achievement.AchievementID);
            }
        }
    }

    //보스 처치

    public void BossKill()
    {
        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.BossKill).ToList();

        foreach (AchievementData achievement in list)
        {
            if (!achievement.IsCompleted && achievement.MissionTargetValue == Manager.GameM.TotalBossKillCount)
            {
                CompleteAchievement(achievement.AchievementID);
            }
        }
    }

}
