using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    /// <summary>
    /// 触控工具
    /// </summary>
    [SerializeField]
    private TouchTool touchTool;

    /// <summary>
    /// 回退按钮
    /// </summary>
    [SerializeField]
    private Button GobackBtn;

    /// <summary>
    /// 爆炸按钮
    /// </summary>
    [SerializeField]
    private Button BoomBtn;

    /// <summary>
    /// 爆炸按钮
    /// </summary>
    [SerializeField]
    private Button SettingBtn;

    /// <summary>
    /// 地图块
    /// </summary>
    private Dictionary<int, MapGrid> mapGrids = new Dictionary<int, MapGrid>();

    /// <summary>
    /// 所有地块背景
    /// </summary>
    private List<RectTransform> mapBgs = new List<RectTransform>();
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

    #region 数据
    /// <summary>
    /// 当前地图信息
    /// </summary>
    private MapData mapData;
    #endregion

    #region 初始化
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(IPlayController IPlayController)
    {
        playCtrl = IPlayController;
        InitMapBG();
        InitMap();

    }

    /// <summary>
    /// 初始化地图
    /// </summary>
    private void InitMap()
    {
        BindBtn();
        InitMapBG();
    }

    /// <summary>
    /// 绑定按钮事件
    /// </summary>
    private void BindBtn()
    {
        touchTool.UngegisterCallBack(PlayOperate);
        touchTool.RegisterCallBack(PlayOperate);

        BoomBtn.onClick.RemoveAllListeners();
        BoomBtn.onClick.AddListener(() =>
        {
            mapData = playCtrl.UseBoom();
            RefreshMap(mapData.gridDatas, false);
        });

        SettingBtn.onClick.RemoveAllListeners();
        SettingBtn.onClick.AddListener(() =>
        {
            playCtrl.StartGame();
            //mapData = playCtrl.UseBoom();
            //RefreshMap(mapData.gridDatas, false);
        });


        GobackBtn.onClick.RemoveAllListeners();
        GobackBtn.onClick.AddListener(() =>
        {
            mapData = playCtrl.UseGoBack();
            RefreshMap(mapData.gridDatas, false);
        });
    }

    /// <summary>
    /// 
    /// </summary>
    public void InitMapBG()
    {
        CleanMap();
        mapData = playCtrl.GetMapDatas();

        for (int i = 1; i <= playCtrl.MapSize(); i++)
        {
            for (int j = 1; j <= playCtrl.MapSize(); j++)
            {
                var bg = GameObject.Instantiate(mapBGItem);
                RectTransform bgRectTransform = bg.GetComponent<RectTransform>();
                bgRectTransform.SetParent(bgPoint);
                bgRectTransform.localScale = Vector3.one;
                bgRectTransform.localRotation = Quaternion.identity;
                bgRectTransform.anchoredPosition = MapTool.GetPosition(i, j);
                mapBgs.Add(bgRectTransform);
            }
        }
        RefreshMap(mapData.gridDatas, false);
    }
    #endregion

    #region 刷新
    /// <summary>
    /// 刷新地图
    /// </summary>
    /// <param name="mapDatas">Map datas.</param>
    private void RefreshMap(List<GridData> gridData, bool showAnimation = true)
    {
        if (gridData != null && gridData.Count > 0)
        {
            for (int i = 0; i < gridData.Count; i++)
            {
                bool isNew = false;
                if (mapGrids == null)
                {
                    mapGrids = new Dictionary<int, MapGrid>();
                }
                if (!mapGrids.ContainsKey(gridData[i].ID))
                {
                    isNew = true;
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
                mapGrids[gridData[i].ID].RefreshData(gridData[i], ref mapGrids, isNew, showAnimation);
            }
        }

        List<int> allDeleteID = new List<int>();

        foreach (var item in mapGrids)
        {
            bool isContain = false;
            for (int i = 0; i < gridData.Count; i++)
            {
                if (gridData[i].ID == item.Value.GetGridData().ID)
                {
                    isContain = true;
                    break;
                }
            }
            if (!isContain)
            {
                allDeleteID.Add(item.Value.GetGridData().ID);
            }
        }
        for (int i = 0; i < allDeleteID.Count; i++)
        {
            mapGrids[allDeleteID[i]].DoDestroy();
            mapGrids.Remove(allDeleteID[i]);
        }
    }

    #endregion

    #region 其他
    /// <summary>
    /// 是否正在播放动画
    /// </summary>
    /// <returns></returns>
    private bool IsHasItemPlayAnimation()
    {
        if (mapGrids != null && mapGrids.Count > 0)
        {
            foreach (var item in mapGrids)
            {
                if (item.Value.IsPlayAnimation())
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 玩家操作回调
    /// </summary>
    /// <param name="playerOperate"></param>
    private void PlayOperate(PlayerOperate playerOperate)
    {
        if (!IsHasItemPlayAnimation())
        {
            switch (playerOperate)
            {
                case PlayerOperate.ToLeft:
                case PlayerOperate.ToRight:
                case PlayerOperate.ToUp:
                case PlayerOperate.ToDown:
                    mapData = playCtrl.Move(playerOperate);
                    RefreshMap(mapData.gridDatas);
                    break;
            }
        }
    }

    /// <summary>
    /// 销毁
    /// </summary>
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
    #endregion

}