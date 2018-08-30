using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeGun : RoleGunBase
{

    public GameObject bullet;
    public LineRenderer lineRenderer;
    public float initSpeed;//初速度
    public float angle;//枪口角度
    public float hSpeed;//水平速度
    public float vSpeed;//垂直速度
    public float gravitySpeed;//重力加速度
    public float height;//枪口到地面的距离
    public float anlgeScale = 0.45f;//修正角度
    public float time;
    public float drowLineTimeCount = 0f;

    void Awake()
    {
        base.BaseAwake();
        if (lineRenderer == null)
        {
            GameObject obj = (new GameObject("Line"));
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            lineRenderer = obj.AddComponent<LineRenderer>();
        }
    }
    void Start()
    {
        base.BaseStart();


        lineRenderer.startWidth = 0.02f;
        lineRenderer.startColor = new Color(1,0,0,0);
        lineRenderer.endWidth = 0.3f;
        lineRenderer.endColor = new Color(1, 0, 0, 1);
        lineRenderer.positionCount = 0;


    }

    void Update()
    {
        base.BaseUpdate();

        angle = (-maodian.transform.eulerAngles.x - 23f);
        angle = angle <= -180 ? 360 + angle : angle;
        angle *= anlgeScale;

        base.CheckOpen();

    }


    void DrowLine()
    {
        if (angle > 0)//枪口上抬
        {
            hSpeed = initSpeed * Mathf.Cos(angle * Mathf.Deg2Rad);
            vSpeed = initSpeed * Mathf.Sin(angle * Mathf.Deg2Rad);
            float t1 = Mathf.Abs(vSpeed / -gravitySpeed);
            float maxHeight = vSpeed * t1 + 0.5f * gravitySpeed * t1;
            float t2 = Mathf.Sqrt(2 * (maxHeight + height) / -gravitySpeed);
            time = t1 + t2;

        }
        else//枪口下压
        {
            hSpeed = initSpeed * Mathf.Cos(Mathf.Abs(angle) * Mathf.Deg2Rad);
            vSpeed = -initSpeed * Mathf.Sin(Mathf.Abs(angle) * Mathf.Deg2Rad);
            float t1 = Mathf.Abs(height / gravitySpeed);//估算时间
            time = t1;
        }

        lineRenderer.positionCount = 0;
        float timeInvertal = 0.05f;
        int count = (int)Mathf.Ceil(time / timeInvertal) + 50;
        lineRenderer.positionCount = count;
        for (int i = 0; i < count; i++)
        {
            Vector3 t = effectPoint.transform.position +
                Vector3.up * (vSpeed * (i * timeInvertal) + 0.5f * gravitySpeed * (i * timeInvertal) * (i * timeInvertal))
                + maodian.parent.forward * (hSpeed * (i * timeInvertal));
            lineRenderer.SetPosition(i, t);
        }
    }

    protected override void SetAimed()
    {
        base.SetAimed();
        if (isOpen)
        {
            //c.transform.rotation = effectPoint.transform.rotation;
            //c.transform.position = effectPoint.transform.position + effectPoint.transform.forward * -1.5f;
            c.transform.rotation = Quaternion.LookRotation(lookFoward.transform.forward);
            c.transform.position = physicGunPos.transform.position;
            drowLineTimeCount += Time.deltaTime;
            if (drowLineTimeCount >= 0.01f)
            {
                DrowLine();
                drowLineTimeCount -= 0.01f;
            }
        }
        
    }

    /// <summary>
    /// 开镜
    /// </summary>
    protected override void OpenSight()
    {
        base.OpenSight();
        EventCenterManager._Instance.SendMessage(EventType.SwitchSight, SightUI.SightType.Grenade);
        drowLineTimeCount = 0.01f;
    }

    protected override void CloseSight()
    {
        base.CloseSight();
        EventCenterManager._Instance.SendMessage(EventType.SwitchSight, SightUI.SightType.Grenade);
        lineRenderer.positionCount = 0;
    }

    public override void SetUse(bool isUse)
    {
        base.SetUse(isUse);
        CloseSight();
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

    public override bool Shoot()
    {
        if (GetIsShoot())
        {
            GrenadeBullet grenade = Instantiate<GameObject>(bullet, 
                effectPoint.transform.position, effectPoint.transform.rotation).GetComponent<GrenadeBullet>();
            grenade.player = player;
            angle = (-maodian.transform.eulerAngles.x - 23f);
            angle = angle <= -180 ? 360 + angle : angle;
            angle *= anlgeScale;
            if (angle > 0)//枪口上抬
            {
                hSpeed = initSpeed * Mathf.Cos(angle * Mathf.Deg2Rad);
                vSpeed = initSpeed * Mathf.Sin(angle * Mathf.Deg2Rad);
            }
            else//枪口下压
            {
                hSpeed = initSpeed * Mathf.Cos(Mathf.Abs(angle) * Mathf.Deg2Rad);
                vSpeed = -initSpeed * Mathf.Sin(Mathf.Abs(angle) * Mathf.Deg2Rad);
            }
            grenade.SetFly(hSpeed, vSpeed, gravitySpeed, maodian.parent.forward);
            grenade.exploretionHurt = data.SingelAtk;
            CloseSight();
            return base.Shoot();
        }
        return false;
    }
}
