using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using static UnityEditor.PlayerSettings;

public static class Load
{

    /// <summary>
    /// 加载图片Sprite，默认png
    /// </summary>
    /// <param name="name"></param>
    /// <param name="eve"></param>
    /// <returns></returns>
    public static void LoadSprite(string name, Action<Sprite> eve)
    {
        Mono.Instance.StartCoroutine(LoadWWWSprite("Image/" + name, eve));
    }
    /// <summary>
    /// 加载图片：Image/Ioc/
    /// </summary>
    /// <param name="name"></param>
    /// <param name="eve"></param>
    public static void LoadIoc(string name, Action<Sprite> eve)
    {
        Mono.Instance.StartCoroutine(LoadWWWSprite("Image/Ioc/" + name,  eve));
    }
    /// <summary>
    /// 加载本地图片，默认png
    /// </summary>
    /// <param name="name"></param>
    /// <param name="eve"></param>
    /// <returns></returns>
    public static void LoadResSprite(string resPath,Action<Sprite> eve,string back = ".png")
    {
        string path = $"file://{resPath}{back}";
        Mono.Instance.StartCoroutine(LoadWWWSprite( path , eve));
    }

    /// <summary>
    /// 加载图片Texture2D,ResPath/wwwPath，匿名函数
    /// </summary>
    /// <param name="name"></param>
    /// <param name="back"></param>
    /// <param name="eve"></param>
    /// <returns></returns>
    public static IEnumerator LoadWWWImage(string resPath, Action<Texture2D> eve)
    {
        Texture2D texture2D = Resources.Load<Texture2D>(resPath);
        if (texture2D != null)
        {
            texture2D.name = resPath;
            eve(texture2D);
        }
        else
        {
            //Debug.Log(wwwPath);
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(resPath);
            yield return www.SendWebRequest();
            if (!www.isNetworkError && !www.isHttpError)
            {
                if (www.isDone && www.downloadHandler.isDone)
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);
                    texture.name = resPath;
                    eve(texture);
                }
                else
                {
                    Debug.LogWarning("请求未完成");
                }
            }
            else
            {
                Debug.LogWarning("加载图片失败: " + www.error);
            }
        }
    }
    /// <summary>
    /// 加载图片Sprite,ResPath/WwwPath,匿名函数
    /// </summary>
    /// <param name="name"></param>
    /// <param name="back"></param>
    /// <param name="eve"></param>
    /// <returns></returns>
    public static IEnumerator LoadWWWSprite(string resPath,  Action<Sprite> eve)
    {
        //Debug.Log(path);
        Texture2D texture2D = Resources.Load<Texture2D>(resPath);
        if (texture2D != null)
        {
            Sprite sp = Sprite.Create(
                  texture2D,
                  new Rect(0, 0, texture2D.width, texture2D.height),
                  new Vector2(0.5f, 0.5f));
            eve(sp);
        }
        else
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(resPath);
            yield return www.SendWebRequest();
            if (!www.isNetworkError && !www.isHttpError)
            {
                if (www.isDone && www.downloadHandler.isDone)
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);
                    Sprite sp = Sprite.Create(
                       texture,
                       new Rect(0, 0, texture.width, texture.height),
                       new Vector2(0.5f, 0.5f));
                    sp.name = resPath;
                    eve(sp);
                }
                else
                {
                    Debug.Log("请求未完成");
                }
            }
            else
            {
                Debug.Log("加载图片失败: " + www.error);
            }
        }
    }
   

}
