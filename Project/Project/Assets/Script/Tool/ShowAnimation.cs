using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShowAnimation : MonoBehaviour
{


    /// <summary>
    /// 动画节点
    /// </summary>
    private RectTransform rect;

    /// <summary>
    /// 动画
    /// </summary>
    private Tweener tweener;

    private Action curCall;

    /// <summary>
    /// Show the specified action.
    /// </summary>
    /// <param name="action">Action.</param>
    public void Show(Action action)
    {
        if (rect == null)
        {
            rect = gameObject.GetComponent<RectTransform>();
        }

        if (rect != null)
        {
            curCall = action;
            rect.localScale = Vector3.zero;
            tweener = rect.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
            tweener.onComplete += () =>
            {
                if (tweener != null)
                {
                    tweener = rect.DOScale(Vector3.one, 0.1f);
                    if (curCall != null)
                    {
                        curCall();
                        curCall = null;
                    }
                }
            };
        }
    }

    /// <summary>
    /// Stop this instance.
    /// </summary>
    public void Stop()
    {
        if (tweener != null)
        {
            tweener.Kill();
            tweener = null;
            if (curCall != null)
            {
                curCall();
                curCall = null;
            }
        }
    }
}
