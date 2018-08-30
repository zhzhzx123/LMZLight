using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public abstract class MsgCallBackBase<T> : MonoBehaviour, IClientMessage<T>
    {

        Queue<T> queue = new Queue<T>();

        #region 生命周期函数 
        public void BaseAwake()
        {
            
        }

        public void BaseUpdate()
        {
            UpdateMessage();
        }

        public void BaseDestroy()
        {
           
        }
        #endregion

        /// <summary>
        /// 执行消息的方法
        /// </summary>
        /// <param name="message"></param>
        protected abstract void NetworkCallback(T message);

        /// <summary>
        /// 通用的网络回调函数
        /// </summary>
        /// <param name="obj_arr"></param>
        protected abstract void GetNetworkMsgCallBack(params object[] obj_arr);

        #region 接口
        public void AddMessage(T m)
        {
            lock (queue)
            {
                queue.Enqueue(m);
            }
        }

        public void UpdateMessage()
        {
            if(queue.Count > 0)
            {
                NetworkCallback(queue.Dequeue());
            }
        }
        #endregion
    }
}

