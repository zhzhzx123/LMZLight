using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class RoomUIMsgCallBack : MsgCallBackBase<RoomPlayerData>
    {
        RoomUI room;
        #region 实现抽象类方法
        protected override void GetNetworkMsgCallBack(params object[] obj_arr)
        {
            if (obj_arr[0].GetType().ToString().Equals("Network.RoomPlayerData"))
            {
                AddMessage((RoomPlayerData)obj_arr[0]);
            }
        }

        protected override void NetworkCallback(RoomPlayerData message)
        {
            room.UpdatePanel(message);
        }
        #endregion

        #region 生命周期函数

        private void Awake()
        {
            base.BaseAwake();
            room = GetComponent<RoomUI>();
        }

        private void OnEnable()
        {
            NetworkManager._Instance.AddCallBack(102, GetNetworkMsgCallBack);
            NetworkManager._Instance.CreateRoom("房间111");
            RoomSingle.roomIP = NetworkTools.GetLocalIP();
        }

        private void Update()
        {
            base.BaseUpdate();
        }

        private void OnDisable()
        {
            NetworkManager._Instance.RemoveCallBack(102, GetNetworkMsgCallBack);
           
            NetworkManager._Instance.DestroyRoom();
        }

        private void OnDestroy()
        {
            base.BaseDestroy();
        }

        #endregion
    }

}


