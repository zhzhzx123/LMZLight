using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;

public class ReviceRoomUI : WindowBase {

    private ReviceRoomSendClient client;
    private Button createButton;
    private Button addButton;
    private InputField ipInput;
    private Toggle roomToggle;
    private Transform room;
    private Transform temp;
    private ToggleGroup tg;
    //private Text roomText;

    public string chooseIP;

    private List<Transform> rooms = new List<Transform>();
    private List<Transform> pools = new List<Transform>();

    public override void Init()
    {
        base.Init();
        client = new ReviceRoomSendClient();
        createButton = transform.Find("Create").GetComponent<Button>();
        addButton = transform.Find("Add").GetComponent<Button>();
        ipInput = transform.Find("IP").GetComponent<InputField>();
        //roomText = transform.Find("Room/Room").GetComponent<Text>();
        roomToggle = transform.Find("Room").GetComponent<Toggle>();
        room = transform.Find("Rooms");
        temp = transform.Find("Temp");
        tg = room.GetComponent<ToggleGroup>();
        createButton.onClick.AddListener(CreateClick);
        addButton.onClick.AddListener(AddClick);


        //roomText.text = "房间数量0";
    }

    public override void Open()
    {
        base.Open();
        //roomText.text = "房间数量0";
        chooseIP = "";
        addButton.interactable = false;
    }

    public override void Destroy()
    {
        base.Destroy();
        client.CloseClient();
    }

    public void UpdateData(Network.RoomData data)
    {
        //roomText.text = "房间数量" + data.GetRoomMessage().Count + "\n";
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].SetParent(temp);
            rooms[i].gameObject.SetActive(false);
            pools.Add(rooms[i]);
        }
        rooms.Clear();
        foreach (var item in data.GetRoomMessage().Keys)
        {
            //roomText.text += "房间名字: " + data.GetRoomMessage()[item].name + "   ip：" + item + "\n";
            string str = "房间名字: " + data.GetRoomMessage()[item].name + "   ip：" + item;
            Transform t = null; ;
            if (pools.Count > 0)
            {
                t = pools[0].transform;
                pools.RemoveAt(0);
            }
            else
            {
                t = Instantiate<GameObject>(roomToggle.gameObject).transform;
                t.transform.localScale = Vector3.one;
                t.GetComponent<Toggle>().group = tg;
                t.GetComponent<Toggle>().onValueChanged.AddListener(ToggleValueChange);
            }
            t.SetParent(room);
            t.gameObject.SetActive(true);
            t.name = item;
            t.Find("Text").GetComponent<Text>().text = str;
            rooms.Add(t);
        }
    }

    public void AddRoomSuccess()
    {
        Debug.Log("AddRoomSuccess");
        UIManager._Instance.OpenWindow(WindowName.WaitStartRoom);
        UIManager._Instance.CloseWindow(WindowName.ReviceRoom);
    }

    void CreateClick()
    {
        //NetworkManager._Instance.CreateRoom("房间11111");
        UIManager._Instance.OpenWindow(WindowName.Room);
        UIManager._Instance.CloseWindow(WindowName.ReviceRoom);
    }

    void AddClick()
    {
        client.SendAddRoom(chooseIP);
        //addButton.interactable = false;
    }


    void ToggleValueChange(bool isOn)
    {
        if (isOn)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].GetComponent<Toggle>().isOn)
                {
                    chooseIP = rooms[i].gameObject.name;
                    break;
                }
            }
        }
        else
        {
            chooseIP = "";
        }
        if (chooseIP.Equals(""))
        {
            addButton.interactable = false;
        }
        else
        {
            addButton.interactable = true;
        }
    }

}
