using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using System;

public class BossReviceHurtCallBack : MsgCallBackBase<NetworkMessage>
{
    Boss boss;
    public void Awake()
    {
        base.BaseAwake();
        if (!NetworkTools.GetLocalIP().Equals(RoomSingle.roomIP))
        {
            Destroy(this);
            return;
        }
        boss = GetComponent<Boss>();
        NetworkManager._Instance.AddCallBack(71, GetNetworkMsgCallBack);
        NetworkManager._Instance.AddCallBack(72, GetNetworkMsgCallBack);
    }

    public void Update()
    {
        base.BaseUpdate();
    }

    public void OnDestroy()
    {
        NetworkManager._Instance.RemoveCallBack(71, GetNetworkMsgCallBack);
        NetworkManager._Instance.RemoveCallBack(72, GetNetworkMsgCallBack);
        base.BaseDestroy();
    }

    protected override void NetworkCallback(NetworkMessage message)
    {
        if (message.type == 72)
        {
            EnemyHurtMessge msg = EnemyHurtMessge.GetMessage(message.message);
            boss.ReduceHP(msg.hurt);
        }
        else if (message.type == 71)
        {
            BossStateMessage m = BossStateMessage.GetMessage(message.message);
            boss.SwitchBossState(m.moveState);
        }
    }

    protected override void GetNetworkMsgCallBack(params object[] obj_arr)
    {
        NetworkMessage message = (NetworkMessage)obj_arr[0];
        if (message.type == 72 && !message.ip.Equals(NetworkTools.GetLocalIP()))
        {
            AddMessage(message);
        }
        else if (message.type == 71 && !message.ip.Equals(NetworkTools.GetLocalIP()))
        {
            AddMessage(message);
        }
    }
}
