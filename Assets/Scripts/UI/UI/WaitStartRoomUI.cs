using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;

public class WaitStartRoomUI : WindowBase {
    private WaitStartRoomSendClient client;
    private Button startButton;
    private Button quitButton;
    private Button bagButton;

    private GameObject player;
    private GameObject players;
    private GameObject temp;
    private List<GameObject> ps = new List<GameObject>();
    private List<GameObject> pools = new List<GameObject>();
    // private Text roomText;

    public override void Init()
    {
        base.Init();
        if (client == null)
        {
            client = new WaitStartRoomSendClient();
        }
        //client = new WaitStartRoomSendClient();
        startButton = transform.Find("Start").GetComponent<Button>();
        quitButton = transform.Find("Quit").GetComponent<Button>();
        bagButton = transform.Find("Bag").GetComponent<Button>();
        //roomText = transform.Find("Room/Room").GetComponent<Text>();
        player = transform.Find("Player").gameObject;
        players = transform.Find("Players").gameObject;
        temp = transform.Find("Temp").gameObject;
        player.SetActive(false);
        startButton.onClick.AddListener(ClickStart);
        quitButton.onClick.AddListener(ClickQuit);
        bagButton.onClick.AddListener(ClickBag);
    }

    public override void Open()
    {
        base.Open();
        client.InitInfo();
        startButton.interactable = true;
        EventCenterManager._Instance.AddListener(EventType.GetPlayerInfo, GetPlayerInfoCallBack);
        EventCenterManager._Instance.AddListener(EventType.SetPlayerInfo, SetPlayerInfoCallBack);
    }

    public override void Close()
    {
        if (gameObject.activeSelf)
        {
            EventCenterManager._Instance.RemoveListener(EventType.GetPlayerInfo, GetPlayerInfoCallBack);
            EventCenterManager._Instance.RemoveListener(EventType.SetPlayerInfo, SetPlayerInfoCallBack);
        }
        base.Close();
    }

    public override void Destroy()
    {
        client.CloseClient();
        base.Destroy();
    }

    private void OnDestroy()
    {
        client.SendQuitRoom();
    }


    public void GetPlayerInfos()
    {
        if (client == null)
        {
            client = new WaitStartRoomSendClient();
        }
        client.SendGetRoomPlayerInfos();
    }


    public void UpdatePanel(RoomPlayerData data)
    {
        string str = "";
        for (int i = 0; i < ps.Count; i++)
        {
            ps[i].transform.SetParent(temp.transform);
            ps[i].SetActive(false);
            pools.Add(ps[i]);
        }
        ps.Clear();
        foreach (var item in data.GetPlayersInfo().Keys)
        {
            if (data.GetPlayersInfo()[item].canStart)
            {
                str = "等待开始游戏";
            }
            else
            {
                str = "玩家准备中";
            }
            GameObject obj = null;
            if (pools.Count > 0)
            {
                obj = pools[0];
                pools.RemoveAt(0);
            }
            else
            {
                obj = Instantiate<GameObject>(player);
                obj.transform.localScale = Vector3.one;
            }
            obj.transform.SetParent(players.transform);
            obj.transform.localScale = Vector3.one;
            obj.SetActive(true);
            obj.transform.Find("NameText").GetComponent<Text>().text = "IP: " + item;
            obj.transform.Find("StartText").GetComponent<Text>().text = str;
            ps.Add(obj);
            //playersText.text += "玩家" + (i + 1) + " --  IP: " + item + " --- " + str + "\n";
        }
    }

    void ClickStart()
    {
        client.SendWaitStart();
        startButton.interactable = false;
        bagButton.interactable = false;
    }

    void ClickQuit()
    {
        client.SendQuitRoom();
        UIManager._Instance.CloseWindow(WindowName.WaitStartRoom);
        UIManager._Instance.OpenWindow(WindowName.ReviceRoom);
    }

    void ClickBag()
    {
        UIManager._Instance.OpenWindow(WindowName.BagPanel);
    }


    void GetPlayerInfoCallBack(params object[] obj_arr)
    {
        EventCenterManager._Instance.SendMessage(EventType.SendPlayerInfo, client.playerInfo.gun1ID, client.playerInfo.gun2ID);
    }

    void SetPlayerInfoCallBack(params object[] obj_arr)
    {
        client.playerInfo.gun1ID = (int)obj_arr[0];
        client. playerInfo.gun2ID = (int)obj_arr[1];
    }

}
