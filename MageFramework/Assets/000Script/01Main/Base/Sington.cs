using UnityEngine;

public class Sington<T> where T : new()
{
    static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }
}
public class SingtonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                _instance = obj.AddComponent<T>();
            }
            DontDestroyOnLoad(_instance.gameObject);
            return _instance;
        }
    }
    private void OnApplicationQuit()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
            _instance = null;
        }
    }
}