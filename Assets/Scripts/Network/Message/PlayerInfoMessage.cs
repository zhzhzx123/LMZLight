using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

namespace Network
{
	public class PlayerInfoMessage 
	{

		public int ID;//玩家id

		public int gun1ID;//主武器id

		public int gun2ID;//副武器id

		public string playerName;//玩家名字

        public string playerIP;//玩家IP

		public PlayerInfoMessage(int ID, int gun1ID, int gun2ID, string playerName, string playerIP)
		{
			this.ID = ID;
			this.gun1ID = gun1ID;
			this.gun2ID = gun2ID;
			this.playerName = playerName;
            this.playerIP = playerIP;
		}

		public static byte[] GetBytes(PlayerInfoMessage message)
		{
			byte[] name = Encoding.UTF8.GetBytes (message.playerName + "*" + message.playerIP);
			byte[] newBytes = new byte[12 + name.Length];
			BitConverter.GetBytes (message.ID).CopyTo (newBytes, 0);
			BitConverter.GetBytes (message.gun1ID).CopyTo (newBytes, 4);
			BitConverter.GetBytes (message.gun2ID).CopyTo (newBytes, 8);
			name.CopyTo (newBytes, 12);
			return newBytes;
		}

		public static PlayerInfoMessage GetMessage(byte[] bytes)
		{
			string[] str = Encoding.UTF8.GetString (bytes, 12, bytes.Length - 12).Split('*');
			return new PlayerInfoMessage (BitConverter.ToInt32(bytes, 0),
				BitConverter.ToInt32(bytes, 4), BitConverter.ToInt32(bytes, 8), str[0], str[1]);
		}

	}
}

