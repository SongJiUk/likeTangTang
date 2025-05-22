using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UI_MissionItem : UI_Base
{
    enum Images
    {
        RewardItmeIconImage,

    }

    enum Buttons
    {
        GetButton,

    }

    enum Texts
    {
        RewardItemValueText,
        MissionNameValueText,
        MissionProgressValueText,
    }

    enum GameObjects
    {
        ProgressText,
        CompleteText,

    }
    enum Sliders
    {
        ProgressSliderObject
    }

    enum MissionState
    {
        Progress,
        Complete,
        Rewarded,
    }

    Data.MissionData missionData;

    private void Awake()
    {
        Init();
    }


    public override bool Init()
    {
        if (!base.Init()) return false;
        ImagesType = typeof(Images);
        ButtonsType = typeof(Buttons);
        TextsType = typeof(Texts);
        gameObjectsType = typeof(GameObjects);
        SlidersType = typeof(Sliders);

        BindImage(ImagesType);
        BindButton(ButtonsType);
        BindText(TextsType);
        BindObject(gameObjectsType);
        BindSlider(SlidersType);

        GetObject(gameObjectsType, (int)GameObjects.ProgressText).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.CompleteText).SetActive(false);
        GetButton(ButtonsType, (int)Buttons.GetButton).gameObject.BindEvent(OnClickGetButton);
        GetButton(ButtonsType, (int)Buttons.GetButton).gameObject.SetActive(false);


        return true;
    }


    public void SetInfo(Data.MissionData _missionData)
    {
        transform.localScale = Vector3.one;
        missionData = _missionData;
        Refresh();
    }

    void Refresh()
    {
        if (missionData == null) return;

        GetText(TextsType, (int)Texts.RewardItemValueText).text = $"x{missionData.RewardValue}";
        GetText(TextsType, (int)Texts.MissionNameValueText).text = $"{missionData.DescriptionTextID}";
        GetSlider(SlidersType, (int)Sliders.ProgressSliderObject).value = 0;

        if(Manager.GameM.MissionDic.TryGetValue(missionData.MissionTarget, out MissionInfo missioninfo))
        {
            if (missioninfo.Progress > 0)
                GetSlider(SlidersType, (int)Sliders.ProgressSliderObject).value = (float)missioninfo.Progress / missionData.MissionTargetValue;

            if (missioninfo.Progress >= missionData.MissionTargetValue)
            {
                SetButton(MissionState.Complete);

                if (missioninfo.isRewarded)
                    SetButton(MissionState.Rewarded);
            }
            else
                SetButton(MissionState.Progress);

            GetText(TextsType, (int)Texts.MissionProgressValueText).text = $"{missioninfo.Progress} / {missionData.MissionTargetValue}";
        }

        string spriteName = Manager.DataM.MaterialDic[missionData.ClearRewardItmeID].SpriteName;
        GetImage(ImagesType, (int)Images.RewardItmeIconImage).sprite = Manager.ResourceM.Load<Sprite>(spriteName);
    }

    void SetButton(MissionState _state)
    {
        GameObject CompleteObj = GetButton(ButtonsType, (int)Buttons.GetButton).gameObject;
        GameObject ProgressObj = GetObject(gameObjectsType, (int)GameObjects.ProgressText).gameObject;
        GameObject RewardedObj = GetObject(gameObjectsType, (int)GameObjects.CompleteText).gameObject;
        switch (_state)
        {
            case MissionState.Progress:
                ProgressObj.SetActive(true);
                CompleteObj.SetActive(false);
                RewardedObj.SetActive(false);
                break;

            case MissionState.Complete:
                ProgressObj.SetActive(false);
                CompleteObj.SetActive(true);
                RewardedObj.SetActive(false);
                break;

            case MissionState.Rewarded:
                ProgressObj.SetActive(false);
                CompleteObj.SetActive(false);
                RewardedObj.SetActive(true);
                break;

        }
    }

    void OnClickGetButton()
    {
        Manager.SoundM.PlayButtonClick();

        Queue<string> name = new();
        Queue<int> count = new();

        name.Enqueue(Manager.DataM.MaterialDic[missionData.ClearRewardItmeID].SpriteName);
        count.Enqueue(missionData.RewardValue);

        UI_RewardPopup rewardPopup = (Manager.UiM.SceneUI as UI_LobbyScene).Ui_RewardPopup;
        rewardPopup.gameObject.SetActive(true);
        Manager.GameM.Dia += missionData.RewardValue;

        if(Manager.GameM.MissionDic.TryGetValue(missionData.MissionTarget, out MissionInfo info))
        {
            info.isRewarded = true;
            Manager.GameM.SaveGame();
        }
            

        Refresh();
        Manager.UiM.CheckRedDotObject(Define.RedDotObjectType.Mission);

        rewardPopup.SetInfo(name, count);

    }
}
