using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using DG.Tweening;

public class UI_TitleScene : UI_Scene
{
    public enum Buttons
    {
        StartButton
    }

    public enum Sliders
    {
        Slider
    }

    public enum Texts
    {
        StartText,
        CountText
    }
    bool isLoadEnd = false;

    public override bool Init()
    {
        SetUIInfo();

        GetButton(typeof(Buttons), (int)Buttons.StartButton).gameObject.BindEvent(() => 
        {
            if(isLoadEnd) Debug.Log("Click Button");
            Manager.SceneM.LoadScene(Define.SceneType.LobbyScene);
        });
        GetButton(typeof(Buttons), (int)Buttons.StartButton).gameObject.SetActive(false);

        GetText(typeof(Texts), (int)Texts.StartText).gameObject.SetActive(false);

        SetInfo();
        return true;
    }

    protected override void SetUIInfo()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Slider>(typeof(Sliders));
        Bind<TextMeshProUGUI>(typeof(Texts));
    }

    void SetInfo()
    {
        Manager.ResourceM.LoadAllAsync<Object>("PrevLoad", (key, loadCount, maxCount) =>
        {
            GetSlider(typeof(Sliders), (int)Sliders.Slider).value = (float)loadCount / maxCount;
            GetText(typeof(Texts), (int)Texts.CountText).text = $"{loadCount} / {maxCount}";
            if (loadCount == maxCount)
            {
                isLoadEnd = true;
                GetButton(typeof(Buttons), (int)Buttons.StartButton).gameObject.SetActive(true);
                GetText(typeof(Texts), (int)Texts.StartText).gameObject.SetActive(true);
                Manager.DataM.Init();
                Manager.GameM.Init();
                Manager.TimeM.Init();
                Manager.AdM.Init();
                
                StartAnim();
            }
                
        });
    }

    public void StartAnim()
    {
        Vector3 OriginPos = GetText(typeof(Texts), (int)Texts.StartText).transform.localScale;

        GetText(typeof(Texts), (int)Texts.StartText).transform.DOScale(OriginPos * 1.5f, 0.5f)
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.InOutSine);
    }

}
