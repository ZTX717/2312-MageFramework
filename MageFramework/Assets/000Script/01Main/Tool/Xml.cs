using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Unity.Mathematics;
using UnityEngine;

// 创建中间类来存储字典的键值对

[Serializable]
public class SerializableDictionary<TKey, TValue>
{
    [XmlAttribute]
    public TKey Key { get; set; }
    [XmlElement]
    public TValue Value { get; set; }
}

/// <summary>
/// 包含了 xml存档，读取的方法（固定资源/XML/目录下）
/// 包含了 Dic 转 xmlDic 方法
/// 注意：字典类的存储一般是已经是配置好的Res数据，所有只会从Res目录下读取
/// </summary>
public class Xml
{
    public static bool isApp;
    public static string savePath = Application.persistentDataPath;
    public static string resPath = Application.dataPath;
    public static string ResPath
    {
        get
        {
            return resPath + "/Resources/Xml/";
        }
    }
    public static string SavePath
    {
        get
        {
            return savePath + "/Xml/";
        }
    }
    public static void Init() { }
    /// <summary>
    /// 得到资源目录下的路径
    /// </summary>
    /// <param name="xmlName"></param>
    /// <returns></returns>
    private static string GetResXmlPath(string xmlName)
    {
        return "Xml/"+xmlName;
    }
    /// <summary>
    /// 得到完整路径，没有就创建，在加载时使用
    /// </summary>
    /// <param name="xmlName"></param>
    /// <returns></returns>
    private static string GetXmlPath(string xmlName)
    {
        //Debug.Log("xml的路径："+ResPath+xmlName);
        return GetPathOrCreate(ResPath + $"{xmlName}.xml");
    }
    /// <summary>
    /// 如果文件不存在，则创建一个
    /// </summary>
    /// <param name="path">带有后缀的文件路径</param>
    /// <returns></returns>
    public static string GetPathOrCreate(string filePath)
    {
        //Debug.Log(filePath);
        if (File.Exists(filePath) == false)
        {
            FileStream 文件流 = new FileStream(filePath, FileMode.Create);
            文件流.Close();
            return null;
        }
        return filePath;
    }
    

    // 将类存储为XML
    public static void SaveToXml<T>(string xmlName, T t) where T : class
    {
        GetXmlPath(xmlName);//创建持久化XML文件
        XmlSerializer xml = new XmlSerializer(typeof(T));
        using (StreamWriter writer = new StreamWriter(GetXmlPath(xmlName)))
        {
            xml.Serialize(writer, t);
        }
    }
    // 从XML中加载类实例
    public static T LoadFromXml<T>(string xmlName) where T : class
    {
        if (isApp)//如果在app中，则会读取本地文档
        {
            var path = GetPathOrCreate(SavePath + $"{xmlName}.xml");
            using (StreamReader reader = new StreamReader(path))
            {
                if (reader.EndOfStream)
                    return default(T);
                else
                {
                    XmlSerializer xml = new XmlSerializer(typeof(T));
                    return (T)xml.Deserialize(reader);
                }
            }
        }
        else
        {
            TextAsset Asset = ResManager.Instance.Load<TextAsset>(GetResXmlPath(xmlName));
            if (Asset == null)
                return default(T);
            using (StringReader reader = new StringReader(Asset.text))
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                return (T)xml.Deserialize(reader);
            }
        }
        
        //从资源目录下加载xml（已弃用）
        //GetXmlPath(xmlName);
        //XmlSerializer xml = new XmlSerializer(typeof(T));
        //using (StreamReader reader = new StreamReader(GetXmlPath(xmlName)))
        //{
        //    if (reader.EndOfStream) return default(T);
        //    return (T)xml.Deserialize(reader);
        //}
    }
    

    // 将字典存储为XML
    public static void SaveDicToXml<TKey, TValue>(string xmlName, Dictionary<TKey, TValue> dictionary)
    {
        GetXmlPath(xmlName);//创建持久化XML文件
        var dic =  DicToXml(dictionary);
        XmlSerializer xml = new XmlSerializer(typeof(List<SerializableDictionary<TKey, TValue>>));
        using (StreamWriter writer = new StreamWriter(GetXmlPath(xmlName)))
        {
            xml.Serialize(writer, dic);
        }
    }
    // 从XML中加载字典
    public static Dictionary<TKey, TValue> LoadXmlToDic<TKey, TValue>(string xmlName)
    {
        TextAsset Asset = ResManager.Instance.Load<TextAsset>(GetResXmlPath(xmlName));
        if (Asset == null)
            return new Dictionary<TKey, TValue>();

        using (StringReader reader = new StringReader(Asset.text))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<SerializableDictionary<TKey, TValue>>));
            List<SerializableDictionary<TKey, TValue>> dic = (List<SerializableDictionary<TKey, TValue>>)serializer.Deserialize(reader);
            return XmlToDic(dic);
        }
        //从资源目录下加载xml（已弃用）
        //XmlSerializer xml = new XmlSerializer(typeof(List<SerializableDictionary<TKey, TValue>>));
        //using (StreamReader reader = new StreamReader(Asset.text))
        //{
        //    if (reader.EndOfStream) return new Dictionary<TKey, TValue>();
        //    var serializableDictionary = (List<SerializableDictionary<TKey, TValue>>)xml.Deserialize(reader);
        //    return XmlToDic(serializableDictionary);
        //}
    }
    //字典转为xmlDic数据
    private static List<SerializableDictionary<TKey, TValue>> DicToXml<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
    {
        var serializableDictionary = new List<SerializableDictionary<TKey, TValue>>();
        foreach (var kvp in dictionary)
        {
            SerializableDictionary<TKey, TValue> entry = new SerializableDictionary<TKey, TValue>
            {
                Key = kvp.Key,
                Value = kvp.Value
            };
            serializableDictionary.Add(entry);
        }
        return serializableDictionary;

    }
    //将xmlDic转为字典
    private static Dictionary<TKey,TValue> XmlToDic<TKey,TValue>(List<SerializableDictionary<TKey, TValue>> xmlDic)
    {
        Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();
        foreach (var entry in xmlDic)
        {
            dic.Add(entry.Key, entry.Value);
        }
        return dic;
    }

}






