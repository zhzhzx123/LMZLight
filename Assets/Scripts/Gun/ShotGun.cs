using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : RoleGunBase
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
    }

    void Update()
    {
        base.BaseUpdate();
        // LookOffset();
        base.CheckOpen();


    }



    //能否开枪


    protected override void OpenSight()
    {
        base.OpenSight();
        EventCenterManager._Instance.SendMessage(EventType.SwitchSight, SightUI.SightType.Shot);

    }

    protected override void CloseSight()
    {
        base.CloseSight();
        EventCenterManager._Instance.SendMessage(EventType.SwitchSight, SightUI.SightType.Shot);
    }

    public override bool GetIsShoot()
    {
        return base.GetIsShoot();
    }

    public override void InitSight()
    {
        base.InitSight();
        CloseSight();
    }
    //public override void PlayEffect()
    //{
    //    GameObject go = Instantiate<GameObject>(fireEffect, effectPoint.transform.position, gun.transform.rotation, transform);
    //    Destroy(go, 1f);
    //}

    public override bool Shoot()
    {
        if (GetIsShoot())
        {
            //Debug.Log("开枪");
            for (int i = 0; i < data.SingelBulletCount; i++)
            {
                //Debug.DrawLine(physicGunPos.position, Quaternion.Euler(Random.Range(-data.BulletOffsetMax, data.BulletOffsetMax), Random.Range(-data.BulletOffsetMax, data.BulletOffsetMax), 0) * lookFoward.forward * 500, Color.red, 5f);
                if (Physics.Raycast(physicGunPos.position, 
                    Quaternion.Euler(Random.Range(-data.BulletOffsetMax, data.BulletOffsetMax), 
                    Random.Range(-data.BulletOffsetMax, data.BulletOffsetMax), Random.Range(-data.BulletOffsetMax, data.BulletOffsetMax)) * lookFoward.forward, 
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
                        GameObject go2 = Instantiate<GameObject>(blood[Random.Range(0, blood.Count - 1)], hitInfo.point, Quaternion.Euler(transform.rotation.eulerAngles) * Quaternion.Euler(0, 180, 0));
                        Destroy(go2, 4f);
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
                    else if (hitInfo.transform.gameObject.tag == "Boss")
                    {
                        float dis = Vector3.Distance(physicGunPos.position, hitInfo.point);
                        BossBase boss = hitInfo.transform.GetComponent<BossBase>();
                        float hurt = data.GetRealAtkDamage(dis);
                        boss.Hurt(hurt);
                    }
                }
            }
            return base.Shoot();
        }
        return false;
    }
    
}