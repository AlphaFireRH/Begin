using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdController
{
    private static AdController _instance = null;

    public static AdController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AdController();
                CreateObj();
            }

            return _instance;
        }
    }

    private static void CreateObj()
    {
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("AdEventListner")) as GameObject;
    }


    /// <summary>
    /// 广告加载状态
    /// </summary>
    private readonly Dictionary<string, bool> _adUnitToLoadedMapping = new Dictionary<string, bool>();
    /// <summary>
    /// 广告展示状态
    /// </summary>
    private readonly Dictionary<string, bool> _adUnitToShownMapping = new Dictionary<string, bool>();
    /// <summary>
    /// 广告奖励状态
    /// </summary>
    private readonly Dictionary<string, List<MoPub.Reward>> _adUnitToRewardsMapping =
        new Dictionary<string, List<MoPub.Reward>>();

    /// <summary>
    /// bannerID
    /// </summary>
    public string[] bannerIdList = new string[] { "b195f8dd8ded45fe847ad89ed1d016da" };
    /// <summary>
    /// 激励视频ID
    /// </summary>
    public string[] rvIdList = new string[] { "920b6145fb1546cf8b5cf2ac34638bb7" };
    /// <summary>
    /// 插屏ID
    /// </summary>
    public string[] insterIdList = new string[] { "24534e1901884e398f1253216226017e" };


    ///// <summary>
    ///// bannerID
    ///// </summary>
    //public string[] bannerIdList = new string[] { "e02f3328546442a0b57c36e49f29a1b1" };
    ///// <summary>
    ///// 激励视频ID
    ///// </summary>
    //public string[] rvIdList = new string[] { "031f925cb81642d7b9e8a0c40c806930" };
    ///// <summary>
    ///// 插屏ID
    ///// </summary>
    //public string[] insterIdList = new string[] { "a2ff4eadef394ed39387e50c5dc84f9e" };

    #region Init
    public void Init()
    {
        AddUnitToList();

        SDKInit();

        FetchInsertAd();
        FetchRewardVideoAd();
    }

    /// <summary>
    /// 插如广告ID
    /// </summary>
    private void AddUnitToList()
    {
        AddAdUnitsToStateMaps(bannerIdList);
        AddAdUnitsToStateMaps(rvIdList);
        AddAdUnitsToStateMaps(insterIdList);
    }

    /// <summary>
    /// 将单元插到队列中
    /// </summary>
    /// <param name="adUnits"></param>
    private void AddAdUnitsToStateMaps(IEnumerable<string> adUnits)
    {
        foreach (var adUnit in adUnits)
        {
            if (!_adUnitToLoadedMapping.ContainsKey(adUnit))
            {
                _adUnitToLoadedMapping[adUnit] = false;
            }
            if (!_adUnitToShownMapping.ContainsKey(adUnit))
            {
                _adUnitToShownMapping[adUnit] = false;
            }
        }
    }

    private void SDKInit()
    {
        // NOTE: the MoPub SDK needs to be initialized on Start() to ensure all other objects have been enabled first.
        var anyAdUnitId = bannerIdList[0];
        MoPub.InitializeSdk(new MoPub.SdkConfiguration
        {
            AdUnitId = anyAdUnitId,

            // Set desired log level here to override default level of MPLogLevelNone
            LogLevel = MoPubBase.LogLevel.MPLogLevelDebug,

            // Uncomment the following line to allow supported SDK networks to collect user information on the basis
            // of legitimate interest.
            //AllowLegitimateInterest = true,

            // Specify the mediated networks you are using here:
            MediatedNetworks = new MoPub.MediatedNetwork[]
            {
            /*
                // Example using AdMob.  Follow this template for other supported networks as well.
                // Note that keys must be strings, and values must be JSON-serializable (strings only, for MoPubRequestOptions).
                new MoPub.SupportedNetwork.AdMob
                {
                    // Network adapter configuration settings (initialization).
                    NetworkConfiguration = {
                        { "key1", value },
                        { "key2", value },
                    },

                    // Global mediation settings (per ad request).
                    MediationSettings = {
                        { "key1", value },
                        { "key2", value },
                    },

                    // Additional options to pass to the MoPub servers (per ad request).
                    MoPubRequestOptions = {
                        { "key1", "value" },
                        { "key2", "value" },
                    }
                },

                // Example using a custom network adapter:
                new MoPub.MediatedNetwork
                {
                    // Specify the class name that implements the AdapterConfiguration interface.
                #if UNITY_ANDROID
                    AdapterConfigurationClassName = "classname",  // include the full package name
                #else // UNITY_IOS
                    AdapterConfigurationClassName = "classname",
                #endif

                    // Specify the class name that implements the MediationSettings interface.
                    // Note: Custom network mediation settings are currently not supported on Android.
                #if UNITY_IOS
                    MediationSettingsClassName = "classname",
                #endif

                    // Fill in settings and configuration options the same way as for supported networks:

                    NetworkConfiguration = { ... },

                #if UNITY_IOS  // See note above.
                    MediationSettings    = { ... },
                #endif

                    MoPubRequestOptions  = { ... },
                }
            */
            },
        });

        MoPub.LoadBannerPluginsForAdUnits(bannerIdList);
        MoPub.LoadInterstitialPluginsForAdUnits(insterIdList);
        MoPub.LoadRewardedVideoPluginsForAdUnits(rvIdList);


#if !(UNITY_ANDROID || UNITY_IOS)
        Debug.LogError("Please switch to either Android or iOS platforms to run sample app!");
#endif

#if UNITY_EDITOR
        Debug.LogWarning("No SDK was loaded since this is not on a mobile device! Real ads will not load.");
#endif

        var nativeAdsGO = GameObject.Find("MoPubNativeAds");
        if (nativeAdsGO != null)
            nativeAdsGO.SetActive(false);
    }

    #endregion

    #region 广告状态修改
    public void AdLoaded(string adUnit)
    {
        _adUnitToLoadedMapping[adUnit] = true;
    }

    public void AdUnLoad(string adUnit)
    {
        _adUnitToLoadedMapping[adUnit] = false;
    }

    public void LoadAvailableRewards(string adUnitId, List<MoPub.Reward> availableRewards)
    {
        // Remove any existing available rewards associated with this AdUnit from previous ad requests
        _adUnitToRewardsMapping.Remove(adUnitId);

        if (availableRewards != null)
        {
            _adUnitToRewardsMapping[adUnitId] = availableRewards;
        }
    }

    public void BannerLoaded(string adUnitId, float height)
    {
        AdLoaded(adUnitId);
        _adUnitToShownMapping[adUnitId] = true;
    }



    public void PlayFail(string adUnit)
    {
        if (adUnit == currentRewardedVideoAdUnits)
        {
            PlayAction(RVPlayResult, 0);
        }
        else if (adUnit == currentInterstitialAdUnits)
        {
            PlayAction(InsertPlayResult, 0);
        }
    }

    public void PlayFinish(string adUnit)
    {
        if (adUnit == currentRewardedVideoAdUnits)
        {
            PlayAction(RVPlayResult, 1);
        }
        else if (adUnit == currentInterstitialAdUnits)
        {
            PlayAction(InsertPlayResult, 1);
        }
    }

    public void TryFetch(string adUnit)
    {
        if (adUnit == currentRewardedVideoAdUnits)
        {
            FetchRewardVideoAd();
        }
        else if (adUnit == currentInterstitialAdUnits)
        {
            FetchInsertAd();
        }
        else if (adUnit == currentBannerAdUnits)
        {
            RequestBanner();
        }
    }
    #endregion

    #region 隐私状态信息
    /// <summary>
    /// 自定义数据字段的默认文本 
    /// </summary>
    private static string _customDataDefaultText = "Optional custom data";

    /// <summary>
    /// 用于填充奖励视频的自定义数据的字符串 
    /// </summary>
    private string _rvCustomData = _customDataDefaultText;

    /// <summary>
    /// 为奖励的富媒体填充自定义数据的字符串 
    /// </summary>
    private string _rrmCustomData = _customDataDefaultText;

    /// <summary>
    /// 指示可以收集个人身份信息的标志 
    /// </summary>
    private bool _canCollectPersonalInfo = false;

    /// <summary>
    /// 此用户收集个人身份信息的当前同意状态 
    /// </summary>
    private MoPub.Consent.Status _currentConsentStatus = MoPub.Consent.Status.Unknown;

    /// <summary>
    /// 指示应获得同意以收集个人可识别信息的标志 
    /// </summary>
    private bool _shouldShowConsentDialog = false;

    /// <summary>
    /// 指示通用数据保护法规（GDPR）适用于此用户的标志 
    /// </summary>
    private bool? _isGdprApplicable = false;

    /// <summary>
    /// 指示发布者已强制应用通用数据保护法规（GDPR）的标志 
    /// </summary>
    private bool _isGdprForced = false;

    /// <summary>
    /// 用于跟踪当前状态的字符串 
    /// </summary>
    private string _status = string.Empty;
    public void UpdateConsentValues()
    {
        bool canCollect = PlayerPrefs.HasKey("privateState");
        if (canCollect)
        {
            canCollect = (PlayerPrefs.GetString("privateState") == "1");
        }

        _canCollectPersonalInfo = MoPub.CanCollectPersonalInfo;
        _currentConsentStatus = MoPub.CurrentConsentStatus;
        _shouldShowConsentDialog = MoPub.ShouldShowConsentDialog;
        _isGdprApplicable = MoPub.IsGdprApplicable;
    }

    /// <summary>
    /// 当前用户 是否需要展示 隐私界面
    /// </summary>
    /// <returns></returns>
    public bool NeedShowPrivateFirstUI()
    {
        bool state = false;

        if (!PlayerPrefs.HasKey("privateState"))
        {
            state = true;
        }

        return state;
    }
    /// <summary>
    /// 设置用户对隐私的选择状态
    /// </summary>
    /// <returns></returns>
    public void SetUserPrivateChoose(bool state)
    {
        PlayerPrefs.SetString("privateState", state ? "1" : "0");
        UpdateConsentValues();
    }

    #endregion

    #region Banner
    private string currentBannerAdUnits = string.Empty;
    /// <summary>
    /// 请求banner
    /// </summary>
    public void RequestBanner()
    {
        currentBannerAdUnits = GetBannerId();
        MoPub.CreateBanner(GetBannerId(), MoPub.AdPosition.BottomCenter);
    }

    /// <summary>
    /// 展示banner
    /// </summary>
    private void ShowBannerAd()
    {
        MoPub.ShowBanner(currentBannerAdUnits, true);
    }

    /// <summary>
    /// 隐藏banner
    /// </summary>
    private void HideBanner()
    {
        MoPub.ShowBanner(currentBannerAdUnits, false);
    }

    /// <summary>
    /// 摧毁banner
    /// </summary>
    private void DestoryBanner()
    {
        MoPub.DestroyBanner(currentBannerAdUnits);
    }

    private string GetBannerId()
    {
        return bannerIdList[0];
    }
    #endregion

    #region Insert

    private string currentInterstitialAdUnits = string.Empty;
    private static Action<int> InsertPlayResult = null;

    /// <summary>
    /// 开始缓冲插屏
    /// </summary>
    public void FetchInsertAd()
    {
        currentInterstitialAdUnits = GetInsertId();
        MoPub.RequestInterstitialAd(currentInterstitialAdUnits);
    }

    /// <summary>
    /// 插屏是否已经缓冲好了
    /// </summary>
    public bool InsertAdCanShow()
    {
        return _adUnitToLoadedMapping[currentInterstitialAdUnits];
    }

    /// <summary>
    /// 展示插屏
    /// </summary>
    public void ShowInsertAd(Action<int> playResult)
    {
        InsertPlayResult = playResult;

        if (insertWait>0)
        {
            insertWait = 0;
            PlayAction(InsertPlayResult, 0);
        }
        else
        {
            if (InsertAdCanShow())
            {
                MoPub.ShowInterstitialAd(currentInterstitialAdUnits);
                AdUnLoad(currentInterstitialAdUnits);
            }
            else
            {
                PlayAction(InsertPlayResult, 0);
            }
        }
    }

    private string GetInsertId()
    {
        return insterIdList[0];
    }

    #endregion

    private int insertWait = 0;
    #region RewardVideo

    private string currentRewardedVideoAdUnits = string.Empty;
    private static Action<int> RVPlayResult = null;

    /// <summary>
    /// 开始缓冲激励视频
    /// </summary>
    public void FetchRewardVideoAd()
    {
        currentRewardedVideoAdUnits = GetRVId();
        MoPub.RequestRewardedVideo(currentRewardedVideoAdUnits);
    }

    public bool RewardVideoAdCanShow()
    {
        return MoPub.HasRewardedVideo(currentRewardedVideoAdUnits);
    }

    /// <summary>
    /// 展示激励视频
    /// </summary>
    public void ShowRewardVideoAd(Action<int> playResult)
    {
        RVPlayResult = playResult;

        if (RewardVideoAdCanShow())
        {
            insertWait = 1;
            MoPub.ShowRewardedVideo(currentRewardedVideoAdUnits);
            AdUnLoad(currentRewardedVideoAdUnits);
        }
        else
        {
            PlayAction(RVPlayResult, 0);
        }
    }

    private string GetRVId()
    {
        return rvIdList[0];
    }

    #endregion


    private void PlayAction(Action<int> tempAction, int result)
    {
        if (tempAction != null)
        {
            tempAction(result);
        }
    }
}


//public enum BannerPosition
//{
//    Bottom,
//    Top
//}

//public enum InterstitialStatus
//{
//    Close,
//    Failed,
//    NotReady
//}

//public enum RewardVideoStatus
//{
//    Successed,
//    Failed,
//    NotReady
//}
