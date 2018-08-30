using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

namespace Network
{
    [System.Serializable]
    public class TransformMessage
    {
        public float x;
        public float y;
        public float z;
        public float angle;
        public float gunAngle;

        public TransformMessage(Vector3 position, Vector3 rotation, float gunAngle)
        {
            x = position.x;
            y = position.y;
            z = position.z;
            angle = rotation.y;
            this.gunAngle = gunAngle;
        }

        public TransformMessage(float x, float y, float z, float angle, float gunAngle)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.angle = angle;
            this.gunAngle = gunAngle;
        }

        public static byte[] GetBytes(TransformMessage t)
        {
            byte[] bx = BitConverter.GetBytes(t.x);
            byte[] by = BitConverter.GetBytes(t.y);
            byte[] bz = BitConverter.GetBytes(t.z);
            byte[] ba = BitConverter.GetBytes(t.angle);
            byte[] bg = BitConverter.GetBytes(t.gunAngle);
            byte[] bytes = new byte[bx.Length + by.Length +
                bz.Length + ba.Length + bg.Length];
            bx.CopyTo(bytes, 0);
            by.CopyTo(bytes, 4);
            bz.CopyTo(bytes, 8);
            ba.CopyTo(bytes, 12);
            bg.CopyTo(bytes, 16);
            return bytes;
        }

        public static TransformMessage GetMessage(byte[] bytes)
        {
            float fx = BitConverter.ToSingle(bytes, 0);
            float fy = BitConverter.ToSingle(bytes, 4);
            float fz = BitConverter.ToSingle(bytes, 8);
            float fa = BitConverter.ToSingle(bytes, 12);
            float fg = BitConverter.ToSingle(bytes, 16);
            return new TransformMessage( fx, fy, fz, fa, fg);
        }
    }
}





