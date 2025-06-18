using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI.Extensions;

public class UI_StageSelectPopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,
        StageScrollContentObject,
        AppearingMonsterContentObject,
        StageSelectScrollView
    }

    enum Texts
    {
        StageSelectTitleText,
        AppearingMonsterText,
        StageSelectButtonText,
    }

    enum Buttons
    {
        StageSelectButton,
        BackButton,
        LArrowImage,
        RArrowImage
    }
    Data.StageData stageData;
    List<UI_MonsterInfoItem> monsterInfoItems = new List<UI_MonsterInfoItem>();
    HorizontalScrollSnap scrollSnap;
    int currentPage = 0;
    int maxPage = 4;
    Action<int> OnSelectionPageChangedEvent;
    Action OnChangedStageInfoEvent;
    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;
        gameObjectsType = typeof(GameObjects);
        TextsType = typeof(Texts);
        ButtonsType = typeof(Buttons);

        BindObject(gameObjectsType);
        BindText(TextsType);
        BindButton(ButtonsType);


        GetButton(ButtonsType, (int)Buttons.StageSelectButton).gameObject.BindEvent(OnClickStageSelectButton);
        GetButton(ButtonsType, (int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);
        GetButton(ButtonsType, (int)Buttons.LArrowImage).gameObject.BindEvent(OnClickLArrowImage);
        GetButton(ButtonsType, (int)Buttons.RArrowImage).gameObject.BindEvent(OnClickRArrowImage);

        OnSelectionPageChangedEvent += OnChangeStage;

        scrollSnap = Utils.FindChild<HorizontalScrollSnap>(gameObject, recursive: true);
        scrollSnap.OnSelectionPageChangedEvent.AddListener(OnChangeStage);
        scrollSnap.StartingScreen = Manager.GameM.CurrentStageData.StageIndex - 1;
        return true;

    }

    public void SetInfo(Data.StageData _stageData, Action _action = null)
    {
        stageData = _stageData;
        if(_action != null)
            OnChangedStageInfoEvent = _action;
        Refresh();
    }

    void Refresh()
    {
        if (stageData == null) return;

        
        StageListRefresh();
        StageInfoRefresh();
        

        

        

    }
   
    void StageListRefresh()
    {
        GameObject Stagecont = GetObject(gameObjectsType, (int)GameObjects.StageScrollContentObject);
        scrollSnap.ChildObjects = new GameObject[Manager.DataM.StageDic.Count];

        foreach (Data.StageData stageData in Manager.DataM.StageDic.Values)
        {
            UI_StageInfoItem item = Manager.UiM.MakeSubItem<UI_StageInfoItem>(Stagecont.transform);
            item.SetInfo(stageData);
            scrollSnap.ChildObjects[stageData.StageIndex - 1] = item.gameObject;
        }
    }

    void StageInfoRefresh()
    {
        UIRefresh();

        List<int> monsterList = stageData.SpawnMonsterNum.ToList();

        GameObject Monstercont = GetObject(gameObjectsType, (int)GameObjects.AppearingMonsterContentObject);

        for (int i = 0; i < monsterList.Count; i++)
        {
            if (Manager.DataM.CreatureDic[monsterList[i]].Type == Define.ObjectType.Monster) continue;

            UI_MonsterInfoItem monster = Manager.UiM.MakeSubItem<UI_MonsterInfoItem>(Monstercont.transform);
            monster.SetInfo(monsterList[i], stageData.StageLevel, this.transform);
            monsterInfoItems.Add(monster);
        }
    }

    
    void UIRefresh()
    {
        GetButton(ButtonsType, (int)Buttons.LArrowImage).gameObject.SetActive(true);
        GetButton(ButtonsType, (int)Buttons.RArrowImage).gameObject.SetActive(true);
        GetButton(ButtonsType, (int)Buttons.StageSelectButton).gameObject.SetActive(false);

        if (stageData.StageIndex == 1)
        {
            GetButton(ButtonsType, (int)Buttons.LArrowImage).gameObject.SetActive(false);
            GetButton(ButtonsType, (int)Buttons.RArrowImage).gameObject.SetActive(true);
        }
        else if (stageData.StageIndex > 1 && stageData.StageIndex < 5)
        {
            GetButton(ButtonsType, (int)Buttons.LArrowImage).gameObject.SetActive(true);
            GetButton(ButtonsType, (int)Buttons.RArrowImage).gameObject.SetActive(true);

        }
        else if (stageData.StageIndex == 5)
        {
            GetButton(ButtonsType, (int)Buttons.LArrowImage).gameObject.SetActive(true);
            GetButton(ButtonsType, (int)Buttons.RArrowImage).gameObject.SetActive(false);
        }

        if (!Manager.GameM.StageClearInfoDic.TryGetValue(stageData.StageIndex, out StageClearInfoData info))
            return;

        if (info.StageIndex == 1 && info.MaxWaveIndex == 0)
            GetButton(ButtonsType, (int)Buttons.StageSelectButton).gameObject.SetActive(true);

        if (info.StageIndex <= stageData.StageIndex)
            GetButton(ButtonsType, (int)Buttons.StageSelectButton).gameObject.SetActive(true);

        if (!Manager.GameM.StageClearInfoDic.TryGetValue(stageData.StageIndex - 1, out StageClearInfoData prevInfo))
            return;

        if (prevInfo.isClear)
            GetButton(ButtonsType, (int)Buttons.StageSelectButton).gameObject.SetActive(true);
        else
            GetButton(ButtonsType, (int)Buttons.StageSelectButton).gameObject.SetActive(false);

    }

    void OnClickStageSelectButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.GameM.CurrentStageData = stageData;
        OnChangedStageInfoEvent?.Invoke();
        Manager.UiM.ClosePopup(this);

    }

    void OnClickBackButton()
    {
        Manager.SoundM.PlayPopupClose();
        Manager.UiM.ClosePopup(this);
    }

    void OnClickLArrowImage()
    {
        if (currentPage > 0)
        {
            Manager.SoundM.PlayButtonClick();
            currentPage--;
            scrollSnap.GoToScreen(currentPage);
        }
    }

    void OnClickRArrowImage()
    {
        if(currentPage < maxPage)
        {
            Manager.SoundM.PlayButtonClick();
            currentPage++;
            scrollSnap.GoToScreen(currentPage);
        }
    }

    void OnChangeStage(int _index)
    {
        currentPage = _index;
        stageData = Manager.DataM.StageDic[_index + 1];

        List<int> monsterList = stageData.SpawnMonsterNum.ToList();

        int count = 0;
        for (int i = 0; i < monsterList.Count; i++)
        {
            if (Manager.DataM.CreatureDic[monsterList[i]].Type == Define.ObjectType.Monster) continue;

            monsterInfoItems[count].SetInfo(monsterList[i], stageData.StageLevel, this.transform);
            count++;
        }

        UIRefresh();
        
    }
}
