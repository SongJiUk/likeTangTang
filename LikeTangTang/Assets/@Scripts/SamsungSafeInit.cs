using System;
using UnityEngine;

public class SamsungSafeInit : MonoBehaviour
{
    void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        bool hasSamsungSdk = false;

        // 1) Samsung Game SDK 매니저 클래스가 있는지 확인
        try
        {
            using (var sdkClass = new AndroidJavaClass("com.samsung.android.gamesdk.GameSDKManager"))
            {
                hasSamsungSdk = sdkClass.GetRawClass() != IntPtr.Zero;
            }
        }
        catch
        {
            hasSamsungSdk = false;
        }

        if (hasSamsungSdk)
        {
            try
            {
                // 2) Unity Activity(Context) 가져오기
                var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                // 3) GameSDKManager 인스턴스 얻어서 initialize(context) 호출
                var sdkManager = new AndroidJavaClass("com.samsung.android.gamesdk.GameSDKManager")
                                    .CallStatic<AndroidJavaObject>("getInstance");
                sdkManager.Call("initialize", currentActivity);

                Debug.Log("✅ Samsung Game SDK initialized successfully.");
            }
            catch (Exception ex)
            {
                Debug.LogWarning("⚠️ Samsung SDK init skipped: " + ex.Message);
            }
        }
        else
        {
            Debug.Log("ℹ️ Non-Samsung device: skipping Samsung Game SDK init.");
        }
#endif
    }
}
