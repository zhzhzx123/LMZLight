using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

namespace Network
{
    public class EnemyTargetMessage
    {
        public string targetIP;

        public int enemyType;//敌人的类型

        public int num;//敌人的编号

        public EnemyTargetMessage(string targetIP, int enemyType, int num)
        {
            this.targetIP = targetIP;
            this.enemyType = enemyType;
            this.num = num;
        }

        public static byte[] GetBytes(EnemyTargetMessage t)
        {
            byte[] btype = BitConverter.GetBytes(t.enemyType);
            byte[] bnum = BitConverter.GetBytes(t.num);
            byte[] ipBytes = Encoding.UTF8.GetBytes(t.targetIP);
            byte[] bytes = new byte[ipBytes.Length + btype.Length + bnum.Length];
            btype.CopyTo(bytes, 0);
            bnum.CopyTo(bytes, 4);
            ipBytes.CopyTo(bytes, 8);
            return bytes;
        }

        public static EnemyTargetMessage GetMessage(byte[] bytes)
        {
            string target = Encoding.UTF8.GetString(bytes, 8, bytes.Length - 8);
            int ntype = BitConverter.ToInt32(bytes, 0);
            int nnum = BitConverter.ToInt32(bytes, 4);
            return new EnemyTargetMessage(target, ntype, nnum);
        }
    }
}

