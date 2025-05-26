using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

/*
 * 



배너 광고	ca-app-pub-3940256099942544/6300978111
전면 광고	ca-app-pub-3940256099942544/1033173712
보상형 광고	ca-app-pub-3940256099942544/5224354917
네이티브 광고	ca-app-pub-3940256099942544/2247696110
 * 
 */
public class AdManager : MonoBehaviour
{
#if UNITY_EDITOR
    private const string APP_ID = "ca-app-pub-3940256099942544~3347511713";   // Android 테스트 App ID
    private const string BANNER_UNIT_ID = "ca-app-pub-3940256099942544/6300978111";
    private const string INTERSTITIAL_UNIT_ID = "ca-app-pub-3940256099942544/1033173712";
    private const string REWARDED_UNIT_ID = "ca-app-pub-3940256099942544/5224354917";
    private const string NATIVE_UNIT_ID = "ca-app-pub-3940256099942544/2247696110";
#elif UNITY_ANDROID
    private const string APP_ID              = "ca-app-pub-3940256099942544~3347511713";   // Android 테스트 App ID
    private const string BANNER_UNIT_ID      = "ca-app-pub-3940256099942544/6300978111";
    private const string INTERSTITIAL_UNIT_ID= "ca-app-pub-3940256099942544/1033173712";
    private const string REWARDED_UNIT_ID    = "ca-app-pub-3940256099942544/5224354917";
    private const string NATIVE_UNIT_ID      = "ca-app-pub-3940256099942544/2247696110";
#elif UNITY_IOS
    private const string APP_ID              = "ca-app-pub-3940256099942544~1458002511";   // iOS 테스트 App ID
    private const string BANNER_UNIT_ID      = "ca-app-pub-3940256099942544/2934735716";
    private const string INTERSTITIAL_UNIT_ID= "ca-app-pub-3940256099942544/4411468910";
    private const string REWARDED_UNIT_ID    = "ca-app-pub-3940256099942544/1712485313";
    private const string NATIVE_UNIT_ID      = "ca-app-pub-3940256099942544/3986624511";
#endif

    BannerView bannerView;
    InterstitialAd interstitialAd;
    RewardedAd rewardedAd;

    Action OnRewardedCallback;


    public void Init()
    {
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("광고 초기화 완료");
            LoadRewardedAd();
            RequestBanner();
        });
    }
    public void LoadRewardedAd()
    {
        AdRequest request = new AdRequest();
        RewardedAd.Load(REWARDED_UNIT_ID, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError($"보상형 광고 로드 실패 : {error}");
                return;
            }

            rewardedAd = ad;
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("광고 닫힘, 다시 로드 시도");
                LoadRewardedAd();
            };

            rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError($"보상형 광고 로드 실패 : {error.GetMessage()}");
            };


        });
    }

    public void ShowRewardedAd(Action _OnRewarded)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            OnRewardedCallback = _OnRewarded;
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log($"보상형 광고 보상 지급 : {reward.Type} / {reward.Amount}");

                OnRewardedCallback?.Invoke();
                if (Manager.GameM.MissionDic.TryGetValue(Define.MissionTarget.ADWatchIng, out MissionInfo missionInfo))
                    missionInfo.Progress++;

                OnRewardedCallback = null;
            });
        }
        else
        {
            Debug.Log("광고 준비 안됌, 로드 시작");
            LoadRewardedAd();
        }
    }

    public void RequestBanner()
    {
        if (bannerView != null) bannerView.Destroy();

        bannerView = new BannerView(BANNER_UNIT_ID, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest();
        bannerView.LoadAd(request);
        HideBanner();
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("배너 광고 로드 완료");
        };

        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError($"배너 광고 로드 실패 : {error.GetMessage()}");
        };
    }

    public void HideBanner()
    {
        bannerView?.Hide();
    }

    public void ShowBanner()
    {
        bannerView?.Show();
    }

    public void DestoryBanner()
    {
        bannerView?.Destroy();
        bannerView = null;
    }

}
