
using UnityEngine;

public abstract class BaseData : MonoBehaviour
{
    //змЪ§Он
    public DataInit Data_Init
    {
        get { return Data.Instance.init; }
    }

    public GameData Data_game
    {
        get { return Data.Instance.game; }
        set { Data.Instance.game = value; }
    }
    public LevelData Data_Level
    {
        get { return Data.Instance.level; }
    }

 

}