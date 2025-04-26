using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float minute = 60f;
    public float TimeRemaining = 60f;

    public void Init()
    {

    }

    public void TimeStart()
    {
        
    }

    IEnumerator CoStartTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            minute--; 
        }

        if(minute == 0)
        {
            
            minute = 60f;
        }
    }

}
