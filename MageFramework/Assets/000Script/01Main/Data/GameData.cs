using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public bool IsOnePlay;
    public int playerNowIndex;//当前存档 = 当前选中的卡片，分开是为了好查找调用
    public int cardNowIndex;//当前选中的卡片，注意：因为加载卡片时，不会激活isOn为其赋值
    public List<PlayerData> playerList;
    public GameData()
    {
        playerList = new List<PlayerData>();
    }


	/// <summary>
	/// 获取当前的存档列表
	/// </summary>
	/// <returns></returns>
	public LevelInfo[] GetLevelList()
	{
		LevelInfo[] list = new LevelInfo[playerList.Count];
		for (int i = 0; i < playerList.Count; i++)
		{
			list[i] = playerList[i].level;
		}
		return list;
    }
	public void AddLevelInfo(LevelInfo info)
	{
		var newPlayer = new PlayerData();
		newPlayer.level = info;
        playerList.Add(newPlayer);

    }


}

