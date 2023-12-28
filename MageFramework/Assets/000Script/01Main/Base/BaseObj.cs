using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseObj : BaseData
{
	private Dictionary<string, GameObject> objDic = new Dictionary<string, GameObject>();
	public string ObjName;

    private void Awake() {
        GetObjType<Component>(); 
        OnAwake();
    }
	private void Start() { OnStart(); }
    public virtual void OnAwake() { }
    public abstract void OnInit(IInfo info);
    public abstract void OnStart();
	public abstract void OnClose();

    public T GetObj<T>(string name)
    {
        return objDic[name].GetComponent<T>();
    }
    public GameObject GetObj(string name)
    {
        return objDic[name];
    }
    public Transform GetPos(string name)
    {
        return objDic[name].transform;
    }
    public void CutObj(string name, bool isOpen = false)
    {
        objDic[name].SetActive(isOpen);
    }
    public void CutObj(string from,string to)
    {
        objDic[from].SetActive(false);
        objDic[to].SetActive(true);
    }

    /// <summary>
    /// 找到全部的UI组件，并给特定组件添加方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
	private void GetObjType<T>() where T : Component
	{
        T[] list = this.GetComponentsInChildren<T>(true);
        //Debug.Log("组件数量" + list.Length);
        for (int i = 0; i < list.Length; i++)
        {
            int s = i;
            string 名称 = list[s].gameObject.name;
            if (objDic.ContainsKey(名称) == false)
            {
                objDic.Add(名称, list[s].gameObject);
            }
        }
    }

}
