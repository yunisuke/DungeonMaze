using System;
using UnityEngine;

public class AdManager
{
    private static AdManager _instance;

    private GoogleAds ads;

    // 初期化フラグ
    private bool isInitialized = false;

    private AdManager () {

    }

    public static AdManager Instance {get {
        if (_instance == null) _instance = new AdManager ();
        return _instance;
    }}

    public void Initialize () {
        if (isInitialized) return;

        if (ads == null) {
            var obj = new GameObject("AdMob");
            GameObject.DontDestroyOnLoad(obj);
            ads = obj.AddComponent<GoogleAds>();
        }

        ads.RequestBanner ();
        HideAds ();

        ads.RequestInterstitial();


        isInitialized = true;
    }

    public void HideAds () {
        ads.HideBannerView ();
    }

    public void ShowAds () {
        ads.ShowBannerView ();
    }

    public void ShowIntersitialAd (EventHandler<EventArgs> callback = null) {
        ads.ShowIntersitialAd(callback);
    }
}
