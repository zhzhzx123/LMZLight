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
    public class BattleServer : UDPServerBase
    {
        //房间内所有的IP
        public List<string> players = new List<string>();

        public BattleServer()
        {
            //测试
            //players.Add("10.8.44.126");
            //players.Add("10.8.44.10");
            //players.Add("10.8.44.117");
            StartServer();
           
            NetworkTools.PrintMessage("开始战斗服务器");
        }

        public override void CloseServer()
        {
            base.CloseServer();
        }

        protected override void ReciveMsg()
        {
            try
            {
                byte[] buffer = new byte[1024];
                while (true)
                {
                    EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                    int length = server.ReceiveFrom(buffer, ref point);//接收数据报
                    byte[] messageBytes = buffer.Skip(0).Take(length).ToArray();//截取数组，从第0位开始，截取length长度的
                    NetworkMessage me = NetworkMessage.GetMessage(messageBytes);
                    SendMsgToAllClient(me);
                }
            }
            catch (Exception e)
            {

                Debug.LogWarning(e.ToString());
            }
          
        }

        /// <summary>
        /// 向所有的客户端发送同步消息
        /// </summary>
        /// <param name="obj_arr"></param>
        void SendMsgToAllClient(params object[] obj_arr)
        {
            try
            {
                NetworkMessage message = (NetworkMessage)obj_arr[0];
                //Debug.Log("收到客户端发来消息" + message.type);
                byte[] bytes = NetworkMessage.GetBytes(message);
                for (int i = 0; i < players.Count; i++)
                {
                   // Debug.Log("向ip" + players[i] + "发送数据");
                    SendMsg(players[i], NetworkConstent.UDPBattleClientPort, bytes);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
    }
}
