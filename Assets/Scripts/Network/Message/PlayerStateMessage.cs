using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

namespace Network { 
    public class PlayerStateMessage
    {
        public PlayerState state;

        public AtkState atkState;

        public PlayerStateMessage(PlayerState state, AtkState atkstate)
        {
            this.state = state;

            this.atkState = atkstate;
        }

        public static byte[] GetBytes(PlayerStateMessage p)
        {
            byte[] newBytes = new byte[8];
            BitConverter.GetBytes((int)p.state).CopyTo(newBytes, 0);
            BitConverter.GetBytes((int)p.atkState).CopyTo(newBytes, 4);
            return newBytes;
        }

        public static PlayerStateMessage GetMessage(byte[] bytes)
        {
            int state = BitConverter.ToInt32(bytes, 0);
            int atkState = BitConverter.ToInt32(bytes, 4);
            return new PlayerStateMessage((PlayerState)state, (AtkState)atkState);    
        }

    }
}
