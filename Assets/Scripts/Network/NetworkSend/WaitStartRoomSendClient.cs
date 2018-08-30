using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class WaitStartRoomSendClient : ClientSendBase
    {

        public PlayerInfoMessage playerInfo;

        public WaitStartRoomSendClient()
        {
            playerInfo = new PlayerInfoMessage(1, 101, 102, "玩家", NetworkTools.GetLocalIP());
        }


        public void InitInfo()
        {
            playerInfo.gun1ID = 101;
            playerInfo.gun2ID = 102;
        }

        /// <summary>
        /// 发送退出房间
        /// </summary>
        public void SendQuitRoom()
        {
            if (client != null)
            {
                SendMsg(RoomSingle.roomIP, NetworkConstent.UDPServerPort,
        NetworkMessage.GetBytes(new NetworkMessage(9, NetworkTools.GetLocalIP(), new byte[0])));

            }
        }

        /// <summary>
        /// 发送可以开始
        /// </summary>
        public void SendWaitStart()
        {
            //byte[] bytes = RoomPlayerInfoMessage.GetBytes(new RoomPlayerInfoMessage("", true));
            byte[] bytes = PlayerInfoMessage.GetBytes(playerInfo); 
            SendMsg(RoomSingle.roomIP, NetworkConstent.UDPServerPort,
                    NetworkMessage.GetBytes(new NetworkMessage(10, NetworkTools.GetLocalIP(), bytes)));
        }


        public void SendGetRoomPlayerInfos()
        {
            SendMsg(RoomSingle.roomIP, NetworkConstent.UDPServerPort,
                    NetworkMessage.GetBytes(new NetworkMessage(14, NetworkTools.GetLocalIP(), null)));
        }

    }
}