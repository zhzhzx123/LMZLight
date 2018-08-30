using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class EnemySendMsg : ClientIntervalSendBase, IEnemySend
{

    public int id;

    public int listNum;

    // Use this for initialization
    void Awake () {
        base.BaseAwake();
	}
	
	// Update is called once per frame
	void Update () {
        base.BaseUpdate();
	}

    public void SwitchState(int state)
    {
        EnemySendClientManager._Instance.SendEnemyStateMsg(state, id, listNum);
    }

    public void SetIDandNum(int id, int num)
    {
        this.id = id;
        this.listNum = num;
    }

    public override void SendMsg()
    {
        EnemySendClientManager._Instance.SendEnemyTransformMsg(transform, id, listNum);
    }

    

    public void DestroyThis()
    {
        timeCount = 0;
        Destroy(this);
    }

    public void Hurt(float hurt)
    {
        EnemySendClientManager._Instance.SendEnemyHurtMsg(hurt, id, listNum);
    }
}
