using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Network
{
    public class EnemyHurtMessge
    {
        public float hurt;//伤害

        public int enemyType;//敌人的类型

        public int num;//敌人的编号

        public EnemyHurtMessge(float hurt, int enemyType, int num)
        {
            this.hurt = hurt;
            this.enemyType = enemyType;
            this.num = num;
        }

        public static byte[] GetBytes(EnemyHurtMessge t)
        {
            byte[] bx = BitConverter.GetBytes(t.hurt);
            byte[] btype = BitConverter.GetBytes(t.enemyType);
            byte[] bnum = BitConverter.GetBytes(t.num);
            byte[] bytes = new byte[bx.Length + btype.Length + bnum.Length];
            bx.CopyTo(bytes, 0);
            btype.CopyTo(bytes, 4);
            bnum.CopyTo(bytes, 8);
            return bytes;
        }

        public static EnemyHurtMessge GetMessage(byte[] bytes)
        {
            float fHurt = BitConverter.ToSingle(bytes, 0);
            int ntype = BitConverter.ToInt32(bytes, 4);
            int nnum = BitConverter.ToInt32(bytes, 8);
            return new EnemyHurtMessge(fHurt, ntype, nnum);
        }

    }
}

