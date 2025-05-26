using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StageInfoItem : UI_Base
{
    enum GameObjects
    {
        FirstClearRewardCompleteObject,
        FirstClearRewardLockObject,
        SecondClearRewardCompleteObject,
        SecondClearRewardLockObject,
        ThirdClearRewardCompleteObject,
        ThirdClearRewardLockObject
    }

    enum Images
    {
        StageImage,
        StageLockImage,
    }

    enum Texts
    {
        StageValueText,
        MaxWaveText,
        MaxWaveValueText,

    }
    Data.StageData stageData;
    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;
        gameObjectsType = typeof(GameObjects);
        ImagesType = typeof(Images);
        TextsType = typeof(Texts);

        BindObject(gameObjectsType);
        BindImage(ImagesType);
        BindText(TextsType);

       
        GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardCompleteObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.SecondClearRewardCompleteObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.ThirdClearRewardCompleteObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardLockObject).SetActive(true);
        GetObject(gameObjectsType, (int)GameObjects.SecondClearRewardLockObject).SetActive(true);
        GetObject(gameObjectsType, (int)GameObjects.ThirdClearRewardLockObject).SetActive(true);

        GetImage(ImagesType, (int)Images.StageLockImage).gameObject.SetActive(true);
        GetImage(ImagesType, (int)Images.StageImage).color = Utils.HexToColor("3A3A3A");
        GetText(TextsType, (int)Texts.MaxWaveText).gameObject.SetActive(false);
        GetText(TextsType, (int)Texts.MaxWaveValueText).gameObject.SetActive(false);


        return true;
    }

    public void SetInfo(Data.StageData _stageData)
    {
        stageData = _stageData;
        Refresh();
        
    }

    void Refresh()
    {
        GetText(TextsType, (int)Texts.StageValueText).text = $"스테이지 {stageData.StageIndex}";
        GetImage(ImagesType, (int)Images.StageImage).sprite = Manager.ResourceM.Load<Sprite>(stageData.StageImage);

        //if (!Manager.GameM.StageClearInfoDic.TryGetValue(stageData.StageIndex, out StageClearInfoData stageClearInfoData)) return;
        if (!Manager.GameM.StageClearInfoDic.TryGetValue(stageData.StageIndex, out StageClearInfoData data)) return;


        if(data.MaxWaveIndex > 0)
        {
            bool isClear = data.isClear;
            string waveText = isClear ? "스테이지 클리어" : (data.MaxWaveIndex + 1).ToString();
            SetStageUI(false, !isClear, waveText);
            SetRewardUI(data);
            return;
        }

        if(data.StageIndex == 1 && data.MaxWaveIndex == 0)
        {
            SetStageUI(false, false, "기록 없음");
            SetRewardUI(data);
        }

        if (Manager.GameM.StageClearInfoDic.TryGetValue(stageData.StageIndex - 1, out StageClearInfoData prevData) && prevData.isClear)
        {
            SetStageUI(false, false, "기록 없음");
            SetRewardUI(data);
        }

        #region 묶기 전
        //if (stageClearInfoData.MaxWaveIndex > 0)
        //{
        //    GetImage(ImagesType, (int)Images.StageLockImage).gameObject.SetActive(false);
        //    GetImage(ImagesType, (int)Images.StageImage).color = Color.white;

        //    if (stageClearInfoData.isClear)
        //    {
        //        GetImage(ImagesType, (int)Images.StageLockImage).gameObject.SetActive(false);
        //        GetImage(ImagesType, (int)Images.StageImage).color = Color.white;
        //        GetText(TextsType, (int)Texts.MaxWaveText).gameObject.SetActive(false);
        //        GetText(TextsType, (int)Texts.MaxWaveValueText).gameObject.SetActive(true);
        //        GetText(TextsType, (int)Texts.MaxWaveText).text = "스테이지 클리어";

        //    }
        //    else
        //    {
        //        GetText(TextsType, (int)Texts.MaxWaveText).gameObject.SetActive(true);
        //        GetText(TextsType, (int)Texts.MaxWaveValueText).gameObject.SetActive(true);
        //        GetText(TextsType, (int)Texts.MaxWaveValueText).text = $"{stageClearInfoData.MaxWaveIndex + 1}";
        //    }

        //    GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardCompleteObject).SetActive(false);
        //    GetObject(gameObjectsType, (int)GameObjects.SecondClearRewardCompleteObject).SetActive(false);
        //    GetObject(gameObjectsType, (int)GameObjects.ThirdClearRewardCompleteObject).SetActive(false);

        //    GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardLockObject).SetActive(stageClearInfoData.isOpenFirstBox);
        //    GetObject(gameObjectsType, (int)GameObjects.SecondClearRewardLockObject).SetActive(stageClearInfoData.isOpenSecondBox);
        //    GetObject(gameObjectsType, (int)GameObjects.ThirdClearRewardLockObject).SetActive(stageClearInfoData.isOpenThirdBox);
        //}
        //else
        //{
        //    //아예 처음일경우
        //    if(stageClearInfoData.StageIndex == 1 && stageClearInfoData.MaxWaveIndex == 0)
        //    {
        //        GetImage(ImagesType, (int)Images.StageLockImage).gameObject.SetActive(false);
        //        GetImage(ImagesType, (int)Images.StageImage).color = Color.white;

        //        GetText(TextsType, (int)Texts.MaxWaveText).gameObject.SetActive(false);
        //        GetText(TextsType, (int)Texts.MaxWaveValueText).gameObject.SetActive(true);
        //        GetText(TextsType, (int)Texts.MaxWaveValueText).text = "기록 없음";


        //        GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardCompleteObject).SetActive(false);
        //        GetObject(gameObjectsType, (int)GameObjects.SecondClearRewardCompleteObject).SetActive(false);
        //        GetObject(gameObjectsType, (int)GameObjects.ThirdClearRewardCompleteObject).SetActive(false);

        //        GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardLockObject).SetActive(stageClearInfoData.isOpenFirstBox);
        //        GetObject(gameObjectsType, (int)GameObjects.SecondClearRewardLockObject).SetActive(stageClearInfoData.isOpenSecondBox);
        //        GetObject(gameObjectsType, (int)GameObjects.ThirdClearRewardLockObject).SetActive(stageClearInfoData.isOpenThirdBox);
        //    }

        //    //새로운 스테이지라면?
        //    if (!Manager.GameM.StageClearInfoDic.TryGetValue(stageData.StageIndex - 1, out StageClearInfoData info)) return;

        //    if(info.isClear)
        //    {
        //        GetImage(ImagesType, (int)Images.StageLockImage).gameObject.SetActive(false);
        //        GetImage(ImagesType, (int)Images.StageImage).color = Color.white;

        //        GetText(TextsType, (int)Texts.MaxWaveText).gameObject.SetActive(false);
        //        GetText(TextsType, (int)Texts.MaxWaveValueText).gameObject.SetActive(true);
        //        GetText(TextsType, (int)Texts.MaxWaveValueText).text = "기록 없음";

        //        GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardCompleteObject).SetActive(false);
        //        GetObject(gameObjectsType, (int)GameObjects.SecondClearRewardCompleteObject).SetActive(false);
        //        GetObject(gameObjectsType, (int)GameObjects.ThirdClearRewardCompleteObject).SetActive(false);

        //        GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardLockObject).SetActive(stageClearInfoData.isOpenFirstBox);
        //        GetObject(gameObjectsType, (int)GameObjects.SecondClearRewardLockObject).SetActive(stageClearInfoData.isOpenSecondBox);
        //        GetObject(gameObjectsType, (int)GameObjects.ThirdClearRewardLockObject).SetActive(stageClearInfoData.isOpenThirdBox);
        //    }
        //}
        #endregion
    }

    void SetStageUI(bool _isLocked, bool _showMaxWaveText, string _maxWaveValue)
    {
        GetImage(ImagesType, (int)Images.StageImage).color = _isLocked ? Utils.HexToColor("3A3A3A") : Color.white;
        GetImage(ImagesType, (int)Images.StageLockImage).gameObject.SetActive(_isLocked);

        GetText(TextsType, (int)Texts.MaxWaveText).gameObject.SetActive(_showMaxWaveText);
        GetText(TextsType, (int)Texts.MaxWaveValueText).gameObject.SetActive(true);

        if (_showMaxWaveText)
            GetText(TextsType, (int)Texts.MaxWaveText).text = "생존 웨이브";
        GetText(TextsType, (int)Texts.MaxWaveValueText).text = _maxWaveValue;
    }

    void SetRewardUI(StageClearInfoData _info)
    {
        GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardCompleteObject).SetActive(_info.isOpenFirstBox);
        GetObject(gameObjectsType, (int)GameObjects.SecondClearRewardCompleteObject).SetActive(_info.isOpenSecondBox);
        GetObject(gameObjectsType, (int)GameObjects.ThirdClearRewardCompleteObject).SetActive(_info.isOpenThirdBox);

        if(_info.isClear)
        {
            GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardLockObject).SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.SecondClearRewardLockObject).SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.ThirdClearRewardLockObject).SetActive(false);
        }
        else
        {
            GetObject(gameObjectsType, (int)GameObjects.FirstClearRewardLockObject).SetActive(!_info.isOpenFirstBox);
            GetObject(gameObjectsType, (int)GameObjects.SecondClearRewardLockObject).SetActive(!_info.isOpenSecondBox);
            GetObject(gameObjectsType, (int)GameObjects.ThirdClearRewardLockObject).SetActive(!_info.isOpenThirdBox);
        }
    }

}
