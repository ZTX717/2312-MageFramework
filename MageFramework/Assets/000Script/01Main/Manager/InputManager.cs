using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;


public class InputManager : SingtonMono<InputManager>
{

    public bool isMove;//可以操作
    public void Init()
    {
        isMove = true;
        Cursor.visible = false; // 隐藏鼠标
        Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标光标在窗口内
    }
    public void Remove()
    {
    }
    public void OnStop()
    {
        Cursor.lockState = CursorLockMode.None; // 解除锁定
        Cursor.visible = true;// 显示鼠标
        isMove = false;//可以操作
    }
    public void OnPlay()
    {
        Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标光标在窗口内
        Cursor.visible = false;// 隐藏鼠标
        isMove = true;
    }
    //写一个简单的控制射线控制对象的方法
    public void Update()
    {
        if (isMove)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                InputMouse(ref hit);
            }
        }
    }
    private void InputMouse(ref RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0))
        {
        
        }
    }
}