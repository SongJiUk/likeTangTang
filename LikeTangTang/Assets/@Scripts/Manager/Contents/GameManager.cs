using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using static Define;



public class GameManager
{
    public PlayerController player { get { return Manager.ObjectM?.Player; } }

    public Character CurrentCharacter {get { return Characters.Find(c => c.isCurrentCharacter == true); } }
    public CameraController Camera { get; set; }
    public GameData gameData = new GameData();

    public List<Equipment> OwnedEquipment
    {
        get { return gameData.OwnedEquipments; }

        set
        {
            gameData.OwnedEquipments = value;
        }
    }

    public Dictionary<EquipmentType, Equipment> EquipedEquipments
    {
        get { return gameData.EquipedEquipments; }
        set { gameData.EquipedEquipments = value; }
    }

    public Dictionary<int, int> ItemDic
    {
        get { return gameData.ItemDictionary; }
        set
        {
            gameData.ItemDictionary = value;
        }
    }

    public List<Character> Characters
    {
        get { return gameData.Characters; }
        set
        {
            gameData.Characters = value;
        }
    }
    public int GachaCountAdsAdvanced
    {
        get { return gameData.GacahCountAdsAdvanced; }
        set { gameData.GacahCountAdsAdvanced = value; }
    }

    public int GachaCountAdsCommon
    {
        get { return gameData.GacahCountAdsCommon; }
        set { gameData.GacahCountAdsCommon = value; }
    }

    public int GoldCountAds
    { 
        get { return gameData.GoldCountAds; }
        set { gameData.GoldCountAds = value; }
    }

    public int SilverKeyCountAds
    { 
        get { return gameData.SilverKeyCountAds; }
        set { gameData.SilverKeyCountAds = value; }
    }

    public int DiaCountAds
    {
        get { return gameData.DiaCountAds; }
        set { gameData.DiaCountAds = value; }
    }

    public int StaminaCountAds
    {
        get { return gameData.StaminaCountAds; }
        set { gameData.StaminaCountAds = value; }
    }

    public int RemainBuyStaminaForDia
    {
        get { return gameData.RemainBuyStaminaForDia; }
        set { gameData.RemainBuyStaminaForDia = value; }
    }
    public int UserLevel
    {
        get { return gameData.userLevel; }
        set { gameData.userLevel = value; }
    }

    public string userName
    {
        get { return gameData.userName; }
        set { gameData.userName = value; }
    }

    public int Gold
    {
        get { return gameData.gold; }
        set
        {
            gameData.gold = value;
            SaveGame();
            OnResourcesChanged?.Invoke();
        }
    }

    public int Dia
    {
        get { return gameData.dia; }
        set
        {
            gameData.dia = value;
            SaveGame();
            OnResourcesChanged?.Invoke();
        }
    }

    public int Stamina
    { 
        get 
        {
            if (gameData.stamina > MAX_STAMINA) gameData.stamina = MAX_STAMINA;
            return gameData.stamina;
        }
        set
        {
            gameData.stamina = value;
            if (gameData.stamina > MAX_STAMINA) gameData.stamina = MAX_STAMINA;
            SaveGame();
            OnResourcesChanged?.Invoke();
        }
    }

    public ContinueData ContinueDatas
    {
        get { return gameData.ContinueDatas; }
        set { gameData.ContinueDatas = value; }
    }
    public StageData CurrentStageData
    {
        get { return gameData.CurrentStageData; }
        set { gameData.CurrentStageData = value; }
    }
    public int CurrentWaveIndex
    {
        get { return gameData.ContinueDatas.CurrentWaveIndex; }
        set { gameData.ContinueDatas.CurrentWaveIndex = value; }
    }

    public WaveData CurrentWaveData
    {
        get { return CurrentStageData.WaveArray[CurrentWaveIndex]; }
    }

    public Dictionary<int, StageClearInfoData> StageClearInfoDic
    { 
        get { return gameData.StageClearInfoDic; }
        set
        {
            gameData.StageClearInfoDic = value;
            //Manager.Achivement.StageClear();
            SaveGame();
        }
    }



    public Map CurrentMap { get; set; }

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
    public bool isLoaded = false;
    public bool isGameEnd = false;

    public int minute;
    public int second;
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
        if (LoadGame()) return;

        PlayerPrefs.SetInt("ISFIRST", 1);
        Character character = new Character();
        character.SetInfo(Define.DEFAULT_PLAYER_ID);
        character.isCurrentCharacter = true;

        Characters = new List<Character>();
        Characters.Add(character);

        CurrentStageData = Manager.DataM.StageDic[1];
        foreach (Data.StageData stage in Manager.DataM.StageDic.Values)
        {
            StageClearInfoData info = new StageClearInfoData
            {
                StageIndex = stage.StageIndex,
                MaxWaveIndex = 0,
            };
            gameData.StageClearInfoDic.Add(stage.StageIndex, info);
        }

        //초기 선물
        FirstGift();

        isLoaded = true;
        SaveGame();
    }

    public void ExchangeMaterial(MaterialData _data, int _count)
    {
        switch(_data.MaterialType)
        {
            case MaterialType.Dia:
                Dia += _count;
                break;

            case MaterialType.Gold:
                Gold += _count;
                break;

            case MaterialType.Stamina:
                Stamina += _count;
                if (Stamina > MAX_STAMINA) Stamina = MAX_STAMINA;
                break;
            case MaterialType.BronzeKey:
            case MaterialType.SilverKey:
            case MaterialType.GoldKey:
                    AddMaterialItem(_data.MaterialID, _count);
                break;

            case MaterialType.RandomScroll:
                int randScroll = UnityEngine.Random.Range(Define.ID_WeaponScroll, Define.ID_BootsScroll);
                AddMaterialItem(randScroll, _count);
                break;

            case MaterialType.WeaponScroll:
            case MaterialType.GloveScroll:
            case MaterialType.RingScroll:
            case MaterialType.HelmetScroll:
            case MaterialType.ArmorScroll:
            case MaterialType.BootsScroll:
                AddMaterialItem(_data.MaterialID, _count);
                break;
        }
    }
    public void FirstGift()
    {
        ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_SILVER_KEY], 10);
        ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_GOLD_KEY], 30);
        ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_DIA], 1000);
        ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_GOLD], 10);
        ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_WeaponScroll], 10);
        ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_GloveScroll], 10);
        ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_RingScroll], 10);
        ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_HelmetScroll], 10);
        ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_ArmorScroll], 10);
        ExchangeMaterial(Manager.DataM.MaterialDic[Define.ID_BootsScroll], 10);
    }


    public bool LoadGame()
    {
        //if (PlayerPrefs.GetInt("ISFIRST", 1)    == 1)
        //{
        //    string _path = Application.persistentDataPath + "/SaveData.json";
        //    if (File.Exists(_path)) 
        //        File.Delete(_path);
        //    return false;
        //}

        if (File.Exists(path) == false) return false;

        string jsonStr = File.ReadAllText(path);
        GameData data = JsonConvert.DeserializeObject<GameData>(jsonStr);
        data.Init();

        if (data != null) gameData = data;

        //가진게 있다면 벗기고, 다시 입힘
        EquipedEquipments = new Dictionary<EquipmentType, Equipment>();
        for(int i =0; i< OwnedEquipment.Count; i++)
        {
            if(OwnedEquipment[i].IsEquiped)
            {
                EquipItem(OwnedEquipment[i].EquipmentData.EquipmentType, OwnedEquipment[i]);
            }
        }

        isLoaded = true;

        return true;
    }

    public void SaveGame()
    {
        if (player != null)
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
        CurrentStageData = Manager.DataM.StageDic[CurrentStageData.StageIndex + 1];
    }

    public int GetMaxStageIndex()
    {
        foreach(StageClearInfoData clearInfo in StageClearInfoDic.Values)
        {
            if (clearInfo.MaxWaveIndex != 10) return clearInfo.StageIndex;
        }

        return 0;
    }

    public void ClearContinueData()
    {
        gameData.ContinueDatas.Clear();
        CurrentWaveIndex = 0;
        SaveGame();
    }

    public GemInfo GetGemInfo()
    {
        float randNum = UnityEngine.Random.value;
        (GemInfo.GemType type, float chace, Vector3 scale)[] gems = new (GemInfo.GemType type, float chace, Vector3 scale)[]
        {
            (GemInfo.GemType.Red, CurrentWaveData.SmallGemDropRate, new Vector3(0.5f, 0.5f, 0.5f)),
            (GemInfo.GemType.Green, CurrentWaveData.GreenGemDropRate, Vector3.one),
            (GemInfo.GemType.Blue, CurrentWaveData.BlueGemDropRate, Vector3.one),
            (GemInfo.GemType.Gold, CurrentWaveData.YellowGemDropRate, Vector3.one)
        };

        float cumulative = 0f;
        foreach (var gem in gems)
        {
            cumulative += gem.chace;
            if (randNum < cumulative)
                return new GemInfo(gem.type, gem.scale);
        }

        return null;
    }

    public GemInfo GetGemInfo(GemInfo.GemType _type)
    {
        if (_type == GemInfo.GemType.Red)
            return new GemInfo(GemInfo.GemType.Red, new Vector3(0.5f, 0.5f, 0.5f));

        return new GemInfo(_type, Vector3.one);
    }

    public void EquipItem(EquipmentType _type, Equipment _equipment)
    {
        if(EquipedEquipments.ContainsKey(_type))
        {
            EquipedEquipments[_type].IsEquiped = false;
            EquipedEquipments.Remove(_type);
        }

        EquipedEquipments.Add(_type, _equipment);
        _equipment.IsEquiped = true;
        _equipment.IsConfirmed = true;
    }

    public void UnEquipItem(Equipment _equipment)
    {
        if(EquipedEquipments.ContainsKey(_equipment.EquipmentData.EquipmentType))
        {
            EquipedEquipments[_equipment.EquipmentData.EquipmentType].IsEquiped = false;
            EquipedEquipments.Remove(_equipment.EquipmentData.EquipmentType);
        }
    }

    public void SortEquipment(EquipmentSortType _sortType, Equipment _equipment = null)
    {

        if (_sortType == EquipmentSortType.Grade)
        {
            OwnedEquipment = OwnedEquipment.
                OrderByDescending(item => item.IsEquiped).
                ThenByDescending(item => _equipment != null && item.EquipmentData.EquipmentType == _equipment.EquipmentData.EquipmentType).
                ThenByDescending(item => item.EquipmentData.EquipmentGarde).
                ThenByDescending(item => item.Level).
                ThenByDescending(item => item.EquipmentData.DataID).
                ThenBy(item => item.EquipmentData.EquipmentType).
                ToList();

        }
        else if (_sortType == EquipmentSortType.Level)
        {
            OwnedEquipment = OwnedEquipment.
                OrderByDescending(item => item.IsEquiped).
                ThenByDescending(item => _equipment != null && item.EquipmentData.EquipmentType == _equipment.EquipmentData.EquipmentType).
                ThenByDescending(item => item.Level).
                ThenByDescending(item => item.EquipmentData.EquipmentGarde).
                ThenByDescending(item => item.EquipmentData.DataID).
                ThenBy(item => item.EquipmentData.EquipmentType).
                ToList();
        }
    }


    public (float hp, float attack) GetCurrentCharacterStat()
    {
        float hpBonus = 0;
        float attackBonus = 0;

        var (equipHpBonus, equipAttackBonus) = GetEquipmentBonus();

        hpBonus = (equipHpBonus);
        attackBonus = (equipAttackBonus);

        return (hpBonus, attackBonus);
    }

    public (float hp, float atk) GetEquipmentBonus()
    {
        float hpBonus = 0;
        float atkBonus = 0;

        foreach (KeyValuePair<EquipmentType, Equipment> pair in EquipedEquipments)
        {
            hpBonus += pair.Value.MaxHpBonus;
            atkBonus += pair.Value.AttackBonus;
        }
        return (hpBonus, atkBonus);
    }

    public List<Equipment> DoGaCha(GachaType gachaType, int _count = 1)
    {
        List<Equipment> ret = new List<Equipment>();

        for (int i = 0; i < _count; i++)
        {
            EquipmentGrade grade = GetRandomGrade(Define.COMMON_GACHA_GRADE_PROB);

            switch (gachaType)
            {
                case GachaType.CommonGacha:
                    grade = GetRandomGrade(Define.COMMON_GACHA_GRADE_PROB);
                    //TODO : 업적 넣을거면 업적
                    break;

                case GachaType.AdvancedGacha:
                    grade = GetRandomGrade(Define.ADVENCED_GACHA_GRADE_PROB);
                    break;
            }


            List<GachaRateData> list = Manager.DataM.GachaTableDataDic[gachaType].GachaRateTable.Where(item => item.EquipGrade == grade).ToList();
            int index = UnityEngine.Random.Range(0, list.Count);
            string key = list[index].EquipmentID;

            if(Manager.DataM.EquipmentDic.ContainsKey(key))
            {
                ret.Add(AddEquipment(key));
            }
        }

        return ret;
    }



    public Equipment AddEquipment(string _key)
    {
        if (_key.Equals("None")) return null;
        Equipment equip = new Equipment();
        equip.Init(_key);
        equip.IsConfirmed = false;
        OwnedEquipment.Add(equip);

        //TODO : EquipmnetInfo Change

        return equip;
    }

    public Equipment MergeEquipment(Equipment _equipment, Equipment _mergeEquipment1, Equipment _mergeEquipment2, bool _isAllMerge = false)
    {
        _equipment = OwnedEquipment.Find(equip => equip == _equipment);
        if(_equipment == null) return null;

        _mergeEquipment1 = OwnedEquipment.Find(equip => equip == _mergeEquipment1);
        if (_mergeEquipment1 == null) return null;

        if(_mergeEquipment2 != null)
        {
            _mergeEquipment2 = OwnedEquipment.Find(equip => equip == _mergeEquipment2);

            if (_mergeEquipment2 == null) return null;
        }

        int level = _equipment.Level;
        bool isEquiped = _equipment.IsEquiped;
        string mergeItemCode = _equipment.EquipmentData.MergeItemCode;
        Equipment newEquipment = AddEquipment(mergeItemCode);
        newEquipment.Level = level;
        newEquipment.IsEquiped = isEquiped;

        OwnedEquipment.Remove(_equipment);
        OwnedEquipment.Remove(_mergeEquipment1);
        OwnedEquipment.Remove(_mergeEquipment2);

        if (!_isAllMerge) SaveGame();

        return newEquipment;

    }
    public EquipmentGrade GetRandomGrade(float[] _prob)
    {
        float randomValue = UnityEngine.Random.value;
        float sum = 0;

        for(int i =0; i<=(int)EquipmentGrade.Epic; i++)
        {
            sum += _prob[i];
            if (randomValue < sum)
                return (EquipmentGrade)i;
        }

        return EquipmentGrade.Common;
    }


    public void AddMaterialItem(int _id, int _count)
    {
        if (ItemDic.ContainsKey(_id))
            ItemDic[_id] += _count;
        else
            ItemDic[_id] = _count;

        SaveGame();
    }

    public void RemoveMaterialItem(int _id, int _count)
    {
        if (ItemDic.ContainsKey(_id))
        {
            ItemDic[_id] -= _count;
            SaveGame();
        }
            
    }
}

