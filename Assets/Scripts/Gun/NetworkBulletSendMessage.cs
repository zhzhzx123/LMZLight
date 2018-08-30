using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using System;

public class NetworkBulletSendMessage : ClientIntervalSendBase, IExploretion {

    public BulletType bulletType;

    private int num = NetworkTools.GetCurrentNum();

    private void Awake()
    {
        base.BaseAwake();
    }

    // Update is called once per frame
    void Update () {
        base.BaseUpdate();
        transform.Translate(Vector3.forward * Time.deltaTime);
    }


    public void Exploretion()
    {
        BulletSendClient._Instance.SendExplosion(transform.position,
                transform.rotation.eulerAngles, 1, num);
    }
    public override void SendMsg()
    {
        base.SendMsg();
        BulletSendClient._Instance.SendTransform(transform.position,
               transform.rotation.eulerAngles, (int)bulletType, num);
    }
}
