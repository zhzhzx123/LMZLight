using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using System;

public class EnemyManager : MsgCallBackBase<NetworkMessage>
{

    public List<GameObject> enemy;
    public List<Enemy> enemies;
    public List<Enemy> death;

    public void Awake()
    {
        base.BaseAwake();
        if (!NetworkTools.GetLocalIP().Equals(RoomSingle.roomIP))
        {
            Destroy(gameObject);
            return;
        }
     
        NetworkManager._Instance.AddCallBack(63, GetNetworkMsgCallBack);
    }

    public void Update()
    {
        base.BaseUpdate();
    }

    public void OnDestroy()
    {
   
        NetworkManager._Instance.RemoveCallBack(63, GetNetworkMsgCallBack);
        base.BaseDestroy();
    }

    protected override void NetworkCallback(NetworkMessage message)
    {
        if (message.type == 63)
        {
            EnemyTargetMessage msg = EnemyTargetMessage.GetMessage(message.message);
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].enemyData.ID == msg.enemyType && enemies[i].enemyData.Listnum == msg.num)
                {
                    Transform target = BattleController.Instance.playerDic[msg.targetIP].transform;
                    enemies[i].SetTarget(target);
                }
            }
        }
    }

    protected override void GetNetworkMsgCallBack(params object[] obj_arr)
    {
        NetworkMessage message = (NetworkMessage)obj_arr[0];
        if (message.type == 63)
        {
            AddMessage(message);
        }
    }
}
