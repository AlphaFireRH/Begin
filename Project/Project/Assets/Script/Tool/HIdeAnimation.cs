using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class HIdeAnimation : MonoBehaviour
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

    public void Hide(Action action)
    {
        if (rect == null)
        {
            rect = gameObject.GetComponent<RectTransform>();
        }

        if (rect != null)
        {
            curCall = action;
            rect.localScale = Vector3.one;
            tweener = rect.DOScale(Vector3.zero, 0.2f);
            tweener.onComplete += () =>
            {
                if (curCall != null)
                {
                    curCall();
                    curCall = null;
                }
            };
        }
    }

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
