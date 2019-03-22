using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdEventListener : MonoBehaviour
{

    [SerializeField]
    private AdController _ctrl = null;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (_ctrl == null)
        {
            _ctrl = AdController.Instance;
            _ctrl.tempListener = this;
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
        MoPubManager.OnRewardedVideoClickedEvent += OnRewardedVideoClickedEvent;
        MoPubManager.OnRewardedVideoLeavingApplicationEvent += OnRewardedVideoLeavingApplicationEvent;
        MoPubManager.OnRewardedVideoClosedEvent += OnRewardedVideoClosedEvent;
        MoPubManager.OnRewardedVideoReceivedRewardEvent += OnRewardedVideoReceivedRewardEvent;
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
        MoPubManager.OnRewardedVideoClickedEvent -= OnRewardedVideoClickedEvent;
        MoPubManager.OnRewardedVideoLeavingApplicationEvent -= OnRewardedVideoLeavingApplicationEvent;
        MoPubManager.OnRewardedVideoClosedEvent -= OnRewardedVideoClosedEvent;
        MoPubManager.OnRewardedVideoReceivedRewardEvent -= OnRewardedVideoReceivedRewardEvent;
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
    /// rv加载请求队列
    /// </summary>
    private Coroutine bannerWait = null;
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
    /// banner加载失败
    /// </summary>
    /// <param name="adUnitId"></param>
    /// <param name="error"></param>
    private void OnAdFailedEvent(string adUnitId, string error)
    {
        ShowError(error);
        FetchBannerAdWithTime(adUnitId);
    }
    #endregion

    #region Interstitial Events
    /// <summary>
    /// rv加载请求队列
    /// </summary>
    private Coroutine insertWait = null;
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
        ShowError(error);

        FetchInsertAdWithTime(adUnitId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="adUnitId"></param>
    private void OnInterstitialDismissedEvent(string adUnitId)
    {
        _ctrl.PlayFinish(adUnitId);

        FetchInsertAd(adUnitId);
    }
    #endregion

    #region Rewarded Video Events
    /// <summary>
    /// rv加载请求队列
    /// </summary>
    private Coroutine rvWait = null;
    /// <summary>
    /// rv播放结果，是否可以领奖
    /// </summary>
    private RvResult rvGetRewardState = RvResult.NoResult;
    /// <summary>
    /// 激励视频已关闭
    /// </summary>
    private bool rvClose = false;

    private Coroutine rvCloseWait = null;
    /// <summary>
    /// RV已加载
    /// </summary>
    /// <param name="adUnitId"></param>
    private void OnRewardedVideoLoadedEvent(string adUnitId)
    {
        rvGetRewardState = RvResult.NoResult;
        rvClose = false;
        if (rvCloseWait != null)
        {
            StopCoroutine(rvCloseWait);
        }
        rvCloseWait = null;

        //var availableRewards = MoPub.GetAvailableRewards(adUnitId);
        _ctrl.AdLoaded(adUnitId);
        //_ctrl.LoadAvailableRewards(adUnitId, availableRewards);
    }

    /// <summary>
    /// RV加载失败
    /// </summary>
    /// <param name="adUnitId"></param>
    /// <param name="error"></param>
    private void OnRewardedVideoFailedEvent(string adUnitId, string error)
    {
        ShowError(error);
        FetchRVAdWithTime(adUnitId);
    }

    /// <summary>
    /// RV播放失败
    /// </summary>
    /// <param name="adUnitId"></param>
    /// <param name="error"></param>
    private void OnRewardedVideoFailedToPlayEvent(string adUnitId, string error)
    {
        ShowError(error);
        rvGetRewardState = RvResult.Fail;
    }

    // Fired when an rewarded video is clicked
    // 单击RV视频时激发 
    public void OnRewardedVideoClickedEvent(string adUnitId)
    {

    }

    // iOS only. Fired when a rewarded video event causes another application to open
    // 仅iOS。当RV视频事件导致另一个应用程序打开时激发 
    public void OnRewardedVideoLeavingApplicationEvent(string adUnitId)
    {
        rvGetRewardState = RvResult.Success;
    }

    /// <summary>
    /// RV关闭
    /// </summary>
    /// <param name="adUnitId"></param>
    private void OnRewardedVideoClosedEvent(string adUnitId)
    {
        rvClose = true;

        rvCloseWait = StartCoroutine(CloseWait(adUnitId));
        PushRvPlayResult(adUnitId);
    }

    /// <summary>
    /// 等待2s 如果还没有返回结果，则认为本次播放失败
    /// </summary>
    /// <param name="adUnitId"></param>
    /// <returns></returns>
    private IEnumerator CloseWait(string adUnitId)
    {
        yield return new WaitForSeconds(2);

        rvGetRewardState = RvResult.Fail;
        PushRvPlayResult(adUnitId);
    }

    /// <summary>
    /// 奖励视频播放完毕时，执行Show时传进来的Action
    /// </summary>
    /// <param name="adUnitId"></param>
    /// <param name="label"></param>
    /// <param name="amount"></param>
    private void OnRewardedVideoReceivedRewardEvent(string adUnitId, string label, float amount)
    {
        rvGetRewardState = RvResult.Success;

        PushRvPlayResult(adUnitId);
    }

    /// <summary>
    /// 尝试返回本次播放结果
    /// </summary>
    /// <param name="adUnitId"></param>
    private void PushRvPlayResult(string adUnitId)
    {
        if (rvGetRewardState != RvResult.NoResult && rvClose)
        {
            if (rvCloseWait != null)
            {
                StopCoroutine(rvCloseWait);
                rvCloseWait = null;
            }
            switch (rvGetRewardState)
            {
                case RvResult.NoResult:
                    break;
                case RvResult.Fail:
                    _ctrl.PlayFail(adUnitId);
                    break;
                case RvResult.Success:
                    _ctrl.PlayFinish(adUnitId);
                    break;
                default:
                    break;
            }

            FetchRVAd(adUnitId);
        }
    }

    #endregion

    public void FetchBannerAdWithTime(string adUnitId)
    {
        if (bannerWait != null)
        {
            StopCoroutine(bannerWait);
        }
        bannerWait = StartCoroutine(WaitTryFetchWithTime(adUnitId));
    }

    public void FetchInsertAd(string adUnitId)
    {
        if (insertWait != null)
        {
            StopCoroutine(insertWait);
        }
        insertWait = StartCoroutine(WaitTryFetch(adUnitId));
    }

    public void FetchInsertAdWithTime(string adUnitId)
    {
        if (insertWait != null)
        {
            StopCoroutine(insertWait);
        }
        insertWait = StartCoroutine(WaitTryFetchWithTime(adUnitId));
    }

    public void FetchRVAd(string adUnitId)
    {
        if (rvWait != null)
        {
            StopCoroutine(rvWait);
        }
        rvWait = StartCoroutine(WaitTryFetch(adUnitId));
    }

    public void FetchRVAdWithTime(string adUnitId)
    {
        if (rvWait != null)
        {
            StopCoroutine(rvWait);
        }
        rvWait = StartCoroutine(WaitTryFetchWithTime(adUnitId));
    }

    WaitForSeconds loadWait = new WaitForSeconds(60.0f);
    IEnumerator WaitTryFetch(string adUnitId)
    {
        yield return loadWait;

        _ctrl.TryFetch(adUnitId);
    }

    WaitForSeconds playWait = new WaitForSeconds(90.0f);
    IEnumerator WaitTryFetchWithTime(string adUnitId)
    {
        yield return playWait;

        _ctrl.TryFetch(adUnitId);
    }

    private void ShowError(string error)
    {
        Debug.LogError(error);
    }








    //string showInfo = "";

    //private void OnGUI()
    //{
    //    GUILayout.Label(showInfo);

    //    if (GUILayout.Button("clean", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.08f)))
    //    {
    //        showInfo = "";
    //    }

    //    if (GUILayout.Button("play", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.08f)))
    //    {
    //        _ctrl.ShowRewardVideoAd((int result) => {
    //            Debug.Log("RewardVideo:   " + result.ToString());
    //        });
    //    }

    //}
}

public enum RvResult
{
    NoResult,
    Fail,
    Success
}
