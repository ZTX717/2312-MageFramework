
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage
{
    public static void Init()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }
    public static void Load(string name)
    {
        Mono.Instance.StopAllCoroutines();
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// 场景加载完成时候调用
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="loadSceneMode"></param>
    private static void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log(System.String.Format($"场景：{0} 已经加载完成({1})", scene.name, loadSceneMode.ToString()));
        switch (scene.name)
        {
            case "001":
                break;
            case "002":
                break;
        }
    }
}
