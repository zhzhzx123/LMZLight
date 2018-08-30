using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class BossSendMsg : ClientIntervalSendBase, IBossSend
{
    int id = 0;

    void Awake()
    {
        if (!NetworkTools.GetLocalIP().Equals(RoomSingle.roomIP))
        {
            Destroy(gameObject);
            return;
        }
        base.BaseAwake();
    }

    // Update is called once per frame
    void Update()
    {
        base.BaseUpdate();
    }

    public void SwitchState(int s1, int s2)
    {
        EnemySendClientManager._Instance.SendBossStateMsg((BossState)s1, (BossAtkState)s2, id);
    }

    public void SetIDandNum(int id, int num)
    {
        this.id = id;
    }

    public override void SendMsg()
    {
        EnemySendClientManager._Instance.SendBossTransformMsg(transform, id);
    }



    public void DestroyThis()
    {
        timeCount = 0;
        Destroy(this);
    }

    public void Hurt(float hurt)
    {
        EnemySendClientManager._Instance.SendBossHurtMsg(hurt, id);
    }
}
