using System.Collections.Generic;
using UnityEngine;


public class PoolManager : Sington<PoolManager>
{
    private Dictionary<string, PoolObj> objPoolDic;
    private Dictionary<string, PoolObj> uiPoolDic;

    private string path = "Pool/";
    private string uiPath = "PoolUI/";

    /// <summary>
    /// ����:�����һ����������г�ʼ��
    /// </summary>
    /// <param name="root"></param>
    public void Init(Transform root)
    {
        objPoolDic = new Dictionary<string, PoolObj>();
        //InitPool(path + "����", 10, root);
        //InitPool(path + "������", 10, root);
    }
    public void InitUI(Transform root)
    {
        uiPoolDic = new Dictionary<string, PoolObj>();
        //InitUIPool(uiPath + "�÷�", 10, root);
        //InitUIPool(uiPath + "��ʾ", 5, root);
    }

    //obj pool
    public void InitPool( string path, int count,Transform pos)
    {
        Debug.Log("pool����obj" + path);
        var obj = ResManager.Instance.Load<GameObject>(path);
        Debug.Log("pool����obj" + obj);
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
        //Debug.Log("pool����UI"+obj);
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
       //Debug.Log("����ش�С"+ uiPoolDic.Count);
    }
}
public class PoolObj
{
    public GameObject prefab;
    public Transform root;
    [SerializeField]
    private List<GameObject> objList = new List<GameObject>();

    /// <summary>
    /// ��������أ�ֱ��ʵ������
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
    /// �����򼤻��������ʵ����
    /// </summary>
    /// <returns></returns>
    public GameObject GetPool()
    {
        //Debug.Log("����ش�С"+objList.Count);
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
    /// ��ȡ��Ծ���б�
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


