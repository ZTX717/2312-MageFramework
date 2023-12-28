using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseUI : BaseData
{
	private Dictionary<string, GameObject> uiDic = new Dictionary<string, GameObject>();
	public string pageName;

    private void Awake() {
        GetUiType<UIBehaviour>(); 
        OnAwake();
        //Debug.Log("页面开始："+uiDic.Count);
    }
    private void OnEnable() {if(enabled) OnOpen(); }
    private void Start() { OnStart(); }
    public virtual void OnAwake() { }
    public virtual void OnOpen() { }
    public abstract void OnInit(IInfo info);
    public abstract void OnStart();
    public abstract void OnClose();

    public T GetUI<T>(string name)
    {
        return uiDic[name].GetComponent<T>();
    }
    public GameObject GetUI(string name)
    {
        return uiDic[name];
    }
    public Transform GetUIPos(string name)
    {
        return uiDic[name].transform;
    }
    public RectTransform GetRect(string name)
    {
        return GetUI<RectTransform>(name);
    }
    public void CutUI(string name, bool isOpen = false)
    {
        uiDic[name].SetActive(isOpen);
    }
    public void CutUI(string from,string to)
    {
        uiDic[from].SetActive(false);
        uiDic[to].SetActive(true);
    }

    /// <summary>
    /// 找到全部的UI组件，并给特定组件添加方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
	private void GetUiType<T>() where T : UIBehaviour
	{
        T[] list = this.GetComponentsInChildren<T>(true);
        //Debug.Log("组件数量" + list.Length);
        for (int i = 0; i < list.Length; i++)
        {
            int s = i;
            string 名称 = list[s].gameObject.name;
            if (uiDic.ContainsKey(名称) == false)
            {
                uiDic.Add(名称, list[s].gameObject);
            }

            if (list[s] is Button)
            {
                (list[s] as Button).onClick.AddListener(() =>
                {
                    OnButton(名称);
                });
            }
            else if (list[s] is Toggle)
            {
                (list[s] as Toggle).onValueChanged.AddListener((b) =>
                {
                    OnToggle(名称, b);
                });
            }
        }
    }
    public virtual void OnButton(string name) { }
    public virtual void OnToggle(string name, bool isOn) { }

}
