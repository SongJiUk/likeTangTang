using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class GameManager
{
    public PlayerController player { get { return Manager.ObjectM?.Player; } }
    
    public GameData gameData = new GameData();
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

    public void Init()
    {
        /*TODO : 
        1. 기존에 하던거 있으면 로드, 
        2. 캐릭터 선택해서 불러오기(캐릭터 여러개 만들거면)
        3. 스테이지 로드
        4. 장비 확인
        5. 초반 기본 아이템 설정

        */
    }

    public void LoadGame()
    {

    }

    public void SaveGame()
    {

    }

    public void StageDataLoad()
    {

    }


}

