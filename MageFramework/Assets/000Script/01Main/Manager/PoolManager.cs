using System.Collections.Generic;
using UnityEngine;


public class PoolManager : Sington<PoolManager>
{
    private Dictionary<string, PoolObj> objPoolDic;
    private Dictionary<string, PoolObj> uiPoolDic;

    private string path = "Pool/";
    private string uiPath = "PoolUI/";

    /// <summary>
    /// 加载:对象池一般在这里进行初始化
    /// </summary>
    /// <param name="root"></param>
    public void Init(Transform root)
    {
        objPoolDic = new Dictionary<string, PoolObj>();
        //InitPool(path + "怪物", 10, root);
        //InitPool(path + "奖励柱", 10, root);
    }
    public void InitUI(Transform root)
    {
        uiPoolDic = new Dictionary<string, PoolObj>();
        //InitUIPool(uiPath + "得分", 10, root);
        //InitUIPool(uiPath + "提示", 5, root);
    }

    //obj pool
    public void InitPool( string path, int count,Transform pos)
    {
        Debug.Log("pool加载obj" + path);
        var obj = ResManager.Instance.Load<GameObject>(path);
        Debug.Log("pool加载obj" + obj);
        ResManager.LoadAsync<GameObject>(path, (o) => {
            PoolObj pool = new PoolObj(count, o, pos);
            objPoolDic.Add(obj.name, pool);
        });
    }
    public T GetObj<T>(string name) where T : MonoBehaviour
    {
        return GetObj(name).GetComponent<T>();
    }
    public T GetObj<T>() where T : MonoBehaviour
    {
        return GetObj<T>(typeof(T).ToString());
    }
    public GameObject GetObj(string name)
    {
        var obj = objPoolDic[name].GetPool();
        obj.transform.SetAsFirstSibling();
        return obj;
    }
    public GameObject[] GetObjList(string name) {
        return objPoolDic[name].GetList();
    }

    //ui pool
    public void InitUIPool(string path, int count, Transform pos)
    {
        var obj = ResManager.Instance.Load<GameObject>(path);
        //Debug.Log("pool加载UI"+obj);
        ResManager.LoadAsync<GameObject>(path, (o) =>
        {
            PoolObj pool = new PoolObj(count, o, pos);
            uiPoolDic.Add(obj.name, pool);
        });
    }
    public T GetUI<T>(string name) where T : Component
    {
        return GetUI(name).GetComponent<T>();
    }
    public T GetUI<T>() where T : Component
    {
        return GetUI<T>(typeof(T).ToString());
    }
    public GameObject GetUI(string name)
    {
        var ui = uiPoolDic[name].GetPool();
        ui.transform.SetAsLastSibling();
        return ui;
    }
    public GameObject[] GetUIList(string name)
    {
        return uiPoolDic[name].GetList();
    }

    public void Remove() 
    {
        foreach (var poolObj in objPoolDic)
        {
            poolObj.Value.Remove();
        }
        objPoolDic.Clear();
        foreach (var poolObj in uiPoolDic)
        {
            poolObj.Value.Remove();
        }
        uiPoolDic.Clear();
    }
    public void Close()
    {
        foreach (var poolObj in objPoolDic)
        {
            poolObj.Value.Close();
        }
        foreach (var poolObj in uiPoolDic)
        {
            poolObj.Value.Close();
        }
       //Debug.Log(uiPoolDic.Count );
       //Debug.Log("对象池大小"+ uiPoolDic.Count);
    }
}
public class PoolObj
{
    public GameObject prefab;
    public Transform root;
    [SerializeField]
    private List<GameObject> objList = new List<GameObject>();

    /// <summary>
    /// 创建对象池，直接实例备用
    /// </summary>
    /// <param name="count"></param>
    /// <param name="obj"></param>
    /// <param name="pos"></param>
    public PoolObj(int count, GameObject obj, Transform pos)
    {
        prefab = obj;
        root = pos;
        FillList(count);
    }
    private void FillList(int count)
    {
        for (int i = 0; i < count; i++)
        {
            AddPool(i);
        }
    }
    public GameObject AddPool(int i)
    {
        GameObject pool = Object.Instantiate(prefab, root);
        pool.name = prefab.name + i.ToString();
        pool.SetActive(false);
        objList.Add(pool);
        return pool;
    }

    /// <summary>
    /// 存在则激活，不存在则实例化
    /// </summary>
    /// <returns></returns>
    public GameObject GetPool()
    {
        //Debug.Log("对象池大小"+objList.Count);
        foreach (var obj in objList)
        {
            if (obj.activeInHierarchy == false)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        var objNew = AddPool(-1);
        objNew.SetActive(true);
        return objNew;
    }
    /// <summary>
    /// 获取活跃的列表
    /// </summary>
    /// <returns></returns>
    public GameObject[] GetList()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (var obj in objList)
        {
            if (obj.activeInHierarchy)
            {
                list.Add(obj);
            }
        }
        return list.ToArray();
    }
    public void Close()
    {
        foreach (var obj in GetList())
        {
            obj.SetActive(false);
        }
    }

    public void Remove()
    {
        foreach (var obj in objList)
        {
            Object.Destroy(obj.gameObject);
        }
        objList.Clear();
    }
}


