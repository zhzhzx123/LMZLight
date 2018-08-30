using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Linq;
using System.Text;

namespace Network
{
    
    [System.Serializable]
    public class NetworkMessage : MessageBase
    {
        public int type;//消息类型
        /*
         *  1 -> 发送房间信息
         *  2 -> 获取房间信息
         *  3 -> 关闭房间
         *  4 -> 战斗场景的位置和旋转信息
         *  5 -> 玩家状态同步
         *  6 -> 玩家同步伤害
         *  7 -> 加入房间
         *  8 -> 加入房间成功
         *  9 -> 退出房间
         *  10 -> 准备开始游戏
         *  11 -> 同步房间所有玩家信息 
         *  12 -> 同步玩家的装备信息等等
         *  13 -> 开始游戏
         *  14 -> 向服务器发送获取房间内玩家信息
         *  15 -> 服务器向客户端发送所有的玩家信息
         *  16 -> 服务器告诉客户端有玩家退出
         *  17 -> 服务器告诉客户端可以开始的玩家
         *  18 -> 同步玩家开枪
         *  
         *  
         *  50 -> 炮弹位置同步
         *  51 -> 炮弹爆炸
         *  
         *  60 -> 小怪的位置同步
         *  61 -> 小怪的状态同步
         *  62 -> 小怪的伤害同步
         *  63 -> 小怪的目标同步
         *  
         *  70 -> boss的位置同步
         *  71 -> boss的状态同步
         *  72 -> boss的血量同步
         *  73 -> boss的炮弹同步
         *  
         *  
         * 
         *  //间接的服务器消息，从100开始
         *  101 -> 服务器返回房间信息，导致数据改变，同步UI
         *  102 -> 更新房间的玩家
         * 
         * */
        public string ip
        {
            get {
                string _ip = "";
                for (int i = 0; i < ip_arr.Length; i++)
                {
                    _ip += ip_arr[i].ToString();
                    if (i != ip_arr.Length - 1)
                    {
                        _ip += ".";
                    }
                }
                return _ip;
            }
        }

        public short[] ip_arr = new short[4];

        public byte[] message;//消息数据

        public NetworkMessage()
        { }

        public NetworkMessage(int type, string ip, byte[] message)
        {
            this.type = type;
            this.message = message;
            string[] str = ip.Split('.');
            for (int i = 0; i < ip_arr.Length; i++)
            {
                ip_arr[i] = short.Parse(str[i]);
            }
        }

        public NetworkMessage(int type, short[] ip, byte[] message)
        {
            this.type = type;
            this.message = message;
            this.ip_arr = ip;
        }

        public static byte[] GetBytes(NetworkMessage obj)
        {
            byte[] t = BitConverter.GetBytes(obj.type);
            long length = t.Length + (obj.ip_arr.Length * 2);
            if (obj.message != null)
            {
                length += obj.message.Length;
            }
            byte[] newBytes = new byte[length];
            t.CopyTo(newBytes, 0);
            for (int i = 0; i < obj.ip_arr.Length; i++)
            {
                BitConverter.GetBytes(obj.ip_arr[i]).CopyTo(newBytes, 4 + i * 2);
            }
            if (obj.message != null)
            {
                obj.message.CopyTo(newBytes, t.Length + (obj.ip_arr.Length * 2));
            }
            return newBytes;
        }

        public static NetworkMessage GetMessage(byte[] bytes)
        {
            try
            {
                int t = BitConverter.ToInt32(bytes, 0);
                short[] ip = new short[4];
                for (int i = 0; i < 4; i++)
                {
                    ip[i] = BitConverter.ToInt16(bytes, 4 + i * 2);
                }
                byte[] b = bytes.Skip(12).Take(bytes.Length - 12).ToArray();
                NetworkMessage me = new NetworkMessage(t, ip, b);
                return me;
            }
            catch (Exception e)
            {

                Debug.LogError(e.ToString());
            }
            return null;
        }
    }
}

