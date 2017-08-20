using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADMobService : ADInterface
{
    private BannerView bannerView;
    private InterstitialAd interstitial;

    public string BannerAdUnitId = "unused";
    public string InterstitialUnitId = "unused";

    public override void LoadAD(ADType adType, string tag = "")
    {
        if (adType == ADType.Banner)
        {
            RequestBanner();
        }
        else if (adType == ADType.Interstitial)
        {
            RequestInterstitial();
        }
    }

    public override void PlayAD(ADType adType, string tag = "")
    {
        if (adType == ADType.Banner)
        {
            ShowBannerAd();
        }
        else if (adType == ADType.Interstitial)
        {
            ShowInterstitialAd();
        }
    }

    public override void CloseAD(ADType adType, string tag = "")
    {
        if (adType == ADType.Banner)
        {
            CloseBannerAD();
        }
        else if(adType == ADType.Interstitial)
        {
            CloseInterstitialAd();
        }
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
            .AddKeyword("game")
            .SetGender(Gender.Male)
            .SetBirthday(new DateTime(1985, 1, 1))
            .TagForChildDirectedTreatment(false)
            .AddExtra("color_bg", "9B30FF")
            .Build();
    }

    #region BannerAD

    private void RequestBanner()
    {
        Debug.Log("RequestBanner");

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(BannerAdUnitId, AdSize.SmartBanner, AdPosition.Top);

        // Register for ad events.
        this.bannerView.OnAdLoaded += this.HandleAdLoaded;
        this.bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
        this.bannerView.OnAdOpening += this.HandleAdOpened;
        this.bannerView.OnAdClosed += this.HandleAdClosed;
        this.bannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;

        // Load a banner ad.
        this.bannerView.LoadAd(this.CreateAdRequest());
    }

    void ShowBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Show();
        }
    }

    void CloseBannerAD()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
        }
    }

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        bannerView.Hide();
        Debug.Log("HandleAdLoaded event received");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleAdOpened event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        bannerView.Destroy();

        RequestBanner();

        Debug.Log("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLeftApplication event received");
    }

    #endregion

    #region InterstitialAd

    void CloseInterstitialAd()
    {
        if(interstitial != null)
        {
            interstitial.Destroy();
        }
    }

    void ShowInterstitialAd()
    {
        if (interstitial != null)
        {
            if (interstitial.IsLoaded())
            {
                interstitial.Show();
            }
            else
            {
                Debug.LogWarning("Interstitial isLoad False !");
            }
        }
    }

    private void RequestInterstitial()
    {
        Debug.Log("RequestInterstitial");

        this.interstitial = new InterstitialAd(InterstitialUnitId);

        // Register for ad events.
        this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
        this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
        this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
        this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
        this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;

        // Load an interstitial ad.
        this.interstitial.LoadAd(this.CreateAdRequest());
    }

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleInterstitialLoaded event received");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log(
            "HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleInterstitialOpened event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        interstitial.Destroy();
        RequestInterstitial();
        Debug.Log("HandleInterstitialClosed event received");
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        Debug.Log("HandleInterstitialLeftApplication event received");
    }

    #endregion
}
