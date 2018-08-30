using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Network
{

    public class RoomData
    {
        private Dictionary<string,RoomMessage> roomDic = new Dictionary<string, RoomMessage>();
        private Dictionary<string, DateTime> roomTime = new Dictionary<string, DateTime>();//最后的响应时间
        public Dictionary<string, RoomMessage> GetRoomMessage()
        {
            return roomDic;
        }

        public void AddMessage(RoomMessage message)
        {
            lock (roomDic)
            {
                if (!roomDic.ContainsKey(message.ip))
                {
                    roomDic.Add(message.ip, message);
                    roomTime.Add(message.ip, DateTime.Now);
                }
                else
                {
                    UpdateMessage(message);
                }
            }
        }

        public void UpdateMessage(RoomMessage message)
        {
            roomDic[message.ip] = message;
            roomTime[message.ip] = DateTime.Now;
        }

        public void RemoveMessage(RoomMessage message)
        {
            lock (roomDic)
            {
                if (roomDic.ContainsKey(message.ip))
                {
                    roomDic.Remove(message.ip);
                }
            }
        }

        public void RemoveMessage(string ip)
        {
            lock (roomDic)
            {
                if (roomDic.ContainsKey(ip))
                {
                    roomDic.Remove(ip);
                }
            }
        }

        public bool CheckRoom()
        {
            List<string> list = new List<string>();
            foreach (var item in roomTime.Keys)
            {
                TimeSpan ts = roomTime[item].Subtract(DateTime.Now).Duration();
                double totalSeconds = ts.TotalSeconds;
                if (totalSeconds >= 3)//3秒无响应，删除该房间
                {
                    list.Add(item);
                }
            }
            
            for (int i = 0; i < list.Count; i++)
            {
                roomTime.Remove(list[i]);
                roomDic.Remove(list[i]);
            }
            // Debug.Log("房间数量: " + roomDic.Count);
            return list.Count > 0 ? true : false;
        }
    }
}

