using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ObjManager : Sington<ObjManager>
{
    public string path = "Obj/";
    public string uiPartPath = "ObjPart/";
    private Dictionary<string, BaseObj> objDic =new Dictionary<string, BaseObj>();
    private Transform objRoot;
    public void Init(Transform pos)
    {
        objRoot = pos;
    }


    /// <summary>
    /// 加载一个节点，用来放游戏代码，
    /// 减少主要脚本放太多代码
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eve"></param>
    public void LoadSet<T>( UnityAction<T> eve = null) where T : BaseObj
    {
        string name = typeof(T).ToString();
        if (objDic.ContainsKey(name))
        {
            objDic[name].gameObject.SetActive(true);
            objDic[name].OnInit(null);//显示
            var t = objDic[name] as T;
            if (eve != null) eve(t);
        }
        else
        {
            var obj =new GameObject(name);
            obj.transform.SetParent(objRoot);
            T t = obj.AddComponent<T>();
            t.OnInit(null);
            t.ObjName = name;
            objDic.Add(name, t);
            if (eve != null) eve(t);
        }
    }
    /// <summary>
    /// 加载UI部件，带BaseUI脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="info"></param>
    /// <param name="eve"></param>
    public void LoadObjPart<T>(IInfo info = null,UnityAction < T> eve = null) where T : BaseObj
    {
        LoadObjPart(typeof(T).ToString(), eve, info);
    }
    /// <summary>
    /// 加载UI部件，不带脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eve"></param>
    public void LoadPart<T>(string name,UnityAction<T> eve = null) where T : MonoBehaviour
    {
        LoadObjPart(typeof(T).ToString(), eve);
    }
    /// <summary>
    /// 加载页面零件 ，不保存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="info"></param>
    /// <param name="eve"></param>
    private void LoadObjPart<T>(string name, UnityAction<T> eve = null, IInfo info = null) where T : MonoBehaviour
    {
        ResManager.LoadAsync<T>(uiPartPath + name, (o) => {
            var obj = Object.Instantiate(o.gameObject, objRoot);
            T t = obj.GetComponent<T>();
            if(t is BaseObj) (t as BaseObj).OnInit(info);
            if (eve != null) eve(t);
        },false);//不缓存，可加载多个
    }


    /// <summary>
    /// 加载页面：脚本预制体同名
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eve"></param>
    public void OpenObjAsync<T>(UnityAction<T> eve = null) where T : BaseObj
    {
        OpenObjAsync(typeof(T).ToString(), eve);
    }
    /// <summary>
    /// 打开ui
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="eve"></param>
    private void OpenObjAsync<T>(string name, UnityAction<T> eve = null,IInfo info = null) where T : BaseObj
    {
        if (objDic.ContainsKey(name))
        {
            objDic[name].gameObject.SetActive(true);
            objDic[name].OnInit(info);//显示
            var t = objDic[name] as T;
            if (eve != null) eve(t);
        }
        else
        {
            ResManager.LoadAsync<T>(path + name, (o) => {
                var obj = Object.Instantiate(o.gameObject, objRoot);
                T t = obj.GetComponent<T>();
                t.OnInit(info);
                t.ObjName = name;
                objDic.Add(name, t);
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
        if (objDic.ContainsKey(name))
        {
            objDic[name].OnClose();//关闭
            objDic[name].gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"关闭了空的UI：{name}");
        }
    }
    public void ClosePage(BaseObj ui)
    {
        ClosePage(ui.ObjName);
    }


    /// <summary>
    /// 获取ui
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T GetPage<T>(string name) where T : BaseUI 
    {
        return objDic[name] as T;
    }
    /// <summary>
    /// 关闭所有UI
    /// </summary>
    public void CloseAllObj()
    {
        foreach (var item in objDic)
        {
            item.Value.OnClose();
            item.Value.gameObject.SetActive(false);
        }
        objDic.Clear();
    }
    /// <summary>
    /// 删除所所有UI
    /// </summary>
    public void Remove()
    {
        foreach (var item in objDic)
        {
            Object.Destroy(item.Value.gameObject);
        }
        objDic.Clear();
    }
}
