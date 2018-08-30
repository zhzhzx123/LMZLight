using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : WindowBase {

    //在加载界面的一瞬间获取这些组件
    private Button loginButton;
    private Button registerButton;
    private InputField userInput;

    public override void Init()
    {
        base.Init();
        loginButton = transform.Find("Button").GetComponent<Button>();
        registerButton = transform.Find("Button (1)").GetComponent<Button>();
        userInput = transform.Find("InputField").GetComponent<InputField>();

        loginButton.onClick.AddListener(ClickLogin);
        registerButton.onClick.AddListener(ClickRegister);
    }


    //每次打开时需要把输入框的内容清理干净
    public override void Open()
    {
        base.Open();
        userInput.text = "";
    }


    void ClickLogin()
    {
        Debug.Log("ClickLogin");
    }


    void ClickRegister()
    {
        Debug.Log("ClickRegister");
        //打开注册界面
        UIManager._Instance.OpenWindow(WindowName.Register);
        //把自己关闭调
        UIManager._Instance.CloseWindow(WindowName.Login);
    }

}
