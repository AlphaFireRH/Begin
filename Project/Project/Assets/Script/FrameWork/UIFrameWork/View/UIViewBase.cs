using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIViewBase : MonoBehaviour
{
    /// <summary>
    /// 界面ID
    /// </summary>
    public ViewID ViewID { get; private set; }

    /// <summary>
    /// 界面类型
    /// </summary>
    public UIType UIType { get; private set; }

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init(ViewData viewData)
    {
        ViewID = viewData.ViewID;
        UIType = viewData.UIType;
        PlayShowAnimation();
    }

    /// <summary>
    /// 播放展示动画
    /// </summary>
    private void PlayShowAnimation()
    {

        ShowAnimation showAnimation = gameObject.GetComponentInChildren<ShowAnimation>();
        if (showAnimation == null)
        {
            showAnimation = gameObject.GetComponent<ShowAnimation>();
        }
        if (showAnimation != null)
        {
            showAnimation.Show(ShowAnimaitionFinish);
        }
        else
        {
            ShowAnimaitionFinish();
        }
    }

    /// <summary>
    /// Shows the animaition finish.
    /// </summary>
    public virtual void ShowAnimaitionFinish()
    {

    }

    /// <summary>
    /// Hides the animation finish.
    /// </summary>
    public virtual void HideAnimationFinish()
    {

    }

    /// <summary>
    /// 关闭
    /// </summary>
    public void Close()
    {
        UIManager.Instance.CloseUI(ViewID);
    }

    /// <summary>
    /// 播放隐藏动画
    /// </summary>
    public void PlayHideAnimation()
    {
        HIdeAnimation hideAnimation = gameObject.GetComponentInChildren<HIdeAnimation>();
        if (hideAnimation == null)
        {
            hideAnimation = gameObject.GetComponent<HIdeAnimation>();
        }
        if (hideAnimation != null)
        {
            hideAnimation.Hide(() =>
            {
                HideAnimationFinish();
                MyDestroy();
            });
        }
        else
        {
            HideAnimationFinish();
            MyDestroy();
        }
    }

    /// <summary>
    /// Mies the destroy.
    /// </summary>
    private void MyDestroy()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}

/// <summary>
/// 弹窗界面
/// </summary>
public class ViewData
{
    /// <summary>
    /// 界面ID
    /// </summary>
    public ViewID ViewID;

    /// <summary>
    /// 界面类型
    /// </summary>
    public UIType UIType;
}
