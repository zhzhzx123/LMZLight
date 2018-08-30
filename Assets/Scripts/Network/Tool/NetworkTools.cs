using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Runtime.Serialization;

namespace Network
{
    public class NetworkTools
    {
        private static string LocalIP = "";

        private static string NetworkSegment = "";

        private static int bulletNum;

        public static int GetCurrentNum()
        {
            if (bulletNum >= 665535)
            {
                bulletNum = 0;
            }
            return bulletNum++;
        }

        public static void SetCurrentNum()
        {
            bulletNum = 0;
        }

        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns>本机IP地址</returns>
        public static string GetLocalIP()
        {
            if (LocalIP == null || LocalIP == "")
            {
                try
                {
                    string HostName = Dns.GetHostName(); //得到主机名
                    IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                    for (int i = 0; i < IpEntry.AddressList.Length; i++)
                    {
                        //从IP地址列表中筛选出IPv4类型的IP地址
                        //AddressFamily.InterNetwork表示此IP为IPv4,
                        //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                        if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                        {
                            LocalIP = IpEntry.AddressList[i].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return LocalIP;
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="ip">尾号</param>
        /// <returns></returns>
        public static string GetClientIP(int ip)
        {
            if(NetworkSegment == null || NetworkSegment == "")
            {
                NetworkSegment = "";
                string[] str_arr = GetLocalIP().Split('.');
                for (int i = 0; i < 3; i++)
                {
                    NetworkSegment += str_arr[i] + ".";
                }
            }
            return NetworkSegment + ip;
        }
        

        public static void PrintMessage(string str)
        {
#if UNITY_EDITOR
            Debug.Log(str);
#endif
        }
    }
}
