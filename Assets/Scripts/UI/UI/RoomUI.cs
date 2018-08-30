using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;

public class RoomUI : WindowBase
{
    RoomUISendClient client = new RoomUISendClient();
    private Button startButton;
    private Button returnButton;
    private Button bagButton;

    private GameObject player;
    private GameObject players;
    private GameObject temp;

    private List<GameObject> ps = new List<GameObject>();
    private List<GameObject> pools = new List<GameObject>();

    //private Text playersText;

    public override void Init()
    {
        base.Init();
       
       
        startButton = transform.Find("Start").GetComponent<Button>();
        returnButton = transform.Find("Return").GetComponent<Button>();
        bagButton = transform.Find("Bag").GetComponent<Button>();
        //playersText = transform.Find("Players/Player").GetComponent<Text>();
        player = transform.Find("Player").gameObject;
        players = transform.Find("Players").gameObject;
        temp = transform.Find("Temp").gameObject;
        player.SetActive(false);
        startButton.onClick.AddListener(ClickStart);
        returnButton.onClick.AddListener(ClickReturn);
        bagButton.onClick.AddListener(ClickBag);
    }

    public override void Open()
    {
        base.Open();
        //playersText.text = "";
    }

    public override void Destroy()
    {
        base.Destroy();
        client.CloseClient();
    }



    public void UpdatePanel(RoomPlayerData data)
    {
        //playersText.text = "";
        string str = "";
        for (int i = 0; i < ps.Count; i++)
        {
            ps[i].transform.SetParent(temp.transform);
            ps[i].SetActive(false);
            pools.Add(ps[i]);
        }
        ps.Clear();
        bool isStart = true;
        foreach (var item in data.GetPlayersInfo().Keys)
        {
            if (data.GetPlayersInfo()[item].canStart)
            {
                str = "等待开始游戏";
            }
            else
            {
                str = "玩家准备中";
                isStart = false;
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
                
            }
            obj.transform.SetParent(players.transform);
            obj.transform.localScale = Vector3.one;
            obj.SetActive(true);
            obj.transform.Find("NameText").GetComponent<Text>().text = "IP: " + item;
            obj.transform.Find("StartText").GetComponent<Text>().text = str;
            ps.Add(obj);
            //playersText.text += "玩家" + (i + 1) + " --  IP: " + item + " --- " + str + "\n";
        }
        startButton.interactable = isStart;

    }

    void ClickStart()
    {
        Network.NetworkManager._Instance.StartGame();
        Debug.Log("开始游戏");
        UIManager._Instance.CloseWindow(WindowName.Room);
        LoadSceneManager._Instance.LoadScene(SceneName.Mission1, StartGameCallBack);
    }

    void ClickReturn()
    {
        UIManager._Instance.CloseWindow(WindowName.Room);
        UIManager._Instance.OpenWindow(WindowName.ReviceRoom);
    }

    /// <summary>
    /// 进入游戏场景的回调
    /// </summary>
    void StartGameCallBack(params object[] obj_Arr)
    {
        
    }

    void ClickBag()
    {
        UIManager._Instance.OpenWindow(WindowName.BagPanel);
    }

}
