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
    public class BattlePlayerClient : ClientSendBase
    {

        public BattlePlayerClient()
        {
           
        }

        /// <summary>
        /// 同步位置方法
        /// </summary>
        /// <param name="trans"></param>
        public void SendTranfromToServer(Transform trans, float gunAngle)
        {
            try
            {
                NetworkMessage message = new NetworkMessage(4, NetworkTools.GetLocalIP(),
                TransformMessage.GetBytes(new TransformMessage(
                trans.position, trans.rotation.eulerAngles, gunAngle)));
                byte[] bytes = NetworkMessage.GetBytes(message);
                //Debug.Log("发送位置数据");
                //测试
                SendMsg(RoomSingle.roomIP, NetworkConstent.UDPServerPort, bytes);
            }
            catch (Exception e)
            {

                Debug.LogError(e.ToString());
            }
        }

        /// <summary>
        /// 同步状态的方法
        /// </summary>
        /// <param name="state"></param>
        public void SendPlayerStateToServer(PlayerState playerState, AtkState atkState)
        {
            try
            {
                NetworkMessage message = new NetworkMessage(5, NetworkTools.GetLocalIP(),
                    PlayerStateMessage.GetBytes(new PlayerStateMessage(playerState,
                    atkState)));
                byte[] bytes = NetworkMessage.GetBytes(message);
                //Debug.Log("发送位置数据");
                //测试
                SendMsg(RoomSingle.roomIP, NetworkConstent.UDPServerPort, bytes);
            }
            catch (Exception e)
            {

                Debug.LogError(e.ToString());
            }
        }

        /// <summary>
        /// 同步状态的方法
        /// </summary>
        /// <param name="state"></param>
        public void SendPlayerHurtToServer(float hurt)
        {
            try
            {
                NetworkMessage message = new NetworkMessage(6, NetworkTools.GetLocalIP(),
                    PlayerHurtMessage.GetBytes(new PlayerHurtMessage(hurt)));
                byte[] bytes = NetworkMessage.GetBytes(message);
                SendMsg(RoomSingle.roomIP, NetworkConstent.UDPServerPort, bytes);
            }
            catch (Exception e)
            {

                Debug.LogError(e.ToString());
            }
        }

        public void SendPlayerShootToServer()
        {
            try
            {
                NetworkMessage message = new NetworkMessage(18, NetworkTools.GetLocalIP(),new byte[0]);
                byte[] bytes = NetworkMessage.GetBytes(message);
                SendMsg(RoomSingle.roomIP, NetworkConstent.UDPServerPort, bytes);
            }
            catch (Exception e)
            {

                Debug.LogError(e.ToString());
            }
        }
    }
}

