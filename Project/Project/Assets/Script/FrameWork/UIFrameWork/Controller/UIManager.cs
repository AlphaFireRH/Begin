using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    public static UIManager Instance { get; private set; }

    /// <summary>
    /// 展示窗队列
    /// </summary>
    private List<UIViewBase> UIViews = new List<UIViewBase>();

    /// <summary>
    /// 展示节点
    /// </summary>
    [SerializeField]
    private RectTransform showPoint;

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 弹窗
    /// </summary>
    /// <param name="viewID"></param>
    public UIViewBase ShowUI(ViewID viewID)
    {
        UIViewBase ret = null;
        ViewData viewData = new ViewData();
        string path = "";
        switch (viewID)
        {
            case ViewID.PlayWindow:
                {
                    path = "UIPrefab/PlayWindowUI/PlayUI";
                }
                break;
            case ViewID.CompleteUI:
                {
                    path = "UIPrefab/PlayWindowUI/CompleteUI";
                }
                break;
            case ViewID.PrivateUI:
                {
                    path = "UIPrefab/PlayWindowUI/PrivateUI";
                }
                break;
            case ViewID.SettingUI:
                {
                    path = "UIPrefab/PlayWindowUI/SettingUI";
                }
                break;
            case ViewID.WebViewUI:
                {
                    path = "UIPrefab/PlayWindowUI/WebViewUI";
                }
                break;
            default:break;
        }
        //防止有空异常
        if (!string.IsNullOrEmpty(path))
        {
            var prefabObj = Resources.Load(path);
            if (prefabObj != null)
            {
                GameObject prefab = prefabObj as GameObject;
                GameObject obj = GameObject.Instantiate(prefab);
                obj.name = prefab.name;
                ret = obj.GetComponent<UIViewBase>();

                obj.transform.SetParent(showPoint);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;

                RectTransform rect = obj.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.offsetMin = Vector2.zero;
                    rect.offsetMax = Vector2.zero;
                }

                if (ret != null)
                {
                    ret.Init(viewData);
                    UIViews.Add(ret);
                }
                else
                {
                    Destroy(obj);
                }
            }
        }
        return ret;
    }

    /// <summary>
    /// 关闭弹窗
    /// </summary>
    /// <param name="viewID"></param>
    public void CloseUI(ViewID viewID)
    {
        for (int i = UIViews.Count - 1; i >= 0; i--)
        {
            if (UIViews[i].ViewID == viewID)
            {
                CloseUI(UIViews[i]);
            }
        }
    }

    /// <summary>
    /// 关闭弹窗
    /// </summary>
    /// <param name="viewID"></param>
    public void CloseUI(UIViewBase view)
    {
        if (UIViews.Contains(view))
        {
            UIViews.Remove(view);
            view.Close();
        }
    }
}