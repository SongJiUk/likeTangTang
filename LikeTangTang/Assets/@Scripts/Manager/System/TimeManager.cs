using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float minute = 0f;
    public float second = 0f;
    public float TimeRemaining = 60f;

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
}
