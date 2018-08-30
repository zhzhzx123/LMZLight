using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Network
{
    public class PlayerHurtMessage 
    {
        public float hurt;//玩家伤害

        public PlayerHurtMessage(float hurt)
        {
            this.hurt = hurt;

        }

        public static byte[] GetBytes(PlayerHurtMessage message)
        {
            byte[] newBytes = BitConverter.GetBytes(message.hurt);
            return newBytes;
        }

        public static PlayerHurtMessage GetMessage(byte[] bytes)
        {
            return new PlayerHurtMessage(BitConverter.ToSingle(bytes, 0));
        }

    }
}

