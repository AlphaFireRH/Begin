using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayUI : UIViewBase, IPlayUIController
{
    #region 预设

    /// <summary>
    /// 地块预设
    /// </summary>
    [SerializeField]
    private GameObject mapItem;

    /// <summary>
    /// 地块预设
    /// </summary>
    [SerializeField]
    private GameObject mapBGItem;

    /// <summary>
    /// 玩法控制器
    /// </summary>
    private IPlayController playCtrl;

    #endregion


    #region 挂点

    /// <summary>
    /// 挂点
    /// </summary>
    [SerializeField]
    private RectTransform point;

    /// <summary>
    /// 挂点
    /// </summary>
    [SerializeField]
    private RectTransform bgPoint;
    #endregion

    #region 组件
    /// <summary>
    /// 地图块
    /// </summary>
    private Dictionary<int, MapGrid> mapGrids = new Dictionary<int, MapGrid>();

    /// <summary>
    /// 
    /// </summary>
    private List<RectTransform> mapBgs = new List<RectTransform>();
    #endregion

    /// <summary>
    /// 当前地图信息
    /// </summary>
    private MapData mapData;

    #region 玩法接口
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(IPlayController IPlayController)
    {
        playCtrl = IPlayController;
        InitMapBG();
        InitMap();

    }
    #endregion

    #region 初始化
    /// <summary>
    /// 初始化地图
    /// </summary>
    private void InitMap()
    {
        CleanMap();
        mapData = playCtrl.GetMapDatas();
        InitMapBG();
        RefreshMap(mapData.gridDatas);
    }

    public void InitMapBG()
    {
        for (int i = 1; i <= playCtrl.MapSize(); i++)
        {
            for (int j = 1; j <= playCtrl.MapSize(); j++)
            {
                var bg = GameObject.Instantiate(mapBGItem);
                RectTransform bgRectTransform = bg.GetComponent<RectTransform>();
                bgRectTransform.SetParent(bgPoint);
                bgRectTransform.localScale = Vector3.one;
                //bgRectTransform.localPosition = Vector3.zero;
                bgRectTransform.localRotation = Quaternion.identity;
                bgRectTransform.anchoredPosition = MapTool.GetPosition(i, j);
                mapBgs.Add(bgRectTransform);
            }
        }
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

    private void CleanMap()
    {
        for (int i = 0; i < mapBgs.Count; i++)
        {
            Destroy(mapBgs[i].gameObject);
        }
        mapBgs.Clear();

        foreach (var item in mapGrids)
        {
            item.Value.MyDestroy();
        }
        mapGrids.Clear();
    }
}