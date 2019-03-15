using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TouchType
{
    Normal,
    Auto,
}

public class TouchTool : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private TouchType touchType;

    /// <summary>
    /// 屏幕距离
    /// </summary>
    private float xDistance = 0.06f;

    /// <summary>
    /// 屏幕距离
    /// </summary>
    private float yDistance = 0.03f;

    /// <summary>
    /// 这一次按下的屏幕坐标
    /// </summary>
    private Vector2 pointerEventDataPosition;

    /// <summary>
    /// 当前回调
    /// </summary>
    private Action<PlayerOperate> curCall;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="callBack"></param>
    public void RegisterCallBack(Action<PlayerOperate> callBack)
    {
        if (callBack != null)
        {
            curCall += callBack;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="callBack"></param>
    public void UngegisterCallBack(Action<PlayerOperate> callBack)
    {
        if (callBack != null)
        {
            curCall -= callBack;
        }
    }

    /// <summary>
    /// 实现接口
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        finishTouch = false;
        pointerEventDataPosition = eventData.position;
    }


    /// <summary>
    /// 实现接口
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!finishTouch)
        {
            var delaMove = eventData.position - pointerEventDataPosition;

            SendDir(delaMove);
        }
    }

    private void SendDir(Vector2 curPosition)
    {
        var delaMove = curPosition - pointerEventDataPosition;

        float dictance = Mathf.Min(Mathf.Max(0, xDistance) * Screen.width, Mathf.Max(0, yDistance) * Screen.height);
        PlayerOperate operate = PlayerOperate.None;
        if ((Mathf.Abs(delaMove.x) >= dictance || Mathf.Abs(delaMove.y) >= dictance))
        {
            if (Mathf.Abs(delaMove.x) > Mathf.Abs(delaMove.y))
            {
                if (delaMove.x > 0)
                {
                    operate = PlayerOperate.ToRight;
                }
                else
                {
                    operate = PlayerOperate.ToLeft;
                }
            }
            else
            {
                if (delaMove.y > 0)
                {
                    operate = PlayerOperate.ToUp;
                }
                else
                {
                    operate = PlayerOperate.ToDown;
                }
            }
        }

        if (curCall != null && operate != PlayerOperate.None)
        {
            finishTouch = true;
            curCall(operate);
        }
    }

    private bool finishTouch = true;

    public void OnDrag(PointerEventData eventData)
    {
        if (touchType == TouchType.Auto && finishTouch == false)
        {
            var delaMove = eventData.position - pointerEventDataPosition;
            SendDir(delaMove);

        }
    }
}
