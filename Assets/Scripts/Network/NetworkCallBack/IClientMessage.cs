using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public interface IClientMessage<T> //处理客户端的主线程消息的方法
    {
        void AddMessage(T m);
        void UpdateMessage();
    }
}

