using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    private bool isLaunch = false;

    /// <summary>
    /// Start this instance.
    /// </summary>
    // Start is called before the firt frame update
    void Start()
    {
        if (!isLaunch)
        {
            GameController.Instance.Init();
            isLaunch = true;
        }
    }

}
