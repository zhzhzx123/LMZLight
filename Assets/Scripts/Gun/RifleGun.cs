using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleGun : RoleGunBase
{



    public List<GameObject> impacts;
    public List<GameObject> blood;    
    private RaycastHit hitInfo;
   
   


    void Awake()
    {
        base.BaseAwake();
    }
    void Start ()
    {
        base.BaseStart();
        
	}

    void Update()
    {
        base.BaseUpdate();
        base.CheckOpen();
    }



    //能否开枪
    public override bool GetIsShoot()
    {
        return base.GetIsShoot();
    }

 
    protected override void OpenSight()
    {
        base.OpenSight(); 
        EventCenterManager._Instance.SendMessage(EventType.SwitchSight, SightUI.SightType.Rifle);
    }

    protected override void CloseSight()
    {
        base.CloseSight();
        EventCenterManager._Instance.SendMessage(EventType.SwitchSight, SightUI.SightType.Rifle);
    }

    protected override void CheckOpen()
    {
        if (isUse && isOpenMicro && Input.GetMouseButtonDown(1) && data.Sighting > 1)
        {
            if (isOpen)
            {
                CloseSight();
            }
            else
            {
                OpenSight();
            }
        }
        SetAimed();
    }

    public override void SetUse(bool isUse)
    {
        base.SetUse(isUse);
        CloseSight();
        if (isUse==true)
        {
            c.transform.SetParent(player);
        }
        else
        {
            c.transform.SetParent(transform);
        }
    }


    public override void InitSight()
    {
        base.InitSight();
        CloseSight();
    }

    public override bool Shoot()
    {
        if (GetIsShoot())
        {                        
                if (Physics.Raycast(physicGunPos.position, lookFoward.forward, out hitInfo, 500, 1 << 10 | 1 << 11 | 1 << 12))
                {                 
                    if (hitInfo.transform.gameObject.tag == "Ground")
                    {
                        GameObject go1 = Instantiate<GameObject>(impacts[Random.Range(0, impacts.Count-2)], hitInfo.point, Quaternion.Euler(transform.rotation.eulerAngles) * Quaternion.Euler(0, 180, 0));
                        Destroy(go1, 2f);                        
                    }
                    else if (hitInfo.transform.gameObject.tag == "enemy")
                    {
                        GameObject go1 = Instantiate<GameObject>(blood[Random.Range(0, blood.Count - 1)], hitInfo.point, Quaternion.identity);
                        Destroy(go1, 2f);
                        float dis = Vector3.Distance(physicGunPos.position, hitInfo.point);
                        float hurt = data.GetRealAtkDamage(dis);
                        if (hitInfo.transform.gameObject.name.Equals("Hand"))
                        {                 
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
            return base.Shoot();
        }
        return false;
    }


  
}
