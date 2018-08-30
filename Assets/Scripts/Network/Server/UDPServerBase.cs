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
    public class UDPServerBase
    {
        #region 变量
        protected Socket server;//套接字

        protected Thread sendTh;//发送线程

        protected Thread reciveTh;//接收线程

        #endregion

        #region 构造方法
        public UDPServerBase()
        {
            
        }

        ~UDPServerBase()
        {
            CloseServer();
        }

        #endregion

        #region 方法方法
        /// <summary>
        /// 启动服务器
        /// </summary>
        public virtual void StartServer()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(new IPEndPoint(IPAddress.Parse(NetworkTools.GetLocalIP()), NetworkConstent.UDPServerPort));//绑定端口号和IP
            server.IOControl((int)NetworkConstent.SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
            NetworkTools.PrintMessage("服务器已启动：" + NetworkTools.GetLocalIP() + " -- " + NetworkConstent.UDPServerPort);

            reciveTh = new Thread(ReciveMsg);
            reciveTh.Start();
        }

        /// <summary>
        /// 关闭服务器
        /// </summary>
        public virtual void CloseServer()
        {
            try
            {
                if (reciveTh != null)
                {
                    reciveTh.Abort();
                    reciveTh = null;
                }

                if (sendTh != null)
                {
                    sendTh.Abort();
                    sendTh = null;
                }

                if (server != null)
                {
                    server.Close();
                    server = null;
                    NetworkTools.PrintMessage("关闭服务器!");
                }
              
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        /// <summary>
        /// 接收发送给本机ip对应端口号的数据报
        /// </summary>
        protected virtual void ReciveMsg()
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                int length = server.ReceiveFrom(buffer, ref point);//接收数据报
                byte[] messageBytes = buffer.Skip(0).Take(length).ToArray();//截取数组，从第0位开始，截取length长度的
                NetworkMessage me = NetworkMessage.GetMessage(messageBytes);
                NetworkManager._Instance.AddMessage(me);
            }
        }


        public virtual void SendMsg(string ip, int port, string msg)
        {
            //定义一个发送的IP和端口
            EndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
            server.SendTo(Encoding.UTF8.GetBytes(msg), point);
        }

        public virtual void SendMsg(string ip, int port, byte[] msg)
        {
            //定义一个发送的IP和端口
            EndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
            server.SendTo(msg, point);
        }
        #endregion

    }
}
