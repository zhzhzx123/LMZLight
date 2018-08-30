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
    public class UDPClientBase
    {
        #region 变量
        protected Socket server;//套接字
        protected Thread reciveTh;//接收线程
        #endregion

        #region 构造方法
        public UDPClientBase()
        {
            
        }

        ~UDPClientBase()
        {
            CloseServer();
        }
        #endregion

        #region 方法方法
        /// <summary>
        /// 启动客户端接收房间信息
        /// </summary>
        public virtual void StartServer(int port)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(new IPEndPoint(IPAddress.Parse(NetworkTools.GetLocalIP()), port));//绑定端口号和IP
            
            //NetworkTools.PrintMessage("客户端已启动：" + NetworkTools.GetLocalIP() + " -- " + NetworkConstent.UDPClientPort);

            reciveTh = new Thread(ReciveMsg);
            reciveTh.Start();
        }

        /// <summary>
        /// 关闭客户端
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

                if (server != null)
                {
                    server.Close();
                    server = null;
                    NetworkTools.PrintMessage("关闭客户端!");
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
            try
            {
                NetworkTools.PrintMessage("客户端开始接收消息");
                byte[] buffer = new byte[1024];
                while (true)
                {
                    EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                    int length = server.ReceiveFrom(buffer, ref point);//接收数据报
                    byte[] messageBytes = buffer.Skip(0).Take(length).ToArray();//截取数组，从第0位开始，截取length长度的
                                                                   //NetworkTools.PrintMessage("接收到消息" + messageBytes.Length);
                    NetworkMessage me = NetworkMessage.GetMessage(messageBytes);
                    //Debug.Log("收到客户端信息; " + me.type);
                    NetworkManager._Instance.AddMessage(me);
                }
            }
            catch (Exception e)
            {

                //Debug.LogError(e.ToString());
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

