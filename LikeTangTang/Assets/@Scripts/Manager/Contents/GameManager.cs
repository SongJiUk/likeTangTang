using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public PlayerController player { get { return Manager.ObjectM?.Player; } }
    


    #region 플레이어 움직임

    Vector2 playerMoveDir;

    public event Action<Vector2> OnMovePlayerDir;
    public Vector2 PlayerMoveDir
    {
        get { return playerMoveDir; }
        set
        {
            playerMoveDir = value;
            OnMovePlayerDir?.Invoke(playerMoveDir);
        }
    }
    #endregion

    #region 재화
    public int Gold { get; set; }
    int gemCount;
    public event Action<int> OnChangeGemCount;
    public int Gem {
        get {return gemCount;}
        set
        {
            gemCount = value;
            OnChangeGemCount?.Invoke(value);
        }
    }
   
    #endregion
    
    #region  전투
    int killCount;
    public event Action<int> OnChangeKillCount;
    public int KillCount
    {
        get {return killCount;}
        set
        {
            killCount = value;
            OnChangeKillCount?.Invoke(killCount);
        }
    }

    #endregion

    //TODO : 플레이어의 레벨을 어디서 관리해줘야할까?
}
