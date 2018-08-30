using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Linq;

namespace Network
{
    public class CreateRoomServer : UDPServerBase
    {
        //房间的玩家
        RoomPlayerData playerData;

        /// <summary>
        /// 房间信息
        /// </summary>
        byte[] message;
        RoomMessage room;

        PlayerInfoMessage playerInfo;

        public CreateRoomServer()
        {
            CreateRoom();
        }

        public CreateRoomServer(string roomName)
        {
            CreateRoom(roomName);
        }

        void CreateRoom()
        {
            CreateRoom("房间1");
        }

        void CreateRoom(string roomName)
        {
            room = new RoomMessage(roomName, NetworkTools.GetLocalIP());

            NetworkManager._Instance.AddCallBack(2, SendRoomMsg);
            NetworkManager._Instance.AddCallBack(7, AddRoomMsgCallBack);
            NetworkManager._Instance.AddCallBack(9, QuitRoomMsgCallBack);
            NetworkManager._Instance.AddCallBack(10, UpdateRoomPlayerMsgCallBack);
            NetworkManager._Instance.AddCallBack(14, ReciveGetPlayerInfos);

            StartServer();
            playerData = new RoomPlayerData();
            playerData.AddPlayerInfo(NetworkTools.GetLocalIP(), new RoomPlayerInfoMessage("", true, NetworkTools.GetLocalIP()));
            playerInfo = new PlayerInfoMessage(1, 101, 102, "房主", NetworkTools.GetLocalIP());
            //RoomSingle.AddPlayer(new PlayerInfoMessage(2,104,105,"房主",NetworkTools.GetLocalIP()));
            NetworkManager._Instance.AddMessage(102, playerData);
            EventCenterManager._Instance.AddListener(EventType.GetPlayerInfo, GetPlayerInfoCallBack);
            EventCenterManager._Instance.AddListener(EventType.SetPlayerInfo, SetPlayerInfoCallBack);
        }

        public override void StartServer()
        {
            base.StartServer();
        }

        public override void CloseServer()
        {
            NetworkManager._Instance.RemoveCallBack(2, SendRoomMsg);
            NetworkManager._Instance.RemoveCallBack(7, AddRoomMsgCallBack);
            NetworkManager._Instance.RemoveCallBack(9, QuitRoomMsgCallBack);
            NetworkManager._Instance.RemoveCallBack(10, UpdateRoomPlayerMsgCallBack);
            NetworkManager._Instance.RemoveCallBack(14, ReciveGetPlayerInfos);
            EventCenterManager._Instance.RemoveListener(EventType.GetPlayerInfo, GetPlayerInfoCallBack);
            EventCenterManager._Instance.RemoveListener(EventType.SetPlayerInfo, SetPlayerInfoCallBack);
            SendAllClose();
            base.CloseServer();
        }

        /// <summary>
        /// 向所有客户端发送房间关闭信息
        /// </summary>
        void SendAllClose()
        {
            server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            EndPoint point = new IPEndPoint(IPAddress.Broadcast, NetworkConstent.UDPClientPort);
            NetworkMessage m = new NetworkMessage(3, NetworkTools.GetLocalIP(), Encoding.UTF8.GetBytes(NetworkTools.GetLocalIP()));
            server.SendTo(NetworkMessage.GetBytes(m), point);
        }


        #region 网络请求回调

        /// <summary>
        /// 向客户端发送房间信息
        /// </summary>
        /// <param name="obj_arr"></param>
        void SendRoomMsg(params object[] obj_arr)
        {
            NetworkMessage m = (NetworkMessage)obj_arr[0];
            NetworkMessage rom = new NetworkMessage(1, room.ip,
                    RoomMessage.GetBytes(room));
            message = NetworkMessage.GetBytes(rom);
            string ip = Encoding.UTF8.GetString(m.message);
            //NetworkTools.PrintMessage("发送房间信息" + ip);
            SendMsg(ip, NetworkConstent.UDPClientPort, message);
        }

        /// <summary>
        /// 向客户端发送可以加入信息
        /// </summary>
        /// <param name="obj_arr"></param>
        void AddRoomMsgCallBack(params object[] obj_arr)
        {
            //房间最多4个人
            if (playerData.GetPlayersInfo().Count < 4)
            {
                NetworkMessage m = (NetworkMessage)obj_arr[0];
                RoomPlayerInfoMessage info = RoomPlayerInfoMessage.GetMessage(m.message);
                playerData.AddPlayerInfo(m.ip, info);
                NetworkManager._Instance.AddMessage(102, playerData);
                Debug.Log("房间加入玩家" + m.ip + " --- " + playerData.GetPlayersInfo().Count);
                byte[] bytes = NetworkMessage.GetBytes(new NetworkMessage(8, NetworkTools.GetLocalIP(), new byte[0]));
                SendMsg(m.ip, NetworkConstent.UDPClientPort, bytes);
                //向所有玩家发送加入信息
                foreach (var item in playerData.GetPlayersInfo().Keys)
                {
                    if (!item.Equals(NetworkTools.GetLocalIP()))
                    {
                        SendPlayerInfosToClient(item);
                    }
                }
                
            }
        }

        /// <summary>
        /// 退出房间的回调
        /// </summary>
        /// <param name="obj_arr"></param>
        void QuitRoomMsgCallBack(params object[] obj_arr)
        {
            NetworkMessage m = (NetworkMessage)obj_arr[0];
            playerData.RemovePlayerInfo(m.ip);
            NetworkManager._Instance.AddMessage(102, playerData);
            RoomSingle.RemovePlayer(m.ip);//装备情况删除
            //向所有玩家发送退出信息
            foreach (var item in playerData.GetPlayersInfo().Keys)
            {
                if (!item.Equals(NetworkTools.GetLocalIP()))
                {
                    SendRemovePlayrInfo(item, new RoomPlayerInfoMessage("", true, m.ip));
                }
            }
        }

        /// <summary>
        /// 更新玩家信息
        /// </summary>
        /// <param name="obj_arr"></param>
        void UpdateRoomPlayerMsgCallBack(params object[] obj_arr)
        {
            NetworkMessage m = (NetworkMessage)obj_arr[0];
            PlayerInfoMessage info = PlayerInfoMessage.GetMessage(m.message);
            playerData.SetPlayerCanStart(m.ip, true);
            NetworkManager._Instance.AddMessage(102, playerData);

            RoomSingle.AddPlayer(info);
            //向其他玩家发送玩家的装备情况
            foreach (var item in playerData.GetPlayersInfo().Keys)
            {
                SendPlayrMessageInfo(item, info);//只要接到玩家的进入游戏的信息，那么玩家就进入了准备状态
            }
            //向所有玩家发送玩家的状态更新
            //foreach (var item in playerData.GetPlayersInfo().Keys)
            //{
             //   SendUpdatePlayrInfo(item, new RoomPlayerInfoMessage("",true, m.ip));
            //}
        }

        /// <summary>
        /// 向房间内的玩家发送开始游戏请求
        /// </summary>
        public void SendOtherPlayerStartGame()
        {
            RoomSingle.AddPlayer(playerInfo);
            byte[] infos = PlayerInfoMessage.GetBytes(RoomSingle.GetInfos()[NetworkTools.GetLocalIP()]);
            byte[] bytes = NetworkMessage.
                GetBytes(new NetworkMessage(13, NetworkTools.GetLocalIP(), infos));
            foreach (var ip in playerData.GetPlayersInfo().Keys)
            {
                SendMsg(ip, NetworkConstent.UDPClientPort, bytes);
            }
        }


        /// <summary>
        /// 发送房间内其他玩家的信息的回调
        /// </summary>
        public void ReciveGetPlayerInfos(params object[] obj_arr)
        {
            Debug.Log("发送玩家信息");
            NetworkMessage m = (NetworkMessage)obj_arr[0];
            SendPlayerInfosToClient(m.ip);
        }

        /// <summary>
        /// 向玩家发送单个更新的玩家信息
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="info"></param>
        public void SendAddPlayrInfo(string ip, RoomPlayerInfoMessage info)
        {
            byte[] bytes = RoomPlayerInfoMessage.GetBytes(info);
            NetworkMessage sendM = new NetworkMessage(15, NetworkTools.GetLocalIP(), bytes);
            SendMsg(ip, NetworkConstent.UDPClientPort, NetworkMessage.GetBytes(sendM));
        }

        /// <summary>
        /// 向玩家发送单个玩家退出
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="info"></param>
        public void SendRemovePlayrInfo(string ip, RoomPlayerInfoMessage info)
        {
            byte[] bytes = RoomPlayerInfoMessage.GetBytes(info);
            NetworkMessage sendM = new NetworkMessage(16, NetworkTools.GetLocalIP(), bytes);
            SendMsg(ip, NetworkConstent.UDPClientPort, NetworkMessage.GetBytes(sendM));
        }

        /// <summary>
        /// 向玩家发送单个玩家更新状态
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="info"></param>
        public void SendUpdatePlayrInfo(string ip, RoomPlayerInfoMessage info)
        {
            byte[] bytes = RoomPlayerInfoMessage.GetBytes(info);
            NetworkMessage sendM = new NetworkMessage(10, NetworkTools.GetLocalIP(), bytes);
            SendMsg(ip, NetworkConstent.UDPClientPort, NetworkMessage.GetBytes(sendM));
        }

        /// <summary>
        /// 向玩家发送房间内所有玩家信息
        /// </summary>
        /// <param name="ip"></param>
        public void SendPlayerInfosToClient(string ip)
        {
            foreach (var item in playerData.GetPlayersInfo().Keys)
            {
                byte[] bytes = RoomPlayerInfoMessage.GetBytes(playerData.GetPlayersInfo()[item]);
                NetworkMessage sendM = new NetworkMessage(15, NetworkTools.GetLocalIP(), bytes);
                SendMsg(ip, NetworkConstent.UDPClientPort, NetworkMessage.GetBytes(sendM));
            }
        }

        /// <summary>
        /// 向玩家发送单个玩家的IP，枪械情况
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="info"></param>
        public void SendPlayrMessageInfo(string ip, PlayerInfoMessage info)
        {
            byte[] bytes = PlayerInfoMessage.GetBytes(info);
            NetworkMessage sendM = new NetworkMessage(17, NetworkTools.GetLocalIP(), bytes);
            SendMsg(ip, NetworkConstent.UDPClientPort, NetworkMessage.GetBytes(sendM));
        }

        #endregion


        #region 事件回调

        void GetPlayerInfoCallBack(params object[] obj_arr)
        {
            EventCenterManager._Instance.SendMessage(EventType.SendPlayerInfo, playerInfo.gun1ID, playerInfo.gun2ID);
        }

        void SetPlayerInfoCallBack(params object[] obj_arr)
        {
            playerInfo.gun1ID = (int)obj_arr[0];
            playerInfo.gun2ID = (int)obj_arr[1];
        }

        #endregion

    }
}

