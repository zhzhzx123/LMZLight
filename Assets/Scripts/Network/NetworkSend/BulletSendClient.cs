using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class BulletSendClient : ClientSendBase
    {
        #region 单例

        private static BulletSendClient instance;

        public static BulletSendClient _Instance
        {
            get {
                if (instance == null)
                {
                    instance = new BulletSendClient();
                }
                return instance;
            }
        }

        private BulletSendClient() { }

        #endregion
        /// <summary>
        /// 同步位置
        /// </summary>
        public void SendTransform(Vector3 p, Vector3 r, int type, int num)
        {
            byte[] b = BulletMessage.GetBytes(new BulletMessage(p, r, type, num));
            byte[] m = NetworkMessage.GetBytes(new NetworkMessage(50, NetworkTools.GetLocalIP(), b));
            SendMsg(RoomSingle.roomIP, NetworkConstent.UDPServerPort, m);
        }

        /// <summary>
        /// 子弹爆炸
        /// </summary>
        /// <param name="p"></param>
        /// <param name="r"></param>
        /// <param name="type"></param>
        /// <param name="num"></param>
        public void SendExplosion(Vector3 p, Vector3 r, int type, int num)
        {
            byte[] b = BulletMessage.GetBytes(new BulletMessage(p, r, type, num));
            byte[] m = NetworkMessage.GetBytes(new NetworkMessage(51, NetworkTools.GetLocalIP(), b));
            SendMsg(RoomSingle.roomIP, NetworkConstent.UDPServerPort, m);
        }


    }
}

