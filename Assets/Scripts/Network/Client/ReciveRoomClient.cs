using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using System;
using System.Linq;

namespace Network
{
    public class ReciveRoomClient : UDPClientBase
    {
        public RoomData rooms;

        /// <summary>
        /// 房间信息
        /// </summary>
        byte[] message;

        Thread sendTh;

        public ReciveRoomClient()
        {
            StartServer(NetworkConstent.UDPClientPort);
            server.IOControl((int)NetworkConstent.SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
        }

        /// <summary>
        /// 开始接收房间信息
        /// </summary>
        public void StartSendRoom()
        {

            sendTh = new System.Threading.Thread(GetRoomMsg);
            sendTh.Start();

        }

        /// <summary>
        /// 停止发送请求房间信息
        /// </summary>
        public void StopSendRoom()
        {
            if (sendTh != null)
            {
                sendTh.Abort();
                sendTh = null;
            }
        }

        #region override

        public override void StartServer(int port)
        {
            base.StartServer(port);
            NetworkMessage m = new NetworkMessage(2, NetworkTools.GetLocalIP(), Encoding.UTF8.GetBytes(NetworkTools.GetLocalIP()));

            NetworkManager._Instance.AddCallBack(1, GetRoomMsgCallBack);
            NetworkManager._Instance.AddCallBack(3, GetCloseRoomMsgCallBack);
            rooms = new RoomData();

            message = NetworkMessage.GetBytes(m);

        }

        public override void CloseServer()
        {
            NetworkManager._Instance.RemoveCallBack(1, GetRoomMsgCallBack);
            NetworkManager._Instance.RemoveCallBack(3, GetCloseRoomMsgCallBack);
            try
            {
                if(sendTh != null)
                {
                    sendTh.Abort();
                    sendTh = null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
            base.CloseServer();
        }

        public override void SendMsg(string ip, int port, string msg)
        { 
            server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            EndPoint point = new IPEndPoint(IPAddress.Broadcast, port);
            server.SendTo(Encoding.UTF8.GetBytes(msg), point);
        }

        public override void SendMsg(string ip, int port, byte[] msg)
        {
            //base.SendMsg(ip, port, msg);
            server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            EndPoint point = new IPEndPoint(IPAddress.Broadcast, port);
            server.SendTo(msg, point);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 向各个ip发送房间信息
        /// </summary>
        void GetRoomMsg()
        {
            try
            {
                while (true)
                {
                    //NetworkTools.PrintMessage("获取房间信息" +
                    //    Encoding.UTF8.GetString(message, 4, message.Length - 4) + " ---- " + (message.Length - 4));
                    SendMsg("", NetworkConstent.UDPServerPort, message);
                    //睡眠1s，每秒发送一次
                    Thread.Sleep(3000);
                    if (rooms.CheckRoom())//检测房间是否还有回应
                    {
                        NetworkManager._Instance.AddMessage(101, rooms);
                    }
                }
            }
            catch (Exception e)
            {

                Debug.LogWarning(e.ToString());
            }
           
        }

        /// <summary>
        /// 房间信息回调
        /// </summary>
        /// <param name="obj_arr"></param>
        void GetRoomMsgCallBack(params object[] obj_arr)
        {
            NetworkMessage message = (NetworkMessage)obj_arr[0];
            rooms.AddMessage(RoomMessage.GetMessage(message.message));

            NetworkManager._Instance.AddMessage(101, rooms);
        }

        /// <summary>
        /// 获取关闭房间的信息
        /// </summary>
        /// <param name="obj_arr"></param>
        void GetCloseRoomMsgCallBack(params object[] obj_arr)
        {
            NetworkMessage message = (NetworkMessage)obj_arr[0];
            rooms.RemoveMessage(Encoding.UTF8.GetString(message.message));

            NetworkManager._Instance.AddMessage(101, rooms);
        }

        #endregion


    }
}

