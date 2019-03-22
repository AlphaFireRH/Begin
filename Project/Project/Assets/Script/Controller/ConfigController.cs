using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConfigController : SingleMono<ConfigController>
{
    private Dictionary<int, GridConfigData> gridConfigDataDic;

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        if (gridConfigDataDic == null)
        {
            gridConfigDataDic = new Dictionary<int, GridConfigData>();
            TextAsset textAsset = Resources.Load<TextAsset>("Config/Config");
            string[] items = textAsset.text.Replace("\r", "").Split('\n');
            for (int i = 1; i < items.Length; i++)
            {
                if (!string.IsNullOrEmpty(items[i]))
                {
                    string[] infos = items[i].Split(',');
                    GridConfigData gridConfigData = new GridConfigData();
                    gridConfigData.Ladder = Convert.ToInt32(infos[0]);
                    gridConfigData.ShowInfo = infos[1];
                    //gridConfigData.GridColor =new Color( Convert.ToInt32(infos[2]);
                    {
                        int r = System.Convert.ToInt32("0x" + infos[2][0] + infos[2][1], 16);
                        int g = System.Convert.ToInt32("0x" + infos[2][2] + infos[2][3], 16);
                        int b = System.Convert.ToInt32("0x" + infos[2][4] + infos[2][5], 16);
                        gridConfigData.GridColor = new Color(r / 255f, g / 255f, b / 255f);
                    }
                    var createDatas = infos[3].Split('|');
                    gridConfigData.CreateMin = new List<int>();
                    for (int j = 0; j < createDatas.Length; j++)
                    {
                        gridConfigData.CreateMin.Add(Convert.ToInt32(createDatas[j]));
                    }
                    {
                        int r = System.Convert.ToInt32("0x" + infos[4][0] + infos[4][1], 16);
                        int g = System.Convert.ToInt32("0x" + infos[4][2] + infos[4][3], 16);
                        int b = System.Convert.ToInt32("0x" + infos[4][4] + infos[4][5], 16);
                        gridConfigData.FontColor = new Color(r / 255f, g / 255f, b / 255f);
                    }
                    gridConfigDataDic[gridConfigData.Ladder] = gridConfigData;
                }
            }
            //int index = 1;
            //do
            //{
            //    if (!gridConfigDataDic.ContainsKey(index))
            //    {
            //        break;
            //    }
            //    index++;
            //} while (true);
            //Debug.Log(index);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ladder">Ladder.</param>
    public GridConfigData GetGridConfigData(int ladder)
    {
        if (gridConfigDataDic.ContainsKey(ladder))
        {
            return gridConfigDataDic[ladder];
        }
        return null;
    }


}