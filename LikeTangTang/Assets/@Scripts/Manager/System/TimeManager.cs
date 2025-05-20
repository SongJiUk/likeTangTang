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
            //TDOO : 업적 출석 : Manager.AchievementM.
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
    public void Init()
    {
        TimeStart();
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

        }
    }

    public void RechargetStamina(int _count = 1)
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
}
