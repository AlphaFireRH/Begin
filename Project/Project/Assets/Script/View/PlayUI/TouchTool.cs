using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TouchType
{
    /// <summary>
    /// 传统
    /// </summary>
    Normal = 0,
    /// <summary>
    /// 半自动
    /// </summary>
    Auto = 1,
}

public class TouchTool : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    //private TouchType touchType = TouchType.Auto;

    /// <summary>
    /// 屏幕距离
    /// </summary>
    private float xNormalDistance = 0.06f;

    /// <summary>
    /// 屏幕距离
    /// </summary>
    private float yNormalDistance = 0.03f;

    /// <summary>
    /// 屏幕距离
    /// </summary>
    private float xAutoDistance = 0.08f;

    /// <summary>
    /// 屏幕距离
    /// </summary>
    private float yAutoDistance = 0.06f;

    /// <summary>
    /// 计算距离
    /// </summary>
    private float moveDistance = 140f * 0.75f;

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

    private void SendDir(Vector2 delaMove)
    {

        float dictance = 0;
        //if (GameController.Instance.TouchType == TouchType.Normal)
        //{
        //    dictance = Mathf.Min(Mathf.Max(0, xNormalDistance) * Screen.width, Mathf.Max(0, yNormalDistance) * Screen.height);
        //}
        //else if (GameController.Instance.TouchType == TouchType.Auto)
        //{
        //    dictance = Mathf.Min(Mathf.Max(0, xAutoDistance) * Screen.width, Mathf.Max(0, yAutoDistance) * Screen.height);
        //}
        if (Screen.height / Screen.width > 1334f / 750f)
        {
            dictance = Screen.width * moveDistance / 750f;
        }
        else
        {
            dictance = Screen.height * moveDistance / 1334f;
        }
        //if (log)
        //{
        //    Debug.Log(dictance + "  " + delaMove.x);
        //}

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
        if (GameController.Instance.TouchType == TouchType.Auto && finishTouch == false)
        {
            var delaMove = eventData.position - pointerEventDataPosition;
            SendDir(delaMove);

        }
    }
}
