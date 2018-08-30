using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using System.Linq;

namespace Network
{
    public class ClientSendBase
    {
        #region 变量
        protected Socket client;//套接字
        #endregion

        #region 构造
        public ClientSendBase()
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        ~ClientSendBase()
        { CloseClient(); }
        #endregion

        #region 方法
        public virtual void CloseClient()
        {
            if (client != null)
            {
                client.Close();
                client = null;
            }
        }

        public virtual void SendMsg(string ip, int port, string msg)
        {
            //定义一个发送的IP和端口
            EndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
            client.SendTo(Encoding.UTF8.GetBytes(msg), point);
        }

        public virtual void SendMsg(string ip, int port, byte[] msg)
        {
            //定义一个发送的IP和端口
            EndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
            client.SendTo(msg, point);
        }
        #endregion

        #region 抽象方法
        public virtual void Send(int type, params object[] obj)
        { }
        #endregion

    }
}

