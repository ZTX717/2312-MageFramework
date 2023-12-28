using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : Sington<UIManager>
{
    public string path = "UI/";
    public string uiPartPath = "UIPart/";
    private Dictionary<string, BaseUI> uiDic =new Dictionary<string, BaseUI>();
    private Transform uiRoot;
    public void Init(Canvas pos)
    {
        uiRoot = pos.transform;
    }

    /// <summary>
    /// 加载UI部件，带BaseUI脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="info"></param>
    /// <param name="eve"></param>
    public void LoadUIPart<T>(IInfo info = null,UnityAction < T> eve = null) where T : BaseUI
    {
        LoadUIPart(typeof(T).ToString(), eve, info);
    }
    /// <summary>
    /// 加载UI部件，不带脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eve"></param>
    public void LoadPart<T>(string name,UnityAction<T> eve = null) where T : Component
    {
        LoadUIPart(typeof(T).ToString(), eve);
    }
    /// <summary>
    /// 加载页面零件 ，不保存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="info"></param>
    /// <param name="eve"></param>
    private void LoadUIPart<T>(string name, UnityAction<T> eve = null, IInfo info = null) where T : Component
    {
        ResManager.LoadAsync<T>(uiPartPath + name, (o) => {
            var obj = Object.Instantiate(o.gameObject, uiRoot);
            T t = obj.GetComponent<T>();
            if(t is BaseUI) (t as BaseUI).OnInit(info);
            if (eve != null) eve(t);
        },false);//不缓存，可加载多个
    }


    /// <summary>
    /// 加载页面及子页面
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eve"></param>
    public void OpenPageAsync<T>(UnityAction<T> eve = null) where T : BaseUI
    {
        OpenUIAsync(typeof(T).ToString(), eve);
    }
    /// <summary>
    /// 打开ui
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="eve"></param>
    private void OpenUIAsync<T>(string name, UnityAction<T> eve = null,IInfo info = null) where T : BaseUI
    {
        //Debug.Log("UI"+name);
        if (uiDic.ContainsKey(name))
        {
            uiDic[name].gameObject.SetActive(true);
            uiDic[name].OnInit(info);//显示
            var t = uiDic[name] as T;
            if (eve != null) eve(t);
        }
        else
        {
            ResManager.LoadAsync<T>(path + name, (o) => {
                var obj = Object.Instantiate(o.gameObject, uiRoot);
                T t = obj.GetComponent<T>();
                t.OnInit(info);
                t.pageName = name;
                uiDic.Add(name, t);
                if (eve != null) eve(t);
            });
        }
    }
    /// <summary>
    /// 关闭ui
    /// </summary>
    /// <param name="name"></param>
    public void ClosePage(string name) 
    {
        if (uiDic.ContainsKey(name))
        {
            uiDic[name].OnClose();//关闭
            uiDic[name].gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"关闭了空的UI：{name}");
        }
    }
    public void ClosePage(BaseUI ui)
    {
        ClosePage(ui.pageName);
    }


    /// <summary>
    /// 获取ui
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T GetPage<T>(string name) where T : BaseUI 
    {
        return uiDic[name] as T;
    }
    /// <summary>
    /// 关闭所有UI
    /// </summary>
    public void CloseAllPage()
    {
        foreach (var item in uiDic)
        {
            item.Value.OnClose();
            item.Value.gameObject.SetActive(false);
        }
        uiDic.Clear();
    }
    /// <summary>
    /// 删除所所有UI
    /// </summary>
    public void Remove()
    {
        foreach (var item in uiDic)
        {
            Object.Destroy(item.Value.gameObject);
        }
        uiDic.Clear();
    }
}
