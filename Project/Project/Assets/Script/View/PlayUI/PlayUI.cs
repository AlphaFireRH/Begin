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
    /// 最高分数
    /// </summary>
    [SerializeField]
    private Text maxScore;

    /// <summary>
    /// 当前分数
    /// </summary>
    [SerializeField]
    private Text curScore;

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
    private Button gobackBtn;

    /// <summary>
    /// 爆炸按钮
    /// </summary>
    [SerializeField]
    private Button boomBtn;

    /// <summary>
    /// 回退按钮
    /// </summary>
    [SerializeField]
    private Image gobackBtnBG;

    /// <summary>
    /// 爆炸按钮
    /// </summary>
    [SerializeField]
    private Image boomBtnBG;


    /// <summary>
    /// 回退按钮
    /// </summary>
    [SerializeField]
    private Image gobackBtnIcon;

    /// <summary>
    /// 爆炸按钮
    /// </summary>
    [SerializeField]
    private Image boomBtnIcon;

    /// <summary>
    /// 适配脚本
    /// </summary>
    [SerializeField]
    private AdaptMove adapt;

    /// <summary>
    /// 爆炸按钮
    /// </summary>
    [SerializeField]
    private Button settingBtn;

    [SerializeField]
    private GameObject boomRVPoint;
    [SerializeField]
    private GameObject boomCountPoint;
    [SerializeField]
    private Text boomCountTxt;
    [SerializeField]
    private GameObject gobackRVPoint;
    [SerializeField]
    private GameObject gobackCountPoint;
    [SerializeField]
    private Text gobackCountTxt;
    /// <summary>
    /// 地图块
    /// </summary>
    private Dictionary<int, MapGrid> mapGrids = new Dictionary<int, MapGrid>();

    /// <summary>
    /// 所有地块背景
    /// </summary>
    private List<RectTransform> mapBgs = new List<RectTransform>();

    [SerializeField]
    private List<Sprite> gobackStateSprite;
    [SerializeField]
    private List<Sprite> boomStateSprite;
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
    /// <summary>
    /// 
    /// </summary>
    private float timer;

    #endregion

    #region 初始化
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(IPlayController IPlayController)
    {
        playCtrl = IPlayController;
        playCtrl.Resister(this);
        InitMapBG();
        BindBtnEvent();
        InitMapBG();
        adapt.SetBanner(BannerType.Down);
        RefreshItemCount();
        RefreshBtnState();
    }

    /// <summary>
    /// 绑定按钮事件
    /// </summary>
    private void BindBtnEvent()
    {
        touchTool.UngegisterCallBack(PlayOperate);
        touchTool.RegisterCallBack(PlayOperate);

        boomBtn.onClick.RemoveAllListeners();
        boomBtn.onClick.AddListener(() =>
        {
            //RefreshBtnState();
            if (GameController.Instance.GetGameState() == GameState.Play)
            {
                AudioController.Instance.PlaySound(AudioType.click);
                if (playCtrl.IsCanUseBoom())
                {
                    playCtrl.UseBoom(
                        (MapData m) =>
                        {
                            mapData = m;
                            RefreshMap(m.gridDatas, false);
                            RefreshItemCount();
                            RefreshBtnState();
                        }
                    );
                }
            }
        });

        settingBtn.onClick.RemoveAllListeners();
        settingBtn.onClick.AddListener(() =>
        {
            if (GameController.Instance.GetGameState() == GameState.Play)
            {
                AudioController.Instance.PlaySound(AudioType.click);
                UIManager.Instance.ShowUI(ViewID.SettingUI);
            }
        });


        gobackBtn.onClick.RemoveAllListeners();
        gobackBtn.onClick.AddListener(() =>
        {
            //RefreshBtnState();
            if (GameController.Instance.GetGameState() == GameState.Play)
            {
                AudioController.Instance.PlaySound(AudioType.click);
                if (playCtrl.IsCanUseGoBack())
                {
                    playCtrl.UseGoBack(
                        (MapData m) =>
                        {
                            mapData = m;
                            RefreshMap(m.gridDatas, false);
                            RefreshItemCount();
                            RefreshBtnState();
                        }
                    );
                }
            }
        });
    }

    /// <summary>
    /// 外部刷新接口
    /// </summary>
    public void Refresh()
    {
        mapData = playCtrl.GetMapDatas();
        RefreshMap(mapData.gridDatas, false);
    }

    /// <summary>
    /// 初始化地图
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

    public void RefreshBtnState()
    {
        bool isCanShowRV = AdController.Instance.RewardVideoAdCanShow();
        if (isCanShowRV || GameController.Instance.GetItemCount(ItemID.Boom) > 0)
        {
            boomBtnIcon.sprite = boomStateSprite[0];
            boomBtnBG.color = new Color(1, 1, 1, 1);
        }
        else
        {
            boomBtnIcon.sprite = boomStateSprite[1];
            boomBtnBG.color = new Color(1, 1, 1, 0.3f);
        }

        if (isCanShowRV || GameController.Instance.GetItemCount(ItemID.Goback) > 0)
        {
            gobackBtnIcon.sprite = gobackStateSprite[0];
            gobackBtnBG.color = new Color(1, 1, 1, 1);
        }
        else
        {
            gobackBtnIcon.sprite = gobackStateSprite[1];
            gobackBtnBG.color = new Color(1, 1, 1, 0.3f);
        }
    }

    public void Update()
    {
        UpdateRefreshBtn();
    }

    private void UpdateRefreshBtn()
    {
        timer += Time.deltaTime;
        if (timer > ConfigData.REWARD_VIDEO_CHECK_TIME)
        {
            timer = 0;
            RefreshBtnState();
        }
    }

    #endregion

    #region 刷新

    /// <summary>
    /// 刷新道具数量
    /// </summary>
    private void RefreshItemCount()
    {
        int boomCount = GameController.Instance.GetItemCount(ItemID.Boom);
        boomRVPoint.SetActive(boomCount <= 0);
        boomCountPoint.SetActive(boomCount > 0);
        boomCountTxt.text = boomCount.ToString();

        int gobackCount = GameController.Instance.GetItemCount(ItemID.Goback);
        gobackRVPoint.SetActive(gobackCount <= 0);
        gobackCountPoint.SetActive(gobackCount > 0);
        gobackCountTxt.text = gobackCount.ToString();
    }

    /// <summary>
    /// 刷新地图
    /// </summary>
    /// <param name="gridData">Grid data.</param>
    /// <param name="showAnimation">If set to <c>true</c> show animation.</param>
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
        RefreshScore();
    }

    /// <summary>
    /// 刷新分数
    /// </summary>
    private void RefreshScore()
    {
        if (maxScore != null)
        {
            maxScore.text = GameController.Instance.MaxScore4Show();
        }

        if (curScore != null)
        {
            curScore.text = mapData.Score.ShowString();
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