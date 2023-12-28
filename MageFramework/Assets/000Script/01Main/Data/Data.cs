using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : Sington<Data>
{
    public DataInit init;//初始化信息：

	public GameData game = new GameData();//存档
	public LevelData level = new LevelData();

    //此处只在开始游戏的时候加载关卡数据
    public void Load(int now)
	{
        level.Load();
        Xml.LoadFromXml<GameData>("00存档");
        game.playerNowIndex = now;//当前存档
    }
    //此处只在退出游戏的时候存档
    public void Save()
    {
        Xml.SaveToXml("00存档",game);
        if (game.playerList.Count == 0)
            return;
        level.Save();
    }

}
