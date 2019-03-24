/// <summary>
/// 
/// </summary>
public interface IPlayUIController
{
    /// <summary>
    /// 初始化
    /// </summary>
    void Init(IPlayController IPlayController);

    /// <summary>
    /// Refresh this instance.
    /// </summary>
    void Refresh();

    /// <summary>
    /// Refresh Btn UI
    /// </summary>
    void RefreshBtnState();
}