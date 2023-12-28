using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;

public class Game : MonoBehaviour
{

    private static Game _instance;
    public static Game Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<Game>();
            return _instance;
        }
    }
    #region 游戏基本参数
    //场景节点
    private Canvas canvas;
    private Transform objRoot;
    private Transform poolRoot;
    //游戏进程中，是否进入过游戏
    private bool isOnePlay;
    public bool IsOnePlay
    {
        get
        {
            if (isOnePlay == false)
            {
                isOnePlay = true;
                return false;
            }
            return isOnePlay;
        }
    }
    #endregion

    public float gameSpeed;
    public bool isApp;
    public DataInit init;

    void Awake()
    {
        //-----------基础参数---------------
        gameSpeed = Time.deltaTime;
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        objRoot = transform.Find("ObjRoot");
        poolRoot = transform.Find("PoolRoot");
        DontDestroyOnLoad(gameObject);
        Debug.Log("游戏初始化");

        //-------------初始化---------------
        SceneManage.Init();
        Data.Instance.init = init;
        ResManager.Instance.Init();
        UIManager.Instance.Init(canvas);
        ObjManager.Instance.Init(objRoot);
        PoolManager.Instance.InitUI(canvas.transform);//UI在开始要用到的
    }
    void Start()
    {
        SceneManage.Load("001");
    }
    private void OnApplicationQuit()
    {
        EndApp();
    }
    #region 游戏进程控制
    public void EndApp()
    {
        Debug.Log("彻底退出游戏");
        Data.Instance.Save();
        if (isApp)
        {
            Debug.Log("当前进程名：" + System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
    public void InitPool()
    {
        if (IsOnePlay)
            return;
        PoolManager.Instance.Init(poolRoot);
    }

    //选中存档后，开始游戏
    public void PlayGame(int now)
    {
        //if (isApp == false)
        //    AssetDatabase.Refresh();//刷新unity，保证资源的更新
        Data.Instance.Load(now);//加载场景数据
        GoOnGame();
        InitPool();//只执行一次
        Debug.Log("开始游戏" + now);
        SceneManage.Load("002");
    }
    public void ExitGame()
    {
        ObjManager.Instance.CloseAllObj();
        UIManager.Instance.CloseAllPage();
        PoolManager.Instance.Close();
        Data.Instance.Save();
        //if(isApp == false)
        //    AssetDatabase.Refresh();//刷新unity，保证资源的更新
    }

    public void StopGame()
    {
        gameSpeed = 0;
    }
    public void GoOnGame()
    {
        gameSpeed = Time.deltaTime;
    }
    #endregion
}
