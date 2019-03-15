using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SingleMono<GameController>
{
    /// <summary>
    /// 是否初始化
    /// </summary>
    private bool isInit = false;

    /// <summary>
    /// ui控制器
    /// </summary>
    [SerializeField]
    private UIManager uimanager;

    #region 玩家数据

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
    public int MaxScore;

    #endregion
    /// <summary>
    /// 初始化游戏
    /// </summary>
    public void Init()
    {
        do
        {
            if (isInit) break;
            if (uimanager == null)
            {
                var go = GameObject.Find("UIRoot");
                if (go != null)
                {
                    uimanager = go.GetComponent<UIManager>();
                }
            }

            uimanager.Init();
            StartGame();

            isInit = true;

        } while (false);
    }

    #region 游戏进程控制

    /// <summary>
    /// 游戏控制器
    /// </summary>
    private PlayController playCtrl;

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        EndGame();

        playCtrl = new PlayController();
        playCtrl.StartGame();
    }

    /// <summary>
    /// 是否暂停游戏
    /// </summary>
    /// <param name="isPause"></param>
    public void Pause(bool isPause)
    {
        if (playCtrl != null)
        {
            playCtrl.Pause(isPause);
        }
    }

    /// <summary>
    /// 结束游戏
    /// </summary>
    public void EndGame()
    {
        if (playCtrl != null)
        {
            playCtrl.EndGame();
            playCtrl = null;
        }
    }

    #endregion
}
