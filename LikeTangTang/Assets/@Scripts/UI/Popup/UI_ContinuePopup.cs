using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ContinuePopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,

    }

    enum Texts
    {
        CountdownValueText,
        ContinueCostValueText,

    }

    enum Buttons
    {
        CloseButton,
        ContinueButton,
        ADContinueButton

    }

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.ContentObject));
    }
    private void Start()
    {
        StartCoroutine(CoCountDown());
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

        GetButton(ButtonsType, (int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);
        GetButton(ButtonsType, (int)Buttons.ContinueButton).gameObject.BindEvent(OnClickContinueButton);
        GetButton(ButtonsType, (int)Buttons.ADContinueButton).gameObject.BindEvent(OnClickAdContinueButton);

        Refresh();
        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        if (Manager.GameM.ItemDic.TryGetValue(Define.ID_CLOVER, out int count))
            GetText(TextsType, (int)Texts.ContinueCostValueText).text = $"1 / {count}";
        else
            GetText(TextsType, (int)Texts.ContinueCostValueText).text = $"<color=red>0</color>";
    }
    void OnClickCloseButton()
    {
        Manager.UiM.ClosePopup(this);
        Manager.GameM.GameOver();
    }

    void OnClickContinueButton()
    {
        Manager.SoundM.PlayButtonClick();
        if(Manager.GameM.ItemDic.TryGetValue(Define.ID_CLOVER, out int count))
        {
            Manager.GameM.RemoveMaterialItem(Define.ID_CLOVER, 1);
            Manager.GameM.player.Resurrection(1);
            Manager.GameM.player.IsDead = false;
            Manager.UiM.ClosePopup(this);
        }
    }

    void OnClickAdContinueButton()
    {
        Manager.SoundM.PlayButtonClick();
        if(Manager.GameM.RebirthCountAds > 0)
        {
            Manager.AdM.ShowRewardedAd(() =>
            {
                Manager.GameM.player.Resurrection(1);
                Manager.GameM.RebirthCountAds--;
                Manager.GameM.player.IsDead = false;
                Manager.UiM.ClosePopup(this);
            });
        }
    }

    IEnumerator CoCountDown()
    {
        int count = 10;
        while(count > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            count--;
            GetText(TextsType, (int)Texts.CountdownValueText).text = count.ToString();
            if (count == 0) break;
        }
        yield return new WaitForSeconds(1f);

        Manager.UiM.ClosePopup(this);
        Manager.GameM.GameOver();
    }
}
