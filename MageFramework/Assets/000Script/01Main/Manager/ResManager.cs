using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ResManager : Sington<ResManager>
{
    private static Dictionary<string, Object> objDic = new Dictionary<string, Object>();

    public void Init() { }

    /// <summary>
    /// 清楚全部缓存
    /// </summary>
    public void AllCacheClear()
    {
        objDic.Clear();
        Resources.UnloadUnusedAssets();
    }
    /// <summary>
    ///  异步加载一个path数组,缓存起来
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pathList"></param>
    public async void LoadListAsync<T>(params string[] pathList)
    {
        foreach (string path in pathList)
        {
            if (objDic.ContainsKey(path) == false)
            {
                Task<Object> loadTask = Task.Run(() => Resources.Load<Object>(path));
                Object obj = await loadTask;
                objDic[path] = obj;
            }
        }
    }

    /// <summary>
    /// 加载一个对象，并返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="isCache"></param>
    /// <returns></returns>
    public T Load<T>(string path, bool isCache = true) where T : Object
    {
        if (objDic.TryGetValue(path, out Object cacheObj))
            return cacheObj as T;

        T obj = Resources.Load<T>(path);
        if (isCache)
            objDic.Add(path, obj);
        return obj;
    }
    /// <summary>
    /// 异步加载对象，传递方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="eve"></param>
    /// <param name="isCache"></param>
    public static void LoadAsync<T>(string path, System.Action<T> eve, bool isCache = true) where T : Object
    {
        //Debug.Log($"加载{path}");
        Mono.Instance.StartCoroutine(LoadAsyncObj(path, eve, isCache));
    }
    private static IEnumerator LoadAsyncObj<T>(string path, System.Action<T> eve, bool isCache = true) where T : Object
    {
        //string str=null;
        //foreach (var item in objDic)
        //{
        //    str += "、"+item.Value.name;
        //}
        if (objDic.ContainsKey(path))
        {
            eve(objDic[path] as T);
        }
        else
        {
            //Debug.Log(path+ "===" + objDic.ContainsKey(path));
            ResourceRequest r = Resources.LoadAsync<T>(path);
            yield return r;
            // Debug.Log(str + "===" + path + objDic.ContainsKey(path));
            if (isCache)
                objDic.Add(path, r.asset);
            eve(r.asset as T);
        }
    }
}
