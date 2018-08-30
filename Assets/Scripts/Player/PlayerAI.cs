using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using System;

public class PlayerAI : PlayerBase  {

	private float h;
	private float v;

	private Vector3 targetPosition;
    private Vector3 targetRotation;
    private float gunTarget;
    private float gunAngle;

    private IBattleAI ib;

    void Awake()
    {
        ib = GetComponent<IBattleAI>();
       
        base.BaseAwake();
       
    }

    // Use this for initialization
    void Start () {
       
        guns = new GunBase[2];
        GameObject gun1 = Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/Gun/Gun_" + RoomSingle.GetInfos()[ib.IP].gun1ID + "_AI"));
        guns[0] = gun1.GetComponent<GunBase>();
        GameObject gun2 = Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/Gun/Gun_" + RoomSingle.GetInfos()[ib.IP].gun2ID + "_AI"));
        guns[1] = gun2.GetComponent<GunBase>();
        base.BaseStart();
    }
	
	// Update is called once per frame
	void Update () {
		base.BaseUpdate ();
		Move ();
		Rotate ();
        GunAngel();
    }

    private void OnDestroy()
    {
    }

    public override void Hurt(float hurt)
    {
        
    }

    public override void ReduceHP(float hurt)
    {
        base.ReduceHP(hurt);
        EventCenterManager._Instance.SendMessage(EventType.UpdatePlayerUI, ib.IP, playerData.MaxHp, playerData.Hp);
    }

    public void Shoot()
    {
        currentGun.PlayEffect();
    }

    #region AI移动

    void Move()
	{
		transform.position = Vector3.Lerp (transform.position, targetPosition, 15 * Time.deltaTime);
	}

	void Rotate()
	{
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler(targetRotation), 30 * Time.deltaTime);
	}


    void GunAngel()
    {
        selfAnim.SetLayerWeight(1, 0.6f);
        gunAngle = Mathf.Lerp(gunAngle, gunTarget, 15 * Time.deltaTime);
        selfAnim.SetFloat("Angle", gunAngle);
    }

    #endregion

    #region 有限状态机
    /// <summary>
    /// 切换攻击状态
    /// </summary>
    protected override void SwitchAtkState(AtkState state)
	{
		if (playerState == PlayerState.Death)
			return;
		if (atkState == state)
			return;
		switch (state) {
		case AtkState.None:
			break;
		case AtkState.Shoot:
            //判断武器是否自动
            if (currentGun.data.IsAuto)
            {
                selfAnim.SetTrigger("MultiShoot");
            }
            else
            {
                selfAnim.SetTrigger("SingleShoot");
            }
            atkState = state;
            //currentGun.PlayEffect();
            break;
		case AtkState.BulletUp:
			selfAnim.SetTrigger ("BulletUp");
			break;
        case AtkState.SwitchGun:
            int index = currentIndex == 0 ? 1 : 0;//只有两把枪
            SwitchGun(index);
            selfAnim.SetTrigger("SwitchGun");
            break;
        }

		atkState = state;
	}

	protected override void UpdateAtkState ()
	{
		if (playerState == PlayerState.Death)
			return;
		switch (atkState) {
		case AtkState.None:
			ontWeight = Mathf.Lerp (ontWeight, 0, 15 * Time.deltaTime);
			selfAnim.SetLayerWeight (2, ontWeight);
			break;
		case AtkState.Shoot:
			ontWeight = Mathf.Lerp (ontWeight, 1, 15 * Time.deltaTime);
			selfAnim.SetLayerWeight (2, ontWeight);
			break;
		case AtkState.BulletUp:
			ontWeight = Mathf.Lerp (ontWeight, 1, 15 * Time.deltaTime);
			selfAnim.SetLayerWeight (2, ontWeight);
			break;
       case AtkState.SwitchGun:
            ontWeight = Mathf.Lerp(ontWeight, 1, 15 * Time.deltaTime);
            selfAnim.SetLayerWeight(3, ontWeight);
            break;
        }
        
    }


	protected override void UpdatePlayerState ()
	{
		if (playerState == PlayerState.Death)
			return;
		switch (playerState) {
		case PlayerState.Idle:
			v = 0;
			h = 0;
			break;
		case PlayerState.Walk:
			{
				Vector3 local = transform.InverseTransformPoint (targetPosition);
				if (local.z > 0) {
					v = 1;
				} else if (local.z == 0) {
					v = 0;
				} else {
					v = -1;
				}
				if (local.x == 0) {
					h = 0;
				} else if (local.x > 0) {
					h = 1;
				} else {
					h = -1;
				}
			}
			break;
		case PlayerState.Run:
			{
				Vector3 local = transform.InverseTransformPoint (targetPosition);
				if (local.x == 0) {
					h = 0;
				} else if (local.x > 0) {
					h = 1;
				} else {
					h = -1;
				}
				v = 2;
			}
			break;
		}
		animV = Mathf.Lerp (animV, v, 10 * Time.deltaTime);
		animH = Mathf.Lerp (animH, h, 10 * Time.deltaTime);
		selfAnim.SetFloat ("H", animH);
		selfAnim.SetFloat ("V", animV);
	}

    protected override void SwitchPlayerState(PlayerState state)
    {
        if (playerState == PlayerState.Death)
            return;
        if (playerState == state)
            return;
        if (state == PlayerState.Death)
        { 
            selfAnim.SetBool("Death", true);
            selfAnim.SetLayerWeight(2, 0);
        }
        playerState = state;
	}


    #endregion

    #region 网络同步调取的方法

    public void SetTarget(TransformMessage t)
    {
        targetPosition.x = t.x;
        targetPosition.y = t.y;
        targetPosition.z = t.z;
        targetRotation = new Vector3(0, t.angle, 0);
        gunTarget = t.gunAngle;
    }

    public void SetState(PlayerState s1, AtkState s2)
    {
        SwitchPlayerState(s1);
        SwitchAtkState(s2);
    }

    #endregion
}
