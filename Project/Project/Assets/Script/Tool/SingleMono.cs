using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMono<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    private static T instace;

    /// <summary>
    /// 获取单例
    /// </summary>
    /// <value>The instance.</value>
    public static T Instance
    {
        get
        {
            if (instace == null)
            {
                var go = new GameObject();
                go.name = typeof(T).Name;
                DontDestroyOnLoad(go);
                instace = go.AddComponent<T>();
            }
            return instace;
        }
    }
}
