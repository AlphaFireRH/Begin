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
    public int MaxScore;

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
             * 1.读取存档
             * 2.设置定期存档
             * 3.UI控制器初始化
             * 4.弹出开始界面
             */
            if (!ReadSaveData())
            {
                //读取存档失败,设置默认存档
                SetDefaultSave();
            }
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
            StartGame();

            isInit = true;

        } while (false);
    }

    /// <summary>
    /// 设置最高分数
    /// </summary>
    /// <param name="curScore"></param>
    public void SetScore(int curScore)
    {
        if (MaxScore < curScore)
        {
            MaxScore = curScore;
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
        saveData.mapData = mapData;
        saveData.historyMap = historyMap;
        saveData.MaxScore = MaxScore;
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
                MaxScore = saveData.MaxScore;
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
    /// 设置默认存档
    /// </summary>
    private void SetDefaultSave()
    {
        itemDic = new Dictionary<int, int>();
        itemDic.Add((int)ItemID.Boom, ConfigData.DEFAULT_BOOM_COUNT);
        itemDic.Add((int)ItemID.Goback, ConfigData.DEFAULT_GOBACK_COUNT);
        mapData = null;
        historyMap = new List<MapData>();
        MaxScore = MaxScore = 0;
        isOpenMusic = true;
        isOpenSound = true;
    }

    #endregion
}
