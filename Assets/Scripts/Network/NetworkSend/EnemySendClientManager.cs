using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class EnemySendClientManager : ClientSendBase
    {
        #region 单例

        private static EnemySendClientManager instance;

        public static EnemySendClientManager _Instance
        {
            get {
                if (instance == null)
                {
                    instance = new EnemySendClientManager();
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 发送小怪的位置信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="id"></param>
        /// <param name="listNum"></param>
        public void SendEnemyTransformMsg(Transform t, int id, int listNum)
        {
            byte[] by = EnemyInfoMessage.GetBytes(
                new EnemyInfoMessage(t.position, t.rotation.eulerAngles, id, listNum));
            NetworkMessage message = new NetworkMessage(60, NetworkTools.GetLocalIP(), by);
            byte[] bytes = NetworkMessage.GetBytes(message);
            SendMsg(RoomSingle.roomIP, NetworkConstent.UDPServerPort, bytes);
        }

        /// <summary>
        /// 小怪的状态信息
        /// </summary>
        /// <param name="state"></param>
        /// <param name="id"></param>
        /// <param name="listNum"></param>
        public void SendEnemyStateMsg(int state, int id, int listNum)
        {
            byte[] by = EnemyStateMessage.GetBytes(
                new EnemyStateMessage(state, id, listNum));
            NetworkMessage message = new NetworkMessage(61, NetworkTools.GetLocalIP(), by);
            byte[] bytes = NetworkMessage.GetBytes(message);
            SendMsg(RoomSingle.roomIP, NetworkConstent.UDPServerPort, bytes);
        }

        /// <summary>
        /// 发送小怪受到伤害
        /// </summary>
        /// <param name="hurt"></param>
        /// <param name="id"></param>
        /// <param name="listNum"></param>
        public void SendEnemyHurtMsg(float hurt, int id, int listNum)
        {
            byte[] by = EnemyHurtMessge.GetBytes(
                new EnemyHurtMessge(hurt, id, listNum));
            NetworkMessage message = new NetworkMessage(62, NetworkTools.GetLocalIP(), by);
            byte[] bytes = NetworkMessage.GetBytes(message);
            SendMsg(RoomSingle.roomIP, NetworkConstent.UDPServerPort, bytes);
        }

        /// <summary>
        /// 同步小怪的目标
        /// </summary>
        /// <param name="hurt"></param>
        /// <param name="id"></param>
        /// <param name="listNum"></param>
        public void SendEnemyTargetMsg(string  ip, int id, int listNum)
        {
            byte[] by = EnemyTargetMessage.GetBytes(
                new EnemyTargetMessage(ip, id, listNum));
            NetworkMessage message = new NetworkMessage(63, NetworkTools.GetLocalIP(), by);
            byte[] bytes = NetworkMessage.GetBytes(message);
            SendMsg(RoomSingle.roomIP, NetworkConstent.UDPServerPort, bytes);
        }


        /// <summary>
        /// 发送Boss的位置信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="id"></param>
        /// <param name="listNum"></param>
        public void SendBossTransformMsg(Transform t, int id)
        {
            byte[] by = EnemyInfoMessage.GetBytes(
                new EnemyInfoMessage(t.position, t.rotation.eulerAngles, id, 0));
            NetworkMessage message = new NetworkMessage(70, NetworkTools.GetLocalIP(), by);
            byte[] bytes = NetworkMessage.GetBytes(message);
            SendMsg(RoomSingle.roomIP, NetworkConstent.UDPServerPort, bytes);
        }

        /// <summary>
        /// Boss的状态信息
        /// </summary>
        /// <param name="state"></param>
        /// <param name="id"></param>
        /// <param name="listNum"></param>
        public void SendBossStateMsg(BossState state,BossAtkState s ,int id)
        {
            byte[] by = BossStateMessage.GetBytes(
                new BossStateMessage(state, s));
            NetworkMessage message = new NetworkMessage(71, NetworkTools.GetLocalIP(), by);
            byte[] bytes = NetworkMessage.GetBytes(message);
            SendMsg(RoomSingle.roomIP, NetworkConstent.UDPServerPort, bytes);
        }

        /// <summary>
        /// 发送Boss受到伤害
        /// </summary>
        /// <param name="hurt"></param>
        /// <param name="id"></param>
        /// <param name="listNum"></param>
        public void SendBossHurtMsg(float hurt, int id)
        {
            byte[] by = EnemyHurtMessge.GetBytes(
                new EnemyHurtMessge(hurt, id, 0));
            NetworkMessage message = new NetworkMessage(72, NetworkTools.GetLocalIP(), by);
            byte[] bytes = NetworkMessage.GetBytes(message);
            SendMsg(RoomSingle.roomIP, NetworkConstent.UDPServerPort, bytes);
        }

    }
}

