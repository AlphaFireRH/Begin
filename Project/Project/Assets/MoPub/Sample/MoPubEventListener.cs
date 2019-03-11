﻿using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local")]
[SuppressMessage("ReSharper", "AccessToStaticMemberViaDerivedType")]
public class MoPubEventListener : MonoBehaviour
{
    [SerializeField]
    private MoPubDemoGUI _demoGUI;


    private void Awake()
    {
        if (_demoGUI == null)
            _demoGUI = GetComponent<MoPubDemoGUI>();

        if (_demoGUI != null) return;
        Debug.LogError("Missing reference to MoPubDemoGUI.  Please fix in the editor.");
        Destroy(this);
    }


    private void OnEnable()
    {
        MoPubManager.OnSdkInitializedEvent += OnSdkInitializedEvent;

        MoPubManager.OnConsentStatusChangedEvent += OnConsentStatusChangedEvent;
        MoPubManager.OnConsentDialogLoadedEvent += OnConsentDialogLoadedEvent;
        MoPubManager.OnConsentDialogFailedEvent += OnConsentDialogFailedEvent;
        MoPubManager.OnConsentDialogShownEvent += OnConsentDialogShownEvent;

        MoPubManager.OnAdLoadedEvent += OnAdLoadedEvent;
        MoPubManager.OnAdFailedEvent += OnAdFailedEvent;

        MoPubManager.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        MoPubManager.OnInterstitialFailedEvent += OnInterstitialFailedEvent;
        MoPubManager.OnInterstitialDismissedEvent += OnInterstitialDismissedEvent;

        MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
        MoPubManager.OnRewardedVideoFailedEvent += OnRewardedVideoFailedEvent;
        MoPubManager.OnRewardedVideoFailedToPlayEvent += OnRewardedVideoFailedToPlayEvent;
        MoPubManager.OnRewardedVideoClosedEvent += OnRewardedVideoClosedEvent;

#if mopub_native_beta
        MoPubManager.OnNativeLoadEvent += OnNativeLoadEvent;
        MoPubManager.OnNativeFailEvent += OnNativeFailEvent;
#endif
    }


    private void OnDisable()
    {
        // Remove all event handlers
        MoPubManager.OnSdkInitializedEvent -= OnSdkInitializedEvent;

        MoPubManager.OnConsentStatusChangedEvent -= OnConsentStatusChangedEvent;
        MoPubManager.OnConsentDialogLoadedEvent -= OnConsentDialogLoadedEvent;
        MoPubManager.OnConsentDialogFailedEvent -= OnConsentDialogFailedEvent;
        MoPubManager.OnConsentDialogShownEvent -= OnConsentDialogShownEvent;

        MoPubManager.OnAdLoadedEvent -= OnAdLoadedEvent;
        MoPubManager.OnAdFailedEvent -= OnAdFailedEvent;

        MoPubManager.OnInterstitialLoadedEvent -= OnInterstitialLoadedEvent;
        MoPubManager.OnInterstitialFailedEvent -= OnInterstitialFailedEvent;
        MoPubManager.OnInterstitialDismissedEvent -= OnInterstitialDismissedEvent;

        MoPubManager.OnRewardedVideoLoadedEvent -= OnRewardedVideoLoadedEvent;
        MoPubManager.OnRewardedVideoFailedEvent -= OnRewardedVideoFailedEvent;
        MoPubManager.OnRewardedVideoFailedToPlayEvent -= OnRewardedVideoFailedToPlayEvent;
        MoPubManager.OnRewardedVideoClosedEvent -= OnRewardedVideoClosedEvent;

#if mopub_native_beta
        MoPubManager.OnNativeLoadEvent -= OnNativeLoadEvent;
        MoPubManager.OnNativeFailEvent -= OnNativeFailEvent;
#endif
    }


    private void AdFailed(string adUnitId, string action, string error)
    {
        var errorMsg = "Failed to " + action + " ad unit " + adUnitId;
        if (!string.IsNullOrEmpty(error))
            errorMsg += ": " + error;
        _demoGUI.UpdateStatusLabel("Error: " + errorMsg);
    }


    private void OnSdkInitializedEvent(string adUnitId)
    {
        _demoGUI.SdkInitialized();
    }


    private void OnConsentStatusChangedEvent(MoPub.Consent.Status oldStatus, MoPub.Consent.Status newStatus,
                                             bool canCollectPersonalInfo)
    {
        _demoGUI.ConsentStatusChanged(newStatus, canCollectPersonalInfo);
    }


    private void OnConsentDialogLoadedEvent()
    {
        _demoGUI.ConsentDialogLoaded = true;
    }


    private void OnConsentDialogFailedEvent(string err)
    {
        _demoGUI.UpdateStatusLabel(err);
    }


    private void OnConsentDialogShownEvent()
    {
        _demoGUI.ConsentDialogLoaded = false;
    }


    // Banner Events


    private void OnAdLoadedEvent(string adUnitId, float height)
    {
        _demoGUI.BannerLoaded(adUnitId, height);
    }


    private void OnAdFailedEvent(string adUnitId, string error)
    {
        AdFailed(adUnitId, "load banner", error);
    }


    // Interstitial Events


    private void OnInterstitialLoadedEvent(string adUnitId)
    {
        _demoGUI.AdLoaded(adUnitId);
    }


    private void OnInterstitialFailedEvent(string adUnitId, string error)
    {
        AdFailed(adUnitId, "load interstitial", error);
    }


    private void OnInterstitialDismissedEvent(string adUnitId)
    {
        _demoGUI.AdDismissed(adUnitId);
    }


    // Rewarded Video Events


    private void OnRewardedVideoLoadedEvent(string adUnitId)
    {
        var availableRewards = MoPub.GetAvailableRewards(adUnitId);
        _demoGUI.AdLoaded(adUnitId);
        _demoGUI.LoadAvailableRewards(adUnitId, availableRewards);
    }


    private void OnRewardedVideoFailedEvent(string adUnitId, string error)
    {
        AdFailed(adUnitId, "load rewarded video", error);
    }


    private void OnRewardedVideoFailedToPlayEvent(string adUnitId, string error)
    {
        AdFailed(adUnitId, "play rewarded video", error);
    }


    private void OnRewardedVideoClosedEvent(string adUnitId)
    {
        _demoGUI.AdDismissed(adUnitId);
    }


#if mopub_native_beta
    private void OnNativeLoadEvent(string adUnitId, AbstractNativeAd.Data nativeAdData)
    {
        _demoGUI.NativeAdLoaded(adUnitId, nativeAdData);
    }


    private void OnNativeFailEvent(string adUnitId, string error)
    {
        AdFailed(adUnitId, "load native ad", error);
    }
#endif
}
