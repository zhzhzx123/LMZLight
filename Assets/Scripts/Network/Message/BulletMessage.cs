using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Network
{
    public class BulletMessage
    {
        public float x;
        public float y;
        public float z;

        public float angleX;
        public float angleY;
        public float angleZ;

        public int bulletType;//子弹的类型

        public int num;//子弹的编号

        public BulletMessage(float x, float y, float z, 
            float angleX, float angleY, float angleZ, int bulletType, int num)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.angleX = angleX;
            this.angleY = angleY;
            this.angleZ = angleZ;
            this.bulletType = bulletType;
            this.num = num;
        }

        public BulletMessage(Vector3 position,
           Vector3 rotate, int bulletType, int num)
        {
            this.x = position.x;
            this.y = position.y;
            this.z = position.z;
            this.angleX = rotate.x;
            this.angleY = rotate.y;
            this.angleZ = rotate.z;
            this.bulletType = bulletType;
            this.num = num;
        }


        public static byte[] GetBytes(BulletMessage t)
        {
            byte[] bx = BitConverter.GetBytes(t.x);
            byte[] by = BitConverter.GetBytes(t.y);
            byte[] bz = BitConverter.GetBytes(t.z);
            byte[] bax = BitConverter.GetBytes(t.angleX);
            byte[] bay = BitConverter.GetBytes(t.angleY);
            byte[] baz = BitConverter.GetBytes(t.angleZ);
            byte[] btype = BitConverter.GetBytes(t.bulletType);
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

        public static BulletMessage GetMessage(byte[] bytes)
        {
            float fx = BitConverter.ToSingle(bytes, 0);
            float fy = BitConverter.ToSingle(bytes, 4);
            float fz = BitConverter.ToSingle(bytes, 8);
            float fax = BitConverter.ToSingle(bytes, 12);
            float fay = BitConverter.ToSingle(bytes, 16);
            float faz = BitConverter.ToSingle(bytes, 20);
            int ntype = BitConverter.ToInt32(bytes, 24);
            int nnum = BitConverter.ToInt32(bytes, 28);
            return new BulletMessage(fx, fy, fz, fax, fay, faz, ntype, nnum);
        }
    }
}

