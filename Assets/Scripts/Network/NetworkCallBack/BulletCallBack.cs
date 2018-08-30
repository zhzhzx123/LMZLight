using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Network
{
    public class BulletCallBack : MsgCallBackBase<NetworkMessage>
    {
        private Dictionary<string, NetworkBullet> bullets = new Dictionary<string, NetworkBullet>();

        protected override void GetNetworkMsgCallBack(params object[] obj_arr)
        {
            NetworkMessage message = (NetworkMessage)obj_arr[0];

            if (message.type == 50 && !message.ip.Equals(NetworkTools.GetLocalIP()))//位置同步 
            {
                BulletMessage bullet = BulletMessage.GetMessage(message.message);
                if ((bullet.bulletType == (int)BulletType.boosMissile || bullet.bulletType == (int)BulletType.bossBullet) && NetworkTools.GetLocalIP().Equals(RoomSingle.roomIP))
                    return;
                if (bullets.ContainsKey(message.ip + bullet.num) && bullet.bulletType != (int)BulletType.bossBullet)//有这颗子弹
                {
                    if (bullet.bulletType == (int)BulletType.bossBullet)
                        return;
                    bullets[message.ip + bullet.num].SetTarget(bullet);
                }
                else//没有这个子弹, 根据子弹的类型生成子弹
                {
                    AddMessage(message);
                    
                }
            }
            else if (message.type == 51 && !message.ip.Equals(NetworkTools.GetLocalIP()))//位置同步 
            {
                BulletMessage bullet = BulletMessage.GetMessage(message.message);
                if (bullet.bulletType == (int)BulletType.bossBullet)
                    return;
                if (bullets.ContainsKey(message.ip + bullet.num))//有这颗子弹
                {
                    //爆炸
                    AddMessage(message);
                }
            }
        }

        protected override void NetworkCallback(NetworkMessage message)
        {
            if (message.type == 50 && !message.ip.Equals(NetworkTools.GetLocalIP()))//位置同步 
            {
                BulletMessage bullet = BulletMessage.GetMessage(message.message);
                if ((bullet.bulletType == (int)BulletType.boosMissile || bullet.bulletType == (int)BulletType.bossBullet) && NetworkTools.GetLocalIP().Equals(RoomSingle.roomIP))
                    return;
                if (!bullets.ContainsKey(message.ip + bullet.num))//有这颗子弹
                {
                    GameObject prefab = Resources.Load<GameObject>("Prefab/Bullet/Net_" + ((BulletType)bullet.bulletType).ToString());
                    GameObject obj = Instantiate<GameObject>(prefab,
                        new Vector3(bullet.x, bullet.y, bullet.z),
                        Quaternion.Euler(bullet.angleX, bullet.angleY, bullet.angleZ));
                    obj.GetComponent<NetworkBullet>().SetTarget(bullet);
                    if(bullet.bulletType != (int)BulletType.bossBullet)
                    {
                        bullets.Add(message.ip + bullet.num, obj.GetComponent<NetworkBullet>());
                    }
                }
            }
            else if (message.type == 51 && !message.ip.Equals(NetworkTools.GetLocalIP()))//包扎同步 
            {
                BulletMessage bullet = BulletMessage.GetMessage(message.message);
                if (bullet.bulletType == (int)BulletType.bossBullet)
                    return;
                if (bullets.ContainsKey(message.ip + bullet.num))//有这颗子弹
                {
                    bullets[message.ip + bullet.num].Explosion();
                    bullets.Remove(message.ip + bullet.num);
                }
            }
        }


        public void Awake()
        {
            base.BaseAwake();
            NetworkTools.SetCurrentNum();
            NetworkManager._Instance.AddCallBack(50, GetNetworkMsgCallBack);
            NetworkManager._Instance.AddCallBack(51, GetNetworkMsgCallBack);
        }

        public void Update()
        {
            base.BaseUpdate();
        }

        public void OnDestroy()
        {
            NetworkManager._Instance.RemoveCallBack(50, GetNetworkMsgCallBack);
            NetworkManager._Instance.RemoveCallBack(51, GetNetworkMsgCallBack);
            base.BaseDestroy();
        }
    }
}

