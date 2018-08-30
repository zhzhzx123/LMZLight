using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Network
{
    public static class NetworkConstent
    {
        public static int UDPServerPort = 62535;//服务器端口号

        public static int UDPClientPort = 63535;//客户端端口号

        public static int UDPBattleClientPort = 63635;//战斗客户端端口号

        public const uint IOC_IN = 0x80000000;
        public static int IOC_VENDOR = 0x18000000;
        public static int SIO_UDP_CONNRESET = (int)(IOC_IN | IOC_VENDOR | 12);

        // public static int bufferSize = 65530;
    }
}
