using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 1.管理所有已经加载进来的界面（Resouces.Load）
 * 2.所有的UI的加载，卸载，打开和关闭都是同该管理器实现的
 * */
public class UIManager
{

    #region 单例
    private static UIManager instance;

    public static UIManager _Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIManager(); 
            }
            return instance;
        }
    }

    private UIManager() {
        //回去UI的Canvas物体
        canvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
        eventSystem = GameObject.Find("EventSystem").gameObject;
        /*
        windowParent = (new GameObject("WindowParent")).transform;
        windowParent.SetParent(canvas.transform);
        windowParent.localScale = Vector3.one;
        tipsParent = (new GameObject("TipsParent")).transform;
        tipsParent.SetParent(canvas.transform);
        tipsParent.localScale = Vector3.one;
        loadingParent = (new GameObject("LoadingParent")).transform;
        loadingParent.SetParent(canvas.transform);
        loadingParent.localScale = Vector3.one;
        */
        windowParent = canvas.transform.Find("WindowParent");
        tipsParent = canvas.transform.Find("TipsParent");
        loadingParent = canvas.transform.Find("LoadingParent");

        GameObject.DontDestroyOnLoad(canvas);
        GameObject.DontDestroyOnLoad(eventSystem);
    }
    #endregion

    /// <summary>
    /// 存储加载进来的UI界面
    /// 键是： 使用枚举表示的预制体的名称
    /// 值是： 每个UI的界面的基类对象
    /// </summary>
    private Dictionary<WindowName, WindowBase> windowDic = new Dictionary<WindowName, WindowBase>();

    /// <summary>
    /// UI的所有的父物体
    /// </summary>
    private Canvas canvas;

    /// <summary>
    /// 窗口父物体
    /// </summary>
    private Transform windowParent;
    /// <summary>
    /// 提示的父物体
    /// </summary>
    private Transform tipsParent;
    /// <summary>
    /// 加载界面的父物体
    /// </summary>
    private Transform loadingParent;

    private GameObject eventSystem;

    /// <summary>
    /// 预制体路径, 根据项目不同自己考虑
    /// </summary>
    private string prefabPath = "Prefab/UI/";


    #region 方法

    /// <summary>
    /// 加载预制体的方法
    /// </summary>
    /// <param name="name">预制体的名字</param>
    public void LoadWindow(WindowName name, UIType type = UIType.Window)
    {
        //从Resouces文件夹中把预支体实例出来，放在Canvas下
        //先判断字典中是否有该预制体
        if(!windowDic.ContainsKey(name))
        {
            GameObject prefab = Resources.Load<GameObject>(prefabPath + name);
            Transform parent = null;
            if (type == UIType.Window)
            {
                parent = windowParent;
            }
            else if (type == UIType.Tips)
            {
                parent = tipsParent;
            }
            else if (type == UIType.Loading)
            {
                parent = loadingParent;
            }
            GameObject obj = GameObject.Instantiate<GameObject>(prefab, parent);
            //从该物体上获取WindowBase
            WindowBase w = obj.GetComponent<WindowBase>();
            //调用界面的初始化方法
            w.Init();
            //把加载进来的物体存储字典中
            windowDic.Add(name, w);
        }
    }

    /// <summary>
    /// 卸载预制体
    /// </summary>
    /// <param name="name">预制体的名字</param>
    public void UnloadWindow(WindowName name)
    {
        //卸载就是把物体从界面中删除，并且把物体从字典中移除
        //判断字典中是否有该物体
        if (windowDic.ContainsKey(name))
        {
            windowDic[name].Close();
            //调用一次界面的卸载时需要调用的方法
            windowDic[name].Destroy();
            //把物体删除
            GameObject.Destroy(windowDic[name]);
            //把物体从字典中移除
            windowDic.Remove(name);
        }
    }


    /// <summary>
    /// 卸载所有的当前的界面
    /// </summary>
    public void UnloadAllWindow()
    {
        foreach (var item in windowDic.Keys)
        {
            UnloadWindow(item);
        }
        windowDic.Clear();
    }

    /// <summary>
    /// 打开界面 
    /// 参数：打开界面的名字
    /// </summary>
    public void OpenWindow(WindowName name, UIType type = UIType.Window)
    {
        //打开界面，先判断是否加载了该预支体
        //如果未加载，则需要加载
        //判断是否有该预制体
        if (!windowDic.ContainsKey(name))
        {
            //没有则加载
            LoadWindow(name, type);
        }
        //界面的打开方法
        windowDic[name].Open();
        //需要把该打开的界面放在父物体的最后
        windowDic[name].transform.SetSiblingIndex(windowDic[name].transform.parent.childCount);
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <param name="name"></param>
    public void CloseWindow(WindowName name)
    {
        //判断是否有该预制体
        if (windowDic.ContainsKey(name))
        {
            //关闭界面
            windowDic[name].Close();
        }
    }

    #endregion

}
