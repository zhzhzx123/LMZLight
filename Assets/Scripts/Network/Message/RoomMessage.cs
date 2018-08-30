using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Network
{
    [System.Serializable]
    public class RoomMessage
    {
        public string name;//房间名字
        public string ip;//房间的IP

        public RoomMessage(string name, string ip)
        {
            this.name = name;
            this.ip = ip;
        }

        public static byte[] GetBytes(RoomMessage r)
        {
            return Encoding.UTF8.GetBytes(r.name + "*" + r.ip);
        }

        public static RoomMessage GetMessage(byte[] bytes)
        {
            string str = Encoding.UTF8.GetString(bytes);
            string[] arr = str.Split('*');
            return new RoomMessage(arr[0], arr[1]);
        }
    }
}

