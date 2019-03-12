using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayUI : UIViewBase, IPlayUIController
{
    /// <summary>
    /// 地块预设
    /// </summary>
    [SerializeField]
    private GameObject mapItem;

    /// <summary>
    /// 玩法控制器
    /// </summary>
    private IPlayController playCtrl;

    /// <summary>
    /// 挂点
    /// </summary>
    [SerializeField]
    private RectTransform point;

    /// <summary>
    /// 地图块
    /// </summary>
    private Dictionary<int, MapGrid> mapGrids = new Dictionary<int, MapGrid>();

    /// <summary>
    /// 
    /// </summary>
    private MapData mapData;

    #region 玩法接口
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(IPlayController IPlayController)
    {
        playCtrl = IPlayController;
        InitMap();

    }
    #endregion

    #region 初始化
    /// <summary>
    /// 初始化地图
    /// </summary>
    private void InitMap()
    {
        mapData = playCtrl.GetMapDatas();
        RefreshMap(mapData.gridDatas);
    }

    /// <summary>
    /// 刷新地图
    /// </summary>
    /// <param name="mapDatas">Map datas.</param>
    private void RefreshMap(List<GridData> gridData)
    {
        if (gridData != null && gridData.Count > 0)
        {
            for (int i = 0; i < gridData.Count; i++)
            {
                if (!mapGrids.ContainsKey(gridData[i].ID))
                {
                    GameObject obj = Instantiate(mapItem);
                    obj.name = mapItem.name + "_" + gridData[i].ID;
                    RectTransform rectTransform = obj.GetComponent<RectTransform>();
                    rectTransform.SetParent(point);
                    rectTransform.localScale = Vector3.one;
                    rectTransform.localPosition = Vector3.zero;
                    rectTransform.localRotation = Quaternion.identity;
                    MapGrid grid = obj.GetComponent<MapGrid>();
                    mapGrids.Add(gridData[i].ID, grid);
                }
                mapGrids[gridData[i].ID].RefreshData(gridData[i], mapGrids);
            }
        }

    }
    #endregion
}