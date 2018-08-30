using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using System;

public class BossReviceCallBack : MsgCallBackBase<NetworkMessage>
{

    NetBoss net;
    public void Awake()
    {
        if (NetworkTools.GetLocalIP().Equals(RoomSingle.roomIP))
        {
            Destroy(gameObject);
            return;
        }
        base.BaseAwake();
        if (NetworkTools.GetLocalIP().Equals(RoomSingle.roomIP))
        {
            Destroy(this);
            return;
        }
        net = GetComponent<NetBoss>();
        NetworkManager._Instance.AddCallBack(70, GetNetworkMsgCallBack);
        NetworkManager._Instance.AddCallBack(71, GetNetworkMsgCallBack);
        NetworkManager._Instance.AddCallBack(72, GetNetworkMsgCallBack);
    }

    public void Update()
    {
        base.BaseUpdate();
    }

    public void OnDestroy()
    {
        NetworkManager._Instance.RemoveCallBack(70, GetNetworkMsgCallBack);
        NetworkManager._Instance.RemoveCallBack(71, GetNetworkMsgCallBack);
        NetworkManager._Instance.RemoveCallBack(72, GetNetworkMsgCallBack);
        base.BaseDestroy();
    }

    protected override void NetworkCallback(NetworkMessage message)
    {
        if (message.type == 71)
        {
            BossStateMessage msg = BossStateMessage.GetMessage(message.message);
            net.SwitchBossState(msg.moveState);
            net.SwitchAtkState(msg.atkState);
        }
        else if (message.type == 72)
        {
            EnemyHurtMessge msg = EnemyHurtMessge.GetMessage(message.message);
            net.ReduceHP(msg.hurt);
        }
    }

    protected override void GetNetworkMsgCallBack(params object[] obj_arr)
    {
        NetworkMessage message = (NetworkMessage)obj_arr[0];
        if (message.type == 70)
        {
            EnemyInfoMessage info = EnemyInfoMessage.GetMessage(message.message);
            net.SetTarget(info);
        }
        else
        {
            if (message.type == 72)
            {
                if (!message.ip.Equals(NetworkTools.GetLocalIP()))
                {
                    AddMessage(message);
                }
            }
            else
            {
                AddMessage(message);
            }
           
        }
    }
}
