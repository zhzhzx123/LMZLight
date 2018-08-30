using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class BattletServerStart : MonoBehaviour {

    public string[] ips;

    public bool isTest = false;

	// Use this for initialization
	void Awake () {
        if (isTest)
        {
            NetworkManager._Instance.StartBattleServer(ips);
            return;
        }
        //判断自己是不是房主
        if (RoomSingle.roomIP.Equals(NetworkTools.GetLocalIP()))
        {
            Debug.Log("启动战斗服务器");
            ips = new string[RoomSingle.GetInfos().Count];
            int i = 0;
            foreach (var item in RoomSingle.GetInfos().Keys)
            {
                ips[i] = item;
                i++;
            }
            NetworkManager._Instance.StartBattleServer(ips);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnDestroy()
    {
        if (isTest)
        {
            NetworkManager._Instance.CloseBattleServer();
            return;
        }
        if (RoomSingle.roomIP.Equals(NetworkTools.GetLocalIP()))
        {
            NetworkManager._Instance.CloseBattleServer();
        }
    }
}
