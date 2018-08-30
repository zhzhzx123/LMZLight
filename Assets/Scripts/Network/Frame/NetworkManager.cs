using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class NetworkManager : Single<NetworkManager>, IManager
    {

        #region 单例
        public NetworkManager()
        {
        }

        #endregion
        #region IManager
        public void Enter()
        {

        }

        public void Exit()
        {

        }

        public void Update()
        {
             /*
             if (messageQueue.Count > 0)
             {
                 NetworkMessage message = messageQueue.Dequeue();
                 //Debug.Log("收到消息!" + message.type);

                 ExcuteCallBack(message.type, message);
             }
             */
        }

        #endregion

        #region 变量

        UDPServerBase createRoomServer;//服务器

        UDPClientBase getRoomClient;

        UDPServerBase battleSever;

        UDPClientBase battleClient;

        //消息队列
        Queue<NetworkMessage> messageQueue = new Queue<NetworkMessage>();

        //消息回调
        Dictionary<int, NetworkCallback> callbackDic = new Dictionary<int, NetworkCallback>();

        #endregion

        #region 处理消息队列的方法

        public void AddMessage(NetworkMessage message)
        {
            ExcuteCallBack(message.type, message);//直接调取消息的回调
        }

        public void AddMessage(int type, params object[] obj_arr)
        {
            ExcuteCallBack(type, obj_arr);//直接调取消息的回调
        }

        public void AddCallBack(int type, NetworkCallback callback)
        {
            if (!callbackDic.ContainsKey(type))
            {
                callbackDic.Add(type, callback);
            }
            else
            {
                callbackDic[type] += callback;
            }
        }

        public void RemoveCallBack(int type, NetworkCallback callback)
        {
            if (callbackDic.ContainsKey(type))
            {
                callbackDic[type] -= callback;
            }
        }

        void ExcuteCallBack(int type, params object[] obj_arr)
        {
            if (callbackDic.ContainsKey(type) && callbackDic[type] != null)
            {
                callbackDic[type](obj_arr);
            }
        }

        #endregion

        #region 房间方法

        /// <summary>
        /// 创建房间
        /// </summary>
        public void CreateRoom()
        {
            if(createRoomServer == null)
            {
                createRoomServer = new CreateRoomServer();
            }
        }

        /// <summary>
        /// 创建房间
        /// </summary>
        public void CreateRoom(string roomName)
        {
            if (createRoomServer == null)
            {
                createRoomServer = new CreateRoomServer(roomName);
            }
        }

        /// <summary>
        /// 删除房间
        /// </summary>
        public void DestroyRoom()
        {
            if (createRoomServer != null)
            {
                createRoomServer.CloseServer();
                createRoomServer = null;
            }
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        public void StartGame()
        {
            (createRoomServer as CreateRoomServer).SendOtherPlayerStartGame();
        }

        /// <summary>
        /// 开始启动接收房间的服务器
        /// </summary>
        public void StartReviceRoomClient()
        {
            if (getRoomClient == null)
            {
                getRoomClient = new ReciveRoomClient();
            }
        }

        /// <summary>
        /// 接收房间
        /// </summary>
        public void ReviceRoom()
        {
            ReciveRoomClient client = getRoomClient as ReciveRoomClient;
            if (client != null)
            {
                client.StartSendRoom();
            }
        }

        /// <summary>
        /// 停止接收房间信息
        /// </summary>
        public void StopReviceRoom()
        {
            ReciveRoomClient client = getRoomClient as ReciveRoomClient;
            if (client != null)
            {
                client.StopSendRoom();
            }
        }

        public void CloseReviceRoom()
        {
            if (getRoomClient != null)
            {
                getRoomClient.CloseServer();
                getRoomClient = null;
            }
        }


        /// <summary>
        /// 启动战斗服务器
        /// </summary>
        public void StartBattleServer()
        {
            DestroyRoom();
            if (battleSever == null)
            {
                battleSever = new BattleServer();
            }
        }

        public void StartBattleServer(string[] ips)
        {
            DestroyRoom();
            if (battleSever == null)
            {
                battleSever = new BattleServer();
                for (int i = 0; i < ips.Length; i++)
                {
                    ((BattleServer)battleSever).players.Add(ips[i]);
                }
            }
        }

        /// <summary>
        /// 停止战斗服务器
        /// </summary>
        public void CloseBattleServer()
        {
            if (battleSever != null)
            {
                battleSever.CloseServer();
                battleSever = null;
            }
        }


        public void StartBattleClient()
        {
            StopReviceRoom();
            if (battleClient == null)
            {
                battleClient = new BattleClient();
            }
        }

        public void CloseBattleClient()
        {
            if (battleClient != null)
            {
                battleClient.CloseServer();
                battleClient = null;
            }
        }

        #endregion

    }
}
