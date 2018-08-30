using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class ReviceRoomUIMsgCallBack : MsgCallBackBase<object> {

        ReviceRoomUI room;

        #region 抽象类方法
        protected override void GetNetworkMsgCallBack(params object[] obj_arr)
        {
            AddMessage(obj_arr[0]);
        }

        protected override void NetworkCallback(object message)
        {
            //Debug.Log(message.GetType().ToString());
            if (message.GetType().ToString().Equals("Network.RoomData"))
            {
                room.UpdateData((RoomData)message);
            }
            else if (message.GetType().ToString().Equals("Network.NetworkMessage"))
            {
                NetworkMessage nm = (NetworkMessage)message;
                if (nm.type == 8)
                {
                    RoomSingle.roomIP = nm.ip;
                    Debug.Log(RoomSingle.roomIP);
                    room.AddRoomSuccess();
                }
            }

        }

        #endregion

        #region 生命周期函数
        // Use this for initialization
        void Awake()
        {
            base.BaseAwake();
            room = GetComponent<ReviceRoomUI>();
            Network.NetworkManager._Instance.StartReviceRoomClient();
        }

        private void OnEnable()
        {
            NetworkManager._Instance.AddCallBack(101, GetNetworkMsgCallBack);
            NetworkManager._Instance.AddCallBack(8, GetNetworkMsgCallBack);
            Network.NetworkManager._Instance.ReviceRoom();
        }

        // Update is called once per frame
        void Update()
        {
            base.BaseUpdate();
        }

        private void OnDisable()
        {
            NetworkManager._Instance.RemoveCallBack(101, GetNetworkMsgCallBack);
            NetworkManager._Instance.RemoveCallBack(8, GetNetworkMsgCallBack);
            Network.NetworkManager._Instance.StopReviceRoom();
        }

        private void OnDestroy()
        {
            base.BaseDestroy();
            
            Network.NetworkManager._Instance.CloseReviceRoom();

        }

        #endregion

    }
}

