using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientIntervalSendBase : MonoBehaviour {

    public float interval = 0.1f;//网络同步时间，测试

    protected float timeCount = 0f;

    protected void BaseAwake()
    {
        timeCount = interval;
    }

    protected void BaseUpdate()
    {
        UpdateNetwork();
    }

    #region 网络位置同步
    void UpdateNetwork()
    {
        timeCount += Time.deltaTime;
        if (timeCount >= interval)
        {
            //位置同步
            //同步位置
            SendMsg();
            timeCount -= interval;
        }
    }
    #endregion

    public virtual void SendMsg()
    { }

}
