using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rpg : RoleGunBase
{


    public GameObject bullet, gun;
    public GameObject fireEffect;


 
    //private float singelBulletCount = 20;//单发子弹数量(除了散弹枪,基本都为1)
    //private float bulletOffsetMax = 5;//子弹的最大偏离角度(只适用一枪多发子弹的枪械)
    //private float interval = 2f;//最小射击间隔(射速)
    //private float offset = 300f;//每发子弹的准心偏移量
    //private float atkTimer;


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
        base.CheckOpen();
    }

    protected override void SetAimed()
    {
        base.SetAimed();
        //if (isOpen)
        //{
        //    c.transform.rotation = effectPoint.transform.rotation;
        //    c.transform.position = effectPoint.transform.position 
        //        + effectPoint.transform.forward * -1.5f;
        //}
    }

    /// <summary>
    /// 开镜
    /// </summary>
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

    public override void SetUse(bool isUse)
    {
        base.SetUse(isUse);
        CloseSight();
    }


    //能否开枪
    public override bool GetIsShoot()
    {
        return base.GetIsShoot();
    }

    public override void InitSight()
    {
        base.InitSight();
        CloseSight();
        EventCenterManager._Instance.SendMessage(EventType.SwitchSight, SightUI.SightType.Rpg);
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
         
            RpgBullet rpg = Instantiate<GameObject>(bullet,
                effectPoint.transform.position, effectPoint.transform.rotation).GetComponent<RpgBullet>();
            rpg.player = player;
            rpg.exploretionHurt = data.SingelAtk;
            CloseSight();
            return base.Shoot();
        }
        return false;
    }



}
