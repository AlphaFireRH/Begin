using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : SingleMono<SaveController>
{
    /// <summary>
    /// 当前所有回调
    /// </summary>
    private List<Action> actionList = new List<Action>();

    /// <summary>
    /// The current coroutine.
    /// </summary>
    private Coroutine curCoroutine = null;

    /// <summary>
    /// 等待60秒
    /// </summary>
    private WaitForSeconds wait = new WaitForSeconds(60);

    public void Awake()
    {
        RegisterCallback();
    }

    /// <summary>
    /// Registers the callback.
    /// </summary>
    private void RegisterCallback()
    {
        if (curCoroutine != null)
        {
            StopCoroutine(curCoroutine);
            curCoroutine = null;
        }

        curCoroutine = StartCoroutine(DelayCall());
    }

    /// <summary>
    /// 延迟回调
    /// </summary>
    /// <returns>The call.</returns>
    private IEnumerator DelayCall()
    {
        do
        {
            yield return wait;
            CallBackAction();
        } while (true);
    }

    /// <summary>
    /// 注册定期存档回调
    /// </summary>
    /// <param name="cur">Current.</param>
    public void Register(Action cur)
    {
        if (!actionList.Contains(cur) && cur != null)
        {
            actionList.Add(cur);
        }
    }

    /// <summary>
    /// 注销定期存档回调
    /// </summary>
    /// <param name="cur">Current.</param>
    public void Unregister(Action cur)
    {
        if (actionList.Count > 0 && cur != null && actionList.Contains(cur))
        {
            actionList.Remove(cur);
        }
    }

    /// <summary>
    /// 失去焦点回调
    /// </summary>
    /// <param name="pause">If set to <c>true</c> pause.</param>
    public void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            CallBackAction();
        }
    }

    /// <summary>
    /// 存档回调
    /// </summary>
    private void CallBackAction()
    {
        if (actionList == null)
        {
            actionList = new List<Action>();
        }
        for (int i = actionList.Count - 1; i >= 0; i--)
        {
            var item = actionList[i];
            if (item == null)
            {
                actionList.Remove(item);
            }
            else
            {
                item();
            }
        }
    }


}
