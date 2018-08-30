using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;

namespace Network
{
    public class ReviceRoomSendClient : ClientSendBase
    {
        /// <summary>
        /// 想要加入的房间
        /// </summary>
        /// <param name="ip"></param>
        public void SendAddRoom(string ip)
        {
            try
            {
                IPAddress.Parse(ip);
                Debug.Log("请求加入: " + ip);
            }
            catch (System.Exception)
            {
                Debug.LogError("请输入有效IP");
                return;
            }
            byte[] info = RoomPlayerInfoMessage.GetBytes(new RoomPlayerInfoMessage("", false, NetworkTools.GetLocalIP()));
            SendMsg(ip, NetworkConstent.UDPServerPort,
                    NetworkMessage.GetBytes(new NetworkMessage(7, NetworkTools.GetLocalIP(), info)));
        }
    }
}

