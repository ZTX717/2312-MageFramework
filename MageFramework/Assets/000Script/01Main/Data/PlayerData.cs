using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
	public int lv;
    public int scoreNow;//控制：当前分数
    public int scoreMax;//控制：当前最大分数
    public string name;
    public int map;
    public Vector3 pos;//不初始化，为Vector.Zero
    public LevelInfo level;
	public List<Item> bag;

    /// <summary>
    /// 为了获取改变存档的值
    /// </summary>
    public int Score
    {
        get { return level.score; }
        set { level.score = value; }
    }
    /// <summary>
    /// 为了获取改变存档的值
    /// </summary>
    public int LevelNow
    {
        get { return level.plan; }
        set { level.plan = value; }
    }
    public PlayerData()
    {
        bag = new List<Item>();
    }
}

