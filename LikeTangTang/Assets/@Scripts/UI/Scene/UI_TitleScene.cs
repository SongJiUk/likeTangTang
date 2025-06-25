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
        ButtonsType = typeof(Buttons);
        SlidersType = typeof(Sliders);
        TextsType = typeof(Texts);
        BindButton(ButtonsType);
        BindSlider(SlidersType);
        BindText(TextsType);


        GetButton(typeof(Buttons), (int)Buttons.StartButton).gameObject.BindEvent(() => 
        {
            Manager.SceneM.LoadScene(Define.SceneType.LobbyScene);
        });
        GetButton(typeof(Buttons), (int)Buttons.StartButton).gameObject.SetActive(false);

        GetText(typeof(Texts), (int)Texts.StartText).gameObject.SetActive(false);

        SetInfo();
        return true;
    }

    private int completedLoadOperations = 0; // 완료된 LoadAllAsync 호출 수
    private int totalExpectedLoadOperations = 2; // 총 LoadAllAsync 호출 수 (Sprite + PrevLoad)

    private int currentLoadedAssetCount = 0; // 현재까지 로드된 총 에셋 수
    private int totalExpectedAssetCount = 0; // 로드할 총 에셋 수

    void SetInfo()
    {
        Manager.ResourceM.LoadAllAsync<Sprite>("Sprite", (key, loadCount, maxCount) =>
        {
            if (loadCount == 1)
            {
                totalExpectedAssetCount += maxCount;
            }
            currentLoadedAssetCount++;
            UpdateLoadingUI(currentLoadedAssetCount, totalExpectedAssetCount);

            if (loadCount == maxCount)
            {
                completedLoadOperations++;
                CheckAllLoadsCompleted();
            }
        });

        // 2. 일반 Object 에셋 로드 시작
        // "PrevLoad" 라벨에 할당된 모든 Object 에셋을 로드합니다.
        Manager.ResourceM.LoadAllAsync<Object>("PrevLoad", (key, loadCount, maxCount) =>
        {
            // 첫 번째 콜백에서 해당 라벨의 총 에셋 수를 totalExpectedAssetCount에 더해줍니다.
            if (loadCount == 1)
            {
                totalExpectedAssetCount += maxCount;
            }
            // 현재 로드된 에셋 수를 증가시키고 UI를 업데이트합니다.
            currentLoadedAssetCount++;
            UpdateLoadingUI(currentLoadedAssetCount, totalExpectedAssetCount);

            // 해당 라벨의 모든 에셋 로드가 완료되면 완료된 작업 수를 증가시키고 전체 완료 여부를 확인합니다.
            if (loadCount == maxCount)
            {
                completedLoadOperations++;
                CheckAllLoadsCompleted();
            }
        });
    }

    void UpdateLoadingUI(int current, int total)
    {
        GetSlider(typeof(Sliders), (int)Sliders.Slider).value = (float)current / total;
        GetText(typeof(Texts), (int)Texts.CountText).text = $"{current} / {total}";
    }

    // 모든 로드 작업이 완료되었는지 확인
    void CheckAllLoadsCompleted()
    {
        // 모든 LoadAllAsync 호출이 완료되었는지 확인
        if (completedLoadOperations == totalExpectedLoadOperations)
        {
             isLoadEnd = true;
            GetButton(typeof(Buttons), (int)Buttons.StartButton).gameObject.SetActive(true);
            GetText(typeof(Texts), (int)Texts.StartText).gameObject.SetActive(true);

            // 모든 초기화 작업 실행
            Manager.DataM.Init();
            Manager.GameM.Init();
            Manager.TimeM.Init();
            Manager.AdM.Init();

            StartAnim();
        }
    }


    public void StartAnim()
    {
        Vector3 OriginPos = GetText(typeof(Texts), (int)Texts.StartText).transform.localScale;

        GetText(typeof(Texts), (int)Texts.StartText).transform.DOScale(OriginPos * 1.5f, 0.5f)
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.InOutSine);
    }

}
