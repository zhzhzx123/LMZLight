using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviceTest : MonoBehaviour {

    public Text text;

    public static ReviceTest Instance;

    public Queue<Network.RoomData> queue = new Queue<Network.RoomData>();

	// Use this for initialization
	void Awake () {
        Instance = this;
    }

    private void Update()
    {
        if(queue.Count > 0)
        {
            Network.RoomData r = queue.Dequeue();
            UpdateData(r);
        }
    }

    public void UpdateData(Network.RoomData data)
    {
        text.text = "房间数量" + data.GetRoomMessage().Count + "\n";
        foreach (var item in data.GetRoomMessage().Keys)
        {
            text.text += data.GetRoomMessage()[item].name + "   ip：" + item + "\n";
        }
    }

    private void OnDestroy()
    {
        Network.NetworkManager._Instance.StopReviceRoom();
    }

    public void Recive()
    {
        Network.NetworkManager._Instance.ReviceRoom();
    }


}
