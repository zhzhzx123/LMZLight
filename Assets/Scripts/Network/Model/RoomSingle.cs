using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class RoomSingle
    {
        public static string roomIP;

        private static Dictionary<string, PlayerInfoMessage> infos
            = new Dictionary<string, PlayerInfoMessage> ();

        public static Dictionary<string, PlayerInfoMessage> GetInfos()
        {
            return infos;
        }

        public static void AddPlayer(PlayerInfoMessage info)
        {
            
                if (infos.ContainsKey(info.playerIP))
                {
                    lock (infos)
                    {
                        infos[info.playerIP] = info;
                    }
                }
                else if (infos.Count < 4)
                {
                    lock (infos)
                    {
                        infos[info.playerIP] = info;
                    }
                }
        }

        public static void RemovePlayer(string ip)
        {
            if (infos.ContainsKey(ip))
            {
                lock (infos)
                {
                    infos.Remove(ip);
                }
            }
        }
    }
}

