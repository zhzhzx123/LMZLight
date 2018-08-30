using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class WaitStartRoomUICallBack : MsgCallBackBase<object>
    {
        WaitStartRoomUI wait;
        //房间的玩家
        RoomPlayerData playerData;

        protected override void GetNetworkMsgCallBack(params object[] obj_arr)
        {
            AddMessage(obj_arr[0]);
        }

        protected override void NetworkCallback(object message)
        {
            if (message.GetType().ToString().Equals("Network.NetworkMessage"))
            {
                Debug.Log("客户端接收");
                NetworkMessage nm = (NetworkMessage)message;
                if (nm.type == 13)
                {
                    PlayerInfoMessage info = PlayerInfoMessage.GetMessage(nm.message);
                    RoomSingle.AddPlayer(info);
                    //Debug.Log("开始游戏");
                    UIManager._Instance.CloseWindow(WindowName.WaitStartRoom);
                    LoadSceneManager._Instance.LoadScene(SceneName.Mission1, StartGameCallBack);
                    //NetworkManager._Instance.RemoveCallBack(13, GetNetworkMsgCallBack);
                }
                else if (nm.type == 10)
                {
                    RoomPlayerInfoMessage info = RoomPlayerInfoMessage.GetMessage(nm.message);
                    playerData.SetPlayerCanStart(info.playerIP, info.canStart);
                    wait.UpdatePanel(playerData);
                }
                else if (nm.type == 15)
                {
                    RoomPlayerInfoMessage info = RoomPlayerInfoMessage.GetMessage(nm.message);
                    playerData.AddPlayerInfo(info.playerIP, info);
                    wait.UpdatePanel(playerData);
                }
                else if (nm.type == 16)
                {
                    RoomPlayerInfoMessage info = RoomPlayerInfoMessage.GetMessage(nm.message);
                    playerData.RemovePlayerInfo(info.playerIP);
                    RoomSingle.RemovePlayer(info.playerIP);
                    wait.UpdatePanel(playerData);
                }
                else if (nm.type == 17)
                {
                    PlayerInfoMessage info = PlayerInfoMessage.GetMessage(nm.message);
                    RoomSingle.AddPlayer(info);
                    playerData.SetPlayerCanStart(info.playerIP, true);
                    wait.UpdatePanel(playerData);
                }
            }
        }

        /// <summary>
        /// 进入游戏场景的回调
        /// </summary>
        void StartGameCallBack(params object[] obj_Arr)
        {
            
        }

        #region 生命周期函数

        private void Awake()
        {
            base.BaseAwake();
            wait = GetComponent<WaitStartRoomUI>();
            playerData = new RoomPlayerData();


        }

        private void OnEnable()
        {
            NetworkManager._Instance.AddCallBack(13, GetNetworkMsgCallBack);
            NetworkManager._Instance.AddCallBack(10, GetNetworkMsgCallBack);
            NetworkManager._Instance.AddCallBack(15, GetNetworkMsgCallBack);
            NetworkManager._Instance.AddCallBack(16, GetNetworkMsgCallBack);
            NetworkManager._Instance.AddCallBack(17, GetNetworkMsgCallBack);
            wait.GetPlayerInfos();
        }

        private void Update()
        {
            base.BaseUpdate();
        }

        private void OnDisable()
        {
            NetworkManager._Instance.RemoveCallBack(13, GetNetworkMsgCallBack);
            NetworkManager._Instance.RemoveCallBack(10, GetNetworkMsgCallBack);
            NetworkManager._Instance.RemoveCallBack(15, GetNetworkMsgCallBack);
            NetworkManager._Instance.RemoveCallBack(16, GetNetworkMsgCallBack);
            NetworkManager._Instance.RemoveCallBack(17, GetNetworkMsgCallBack);
        }

        private void OnDestroy()
        {
            base.BaseDestroy();
        }

        #endregion

    }
}

