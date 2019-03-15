using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptMove : MonoBehaviour
{
    /// <summary>
    /// 适配节点
    /// </summary>
    public RectTransform rect;

    public void SetBanner(BannerType bannerType)
    {
        if (rect == null)
        {
            rect = gameObject.GetComponent<RectTransform>();
        }
        do
        {
            if (rect == null) break;
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = Vector2.zero;
            float bannerSize = 150;
            switch (bannerType)
            {
                case BannerType.Down:
                    rect.offsetMin = new Vector2(0, bannerSize);
                    break;
                case BannerType.Top:
                    rect.offsetMax = new Vector2(0, -bannerSize);
                    break;
                default:
                    break;
            }
        } while (false);


    }
}

public enum BannerType
{
    Top,
    Down,

}