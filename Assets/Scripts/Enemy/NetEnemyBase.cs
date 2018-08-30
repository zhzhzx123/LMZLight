using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class NetEnemyBase : EnemyBase
{
    public int id;
    public int listNum;
    protected Vector3 targetPosition;
    protected Vector3 targetRotation;

    protected override  void BaseAwake()
    {
        base.BaseAwake();
    }

    protected override void BaseStart()
    {
        base.BaseStart();
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        Move();
        Rotate();
    }

    public override void Hurt(float v)
    {
        base.Hurt(v);
        EnemySendClientManager._Instance.SendEnemyHurtMsg(v, id, listNum);
        base.ReduceHP(v);
    }

    void Move()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 15 * Time.deltaTime);
    }

    void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation), 30 * Time.deltaTime);
    }


    public void SetTarget(EnemyInfoMessage t)
    {
        targetPosition.x = t.x;
        targetPosition.y = t.y;
        targetPosition.z = t.z;
        targetRotation = new Vector3(t.angleX, t.angleY, t.angleZ);
    }

    public void SetIDandListNum(int id, int num)
    {
        this.id = id;
        this.listNum = num;
    }


    public override void SetTarget(Transform tag)
    {
        if (target == null)
        {
            target = tag;
            EnemySendClientManager._Instance.SendEnemyTargetMsg(NetworkTools.GetLocalIP(), id, listNum);
        }
    }


}
