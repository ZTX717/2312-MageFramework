using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
	public int lv;
    public int scoreNow;//���ƣ���ǰ����
    public int scoreMax;//���ƣ���ǰ������
    public string name;
    public int map;
    public Vector3 pos;//����ʼ����ΪVector.Zero
    public LevelInfo level;
	public List<Item> bag;

    /// <summary>
    /// Ϊ�˻�ȡ�ı�浵��ֵ
    /// </summary>
    public int Score
    {
        get { return level.score; }
        set { level.score = value; }
    }
    /// <summary>
    /// Ϊ�˻�ȡ�ı�浵��ֵ
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

