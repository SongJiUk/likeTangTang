using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float minute = 0f;
    public float second = 0f;
    public float TimeRemaining = 60f;

    public int AttendanceDay
    {
        get
        {
            int savedTime = PlayerPrefs.GetInt("AttendanceDay", 1);
            return savedTime;
        }
        set
        {
            PlayerPrefs.SetInt("AttendanceDay", value);
            Manager.AchievementM.Attendance();
            PlayerPrefs.Save();
        }
    }

    public DateTime LastLoginTime
    {
        get
        {
            string savedTimeStr = PlayerPrefs.GetString("LastLoginTime", string.Empty);
            if (!string.IsNullOrEmpty(savedTimeStr))
            {
                return DateTime.Parse(savedTimeStr);
            }
            else
                return DateTime.Now;
        }

        set
        {

            string savedTimeStr = value.ToString();
            PlayerPrefs.SetString("LastLoginTime", savedTimeStr);
            PlayerPrefs.Save();

        }
    }
    DateTime lastRewardTime;
    public DateTime LastRewardTime
    {
        get
        {
            if(lastRewardTime == default(DateTime))
            {
                string savedTimeStr = PlayerPrefs.GetString("LastRewardTime", string.Empty);
                if (!string.IsNullOrEmpty(savedTimeStr))
                    lastRewardTime = DateTime.Parse(savedTimeStr);
                else
                    lastRewardTime = DateTime.Now;
            }

            return lastRewardTime;
        }

        set
        {
            lastRewardTime = value;
            string timeStr = value.ToString();
            PlayerPrefs.SetString("LastRewardTime", timeStr);
            PlayerPrefs.Save();
        }
    }

    public TimeSpan TimeSinceLastReward
    {
        get
        {
            TimeSpan timeSpan = DateTime.Now - LastRewardTime;
            if (timeSpan > TimeSpan.FromHours(24))
                return TimeSpan.FromHours(24);

            return timeSpan;
        }
    }

    public float StaminaTime
    {
        get
        {
            float time = PlayerPrefs.GetFloat("StaminaTime", Define.STAMINA_RECHARGET_INTERVAL);
            return time;
        }

        set
        {
            float time = value;
            PlayerPrefs.SetFloat("StaminaTime", time);
            PlayerPrefs.Save();
        }
    }

    public DateTime LastGeneratedStaminaTime
    {
        get
        {
            string savedTime = PlayerPrefs.GetString("LastGeneratedStaminaTime", string.Empty);
            if (!string.IsNullOrEmpty(savedTime))
                return DateTime.Parse(savedTime);
            else
                return DateTime.Now;
        }
        set
        {
            string timeStr = value.ToString();
            PlayerPrefs.SetString("LastGeneratedStaminaTime", timeStr);
            PlayerPrefs.Save();
        }
    }

    public DateTime LastAttendanceResetDate
    {
        get
        {
            string savedTime = PlayerPrefs.GetString("LastAttendanceResetDate", string.Empty);
            if (!string.IsNullOrEmpty(savedTime))
                return DateTime.Parse(savedTime);
            else
                return DateTime.Now;
        }

        set
        {
            string timeStr = value.ToString();
            PlayerPrefs.SetString("LastAttendanceResetDate", timeStr);
            PlayerPrefs.Save();
        }
    }

    public void GiveOfflioneReward(Data.OfflineRewardData _offlineRewardData)
    {
        Queue<string> name = new();
        Queue<int> count = new();
        int gold = (int)CalculateGoldPerMinute(_offlineRewardData.Reward_Gold);

        name.Enqueue(Manager.DataM.MaterialDic[Define.ID_GOLD].SpriteName);
        count.Enqueue(gold);

        Manager.GameM.Gold += gold;
        LastRewardTime = DateTime.Now;
        if (Manager.GameM.MissionDic.TryGetValue(Define.MissionTarget.OfflineRewardGet, out MissionInfo info)) info.Progress++;
        Manager.GameM.OfflineRewardGetCount++;

        UI_RewardPopup popup = (Manager.UiM.SceneUI as UI_LobbyScene).Ui_RewardPopup;
        popup.gameObject.SetActive(true);
        popup.SetInfo(name, count);
    }

    public void GiveFastOfflioneReward(Data.OfflineRewardData _offlineRewardData)
    {
        Queue<string> name = new();
        Queue<int> count = new();
        int gold = _offlineRewardData.Reward_Gold * 5;

        name.Enqueue(Manager.DataM.MaterialDic[Define.ID_GOLD].SpriteName);
        count.Enqueue(gold);
        name.Enqueue(Manager.DataM.MaterialDic[Define.ID_RandomScroll].SpriteName);
        count.Enqueue(_offlineRewardData.FastReward_Scroll);
        name.Enqueue(Manager.DataM.MaterialDic[Define.ID_SILVER_KEY].SpriteName);
        count.Enqueue(_offlineRewardData.FastReward_Scroll);

        if (Manager.GameM.MissionDic.TryGetValue(Define.MissionTarget.FastOfflineRewardGet, out MissionInfo info)) info.Progress++;
        Manager.GameM.FastOfflineRewardGetCount++;

        UI_RewardPopup popup = (Manager.UiM.SceneUI as UI_LobbyScene).Ui_RewardPopup;
        popup.gameObject.SetActive(true);

        Manager.GameM.ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_GOLD], gold);
        Manager.GameM.ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_RandomScroll], _offlineRewardData.FastReward_Scroll);
        Manager.GameM.ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_SILVER_KEY], _offlineRewardData.FastReward_Scroll);

        popup.SetInfo(name, count);
    }


    public void Init()
    {
        TimeStart();
        Manager.AchievementM.Attendance();
    }

    void TimeStart()
    {
        StartCoroutine(CoStartTimer());
    }
    public void TimeReset()
    {
        minute = 0f;
        second = 0f;
    }

    IEnumerator CoStartTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            StaminaTime--;
            second++;

            TimeSpan timeSpan = TimeSpan.FromSeconds(StaminaTime);

            if(StaminaTime == 0)
            {
                RechargetStamina();
                StaminaTime = Define.STAMINA_RECHARGET_INTERVAL;
            }


            if (second >= 60f)
            {
                minute++;
                second = 0f;
            }

            if(minute == 0)
            {
                CheckAttendance();
                minute = 60;
            }

        }
    }

    public void RechargetStamina(int _count = 5)
    {
        if(Manager.GameM.Stamina < Define.MAX_STAMINA)
        {
            Manager.GameM.Stamina += _count;
            LastGeneratedStaminaTime = DateTime.Now;
        }
    }

    public void CheckAttendance()
    {
        if (!IsSameDay(LastLoginTime, DateTime.Now))
        {
            AttendanceDay++;
            LastLoginTime = DateTime.Now;

            Manager.GameM.GachaCountAdsAdvanced = 1;
            Manager.GameM.GachaCountAdsCommon = 1;
            Manager.GameM.GoldCountAds = 1;
            Manager.GameM.SilverKeyCountAds = 3;
            Manager.GameM.DiaCountAds = 3;
            Manager.GameM.StaminaCountAds = 2;
            Manager.GameM.RemainBuyStaminaForDia = 3;

            //TODO : 미션정보, 빠른모험 등등
            Manager.GameM.MissionDic.Clear();
            Manager.GameM.MissionDic = new Dictionary<Define.MissionTarget, MissionInfo>()
            {
                {Define.MissionTarget.StageEnter, new MissionInfo() { Progress = 0, isRewarded = false} },
                {Define.MissionTarget.EquipmentLevelUp, new MissionInfo() { Progress = 0, isRewarded = false} },
                {Define.MissionTarget.EquipmentMerge, new MissionInfo() { Progress = 0, isRewarded = false} },
                {Define.MissionTarget.GachaOpen, new MissionInfo() { Progress = 0, isRewarded = false} },
                {Define.MissionTarget.OfflineRewardGet, new MissionInfo() { Progress = 0, isRewarded = false} },
                {Define.MissionTarget.MonsterKill, new MissionInfo() { Progress = 0, isRewarded = false} },
                {Define.MissionTarget.EliteMonsterKill, new MissionInfo() { Progress = 0, isRewarded = false} },
                {Define.MissionTarget.StageClear, new MissionInfo() { Progress = 0, isRewarded = false} },
                {Define.MissionTarget.ADWatchIng, new MissionInfo() { Progress = 0, isRewarded = false} }
            };

            Manager.GameM.SaveGame();
        }
    }

    public int GetCurrentMinute()
    {
        return (int)minute;
    }

    public int GetCurrentSecond()
    {
        return (int)second;
    }

    public void TimeStop()
    {
        Time.timeScale = 0f;
    }

    public void TimeReStart()
    {
        Time.timeScale = 1f;
    }

    public bool IsSameDay(DateTime _savedTime, DateTime _currentTime)
    {
        if (LastLoginTime.Day == DateTime.Now.Day)
            return true;
        else
            return false;
    }

    public float CalculateGoldPerMinute(float _goldPerHour)
    {
        float goldPerMinute = _goldPerHour / 60f * (int)TimeSinceLastReward.TotalMinutes;
        return goldPerMinute;
    }
}
