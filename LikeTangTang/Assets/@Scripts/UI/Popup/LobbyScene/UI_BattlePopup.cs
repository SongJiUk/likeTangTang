using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_BattlePopup : UI_Popup
{

    public enum GameObjects
    {
        ContentObject,
        SettingButtonRedDotObject,
        AttendanceCheckButtonRedDotObject,
        MissionButtonRedDotObject,
        AchievementButtonRedDotObject,
        OfflineRewardButtonRedDotObject,
        FirstClearRedDotObject,
        FirstClearRewardUnlockObject,
        FirstClearRewardCompleteObject,
        SecondClearRedDotObject,
        SecondClearRewardUnlockObject,
        SecondClearRewardCompleteObject,
        ThirdClearRedDotObject,
        ThirdClearRewardUnlockObject,
        ThirdClearRewardCompleteObject
    }

    public enum Buttons
    {
        SettingButton,
        AttendanceCheckButton,
        MissionButton,
        AchievementButton,
        StageSelectButton,
        FirstClearRewardButton,
        SecondClearRewardButton,
        ThirdClearRewardButton,
        GameStartButton,
        OfflineRewardButton
    }

    public enum Texts
    {
        StageNameText,
        SurvivalWaveText,
        SurvivalWaveValueText
    }

    public enum Images
    {
        StageImage,
        FirstClearRewardItemImage,
        SecondClearRewardItemImage,
        ThirdClearRewardItemImage
    }

    public enum Sliders
    {
        StageRewardProgressFillArea
    }

    enum RewardBoxState
    {
        Lock,
        UnLock,
        Complete,
        RedDot
    }
    public bool isOpen = false;
    
    Data.StageData currentStageData;
    Action OnChangedStageInfo;
    class RewardBox
    {
        public GameObject ItemImage;
        public GameObject UnLockObject;
        public GameObject CompleteObject;
        public GameObject RedDotObject;
        public RewardBoxState state;
    }

    List<RewardBox> boxes = new List<RewardBox>();

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        StartCoroutine(CoCheckPopup());
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.ContentObject));

    }

    public override bool Init()
    {
        if (!base.Init()) return false;

        gameObjectsType = typeof(GameObjects);
        ButtonsType = typeof(Buttons);
        TextsType = typeof(Texts);
        ImagesType = typeof(Images);
        SlidersType = typeof(Sliders);

        BindObject(gameObjectsType);
        BindButton(ButtonsType);
        BindText(TextsType);
        BindImage(ImagesType);
        BindSlider(SlidersType);

        GetObject(gameObjectsType, (int)GameObjects.AchievementButtonRedDotObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.AttendanceCheckButtonRedDotObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.MissionButtonRedDotObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.OfflineRewardButtonRedDotObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.SettingButtonRedDotObject).SetActive(false);


        GetButton(ButtonsType, (int)Buttons.SettingButton).gameObject.BindEvent(OnClickSettingButton);
        GetButton(ButtonsType, (int)Buttons.AttendanceCheckButton).gameObject.BindEvent(OnClickAttendanceCheckButton);
        GetButton(ButtonsType, (int)Buttons.MissionButton).gameObject.BindEvent(OnClickMissionButton);
        GetButton(ButtonsType, (int)Buttons.AchievementButton).gameObject.BindEvent(OnClickAchievementButton);
        GetButton(ButtonsType, (int)Buttons.StageSelectButton).gameObject.BindEvent(OnClickStageSelectButton);
        GetButton(ButtonsType, (int)Buttons.FirstClearRewardButton).gameObject.BindEvent(OnClickFirstClearRewardButton);
        GetButton(ButtonsType, (int)Buttons.SecondClearRewardButton).gameObject.BindEvent(OnClickSecondClearRewardButton);
        GetButton(ButtonsType, (int)Buttons.ThirdClearRewardButton).gameObject.BindEvent(OnClickThirdClearRewardButton);
        GetButton(ButtonsType, (int)Buttons.GameStartButton).gameObject.BindEvent(OnClickGameStartButton);
        GetButton(ButtonsType, (int)Buttons.OfflineRewardButton).gameObject.BindEvent(OnClickOfflineRewardButton);

        Manager.GameM.RefreshUI = RefreshUpsideGroup;
        OnChangedStageInfo = Refresh;
        InitBoxes();
        Refresh();
        return true;
    }

    void InitBoxes()
    {
        RewardBox box1 = new RewardBox
        {
            ItemImage = GetImage(ImagesType, (int)Images.FirstClearRewardItemImage).gameObject,
            UnLockObject = GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardUnlockObject),
            CompleteObject = GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardCompleteObject),
            RedDotObject = GetObject(gameObjectsType, (int)GameObjects.FirstClearRedDotObject),
        };
        boxes.Add(box1);

        RewardBox box2 = new RewardBox
        {
            ItemImage = GetImage(ImagesType, (int)Images.SecondClearRewardItemImage).gameObject,
            UnLockObject = GetObject(gameObjectsType, (int)GameObjects.SecondClearRewardUnlockObject),
            CompleteObject = GetObject(gameObjectsType, (int)GameObjects.SecondClearRewardCompleteObject),
            RedDotObject = GetObject(gameObjectsType, (int)GameObjects.SecondClearRedDotObject),
        };
        boxes.Add(box2);

        RewardBox box3 = new RewardBox
        {
            ItemImage = GetImage(ImagesType, (int)Images.ThirdClearRewardItemImage).gameObject,
            UnLockObject = GetObject(gameObjectsType, (int)GameObjects.ThirdClearRewardUnlockObject),
            CompleteObject = GetObject(gameObjectsType, (int)GameObjects.ThirdClearRewardCompleteObject),
            RedDotObject = GetObject(gameObjectsType, (int)GameObjects.ThirdClearRedDotObject),
        };
        boxes.Add(box3);

        for (int i = 0; i < boxes.Count; i++)
        {
            boxes[i].UnLockObject.SetActive(true);
            boxes[i].CompleteObject.SetActive(false);
            boxes[i].RedDotObject.SetActive(false);
        }
    }



    void Refresh()
    {
        if (Manager.GameM.CurrentStageData == null)
            Manager.GameM.CurrentStageData = Manager.DataM.StageDic[1];

        GetText(TextsType, (int)Texts.StageNameText).text = Manager.GameM.CurrentStageData.StageName;
        
        RefreshUpsideGroup();


        if (Manager.GameM.StageClearInfoDic.TryGetValue(Manager.GameM.CurrentStageData.StageIndex, out StageClearInfoData info))
        {
            if (info.MaxWaveIndex == 0)
                GetText(TextsType, (int)Texts.SurvivalWaveValueText).text = "기록 없음";
            else
                GetText(TextsType, (int)Texts.SurvivalWaveValueText).text = (info.MaxWaveIndex + 1).ToString();
        }
        else
            GetText(TextsType, (int)Texts.SurvivalWaveValueText).text = "기록 없음";

        GetImage(ImagesType, (int)Images.StageImage).sprite = Manager.ResourceM.Load<Sprite>(Manager.GameM.CurrentStageData.StageImage);

       

        if (info != null)
        {
            currentStageData = Manager.GameM.CurrentStageData;
            int itemcode = currentStageData.FirstWaveClearRewardItemID;

            InitBoxes();
            SetRewardBox(info);

            int wave = info.MaxWaveIndex;
            if (info.isClear)
            {
                GetText(TextsType, (int)Texts.SurvivalWaveText).gameObject.SetActive(false);
                GetText(TextsType, (int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                GetText(TextsType, (int)Texts.SurvivalWaveValueText).text = "스테이지 클리어";
                GetSlider(SlidersType, (int)Sliders.StageRewardProgressFillArea).value = wave + 1;
            }
            else
            {
                if(info.MaxWaveIndex == 0)
                {
                    GetText(TextsType, (int)Texts.SurvivalWaveText).gameObject.SetActive(false);
                    GetText(TextsType, (int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                    GetText(TextsType, (int)Texts.SurvivalWaveValueText).text = "기록없음";
                    GetSlider(SlidersType, (int)Sliders.StageRewardProgressFillArea).value = wave;
                }
                else
                {
                    GetText(TextsType, (int)Texts.SurvivalWaveText).gameObject.SetActive(true);
                    GetText(TextsType, (int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                    GetText(TextsType, (int)Texts.SurvivalWaveValueText).text = (info.MaxWaveIndex + 1).ToString();
                    GetSlider(SlidersType, (int)Sliders.StageRewardProgressFillArea).value = wave + 1;
                }
            }
        }
    }

    void SetRewardBox(StageClearInfoData _info)
    {
        int wave = _info.MaxWaveIndex + 1;
        if (wave < 3)
        {
            InitBoxes();
        }
        else if (wave < 6)
        {
            if (_info.isOpenFirstBox)
                SetBoxState(0, RewardBoxState.Complete);
            else
                SetBoxState(0, RewardBoxState.RedDot);
        }
        else if (wave < 10)
        {
            if (_info.isOpenFirstBox)
                SetBoxState(0, RewardBoxState.Complete);
            else
                SetBoxState(0, RewardBoxState.RedDot);

            if (_info.isOpenSecondBox)
                SetBoxState(1, RewardBoxState.Complete);
            else
                SetBoxState(1, RewardBoxState.RedDot);
        }
        else
        {
            if (_info.isOpenFirstBox)
                SetBoxState(0, RewardBoxState.Complete);
            else
                SetBoxState(0, RewardBoxState.RedDot);

            if (_info.isOpenSecondBox)
                SetBoxState(1, RewardBoxState.Complete);
            else
                SetBoxState(1, RewardBoxState.RedDot);

            if (_info.isOpenThirdBox)
                SetBoxState(2, RewardBoxState.Complete);
            else
                SetBoxState(2, RewardBoxState.RedDot);
        }

    }

    void SetBoxState(int _index, RewardBoxState _state)
    {
        boxes[_index].UnLockObject.SetActive(false);
        boxes[_index].CompleteObject.SetActive(false);
        boxes[_index].RedDotObject.SetActive(false);
        boxes[_index].state = _state;

        switch (_state)
        {
            case RewardBoxState.Lock:
                boxes[_index].UnLockObject.SetActive(true);
                break;
            case RewardBoxState.UnLock:
                boxes[_index].UnLockObject.SetActive(false);
                break;
            case RewardBoxState.Complete:
                boxes[_index].CompleteObject.SetActive(true);
                break;
            case RewardBoxState.RedDot:
                boxes[_index].RedDotObject.SetActive(true);
                break;
        }
    }
    
    public void RefreshUpsideGroup()
    {
        if (SceneManager.GetActiveScene().name != Define.SceneType.LobbyScene.ToString()) return;

        if (Manager.GameM.IsMissionPossibleAcceptItem)
            GetObject(gameObjectsType, (int)GameObjects.MissionButtonRedDotObject).SetActive(true);
        else
            GetObject(gameObjectsType, (int)GameObjects.MissionButtonRedDotObject).SetActive(false);

        if (Manager.GameM.IsAchievementAcceptItem)
            GetObject(gameObjectsType, (int)GameObjects.AchievementButtonRedDotObject).SetActive(true);
        else
            GetObject(gameObjectsType, (int)GameObjects.AchievementButtonRedDotObject).SetActive(false);

        if (Manager.GameM.AttendanceReceived[Manager.TimeM.AttendanceDay - 1])
            GetObject(gameObjectsType, (int)GameObjects.AttendanceCheckButtonRedDotObject).SetActive(false);
        else
            GetObject(gameObjectsType, (int)GameObjects.AttendanceCheckButtonRedDotObject).SetActive(true);
    }


    IEnumerator CoCheckPopup()
    {
        yield return new WaitForEndOfFrame();
        if (PlayerPrefs.GetInt("ISFIRST") == 1)
        {
            Manager.UiM.ShowPopup<UI_BeginnerSupportRewardPopup>();
            PlayerPrefs.SetInt("ISFIRST", 0);
            PlayerPrefs.Save();
        }

        if (Manager.GameM.ContinueDatas.isContinue)
            Manager.UiM.ShowPopup<UI_BackToBattlePopup>();
        else
            Manager.GameM.ClearContinueData();
    }


    void OnClickSettingButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.UiM.ShowPopup<UI_SettingPopup>();
    }

    void OnClickAttendanceCheckButton()
    {
        Manager.SoundM.PlayButtonClick();
        UI_CheckOutPopup popup = Manager.UiM.ShowPopup<UI_CheckOutPopup>();
        popup.SetInfo(Manager.TimeM.AttendanceDay);
    }

    void OnClickMissionButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.UiM.ShowPopup<UI_MissionPopup>();
    }

    void OnClickAchievementButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.UiM.ShowPopup<UI_AchievementPopup>();
    }

    void OnClickStageSelectButton()
    {
        Manager.SoundM.PlayButtonClick();

        UI_StageSelectPopup popup = Manager.UiM.ShowPopup<UI_StageSelectPopup>();
        popup.SetInfo(currentStageData, OnChangedStageInfo);

    }

    void OnClickFirstClearRewardButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (boxes[0].state != RewardBoxState.RedDot) return;

        if (Manager.GameM.StageClearInfoDic.ContainsKey(currentStageData.StageIndex))
        {
            Manager.GameM.StageClearInfoDic[currentStageData.StageIndex].isOpenFirstBox = true;
            SetBoxState(0, RewardBoxState.Complete);

            Queue<string> name = new();
            Queue<int> count = new();

            int itemID = currentStageData.FirstWaveClearRewardItemID;
            if (Manager.DataM.MaterialDic.TryGetValue(itemID, out Data.MaterialData data))
            {
                name.Enqueue(data.SpriteName);
                count.Enqueue(currentStageData.FirstWaveClearRewardItemValue);

                UI_RewardPopup popup = (Manager.UiM.SceneUI as UI_LobbyScene).Ui_RewardPopup;
                popup.gameObject.SetActive(true);

                Manager.GameM.ExchangeMaterial(data, currentStageData.FirstWaveClearRewardItemValue);
                popup.SetInfo(name, count);

            }
        }

    }

    void OnClickSecondClearRewardButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (boxes[1].state != RewardBoxState.RedDot) return;

        if(Manager.GameM.StageClearInfoDic.ContainsKey(currentStageData.StageIndex))
        {
            Manager.GameM.StageClearInfoDic[currentStageData.StageIndex].isOpenSecondBox = true;
            SetBoxState(1, RewardBoxState.Complete);

            Queue<string> name = new();
            Queue<int> count = new();

            int itemID = currentStageData.SecondWaveClearRewardItemID;
            if(Manager.DataM.MaterialDic.TryGetValue(itemID, out Data.MaterialData data))
            {
                name.Enqueue(data.SpriteName);
                count.Enqueue(currentStageData.SecondWaveClearRewardItemValue);

                UI_RewardPopup popup = (Manager.UiM.SceneUI as UI_LobbyScene).Ui_RewardPopup;
                popup.gameObject.SetActive(true);

                Manager.GameM.ExchangeMaterial(data, currentStageData.SecondWaveClearRewardItemValue);
                popup.SetInfo(name, count);

            }
        }
    }

    void OnClickThirdClearRewardButton()
    {
        Manager.SoundM.PlayButtonClick();
        if (boxes[2].state != RewardBoxState.RedDot) return;

        if (Manager.GameM.StageClearInfoDic.ContainsKey(currentStageData.StageIndex))
        {
            Manager.GameM.StageClearInfoDic[currentStageData.StageIndex].isOpenThirdBox = true;
            SetBoxState(2, RewardBoxState.Complete);

            Queue<string> name = new();
            Queue<int> count = new();

            int itemID = currentStageData.ThirdWaveClearRewardItemID;
            if (Manager.DataM.MaterialDic.TryGetValue(itemID, out Data.MaterialData data))
            {
                name.Enqueue(data.SpriteName);
                count.Enqueue(currentStageData.ThirdWaveClearRewardItemValue);

                UI_RewardPopup popup = (Manager.UiM.SceneUI as UI_LobbyScene).Ui_RewardPopup;
                popup.gameObject.SetActive(true);

                Manager.GameM.ExchangeMaterial(data, currentStageData.ThirdWaveClearRewardItemValue);
                popup.SetInfo(name, count);

            }
        }

    }

    void OnClickGameStartButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.GameM.isGameEnd = false;

        if (Manager.GameM.Stamina < Define.GAMESTART_NEED_STAMINA)
        {
            Manager.UiM.ShowPopup<UI_StaminaChargePopup>();
            return;
        }

        Manager.GameM.Stamina -= Define.GAMESTART_NEED_STAMINA;

        if (Manager.GameM.MissionDic.TryGetValue(Define.MissionTarget.StageEnter, out MissionInfo mission))
        {
            mission.Progress++;
            Manager.GameM.SaveGame();
        }
            

        Manager.SceneM.LoadScene(Define.SceneType.GameScene, transform);

    }

    void OnClickOfflineRewardButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.UiM.ShowPopup<UI_OfflineRewardPopup>();
    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UI_CheckOutPopup popup = Manager.UiM.ShowPopup<UI_CheckOutPopup>();
            popup.SetInfo(++Manager.TimeM.AttendanceDay);
        }

    }

}
