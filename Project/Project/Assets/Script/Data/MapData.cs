using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图数据
/// </summary>
public class MapData
{
    /// <summary>
    /// 上一次的方向
    /// </summary>
    public PlayerOperate lastMoveDirection;

    /// <summary>
    /// 地块信息
    /// </summary>
    public List<GridData> gridDatas = new List<GridData>();

    /// <summary>
    /// 克隆数据
    /// </summary>
    /// <returns></returns>
    public MapData Clone()
    {
        MapData mapData = new MapData();
        mapData.lastMoveDirection = lastMoveDirection;
        mapData.gridDatas = new List<GridData>();
        if (gridDatas != null)
        {
            for (int i = 0; i < gridDatas.Count; i++)
            {
                mapData.gridDatas.Add(gridDatas[i].Clone());
            }
        }
        return mapData;
    }
}
