using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    private int maxScore;

    /// <summary>
    /// 
    /// </summary>
    public bool isOpenMusic;

    /// <summary>
    /// 
    /// </summary>
    public bool isOpenSound;

    #endregion
    /// <summary>
    /// 初始化游戏
    /// </summary>
    public void Init()
    {
        do
        {
            if (isInit) break;

            /*
             * 读取存档
             * 设置定期存档
             * UI控制器初始化
             * 广告初始化
             * 弹出开始界面
             */
            if (!ReadSaveData())
            {
                //读取存档失败,设置默认存档
                SetDefaultSave();
            }
            PlayerPrefs.DeleteAll();
            SaveController.Instance.Register(SaveData);

            if (uimanager == null)
            {
                var go = GameObject.Find("UIRoot");
                if (go != null)
                {
                    uimanager = go.GetComponent<UIManager>();
                }
            }

            uimanager.Init();

            
            AdController.Instance.Init();
            
            StartGame();

            isInit = true;
            CheckPrivateState();

        } while (false);
    }

    /// <summary>
    /// 检测 是否需要展示 隐私界面
    /// </summary>
    public void CheckPrivateState()
    {
        if (AdController.Instance.NeedShowPrivateFirstUI())
        {
            uimanager.ShowUI(ViewID.PrivateFirstUI);
        }
    }

    /// <summary>
    /// 设置最高分数
    /// </summary>
    /// <param name="curScore"></param>
    public void SetScore(int curScore)
    {
        if (maxScore < curScore)
        {
            maxScore = curScore;
        }
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
        if (mapData == null)
        {
            playCtrl.StartGame();
        }
        else
        {

            playCtrl.StartGame(mapData, historyMap);
        }

    }

    public void Continue() 
    {
        UseBoom();
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
            mapData = null;
            historyMap = new List<MapData>();
        }
    }
    private JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects };

    /// <summary>
    /// 存档
    /// </summary>
    private void SaveData()
    {
        string path = Application.persistentDataPath + ConfigData.SAVE_FILE_PATH;
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        SaveData saveData = new SaveData();
        saveData.itemDic = itemDic;
        saveData.mapData = null;
        saveData.historyMap = null;
        if (playCtrl != null)
        {
            saveData.mapData = playCtrl.GetCurSaveData();
            saveData.historyMap = playCtrl.GetCurSaveDatas();
        }
        saveData.MaxScore = maxScore;
        saveData.isOpenMusic = isOpenMusic;
        saveData.isOpenSound = isOpenSound;
        string saveInfo = JsonConvert.SerializeObject(saveData, JsonSerializerSettings);
        File.WriteAllText(path, saveInfo);
    }

    /// <summary>
    /// 读取存档
    /// </summary>
    /// <returns></returns>
    private bool ReadSaveData()
    {
        try
        {
            string path = Application.persistentDataPath + ConfigData.SAVE_FILE_PATH;
            if (File.Exists(path))
            {
                var file = File.ReadAllText(path);
                SaveData saveData = JsonConvert.DeserializeObject<SaveData>(file, JsonSerializerSettings);
                itemDic = saveData.itemDic;
                mapData = saveData.mapData;
                historyMap = saveData.historyMap;
                maxScore = saveData.MaxScore;
                isOpenMusic = saveData.isOpenMusic;
                isOpenSound = saveData.isOpenSound;
                return true;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
        return false;
    }

    /// <summary>
    /// 最大分数
    /// </summary>
    /// <returns>The score.</returns>
    public int MaxScore()
    {
        return maxScore;
    }

    /// <summary>
    /// 当前分数
    /// </summary>
    /// <returns>The score.</returns>
    public int CurScore()
    {
        if (playCtrl != null)
        {
            playCtrl.Score();
        }
        return 0;
    }

    /// <summary>
    /// Uses the boom.
    /// </summary>
    public void UseBoom()
    {
        if (playCtrl != null)
        {
            playCtrl.UseBoom();
        }
    }

    /// <summary>
    /// Uses the goback.
    /// </summary>
    public void UseGoback()
    {
        if (playCtrl != null)
        {
            playCtrl.UseGoBack();
        }
    }



    /// <summary>
    /// 设置默认存档
    /// </summary>
    private void SetDefaultSave()
    {
        itemDic = new Dictionary<int, int>();
        itemDic.Add((int)ItemID.Boom, ConfigData.DEFAULT_BOOM_COUNT);
        itemDic.Add((int)ItemID.Goback, ConfigData.DEFAULT_GOBACK_COUNT);
        mapData = null;
        historyMap = new List<MapData>();
        if (playCtrl != null)
        {
            mapData = playCtrl.GetCurSaveData();
            historyMap = playCtrl.GetCurSaveDatas();
        }
        isOpenMusic = true;
        isOpenSound = true;
    }

    #endregion
}
