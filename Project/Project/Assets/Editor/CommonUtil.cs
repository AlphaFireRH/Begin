using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class CommonUtil : Editor
{


    [MenuItem("Tool/Clean")]
    public static void RemoveArchives()
    {
        try
        {
            UnityEngine.Debug.Log(Application.persistentDataPath);
            DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)            //判断是否文件夹
                {
                    DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                    subdir.Delete(true);          //删除子目录和文件
                }
                else
                {
                    try
                    {
                        File.Delete(i.FullName);      //删除指定文件

                    }
                    catch (Exception e)
                    {
                        //Process[] pcs = Process.GetProcesses();
                        //foreach (Process p in pcs)
                        //{
                        //    if (p.MainModule.FileName == i.FullName)
                        //    {

                        //        p.Kill();
                        //    }

                        //    UnityEngine.Debug.Log(p.StartInfo.Arguments);
                        //}
                        //File.Delete(i.FullName);      //删除指定文件


                    }
                }
            }
            PlayerPrefs.DeleteAll();

            UnityEngine.Debug.Log("成功删除存档!");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log("删除存档失败!");
            throw;
        }
    }

    [MenuItem("Tool/SetVersion")]
    public static void SetVersion()
    {
        PlayerSettings.bundleVersion = ConfigData.nowVersion;
        PlayerSettings.Android.bundleVersionCode = ConfigData.nowBundle;
    }


    /// <summary>
    /// 设置选择的贴图
    /// </summary>
    [MenuItem("Tool/ImageImporterSet")]
    public static void SetTexture()
    {
        System.Object[] selects = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);

        foreach (Texture2D texture in selects)
        {
            string path = AssetDatabase.GetAssetPath(texture);
            TextureImporter impoter = AssetImporter.GetAtPath(path) as TextureImporter;

            if (impoter.textureType != TextureImporterType.Sprite)
            {
                impoter.textureType = TextureImporterType.Sprite;
                impoter.filterMode = FilterMode.Bilinear;
                impoter.mipmapEnabled = false;

                AssetDatabase.ImportAsset(path);
            }

        }

        AssetDatabase.Refresh();
    }


}
