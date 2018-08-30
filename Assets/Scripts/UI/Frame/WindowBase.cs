using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBase : MonoBehaviour
{
    
    /// <summary>
    /// 加载时调动的方法，一次调用
    /// </summary>
    public virtual void Init()
    { }

    /// <summary>
    /// 卸载界面
    /// </summary>
    public virtual void Destroy()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// 打开界面
    /// </summary>
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

       
	
}
