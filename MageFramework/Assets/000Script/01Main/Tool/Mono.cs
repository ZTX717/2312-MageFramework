
using UnityEngine;
using UnityEngine.Events;

public class Mono : SingtonMono<Mono>
{
    public event UnityAction<float> UpdateTimeEvent;
    public event UnityAction UpdateEvent;

    private void Update()
    {
        UpdateTimeEvent?.Invoke(Game.Instance.gameSpeed);
        UpdateEvent?.Invoke();
    }
    public void AddUpdate(UnityAction<float> fun)
    {
        UpdateTimeEvent += fun;
    }
    public void AddUpdate(UnityAction fun)
    {
        UpdateEvent += fun;
    }
    public void RemoveUpdate(UnityAction<float> fun)
    {
        UpdateTimeEvent -= fun;
    }
    public void RemoveUpdate(UnityAction fun)
    {
        UpdateEvent -= fun;
    }
    private void O1nDisable()
    {
        StopAllCoroutines();
    }
}
