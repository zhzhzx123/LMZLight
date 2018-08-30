using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class RoomPlayerData
    {
        //ip  ---  玩家信息
        private Dictionary<string, RoomPlayerInfoMessage> playerDic = new Dictionary<string, RoomPlayerInfoMessage>();

        public Dictionary<string, RoomPlayerInfoMessage> GetPlayersInfo()
        {
            return playerDic;
        }

        public void AddPlayerInfo(string ip, RoomPlayerInfoMessage info)
        {
            if (playerDic.Count >= 4)
                return;
            if (!playerDic.ContainsKey(ip))
            {
                lock (playerDic)
                {
                    playerDic.Add(ip, info);
                }
            }
            else
            {
                playerDic[ip] = info;
            }
        }

        public void RemovePlayerInfo(string ip)
        {
            if (playerDic.ContainsKey(ip))
            {
                lock (playerDic)
                {
                    playerDic.Remove(ip);
                }
            }
        }


        public void SetPlayerCanStart(string ip, bool canStart)
        {
            if (playerDic.ContainsKey(ip))
            {
                lock (playerDic)
                {
                    playerDic[ip].canStart = canStart;
                }
            }
        }


    }
}


