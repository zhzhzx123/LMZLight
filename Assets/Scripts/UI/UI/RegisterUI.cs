using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUI : WindowBase {

    private Button loginButton;

    public override void Init()
    {
        base.Init();
        loginButton = transform.Find("Button").GetComponent<Button>();

        loginButton.onClick.AddListener(ClickLogin);
    }


    void ClickLogin()
    {
        Debug.Log("ClickLogin");
        //打开登录界面
        UIManager._Instance.OpenWindow(WindowName.Login);
        //关闭自己
        UIManager._Instance.CloseWindow(WindowName.Register);
    }

}
