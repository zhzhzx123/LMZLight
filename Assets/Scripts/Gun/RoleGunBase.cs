using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleGunBase : GunBase {

    public Camera c;

    private void Start()
    {
        c.transform.position= GameObject.Find("Player/WatchPoint/Camera").transform.position;
        CloseSight();
    }
    protected virtual void CheckOpen()
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


    protected virtual void SetAimed()
    {
        if (isOpen)
        {
            c.transform.rotation = Quaternion.LookRotation(lookFoward.transform.forward);
            c.transform.position = physicGunPos.transform.position;
           
        }
    }

    /// <summary>
    /// 开镜
    /// </summary>
    protected virtual void OpenSight()
    {
        c.fieldOfView = 60 / data.Sighting;
        c.gameObject.SetActive(true);
        isOpen = true;
        EventCenterManager._Instance.SendMessage(EventType.OpenAimed, isOpen);
        //EventCenterManager._Instance.SendMessage(EventType.SwitchSight, SightUI.SightType.Sniper);
    }

    protected virtual void CloseSight()
    {
        c.gameObject.SetActive(false);
        isOpen = false;
        EventCenterManager._Instance.SendMessage(EventType.OpenAimed, isOpen);
        //EventCenterManager._Instance.SendMessage(EventType.SwitchSight, SightUI.SightType.Rifle);
    }
}
