﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private bool init = false;

    private void OnGUI()
    {
        if (!init)
        {
            if (GUILayout.Button("Init", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.08f)))
            {
                AdController.Instance.Init();
                init = true;

                StartCoroutine(WaitBanner());
                StartCoroutine(WaitBeginAd());
            }
        }
        else
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);
            if (GUILayout.Button("RequestBanner", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.08f)))
            {
                AdController.Instance.RequestBanner();
            }
            GUILayout.Space(10);
            //if (GUILayout.Button("HideBanner", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.08f)))
            //{
            //    AdController.Instance.HideBanner();
            //}
            GUILayout.EndHorizontal();



            GUILayout.Space(10);



            GUILayout.BeginHorizontal();
            if (GUILayout.Button("FetchInsertAd", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.08f)))
            {
                AdController.Instance.FetchInsertAd();
            }
            GUILayout.Space(10);
            GUILayout.Label(AdController.Instance.InsertAdCanShow().ToString());
            GUILayout.Space(10);
            if (GUILayout.Button("ShowInsertAd", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.08f)))
            {
                AdController.Instance.ShowInsertAd((int result)=> {
                    Debug.Log("Insert:   "+ result.ToString());
                });
            }
            GUILayout.EndHorizontal();



            GUILayout.Space(10);



            GUILayout.BeginHorizontal();
            if (GUILayout.Button("FetchRewardVideoAd", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.08f)))
            {
                AdController.Instance.FetchRewardVideoAd();
            }
            GUILayout.Space(10);
            GUILayout.Label(AdController.Instance.RewardVideoAdCanShow().ToString());
            GUILayout.Space(10);
            if (GUILayout.Button("ShowRewardVideoAd", GUILayout.Width(Screen.width * 0.2f), GUILayout.Height(Screen.height * 0.08f)))
            {
                AdController.Instance.ShowRewardVideoAd((int result) => {
                    Debug.Log("RewardVideo:   " + result.ToString());
                });
            }
            GUILayout.EndHorizontal();
        }
    }

    WaitForSeconds loadWait = new WaitForSeconds(3.0f);
    IEnumerator WaitBanner()
    {
        yield return loadWait;

        AdController.Instance.RequestBanner();
    }

    IEnumerator WaitBeginAd()
    {
        yield return loadWait;

        FretchAd();
    }

    private void FretchAd()
    {
        AdController.Instance.FetchInsertAd();

        AdController.Instance.FetchRewardVideoAd();
    }
}
