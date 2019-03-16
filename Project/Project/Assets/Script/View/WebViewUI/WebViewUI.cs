using UnityEngine;
using UnityEngine.UI;

public class WebViewUI : UIViewBase
{

    [SerializeField]
    private RectTransform BottomRect;

    [SerializeField]
    private GameObject ContentPrefab;

    private GameObject ViewContent;
    private UniWebView WebView;


    private bool IsOut = false;
    float bottomValue = 1;
    float topValue = 0;
    float nowScreenH = 1334;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(ViewData viewData)
    {
        SetInfo();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            if (IsOut)
            {
                WebView.Hide();
                IsOut = false;
                WebView.Show();
            }
        }
        else
        {
            WebView.Hide();
            IsOut = true;
        }
    }

    private void OnDestroy()
    {
        Destroy(ViewContent);
    }

    public void OnClickBack()
    {
        UIManager.Instance.CloseUI(this);
    }

    private void SetInfo()
    {
        CanvasScaler tempScaler = gameObject.GetComponentInParent<CanvasScaler>();

        if (tempScaler != null)
        {
            nowScreenH = (tempScaler.transform as RectTransform).sizeDelta.y;
        }

        ViewContent = GameObject.Instantiate(ContentPrefab, ContentPrefab.transform.parent);
        WebView = ViewContent.AddComponent<UniWebView>();

        bottomValue = BottomRect.rect.yMax / nowScreenH;
        //topValue = Screen.safeArea.yMin / (float)Screen.height;
        topValue = GetSafeyMin() / (float)Screen.height;
        WebView.insets = GetRect(topValue, 0, 0, bottomValue);

        WebView.url = "http://gg.3951.com/privacy/";
        WebView.loadOnStart = true;
        WebView.autoShowWhenLoadComplete = true;
        WebView.backButtonEnable = false;
    }


    private float GetSafeyMin()
    {
        float tempH = Screen.safeArea.yMin;
#if UNITY_ANDROID
        //AndroidJavaObject activity = new AndroidJavaClass("com.example.myjar.MyActivity").GetStatic<AndroidJavaObject>("currentActivity");
        //int realScreenH = activity.Call<int>("getScreentHeight");

        //if (Mathf.Abs(realScreenH - Screen.height) > 10)
        //{
        //    tempH = Mathf.Abs(realScreenH - Screen.height);
        //}
#elif UNITY_iOS
        tempH = Screen.safeArea.yMin;
#endif

        return tempH;
    }


    private static AndroidJavaObject audioManager = null;
    private static AndroidJavaClass UnityPlayer = null;
    private static AndroidJavaObject currentActivity = null;


    public UniWebViewEdgeInsets GetRect(float TopRate, float LeftRate, float RightRate, float BottomRate)
    {
        int Top = (int)(Screen.height * TopRate);
        int Left = (int)(Screen.width * LeftRate);
        int Bottom = (int)(Screen.height * BottomRate);
        int Right = (int)(Screen.width * RightRate);
        UniWebViewEdgeInsets WebRect = new UniWebViewEdgeInsets(Top, Left, Bottom, Right);
        return WebRect;
    }

    private void Log()
    {
        
        Debug.LogError("UniWebViewHelper.screenHeight:   " + UniWebViewHelper.screenHeight.ToString());
        Debug.LogError("bottomValue:   " + bottomValue.ToString());
        Debug.LogError("nowScreenH:   " + nowScreenH.ToString());
        Debug.LogError("BottomRect.rect.yMax:   " + BottomRect.rect.yMax.ToString());
        Debug.LogError("Screen.height:   " + Screen.height.ToString());
        Debug.LogError("Screen.safeArea.size.y:   " + Screen.safeArea.size.y.ToString());
        Debug.LogError("Screen.safeArea.yMin:   " + Screen.safeArea.yMin.ToString());
        Debug.LogError("Screen.safeArea.y:   " + Screen.safeArea.y.ToString());
    }
}
