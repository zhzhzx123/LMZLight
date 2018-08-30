using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Network
{
    public class EnemyInfoMessage 
    {
        public float x;
        public float y;
        public float z;

        public float angleX;
        public float angleY;
        public float angleZ;

        public int enemyType;//敌人的类型  就是敌人的ID

        public int num;//敌人的编号

        public EnemyInfoMessage(float x, float y, float z,
            float angleX, float angleY, float angleZ, int enemyType, int num)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.angleX = angleX;
            this.angleY = angleY;
            this.angleZ = angleZ;
            this.enemyType = enemyType;
            this.num = num;
        }

        public EnemyInfoMessage(Vector3 position,
           Vector3 rotate, int enemyType, int num)
        {
            this.x = position.x;
            this.y = position.y;
            this.z = position.z;
            this.angleX = rotate.x;
            this.angleY = rotate.y;
            this.angleZ = rotate.z;
            this.enemyType = enemyType;
            this.num = num;
        }


        public static byte[] GetBytes(EnemyInfoMessage t)
        {
            byte[] bx = BitConverter.GetBytes(t.x);
            byte[] by = BitConverter.GetBytes(t.y);
            byte[] bz = BitConverter.GetBytes(t.z);
            byte[] bax = BitConverter.GetBytes(t.angleX);
            byte[] bay = BitConverter.GetBytes(t.angleY);
            byte[] baz = BitConverter.GetBytes(t.angleZ);
            byte[] btype = BitConverter.GetBytes(t.enemyType);
            byte[] bnum = BitConverter.GetBytes(t.num);
            byte[] bytes = new byte[bx.Length + by.Length +
                bz.Length + bax.Length + bay.Length + baz.Length + btype.Length + bnum.Length];
            bx.CopyTo(bytes, 0);
            by.CopyTo(bytes, 4);
            bz.CopyTo(bytes, 8);
            bax.CopyTo(bytes, 12);
            bay.CopyTo(bytes, 16);
            baz.CopyTo(bytes, 20);
            btype.CopyTo(bytes, 24);
            bnum.CopyTo(bytes, 28);
            return bytes;
        }

        public static EnemyInfoMessage GetMessage(byte[] bytes)
        {
            float fx = BitConverter.ToSingle(bytes, 0);
            float fy = BitConverter.ToSingle(bytes, 4);
            float fz = BitConverter.ToSingle(bytes, 8);
            float fax = BitConverter.ToSingle(bytes, 12);
            float fay = BitConverter.ToSingle(bytes, 16);
            float faz = BitConverter.ToSingle(bytes, 20);
            int ntype = BitConverter.ToInt32(bytes, 24);
            int nnum = BitConverter.ToInt32(bytes, 28);
            return new EnemyInfoMessage(fx, fy, fz, fax, fay, faz, ntype, nnum);
        }
    }
}
