using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

namespace Network
{
    public class RoomPlayerInfoMessage
    {
        public string playerName;

        public bool canStart;

        public string playerIP;

        public RoomPlayerInfoMessage(string playerName, bool canStart, string playerIP)
        {
            this.playerName = playerName;
            this.canStart = canStart;
            this.playerIP = playerIP;
        }

        public static byte[] GetBytes(RoomPlayerInfoMessage message)
        {
            byte[] name = Encoding.UTF8.GetBytes(message.playerName + "*" + message.playerIP);
            byte[] newBytes = new byte[name.Length + 2];
            BitConverter.GetBytes(message.canStart).CopyTo(newBytes, 0);
            name.CopyTo(newBytes, 2);
            return newBytes;
        }

        public static RoomPlayerInfoMessage GetMessage(byte[] bytes)
        {
            bool canStart = BitConverter.ToBoolean(bytes, 0);
            string[] str = Encoding.UTF8.GetString(bytes, 2, bytes.Length - 2).Split('*');
            return new RoomPlayerInfoMessage(str[0], canStart, str[1]);
        }
    }
}

