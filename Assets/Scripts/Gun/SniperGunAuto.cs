using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperGunAuto : RoleGunBase
{
  
    public List<GameObject> impacts;
    public List<GameObject> blood;
    private RaycastHit hitInfo;



    void Awake()
    {
        base.BaseAwake();
    }
    void Start()
    {
        base.BaseStart();
        //scaleIndex = -1;
        //InitSight();
        CloseSight();
    }

    void Update()
    {
        base.BaseUpdate();
        // LookOffset();
        base.CheckOpen();


    }
    public override void InitSight()
    {
        base.InitSight();
        CloseSight();
    }

    protected override void OpenSight()
    {
        base.OpenSight();
        EventCenterManager._Instance.SendMessage(EventType.SwitchSight, SightUI.SightType.Sniper);

    }

    protected override void CloseSight()
    {
        base.CloseSight();
        EventCenterManager._Instance.SendMessage(EventType.SwitchSight, SightUI.SightType.Rifle);
    }

    public override void SetPhysicGunPos(Transform pgp, Transform lf, Transform md)
    {
        base.SetPhysicGunPos(pgp, lf, md);

    }

    //能否开枪
    public override bool GetIsShoot()
    {
        return base.GetIsShoot();
    }



    public override void SetUse(bool isUse)
    {
        base.SetUse(isUse);
        CloseSight();
        if (isUse == true)
        {
            c.transform.SetParent(player);
        }
        else
        {
            c.transform.SetParent(transform);
        }
    }

    public override bool Shoot()
    {
        if (GetIsShoot())
        {
            Debug.Log("开枪");
            for (int i = 0; i < data.SingelBulletCount; i++)
            {

                if (Physics.Raycast(physicGunPos.position,
                Quaternion.Euler(Random.Range(-data.BulletOffsetMax, data.BulletOffsetMax),
                Random.Range(-data.BulletOffsetMax, data.BulletOffsetMax), 0) * lookFoward.forward,
                out hitInfo, 500, 1 << 10 | 1 << 11 | 1 << 12))
                {
                    //Debug.LogError(hitInfo.transform.gameObject.tag);
                    if (hitInfo.transform.gameObject.tag == "Ground")
                    {
                        GameObject go1 = Instantiate<GameObject>(impacts[Random.Range(0, impacts.Count - 1)], hitInfo.point, Quaternion.Euler(transform.rotation.eulerAngles) * Quaternion.Euler(0, 180, 0));
                        Destroy(go1, 4f);
                    }
                    else if (hitInfo.transform.gameObject.tag == "enemy")
                    {
                        GameObject go1 = Instantiate<GameObject>(blood[Random.Range(0, blood.Count - 1)], hitInfo.point, Quaternion.Euler(transform.rotation.eulerAngles) * Quaternion.Euler(0, 180, 0));
                        Destroy(go1, 4f);
                        float dis = Vector3.Distance(physicGunPos.position, hitInfo.point);
                        float hurt = data.GetRealAtkDamage(dis);
                        if (hitInfo.transform.gameObject.name.Equals("Hand"))
                        {
                            //Debug.LogError("爆头");
                            hurt *= 2;
                        }

                        GetEnemyBase e = hitInfo.transform.GetComponent<GetEnemyBase>();
                        e.enemy.SetTarget(player);
                        e.enemy.Hurt(hurt);
                    }
                }
            }     
            return base.Shoot();
        }
        return false;
    }

}
