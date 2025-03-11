using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public PlayerController player { get { return Manager.ObjectM?.Player; } }


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



}
