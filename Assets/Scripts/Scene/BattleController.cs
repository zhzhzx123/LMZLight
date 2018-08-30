using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using UnityEngine.EventSystems;

public class BattleController : MonoBehaviour {

    public Dictionary<string, PlayerBase> playerDic = new Dictionary<string, PlayerBase>();

    public static BattleController Instance;

    void Awake () {
        Instance = this;
        UIManager._Instance.OpenWindow(WindowName.Battle);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerDic.Add(NetworkTools.GetLocalIP(), player.GetComponent<PlayerBase>());
        foreach (string item in RoomSingle.GetInfos().Keys)
        {
            if (!item.Equals(NetworkTools.GetLocalIP()))
            {
                PlayerInfoMessage m = RoomSingle.GetInfos()[item];
                InstancePlayerAI(item, m.ID, m.gun1ID, m.gun2ID);
            }
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Cursor.visible)
        {
            UIManager._Instance.OpenWindow(WindowName.QuitBattle);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UIManager._Instance.UnloadWindow(WindowName.Battle);
        UIManager._Instance.UnloadWindow(WindowName.QuitBattle);
    }

    void InstancePlayerAI(string ip, int playerID, int gun1, int gun2)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefab/Player/PlayerAI");
        GameObject ai = Instantiate<GameObject>(prefab);
        BattleAIClient ib = ai.GetComponent<BattleAIClient>();
        ib.ip = ip;
        playerDic.Add(ip, ai.GetComponent<PlayerBase>());
    }


	
    
}
