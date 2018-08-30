using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class BattleAIClient : MsgCallBackBase<NetworkMessage> ,IBattleAI
    {
        public PlayerAI player;

        public string ip;
        public string IP
        {
            get
            {
                return ip;
            }
        }

        public void Awake()
        {
            base.BaseAwake();
            player = GetComponent<PlayerAI>();
            NetworkManager._Instance.AddCallBack(4, GetNetworkMsgCallBack);
            NetworkManager._Instance.AddCallBack(5, GetNetworkMsgCallBack);
            NetworkManager._Instance.AddCallBack(6, GetNetworkMsgCallBack);
            NetworkManager._Instance.AddCallBack(18, GetNetworkMsgCallBack);
        }

        public void Update()
        {
            base.BaseUpdate();
        }

        public void OnDestroy()
        {
            NetworkManager._Instance.RemoveCallBack(4, GetNetworkMsgCallBack);
            NetworkManager._Instance.RemoveCallBack(5, GetNetworkMsgCallBack);
            NetworkManager._Instance.RemoveCallBack(6, GetNetworkMsgCallBack);
            NetworkManager._Instance.RemoveCallBack(18, GetNetworkMsgCallBack);
            base.BaseDestroy();
        }

        #region 实现抽象类的方法
        protected override void NetworkCallback(NetworkMessage message)
        {
            if (message.type == 5 && message.ip == IP)
            {
                PlayerStateMessage m = PlayerStateMessage.GetMessage(message.message);
                player.SetState(m.state, m.atkState);
            }
            else if (message.type == 6 && message.ip == IP)
            {
                PlayerHurtMessage m = PlayerHurtMessage.GetMessage(message.message);
                player.ReduceHP(m.hurt);
            }
            else if (message.type == 18 && message.ip == IP)
            {
                player.Shoot();
            }
        }

        /// <summary>
        /// 网络回调
        /// </summary>
        /// <param name="obj_arr"></param>
        protected override void GetNetworkMsgCallBack(params object[] obj_arr)
        {
            try
            {
                NetworkMessage message = (NetworkMessage)obj_arr[0];

                if (message.type == 4 && IP.Equals(message.ip))
                {
                    TransformMessage tr = TransformMessage.GetMessage(message.message);
                    //同步位置
                    player.SetTarget(tr);
                }
                else if (message.type == 5 && message.ip == IP)
                {
                    AddMessage(message);
                }
                else if (message.type == 6 && message.ip == IP)
                {
                    AddMessage(message);
                }
                else if (message.type == 18 && message.ip == IP)
                {
                    AddMessage(message);
                }
            }
            catch (System.Exception e)
            {

                Debug.LogError(e.ToString());
            }
        }

        #endregion
    }

}
