using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Network
{
    public class EnemyStateMessage 
    {
        public int state;

        public int enemyType;//敌人的类型

        public int num;//敌人的编号

        public EnemyStateMessage(int state, int enemyType, int num)
        {
            this.state = state;
            this.enemyType = enemyType;
            this.num = num;
        }

        public static byte[] GetBytes(EnemyStateMessage t)
        {
            byte[] bx = BitConverter.GetBytes(t.state);
            byte[] btype = BitConverter.GetBytes(t.enemyType);
            byte[] bnum = BitConverter.GetBytes(t.num);
            byte[] bytes = new byte[bx.Length + btype.Length + bnum.Length];
            bx.CopyTo(bytes, 0);
            btype.CopyTo(bytes, 4);
            bnum.CopyTo(bytes, 8);
            return bytes;
        }

        public static EnemyStateMessage GetMessage(byte[] bytes)
        {
            int nState = BitConverter.ToInt32(bytes, 0);
            int ntype = BitConverter.ToInt32(bytes, 4);
            int nnum = BitConverter.ToInt32(bytes, 8);
            return new EnemyStateMessage(nState, ntype, nnum);
        }
    }
}
