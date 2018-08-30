using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using System.Linq;

public class Test1 : MonoBehaviour {

    static Socket socket;

	// Use this for initialization
	void Start () {
        short a = 1;
        Debug.Log( BitConverter.GetBytes(true).Length);
        Debug.Log(Network.NetworkTools.GetLocalIP());
    }


    //创建Socket
    void StartServer()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(new IPEndPoint(IPAddress.Parse(Network.NetworkTools.GetLocalIP()), 6001));//绑定端口号和IP
        Console.WriteLine("服务端已经开启");
    }


    /// <summary>
    /// 向特定ip的主机的端口发送数据报
    /// </summary>
    static void sendMsg(string msg)
    {
        //定义一个发送的IP和端口
        EndPoint point = new IPEndPoint(IPAddress.Parse("169.254.202.67"), 6000);
        socket.SendTo(Encoding.UTF8.GetBytes(msg), point);
    }
    /// <summary>
    /// 接收发送给本机ip对应端口号的数据报
    /// </summary>
    static void ReciveMsg()
    {
        while (true)
        {
            EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
            byte[] buffer = new byte[1024];
            int length = socket.ReceiveFrom(buffer, ref point);//接收数据报
            string message = Encoding.UTF8.GetString(buffer, 0, length);
            Console.WriteLine(point.ToString() + message);

        }
    }



}
