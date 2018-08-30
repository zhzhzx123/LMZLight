using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour {

    public GameObject effectPoint;//特效点, 枪口
	public EffectCtrlBase effect;//特效控制
    public Transform physicGunPos, lookFoward, maodian;
    public int ID;//枪的ID
    public GunRow data;
    public float gunH, gunV, gunBackV;
    public bool isUse = false;
    public bool isOpenMicro = true;//是否可以开镜
    public bool isOpen = false;
    private float downLength;
    protected Transform player;
    public AudioSource shootAudio, reloadAudio;

    protected float shootIntervalCount = 0f;

    protected void BaseAwake()
    {
        //data = new GunData();
        data = GunModel_S.Instance.GetGunInfo(data.ID);
    }

    protected void BaseStart()
    {
        player = GameObject.Find("Player").transform;
        shootIntervalCount = data.Interval;
        effect.transform.position = effectPoint.transform.position;
        effect.transform.rotation = effectPoint.transform.rotation;
        EventCenterManager._Instance.SendMessage(EventType.UpdateBattleGunUI, data.ID, data.Cartridge, data.CarryCap);
    }

    protected void BaseUpdate()
    {
        shootIntervalCount += Time.deltaTime;
        LookOffset();
        StopShootAudio();
        GetIsShoot();
    }

    /// <summary>
    /// 是否可以开枪
    /// </summary>
    /// <returns></returns>
    public virtual bool GetIsShoot()
    {
        if (shootIntervalCount >= data.Interval && data.Cartridge > 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 开枪攻击
    /// </summary>
    public virtual bool Shoot()
    {
        if (GetIsShoot())
        {
            //开枪
            //Debug.Log("开枪");
            PlayEffect();
            if (!shootAudio.isPlaying)
            {
                shootAudio.Play();
            }                 
            gunH = Random.Range(-data.Offset, data.Offset);
            gunV = Random.Range(data.Offset * 0.5f, data.Offset);          
            downLength = data.Offset * 0.5f;
            shootIntervalCount = 0;
            data.Cartridge -= 1;
            EventCenterManager._Instance.SendMessage(EventType.UpdateBattleGunUI, data.ID, data.Cartridge, data.CarryCap);
            return true;
        }
        return false;
    }


    public virtual void StopShootAudio()
    {
        if (shootIntervalCount > data.Interval + 0.1f)
        {
            shootAudio.Stop();
        }
    }

    /// <summary>
    /// 播放特效和音效
    /// </summary>
    public virtual void PlayEffect()
    {
        if (effect != null)
        {
            effect.Play();
        }
    }

    public virtual void InitSight()
    { }

    public virtual void SetUse(bool isUse)
    {
        this.isUse = isUse;
        isOpenMicro = isUse;
    }

    public virtual void LookOffset()
    {
        if (GetIsShoot()&& data.IsAuto && Input.GetMouseButton(0))
        {

        }
        else if(GetIsShoot() && !data.IsAuto && Input.GetMouseButtonDown(0))
        {

        }
        else
        {
            if (Mathf.Abs(gunH) > 0.2f)
            {
                gunH = gunH * 0.7f;             
            }
            else if (Mathf.Abs(gunH) <= 0.2f)
            {
                gunH = 0;      
            }
         
            if (gunV > 0.2f)
            {
                gunV = gunV * 0.9f;
                downLength += gunV;            
            }
            else if (gunV <= 0.2f)
            {
                gunV = 0;
                gunBackV = 5;
                BackOffset();
            }
        }
    }

    public virtual void BackOffset()
    {
        gunBackV = gunBackV * 1.5f;
        downLength -= gunBackV;
        if (downLength <= data.Offset * 0.5f)
        {            
            gunBackV = 0;
            downLength = 0;
        }
    }


    public virtual void SetPhysicGunPos(Transform pgp, Transform lf, Transform md)
    {
        physicGunPos = pgp;
        lookFoward = lf;
        maodian = md;
    }

    public void ReplaceBullet()
    {
        if (data.CarryCap != 0)
        {

        if (data.CarryCap >= data.MaxCartridge - data.Cartridge)
        {
            data.CarryCap -= (data.MaxCartridge - data.Cartridge);
            data.Cartridge = data.MaxCartridge;
      
        }
        else
        {
            data.Cartridge += data.CarryCap;
            data.CarryCap = 0;
        
        }
        EventCenterManager._Instance.SendMessage(EventType.UpdateBattleGunUI, data.ID, data.Cartridge, data.CarryCap);
        }
    }

}
