using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    /// <summary>
    /// 该类是处理主机小怪的网络回调
    /// </summary>
    public class ServerEnemyCallBack : MsgCallBackBase<NetworkMessage>
    {
        private EnemyBase enemy;
        protected override void GetNetworkMsgCallBack(params object[] obj_arr)
        {
            NetworkMessage message = (NetworkMessage)obj_arr[0];
            if (message.type == 62 && !NetworkTools.GetLocalIP().Equals(message.ip))
            {
                EnemyHurtMessge hurt = EnemyHurtMessge.GetMessage(message.message);
                if (hurt.enemyType == enemy.enemyData.ID && hurt.num == enemy.enemyData.Listnum)
                {
                    AddMessage((NetworkMessage)obj_arr[0]);
                }
            }
        }

        protected override void NetworkCallback(NetworkMessage message)
        {
            if (message.type == 62)
            {
                EnemyHurtMessge hurt = EnemyHurtMessge.GetMessage(message.message);
                enemy.ReduceHP(hurt.hurt);
            }
        }

        public void Awake()
        {
            base.BaseAwake();
            enemy = GetComponent<EnemyBase>();
            NetworkManager._Instance.AddCallBack(62, GetNetworkMsgCallBack);
        }

        public void Update()
        {
            base.BaseUpdate();
        }

        public void OnDestroy()
        {
            NetworkManager._Instance.RemoveCallBack(62, GetNetworkMsgCallBack);
            base.BaseDestroy();
        }
    }

}
