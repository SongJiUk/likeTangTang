using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using Newtonsoft.Json;
using System.IO;

public class GameManager
{
    public PlayerController player { get { return Manager.ObjectM?.Player; } }
    public CameraController Camera {get; set;}
    public GameData gameData = new GameData();


    public int UserLevel
    {
        get { return gameData.userLevel;}
        set { gameData.userLevel = value; }
    }

    public string userName
    {
        get { return gameData.userName;}
        set { gameData.userName = value;}
    }

    public int Gold
    {
        get { return gameData.gold;}
        set
        {
            gameData.gold = value;
            SaveGame();
            OnResourcesChanged?.Invoke();
        }
    }

    public int Dia
    {
        get { return gameData.dia;}
        set 
        {
            gameData.dia = value;
            SaveGame();
            OnResourcesChanged?.Invoke();
        }
    }

    public ContinueData ContinueDatas
    {
        get { return gameData.ContinueDatas;}
        set { gameData.ContinueDatas = value;}
    }
    public StageData CurretnStageData
    {
        get { return gameData.CurrentStageData;}
        set { gameData.CurrentStageData = value;}
    }
    public int CurrentWaveIndex
    {
        get { return gameData.ContinueDatas.CurrentWaveIndex;}
        set { gameData.ContinueDatas.CurrentWaveIndex = value;}
    }
    
    public WaveData CurrentWaveData
    {
        get { return gameData.CurrentStageData.WaveArray[CurrentWaveIndex];}
    }



    //public Map CurrentMap {get; set;}

    #region Action
    public event Action OnResourcesChanged;
    #endregion
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

    string path;
    bool isLoaded = false;
    public void Init()
    {
        /*TODO : 
        1. 기존에 하던거 있으면 로드, 
        2. 캐릭터 선택해서 불러오기(캐릭터 여러개 만들거면)
        3. 스테이지 로드
        4. 장비 확인
        5. 초반 기본 아이템 설정
        */
        path = Application.persistentDataPath + "/SaveData.json";
        
        if(LoadGame()) return;

        
        
       

        gameData.CurrentStageData = Manager.DataM.Stagedic[1];
        foreach(Data.StageData stage in Manager.DataM.Stagedic.Values)
        {
            StageClearInfoData info = new StageClearInfoData
            {
                StageIndex = stage.StageIndex,
                MaxWaveIndex =0,
            };
            gameData.StageClearInfoDic.Add(stage.StageIndex, info);
        }

        isLoaded = true;
        SaveGame();

    }

    public bool LoadGame()
    {
        if(PlayerPrefs.GetInt("ISFIRST", 1) == 1)
        {
            string path = Application.persistentDataPath + "/SaveData.json";
            if(File.Exists(path)) File.Delete(path);
            return false;
        }

        if(File.Exists(path) == false) return false;

        string jsonStr = File.ReadAllText(path);
        GameData data = JsonConvert.DeserializeObject<GameData>(jsonStr);
        if(data != null) gameData = data;

        isLoaded = true;

        return true;
    }

    public void SaveGame()
    {
        if(player != null)
        {
            gameData.ContinueDatas.SavedBattleSkill = player.Skills?.SavedBattleSkill;
        }
        
        string jsonStr = JsonConvert.SerializeObject(gameData);
        File.WriteAllText(path, jsonStr);
    }

    public void StageDataLoad()
    {

    }

    public void SetNextStage()
    {
        CurretnStageData = Manager.DataM.Stagedic[CurretnStageData.StageIndex + 1];
    }

    public void ClearContinueData()
    {
        gameData.ContinueDatas.Clear();
        CurrentWaveIndex = 0;
        SaveGame();
    }


}

