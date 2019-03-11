using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdEventListener : MonoBehaviour
{

    [SerializeField]
    private AdController _ctrl=null;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (_ctrl == null)
        {
            _ctrl = AdController.Instance;
        }

        if (_ctrl == null)
        {
            Debug.LogError("Missing reference to MoPubDemoGUI.  Please fix in the editor.");
            Destroy(this);
        }
    }

    /// <summary>
    /// 注册事件
    /// </summary>
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
    }

    /// <summary>
    /// 移除事件
    /// </summary>
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
    }


    /// <summary>
    /// 广告播放失败
    /// </summary>
    /// <param name="adUnitId"></param>
    /// <param name="action"></param>
    /// <param name="error"></param>
    private void AdFailed(string adUnitId, string action, string error)
    {
        var errorMsg = "Failed to " + action + " ad unit " + adUnitId;
        if (!string.IsNullOrEmpty(error))
        {
            errorMsg += ": " + error;
        }

        Debug.Log(errorMsg);
    }

    /// <summary>
    /// SDK初始化完成事件
    /// </summary>
    /// <param name="adUnitId"></param>
    private void OnSdkInitializedEvent(string adUnitId)
    {
        _ctrl.UpdateConsentValues();
    }

    #region 隐私信息确认相关
    /// <summary>
    /// 确认状态修改事件
    /// </summary>
    /// <param name="oldStatus"></param>
    /// <param name="newStatus"></param>
    /// <param name="canCollectPersonalInfo"></param>
    private void OnConsentStatusChangedEvent(MoPub.Consent.Status oldStatus, MoPub.Consent.Status newStatus,
                                             bool canCollectPersonalInfo)
    {
        //_demoGUI.ConsentStatusChanged(newStatus, canCollectPersonalInfo);
    }

    /// <summary>
    /// 确认弹窗加载完毕事件
    /// </summary>
    private void OnConsentDialogLoadedEvent()
    {
        //_demoGUI.ConsentDialogLoaded = true;
    }

    /// <summary>
    /// 确认弹窗失败事件
    /// </summary>
    /// <param name="err"></param>
    private void OnConsentDialogFailedEvent(string err)
    {
        //_demoGUI.UpdateStatusLabel(err);
    }

    /// <summary>
    /// 确认弹窗展示事件
    /// </summary>
    private void OnConsentDialogShownEvent()
    {
        //_demoGUI.ConsentDialogLoaded = false;
    }
    #endregion

    #region Banner Events

    /// <summary>
    /// banner已加载
    /// </summary>
    /// <param name="adUnitId"></param>
    /// <param name="height"></param>
    private void OnAdLoadedEvent(string adUnitId, float height)
    {
        _ctrl.BannerLoaded(adUnitId, height);
    }

    /// <summary>
    /// banner展示失败
    /// </summary>
    /// <param name="adUnitId"></param>
    /// <param name="error"></param>
    private void OnAdFailedEvent(string adUnitId, string error)
    {
        AdFailed(adUnitId, "load banner", error);
    }
    #endregion

    #region Interstitial Events

    /// <summary>
    /// 插屏已加载
    /// </summary>
    /// <param name="adUnitId"></param>
    private void OnInterstitialLoadedEvent(string adUnitId)
    {
        _ctrl.AdLoaded(adUnitId);
    }

    /// <summary>
    /// 插屏播放失败
    /// </summary>
    /// <param name="adUnitId"></param>
    /// <param name="error"></param>
    private void OnInterstitialFailedEvent(string adUnitId, string error)
    {
        AdFailed(adUnitId, "load interstitial", error);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="adUnitId"></param>
    private void OnInterstitialDismissedEvent(string adUnitId)
    {
        _ctrl.AdDismissed(adUnitId);
    }
    #endregion

    #region Rewarded Video Events

    /// <summary>
    /// RV已加载
    /// </summary>
    /// <param name="adUnitId"></param>
    private void OnRewardedVideoLoadedEvent(string adUnitId)
    {
        var availableRewards = MoPub.GetAvailableRewards(adUnitId);
        _ctrl.AdLoaded(adUnitId);
        _ctrl.LoadAvailableRewards(adUnitId, availableRewards);
    }

    /// <summary>
    /// RV加载失败
    /// </summary>
    /// <param name="adUnitId"></param>
    /// <param name="error"></param>
    private void OnRewardedVideoFailedEvent(string adUnitId, string error)
    {
        AdFailed(adUnitId, "load rewarded video", error);
    }

    /// <summary>
    /// RV播放失败
    /// </summary>
    /// <param name="adUnitId"></param>
    /// <param name="error"></param>
    private void OnRewardedVideoFailedToPlayEvent(string adUnitId, string error)
    {
        AdFailed(adUnitId, "play rewarded video", error);
    }

    /// <summary>
    /// RV关闭
    /// </summary>
    /// <param name="adUnitId"></param>
    private void OnRewardedVideoClosedEvent(string adUnitId)
    {
        _ctrl.AdDismissed(adUnitId);
    }
    #endregion
}
