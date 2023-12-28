using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
[Serializable]
public class LevelData
{
    public Dictionary<int, Level> 关卡Dic = new Dictionary<int, Level>();

    public void Load()
    {
        Debug.Log("关卡————————————————load————————————————————" + 关卡Dic.Count);
        关卡Dic = Xml.LoadXmlToDic<int, Level>("0关卡");
        关卡Dic = Xml.LoadXmlToDic<int, Level>("1关卡");
        Debug.Log("关卡————————————————load————————————————————" + 关卡Dic.Count);
   
    }
    public void Save() 
    {
        Debug.Log("关卡———————————————save—————————————————————"+关卡Dic.Count);
        Xml.SaveDicToXml("0关卡", 关卡Dic);
        Xml.SaveDicToXml("1关卡", 关卡Dic);
    }
   

}
[Serializable]
public class Level
{
    public int index;
    public string name;
    public List<Vector3> awardPosList;
    public Level() { }

    public void Init(int i , string n, List<Vector3> list)
    {
        index = i;
        name = n;
        awardPosList = list;
    }
}