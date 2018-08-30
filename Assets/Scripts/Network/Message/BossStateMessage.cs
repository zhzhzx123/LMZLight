using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Network
{
    public class BossStateMessage
    {

        public BossState moveState;

        public BossAtkState atkState;

        public BossStateMessage(BossState moveState, BossAtkState atkstate)
        {
            this.moveState = moveState;

            this.atkState = atkstate;
        }

        public static byte[] GetBytes(BossStateMessage p)
        {
            byte[] newBytes = new byte[8];
            BitConverter.GetBytes((int)p.moveState).CopyTo(newBytes, 0);
            BitConverter.GetBytes((int)p.atkState).CopyTo(newBytes, 4);
            return newBytes;
        }

        public static BossStateMessage GetMessage(byte[] bytes)
        {
            int state = BitConverter.ToInt32(bytes, 0);
            int atkState = BitConverter.ToInt32(bytes, 4);
            return new BossStateMessage((BossState)state, (BossAtkState)atkState);
        }
    }

}
