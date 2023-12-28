using System;
using System.Collections.Generic;
using UnityEngine;

public static  class EventManager 
{
    private static Dictionary<string, Delegate> eveDic = new Dictionary<string, Delegate>();

    public static void AddEvent(string eventName, Action callback)
    {
        if (!eveDic.ContainsKey(eventName))
        {
            eveDic.Add(eventName, null);
        }
        eveDic[eventName] = (Action)eveDic[eventName] + callback;
    }
    public static void AddEvent<T>(string eventName, Action<T> callback)
    {
        if (!eveDic.ContainsKey(eventName))
        {
            eveDic.Add(eventName, null);
        }
        eveDic[eventName] = (Action<T>)eveDic[eventName] + callback;
    }
    /// <summary>
    /// ע���¼���EventManager.AddEvent("�¼�����"(this)=>{});
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public static void AddEvent(string eventName, Action<object> callback)
    {
        if (!eveDic.ContainsKey(eventName))
        {
            eveDic.Add(eventName, null);
        }
        eveDic[eventName] = (Action<object>)eveDic[eventName] + callback;
    }
    public static void AddEvent<T>(string eventName, Action<object, T> callback)
    {
        if (!eveDic.ContainsKey(eventName))
        {
            eveDic.Add(eventName, null);
        }
        eveDic[eventName] = (Action<object, T>)eveDic[eventName] + callback;
    }

    public static void RemoveEvent(string eventName, Action callback)
    {
        if (eveDic.ContainsKey(eventName))
        {
            eveDic[eventName] = (Action)eveDic[eventName] - callback;
            if (eveDic[eventName] == null)
            {
                eveDic.Remove(eventName);
            }
        }
    }
    public static void RemoveEvent<T>(string eventName, Action<T> callback)
    {
        if (eveDic.ContainsKey(eventName))
        {
            eveDic[eventName] = (Action<T>)eveDic[eventName] - callback;
            if (eveDic[eventName] == null)
            {
                eveDic.Remove(eventName);
            }
        }
    }
    /// <summary>
    /// ע���¼���EventManager.RemoveEvent("�¼�����"(this)=>{});
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public static void RemoveEvent(string eventName, Action<object> callback)
    {
        if (eveDic.ContainsKey(eventName))
        {
            eveDic[eventName] = (Action<object>)eveDic[eventName] - callback;
            if (eveDic[eventName] == null)
            {
                eveDic.Remove(eventName);
            }
        }
    }
    public static void RemoveEvent<T>(string eventName, Action<object, T> callback)
    {
        if (eveDic.ContainsKey(eventName))
        {
            eveDic[eventName] = (Action<object, T>)eveDic[eventName] - callback;
            if (eveDic[eventName] == null)
            {
                eveDic.Remove(eventName);
            }
        }
    }

    public static void TriggerEvent( string eventName)
    {
        if (eveDic.ContainsKey(eventName))
        {
            ((Action)eveDic[eventName]).Invoke();
        }
    }
    public static void TriggerEvent<T>( string eventName, T sender)
    {
        if (eveDic.ContainsKey(eventName))
        {
            ((Action<T>)eveDic[eventName])(sender);
        }
    }
    /// <summary>
    /// �����¼���this.TrggerEvent("�¼�����",T);
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventName"></param>
    public static void TriggerEvent(this object sender, string eventName)
    {
        if (eveDic.ContainsKey(eventName))
        {
            ((Action<object>)eveDic[eventName])(sender);
        }
    }
    public static void TriggerEvent<T>(this object sender, string eventName, T parameter)
    {
        if (eveDic.ContainsKey(eventName))
        {
            ((Action<object, T>)eveDic[eventName])(sender, parameter);
        }
    }
}