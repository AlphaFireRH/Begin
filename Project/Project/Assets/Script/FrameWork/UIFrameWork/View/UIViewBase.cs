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
    public void Init(ViewData viewData)
    {
        ViewID = viewData.ViewID;
        UIType = viewData.UIType;
    }

    /// <summary>
    /// 播放展示动画
    /// </summary>
    public void PlayShowAnimation()
    {
        //if (finishAction != null)
        //{
        //    finishAction();
        //}
    }

    /// <summary>
    /// 关闭
    /// </summary>
    public void Close()
    {

    }

    /// <summary>
    /// 播放隐藏动画
    /// </summary>
    public void PlayHideAnimation()
    {
        //if (finishAction != null)
        //{
        //    finishAction();
        //}
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
