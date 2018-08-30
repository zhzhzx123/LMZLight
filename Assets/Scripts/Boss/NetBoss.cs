using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class NetBoss : BossBase {

    protected Vector3 targetPosition;
    protected Vector3 targetRotation;


    private void Awake()
    {
        bossAnimator = GetComponent<Animator>();
    }


    private void Update()
    {
        UpdateBossState();

        UpdateAtkState();
        Move();
        Rotate();
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

    /// <summary>
    /// 切换Boss状态
    /// </summary>
    /// <param name="state"></param>
    public override void SwitchBossState(BossState state)
    {
        if (bossState == BossState.Death)
        {
            return;
        }
        switch (state)
        {
            case BossState.Idle:
                break;
            case BossState.Walk:
                bossAnimator.SetBool("Walk", true);
                break;
            case BossState.Run:
                bossAnimator.SetBool("Run", true);
                break;
            case BossState.RunAndJump:
                bossAnimator.SetBool("RunAndJump", true);
                bossAnimator.SetFloat("RunAndJumpSpeed", 2.0f);
                break;
            case BossState.Jump:
                bossAnimator.SetTrigger("Jump");
                break;
            case BossState.JumpForward:
                bossAnimator.SetTrigger("Jump");
                break;
            case BossState.GetHit:
                bossAnimator.SetTrigger("GetHit");
                break;
            case BossState.Death:
                bossAnimator.SetBool("Death", true);
                Collider[] coll_arr = GetComponentsInChildren<Collider>();
                for (int i = 0; i < coll_arr.Length; i++)
                {
                    Destroy(coll_arr[i]);
                }
                break;

        }
        bossState = state;

    }

    public void BackIdle()
    {

    }

    public override void SetHurtState()
    {
        base.SetHurtState();
        SwitchBossState(BossState.GetHit);
        EnemySendClientManager._Instance.SendBossStateMsg(bossState, bossAtkState, 0);
    }

    /// <summary>
    /// 每个状态应执行的行为
    /// </summary>
    protected override void UpdateBossState()
    {
        if (bossState == BossState.Death)
        {
            return;
        }

        switch (bossState)
        {
            case BossState.Idle:

                break;
            case BossState.Walk:

                break;
            case BossState.Run:

                break;
            case BossState.RunAndJump:

                break;
            case BossState.Jump:

                break;
            case BossState.JumpForward:

                break;
            case BossState.GetHit:
                break;
            case BossState.Death:
                break;
        }
    }

    /// <summary>
    /// 切换攻击状态
    /// </summary>
    /// <param name="state"></param>
    public override void SwitchAtkState(BossAtkState state)
    {
        if (bossState == BossState.Death)
        {
            return;
        }
        if (bossAtkState == state)
        {
            return;
        }
        switch (state)
        {
            case BossAtkState.None:
                break;
            case BossAtkState.Shoot:
                bossAnimator.SetBool("Shoot", true);
                bossAnimator.SetFloat("ShootSpeed", 1.5f);
                break;
            case BossAtkState.Trample:
                bossAnimator.SetTrigger("Trample");
                bossAnimator.SetFloat("TrampleSpeed", 1.5f);
                break;
            case BossAtkState.Skill:
                break;
        }
        bossAtkState = state;
       
    }

    /// <summary>
    /// 各种状态执行的行为
    /// </summary>
    protected override void UpdateAtkState()
    {
        if (bossState == BossState.Death)
        {
            return;
        }
        switch (bossAtkState)
        {
            case BossAtkState.None:
                onWeight = Mathf.Lerp(onWeight, 0, 15 * Time.deltaTime);
                bossAnimator.SetLayerWeight(2, onWeight);
                break;
            case BossAtkState.Shoot:
                onWeight = Mathf.Lerp(onWeight, 1, 15 * Time.deltaTime);
                bossAnimator.SetLayerWeight(2, onWeight);
                break;
            case BossAtkState.Trample:
                //LookPlayer();
                onWeight = Mathf.Lerp(onWeight, 1, 15 * Time.deltaTime);
                bossAnimator.SetLayerWeight(0, onWeight);
                break;
            case BossAtkState.Skill:
                break;
        }
    }

    public override void Hurt(float hurt)
    {
        base.Hurt(hurt);
        EnemySendClientManager._Instance.SendBossHurtMsg(hurt, 0);
        ReduceHP(hurt);
    }


    public override void ReduceHP(float hurt)
    {
        base.ReduceHP(hurt);

        if (bossHP <= 0)
        {
            bossHP = 0;
            SwitchBossState(BossState.Death);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("asdsadsadsad");
        AnimatorStateInfo bossStateInfo = bossAnimator.GetCurrentAnimatorStateInfo(0);
        if ((collider.gameObject.tag == "Player" && bossStateInfo.IsName("jump")))
        {
            Debug.Log("跳跃踩中");
            PlayerBase player = collider.gameObject.GetComponent<PlayerBase>();
            player.Hurt(50f);
        }
        else if ((collider.gameObject.tag == "Player" && bossStateInfo.IsName("trample")))
        {
            Debug.Log("踩踏踩中");
            PlayerBase player = collider.gameObject.GetComponent<PlayerBase>();
            player.Hurt(20f);
        }
    }

    public void Shoot()
    {

    }
}
