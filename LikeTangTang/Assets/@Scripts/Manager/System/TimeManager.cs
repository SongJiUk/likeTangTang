using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float minute = 0f;
    public float second = 0f;
    public float TimeRemaining = 60f;

    public void Init()
    {
        TimeStart();
    }

    public void TimeStart()
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
            second++;

            if (second >= 60f)
            {
                minute++;
                second = 0f;
            }

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
}
