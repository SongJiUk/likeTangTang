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

    //TODO : Grid로 Gem관리해주기.
    public int Gem { get; set; }
    public int Gold { get; set; }
}
