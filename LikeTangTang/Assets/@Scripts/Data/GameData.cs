using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int userLevel = 1;
    public string userName = "Player";
    public int gold = 0;
    public int dia = 0;

    // TODO : 모든 정보가 다 들어간다고 생각하면 됌. 
    public ContinueData continueData= new ContinueData();


}
