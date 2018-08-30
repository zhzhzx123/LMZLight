using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Network
{
    /// <summary>
    /// 该类是处理非主机的小怪的回调
    /// </summary>
    public class ClientEnemyCallBack : MsgCallBackBase<NetworkMessage>
    {

        private Dictionary<string, NetEnemyBase> enemyDic = new Dictionary<string, NetEnemyBase>();

        protected override void NetworkCallback(NetworkMessage message)
        {
            if (message.type == 60)
            {
                EnemyInfoMessage info = EnemyInfoMessage.GetMessage(message.message);
                string key = info.enemyType.ToString() + info.num.ToString();
                if (!enemyDic.ContainsKey(key))
                {
                    GameObject prefab = Resources.Load<GameObject>("Prefab/Enemy/Enemy_AI_" + info.enemyType);
                    GameObject obj = Instantiate<GameObject>(prefab,
                        new Vector3(info.x, info.y, info.z), Quaternion.Euler(info.angleX, info.angleY, info.angleZ), transform);
                    NetEnemyBase net = obj.GetComponent<NetEnemyBase>();
                    net.SetIDandListNum(info.enemyType, info.num);
                    net.SetTarget(info);
                    enemyDic.Add(key, net);
                }
            }
            else if (message.type == 61)
            {
                //Debug.Log("同步小怪状态");
                EnemyStateMessage info = EnemyStateMessage.GetMessage(message.message);
                string key = info.enemyType.ToString() + info.num.ToString();
                if (enemyDic.ContainsKey(key))
                {
                    enemyDic[key].SwitchEnemyState((EnemyState)info.state);
                    if (info.state == (int)(EnemyState.Death))
                    {
                        enemyDic.Remove(key);
                    }
                }
            }
            else if (message.type == 62 && !NetworkTools.GetLocalIP().Equals(message.ip))
            {
                EnemyHurtMessge info = EnemyHurtMessge.GetMessage(message.message);
                string key = info.enemyType.ToString() + info.num.ToString();
                if (enemyDic.ContainsKey(key))
                {
                    enemyDic[key].ReduceHP(info.hurt);
                }
            }
        }

        protected override void GetNetworkMsgCallBack(params object[] obj_arr)
        {
            NetworkMessage message = (NetworkMessage)obj_arr[0];
            if (message.type == 60)//位置同步 
            {
                //Debug.Log("同步小怪位置");
                EnemyInfoMessage info = EnemyInfoMessage.GetMessage(message.message);
                string key = info.enemyType.ToString() + info.num.ToString();
                if (enemyDic.ContainsKey(key))
                {
                    enemyDic[key].SetTarget(info);
                }
                else
                {
                    AddMessage(message);
                }
            }
            else if (message.type == 61 || message.type == 62)
            {
                AddMessage(message);
            }
        }

        public void Awake()
        {
            base.BaseAwake();
            if (NetworkTools.GetLocalIP().Equals(RoomSingle.roomIP))
            {
                Destroy(this);
                return;
            }
            NetworkManager._Instance.AddCallBack(60, GetNetworkMsgCallBack);
            NetworkManager._Instance.AddCallBack(61, GetNetworkMsgCallBack);
            NetworkManager._Instance.AddCallBack(62, GetNetworkMsgCallBack);
        }

        public void Update()
        {
            base.BaseUpdate();
        }

        public void OnDestroy()
        {
            NetworkManager._Instance.RemoveCallBack(60, GetNetworkMsgCallBack);
            NetworkManager._Instance.RemoveCallBack(61, GetNetworkMsgCallBack);
            NetworkManager._Instance.RemoveCallBack(62, GetNetworkMsgCallBack);
            base.BaseDestroy();
        }


    }
}
