using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    /// <summary>
    /// 道具数量
    /// </summary>
    public Dictionary<int, int> itemDic = new Dictionary<int, int>();

    /// <summary>
    /// 当前地图
    /// </summary>
    public MapData mapData = null;

    /// <summary>
    /// 历史低于
    /// </summary>
    public List<MapData> historyMap = null;

    /// <summary>
    /// 最高分数
    /// </summary>
    public MyInt MaxstringScore;

    /// <summary>
    /// 
    /// </summary>
    public bool isOpenMusic;

    /// <summary>
    /// 
    /// </summary>
    public bool isOpenSound;

    /// <summary>
    /// The type of the touch.
    /// </summary>
    public TouchType touchType;
}
