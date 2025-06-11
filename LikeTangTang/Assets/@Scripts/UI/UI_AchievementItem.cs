using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class UI_AchievementItem : UI_Base
{
    enum GameObjects
    {
        
    }

    enum Texts
    {
        RewardItmeValueText,
        AchievementNameValueText,
        AchievementValueText,
        CompleteText,
        ProgressText
    }

    enum Buttons
    {
        GetButton,
    }

    enum Sliders
    {
        ProgressSlider
    }
    enum Images
    {
        RewardItmeIcon,
    }

    enum MissionState
    {
        Progress,
        Complete,
        Rewarded,
    }

    Data.AchievementData achievementData;

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
        ImagesType = typeof(Images);
        SlidersType = typeof(Sliders);

        BindObject(gameObjectsType);
        BindText(TextsType);
        BindButton(ButtonsType);
        BindImage(ImagesType);
        BindSlider(SlidersType);

        GetButton(ButtonsType, (int)Buttons.GetButton).gameObject.BindEvent(OnClickGetItemButton);

        Refresh();

        return true;

    }

    public void SetInfo(Data.AchievementData _data)
    {
        transform.localScale = Vector3.one;
        achievementData = _data;
        Refresh();
    }

    void Refresh()
    {
        if (achievementData == null)
        {
            LastAchievement();
            return;
        }

        GetImage(ImagesType, (int)Images.RewardItmeIcon).sprite = Manager.ResourceM.Load<Sprite>(Manager.DataM.MaterialDic[achievementData.ClearRewardItemID].SpriteName);
        GetText(TextsType, (int)Texts.RewardItmeValueText).text = $"x {achievementData.RewardValue}";
        GetText(TextsType, (int)Texts.AchievementNameValueText).text = $"{achievementData.DescriptionTextID}";
        GetSlider(SlidersType, (int)Sliders.ProgressSlider).value = 0;

        int progress = Manager.AchievementM.GetProgressValue(achievementData.MissionTarget);
        if (progress > 0)
            GetSlider(SlidersType, (int)Sliders.ProgressSlider).value = (float)progress / achievementData.MissionTargetValue;

        if (progress >= achievementData.MissionTargetValue)
        {
            SetButton(MissionState.Complete);

            if (achievementData.IsRewarded)
                SetButton(MissionState.Rewarded);
            
        }
        else
            SetButton(MissionState.Progress);

        GetText(TextsType, (int)Texts.AchievementValueText).text = $"{progress} / {achievementData.MissionTargetValue}";
    }

    void LastAchievement()
    {
        SetButton(MissionState.Rewarded);
    }

    void SetButton(MissionState _missionState)
    {
        GameObject GetButtonObj = GetButton(ButtonsType, (int)Buttons.GetButton).gameObject;
        GameObject CompleteObj = GetText(TextsType, (int)Texts.CompleteText).gameObject;
        GameObject ProgressObj = GetText(TextsType, (int)Texts.ProgressText).gameObject;
        switch (_missionState)
        {
            case MissionState.Progress:
                GetButtonObj.SetActive(false);
                CompleteObj.SetActive(false);
                ProgressObj.SetActive(true);
                break;

            case MissionState.Complete:
                GetButtonObj.SetActive(true);
                CompleteObj.SetActive(false);
                ProgressObj.SetActive(false);
                break;

            case MissionState.Rewarded:
                GetButtonObj.SetActive(false);
                CompleteObj.SetActive(true);
                ProgressObj.SetActive(false);
                break;

        }
    }

    void OnClickGetItemButton()
    {
        Manager.SoundM.PlayButtonClick();

        Queue<string> name = new();
        Queue<int> count = new();

        name.Enqueue(Manager.DataM.MaterialDic[achievementData.ClearRewardItemID].SpriteName);
        count.Enqueue((int)(achievementData.RewardValue * Manager.GameM.CurrentCharacter.Evol_DiaBouns));

        UI_RewardPopup popup = (Manager.UiM.SceneUI as UI_LobbyScene).Ui_RewardPopup;
        popup.gameObject.SetActive(true);
        Manager.GameM.ExchangeMaterial(Manager.DataM.MaterialDic[achievementData.ClearRewardItemID], (int)(achievementData.RewardValue * Manager.GameM.CurrentCharacter.Evol_DiaBouns));
        Manager.AchievementM.RewardedAchievement(achievementData.AchievementID);
        achievementData = Manager.AchievementM.GetNextAchievement(achievementData.AchievementID);
        Refresh();

        popup.SetInfo(name, count);


        Manager.UiM.CheckRedDotObject(Define.RedDotObjectType.AchievementPopup);
    }
}
