using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : BossBase
{
    IBossSend iboss;

    public Collider[] playerColliders;
    public List<Collider> playerCollidersList = new List<Collider>();

    public PlayerBase player;

    protected float dotResult;//与玩家的点乘结果
    protected float crossResult;//与玩家的叉乘结果
    protected float angle;//与玩家相差的角度

    protected float shootTime;
    protected float shootInterval = 2f;
    protected float jumpTime;
    protected float jumpInterval = 2.2f;
    protected float runAndJumpTime;
    protected float runAndJumpInterval = 3.3f;
    protected float jumpFowardTime;
    protected float jumpBackTime;
    protected float jumpSkillTime;
    protected float jumpSkillInterval;//向玩家跳跃时间间隔
    protected int shootCount = 0;
    protected bool isShootMissile = false;//是否可以发射导弹
    protected bool isStartFight = false;


    public Transform shootPoint01;
    public Transform shootPoint02;
    public Transform missilePoint;
    public GameObject shootEffect;
    public GameObject shootMissile;
    public GameObject BossText;
    protected float distance;
    protected bool playerIsForward = false;//玩家是否在boss的前方
    protected bool playerIsBack = false;//玩家是否在boss的后方
    AnimatorStateInfo bossStateInfo;
    // Use this for initialization
    void Awake()
    {
        bossAnimator = GetComponent<Animator>();
        BossText.SetActive(false);
        iboss = GetComponent<IBossSend>();
    }

    void Start()
    {
        StartCoroutine("Patrol");
        StartCoroutine("ReFindPlayer");
    }
    /// <summary>
    /// Boss巡逻方法,每隔5秒钟探查一次四周是否有玩家,直到找到玩家为止
    /// </summary>
    /// <returns></returns>
    IEnumerator Patrol()
    {
        yield return new WaitForSeconds(5.0f);
        Debug.Log("开始巡逻");
        playerColliders = Physics.OverlapSphere(transform.position, 100f, 1 << 8 | 1 << 9);
        if (playerColliders.Length > 0)
        {
            Debug.Log("找到玩家");
            int playerid = Random.Range(0, playerColliders.Length);
            for (int i = 0; i < playerColliders.Length; i++)
            {
                if (playerColliders[i].GetComponent<PlayerBase>() != null && i == playerid)
                {
                    player = playerColliders[i].GetComponent<PlayerBase>();
                    shootPoint01.GetComponent<BossShootPoint>().player = player;
                    shootPoint02.GetComponent<BossShootPoint>().player = player;

                }
            }
        }
        else
        {
            if (player == null)
            {
                StartCoroutine("Patrol");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        UpdateBossState();
        CheckBossState();

        UpdateAtkState();
        CheckAtkState();

        CheckPlayerIsDeath();
        CheckFight();
        if (Input.GetKeyDown(KeyCode.J))
        {
            SwitchBossState( BossState.Death);
        }
        
    }

    protected void CheckFight()
    {
        if (bossHP < bossMaxHP && isStartFight == false)
        {
            SwitchPlayer();
            isStartFight = true;
        }
    }



    protected override void CheckPlayerPosition()
    {
        if (player != null)
        {
            Vector3 vectorPlayer = player.transform.position - transform.position;
            dotResult = Vector3.Dot(transform.forward.normalized, vectorPlayer.normalized);
            angle = Vector3.Angle(transform.forward.normalized, vectorPlayer.normalized);
            crossResult = Vector3.Cross(transform.forward.normalized, vectorPlayer.normalized).y;

            if (dotResult > 0 && crossResult > 0)
            {
                //Debug.Log("玩家在我右前方");
                playerIsForward = true;
                playerIsBack = false;
                if (Mathf.Abs(leftRightAngle) < angle)
                {
                    leftRightAngle -= rotateSpeed * Time.deltaTime;
                }
                else if (Mathf.Abs(leftRightAngle) > angle && leftRightAngle <= 0)
                {
                    leftRightAngle += rotateSpeed * Time.deltaTime;
                }
                leftRightAngle = leftRightAngle < -90 ? -90 : leftRightAngle;
                //Debug.Log("角度:" + leftRightAngle);

                //bossAnimator.SetFloat("UpDownAngle", upDownAngle);
            }
            else if (dotResult > 0 && crossResult < 0)
            {
                //Debug.Log("玩家在我的左前方");
                playerIsForward = true;
                playerIsBack = false;
                if (leftRightAngle < angle)
                {
                    leftRightAngle += rotateSpeed * Time.deltaTime;
                }
                else if (leftRightAngle > angle && leftRightAngle >= 0)
                {
                    leftRightAngle -= rotateSpeed * Time.deltaTime;
                }
                leftRightAngle = leftRightAngle > 90 ? 90 : leftRightAngle;

                //bossAnimator.SetFloat("UpDownAngle", upDownAngle);
            }
            else if (dotResult < 0 && crossResult < 0)
            {
                //Debug.Log("玩家在我的左后方");
                playerIsForward = false;
                playerIsBack = true;
            }
            else if (dotResult <= 0 && crossResult > 0)
            {
                //Debug.Log("玩家在我的右后方");
                playerIsForward = false;
                playerIsBack = true;

            }

            distance = Vector3.Distance(player.transform.position, transform.position);
            //Debug.Log("距离" + distance);
            //Debug.Log("点乘结果: " + dotResult);
            //Debug.Log("角度: " + angle);
            //Debug.Log("叉乘结果: " + crossResult);
        }
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
        if (player != null)
        {
            if (player.playerState == PlayerState.Death)
            {
                return;
            }
        }
        switch (state)
        {
            case BossState.Idle:
                break;
            case BossState.Walk:
                bossAnimator.SetBool("Walk", true);
                jumpSkillInterval = Random.Range(3, 11);
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
        if (bossState != BossState.GetHit)
        {
            iboss.SwitchState((int)bossState, (int)bossAtkState);
        }
        if (bossState == BossState.Death)
        {
            iboss.DestroyThis();
        }
    }

    public override void SetHurtState()
    {
        base.SetHurtState();
        SwitchBossState(BossState.GetHit);
        iboss.SwitchState((int)bossState, (int)bossAtkState);
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
        if (player != null)
        {
            if (player.playerState == PlayerState.Death)
            {
                return;
            }
        }
       

        switch (bossState)
        {
            case BossState.Idle:
                CheckPlayerPosition();
                break;
            case BossState.Walk:
                CheckPlayerPosition();
                bossStateInfo = bossAnimator.GetCurrentAnimatorStateInfo(0);
                //由于boss需要播放完践踏动画后再播放移动动画,所以当播放践踏动画时不允许boss移动
                if (!bossStateInfo.IsName("trample"))
                {
                    Move();
                    jumpSkillTime += Time.deltaTime;
                }
               
                if (jumpSkillInterval - jumpSkillTime <= 1.0f)
                {
                    BossText.SetActive(true);
                }
                else
                {
                    BossText.SetActive(false);
                }
                break;
            case BossState.Run:
                CheckPlayerPosition();
                Move();
                LookPlayer();
                break;
            case BossState.RunAndJump:
                CheckPlayerPosition();
                Move();
                LookPlayer();
                runAndJumpTime += Time.deltaTime;
                //跳跃起来后将跑的移动速度增加
                if (runAndJumpTime >= 1.2f && runAndJumpTime <= 2.25f)
                {
                    runSpeed = 40f;
                }
                else
                {
                    runSpeed = 10f;
                }
                break;
            case BossState.Jump:
                CheckPlayerPosition();
                jumpTime += Time.deltaTime;
                if (jumpTime >= 0.5f && jumpTime <= jumpInterval)
                {
                    LookPlayer();
                }
                break;
            case BossState.JumpForward:
                CheckPlayerPosition();
                jumpFowardTime += Time.deltaTime;
                if (jumpFowardTime >= 0.5f && jumpTime <= jumpInterval && player != null)
                {
                    LookPlayer();
                    Vector3 velocity = Vector3.zero;
                    transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref velocity, 0.1f);

                    //transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.5f);
                }
                
                break;
            case BossState.GetHit:
                break;
            case BossState.Death:
                break;
        }
    }


    /// <summary>
    /// 检查玩家是否死亡
    /// </summary>
    public void CheckPlayerIsDeath()
    {
        if (player != null)
        {
            if (player.playerState == PlayerState.Death)
            {
                //player = null;
                StopFight();
                SwitchPlayer();
            }
        }

        //跳跃动画和踩踏动画时开启刚体isKinematic功能
        if (bossStateInfo.IsName("trample"))
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

    }

    /// <summary>
    /// 每隔1分钟重新切换一次玩家
    /// </summary>
    IEnumerator  ReFindPlayer()
    {
        yield return new WaitForSeconds(60f);
        if (player != null)
        {
            SwitchPlayer();
        }
    }

    /// <summary>
    /// 切换寻找玩家
    /// </summary>
    public void SwitchPlayer()
    {
        playerColliders = Physics.OverlapSphere(transform.position, 150f, 1 << 8 | 1 << 9);
        playerCollidersList.Clear();
        if (playerColliders.Length > 0)
        {

            for (int i = 0; i < playerColliders.Length; i++)
            {

                playerCollidersList.Add(playerColliders[i]);

            }
            for (int i = 0; i < playerCollidersList.Count; i++)
            {
                if (playerCollidersList[i].GetComponent<PlayerBase>() != null)
                {
                    if (playerCollidersList[i].GetComponent<PlayerBase>().playerState == PlayerState.Death)
                    {
                        playerCollidersList.RemoveAt(i);
                    }
                }
            }

            if (playerCollidersList.Count > 0)
            {
                int playerid = Random.Range(0, playerCollidersList.Count);
                for (int i = 0; i < playerCollidersList.Count; i++)
                {
                    if (playerCollidersList[i].GetComponent<PlayerBase>() != null && i == playerid)
                    {
                        player = playerColliders[i].GetComponent<PlayerBase>();
                        shootPoint01.GetComponent<BossShootPoint>().player = player;
                        shootPoint02.GetComponent<BossShootPoint>().player = player;
                    }
                }
            }
            else
            {
                Debug.Log("当前巡逻范围内玩家已阵亡");
            }


        }
        else
        {
            Debug.Log("当前巡逻范围内玩家已阵亡");
        }
    }


    /// <summary>
    /// 当当前场景所有玩家都已经死亡的情况下让Boss停止战斗
    /// </summary>
    public void StopFight()
    {
        SwitchBossState(BossState.Idle);
        SwitchAtkState(BossAtkState.None);
        player = null;
        bossAnimator.SetBool("Walk", false);
        bossAnimator.SetBool("Run", false);
        bossAnimator.SetBool("RunAndJump", false);
        bossAnimator.SetBool("Shoot", false);
        bossAnimator.SetBool("Trample", false);
        bossAnimator.SetTrigger("StopTrample");
    }

    /// <summary>
    /// 怪物移动方法
    /// </summary>
    public void Move()
    {

        if (bossState == BossState.Walk)
        {
            transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
        }
        else if (bossState == BossState.Run || bossState == BossState.RunAndJump)
        {
            transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 看向玩家方法
    /// </summary>
    public void LookPlayer()
    {
        if (player != null)
        {
            Vector3 v3 = player.transform.position - transform.position;
            v3.y = 0;//让boss不会上下旋转
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(v3), 2 * Time.deltaTime);
        }

    }

    /// <summary>
    /// 判断是否切换Boss状态
    /// </summary>
    protected override void CheckBossState()
    {
        if (bossState == BossState.Death)
        {
            return;
        }
        if (player != null)
        {
            if (player.playerState == PlayerState.Death)
            {
                return;
            }
        }
        switch (bossState)
        {
            case BossState.Idle:
                CheckForwardOrBack();
                if (player != null)
                {
                    bossStateInfo = bossAnimator.GetCurrentAnimatorStateInfo(0);
                    if (distance > 100)
                    {
                        SwitchBossState(BossState.RunAndJump);
                    }
                    else if (distance > 50)
                    {
                        SwitchBossState(BossState.Run);
                    }
                    else if (distance <= 50 && distance >= 8)
                    {
                        SwitchBossState(BossState.Walk);
                    }


                    if (distance < 8 && !bossStateInfo.IsName("jump"))
                    {
                        SwitchAtkState(BossAtkState.Trample);
                    }
                    else if (bossStateInfo.IsName("idle2") && distance < 8)
                    {

                        SwitchAtkState(BossAtkState.Trample);
                    }





                }
                break;
            case BossState.Walk:
                CheckForwardOrBack();
                bossStateInfo = bossAnimator.GetCurrentAnimatorStateInfo(0);
                if (distance > 50)
                {
                    bossAnimator.SetBool("Walk", false);
                    SwitchBossState(BossState.Run);
                    //jumpSkillTime = 0;
                }
                else if (distance < 8 && !bossStateInfo.IsName("jump"))
                {
                    bossAnimator.SetBool("Walk", false);
                    SwitchAtkState(BossAtkState.Trample);
                }

                if (bossStateInfo.IsName("gethit"))
                {
                    jumpSkillTime = 0;
                }

                //当累加时间超过boss前跳技能时间同时第0层动画不是跳跃时执行再切换到前跳状态
                bossStateInfo = bossAnimator.GetCurrentAnimatorStateInfo(0);
                if (jumpSkillTime >= jumpSkillInterval && !bossStateInfo.IsName("jump"))
                {
                    bossAnimator.SetBool("Walk", false);
                    SwitchBossState(BossState.JumpForward);
                    jumpSkillTime = 0;
                }
                break;
            case BossState.Run:
                CheckForwardOrBack();
                if (distance <= 50)
                {
                    bossAnimator.SetBool("Run", false);
                    SwitchBossState(BossState.Walk);
                }
                else if (distance > 100)
                {
                    bossAnimator.SetBool("Run", false);
                    SwitchBossState(BossState.RunAndJump);
                }
                break;
            case BossState.RunAndJump:
                CheckForwardOrBack();
                if (distance <= 100 && distance > 50)
                {
                    bossAnimator.SetBool("RunAndJump", false);
                    SwitchBossState(BossState.Run);
                }
                break;
            case BossState.Jump:
                if (playerIsForward && jumpTime >= jumpInterval)
                {
                    SwitchBossState(BossState.Idle);
                    jumpTime = 0;
                }
                break;
            case BossState.JumpForward:
                if (playerIsForward && jumpFowardTime >= jumpInterval)
                {
                    SwitchBossState(BossState.Idle);
                    jumpFowardTime = 0;
                }
                break;
            case BossState.JumpBackwards:
                break;
            case BossState.Death:
                break;
            default:
                break;
        }
    }

    public void CheckForwardOrBack()
    {
        if (playerIsBack)
        {
            SwitchBossState(BossState.Jump);
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
        if (player != null)
        {
            if (player.playerState == PlayerState.Death)
            {
                return;
            }
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
        iboss.SwitchState((int)bossState, (int)bossAtkState);
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
        if (player != null)
        {
            if (player.playerState == PlayerState.Death)
            {
                return;
            }
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



    /// <summary>
    /// 检测攻击的状态是否切换
    /// </summary>
    protected override void CheckAtkState()
    {
        if (bossState == BossState.Death)
        {
            return;
        }
        if (player != null)
        {
            if (player.playerState == PlayerState.Death)
            {
                return;
            }
        }
        switch (bossAtkState)
        {
            case BossAtkState.None:
                bossStateInfo = bossAnimator.GetCurrentAnimatorStateInfo(0);
                if (playerIsForward && distance <= 100 && distance >= 8)
                {
                    SwitchAtkState(BossAtkState.Shoot);
                }
                else if (playerIsForward && distance < 8 && !bossStateInfo.IsName("jump"))
                {
                    SwitchAtkState(BossAtkState.Trample);
                }
                break;
            case BossAtkState.Shoot:
                if (playerIsForward)
                {
                    //onWeight = Mathf.Lerp(onWeight, 0.6f, 15 * Time.deltaTime);
                    bossAnimator.SetLayerWeight(1, 0.6f);
                    bossAnimator.SetFloat("LeftRightAngle", leftRightAngle);
                }
                else
                {
                    SwitchAtkState(BossAtkState.None);
                    bossAnimator.SetBool("Shoot", false);
                }

                if (distance > 100 && playerIsForward)
                {
                    bossAnimator.SetBool("Shoot", false);
                    SwitchAtkState(BossAtkState.None);
                }
                else if (distance < 8 && playerIsForward && !bossStateInfo.IsName("jump"))
                {
                    bossAnimator.SetBool("Shoot", false);
                    SwitchAtkState(BossAtkState.Trample);


                }
                break;
            case BossAtkState.Trample:
                if (distance >= 8)
                {
                    bossAnimator.SetTrigger("StopTrample");
                    SwitchAtkState(BossAtkState.None);
                    CheckForwardOrBack();
                    if (player != null)
                    {
                        if (distance > 100)
                        {
                            SwitchBossState(BossState.RunAndJump);
                        }
                        else if (distance > 50)
                        {
                            SwitchBossState(BossState.Run);
                        }
                        else if (distance <= 50 && distance >= 8)
                        {
                            SwitchBossState(BossState.Walk);
                        }


                    }
                }
                break;
            case BossAtkState.Skill:
                break;
        }
    }



    /// <summary>
    /// 发射子弹方法添加在动画shoot事件帧中
    /// </summary>
    public void Shoot()
    {
        GameObject shootEffect01 = Instantiate(shootEffect, shootPoint01.position, shootPoint01.rotation);
        GameObject shootEffect02 = Instantiate(shootEffect, shootPoint02.position, shootPoint02.rotation);
        if (isShootMissile == false)
        {
            ++shootCount;
        }
        //每射七次发射一次追踪导弹
        if (shootCount >= 7)
        {
            isShootMissile = true;
            StartCoroutine("ShootMissile");
            StartCoroutine("StopShootMissile");
            shootCount = 0;

        }
    }
    /// <summary>
    /// 让Boss返回Idle状态方法添加在动画gethit中
    /// </summary>
    public void BackIdle()
    {
        SwitchBossState(BossState.Idle);
    }

   
    //发射追踪弹
    IEnumerator ShootMissile()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject shootMissile01 = Instantiate(shootMissile, missilePoint.position, missilePoint.rotation);
        StartCoroutine("ShootMissile");
        bossStateInfo = bossAnimator.GetCurrentAnimatorStateInfo(0);
        //当Boss当前动画是受击动画或者死亡时停止发射追踪导弹
        if (bossStateInfo.IsName("gethit") || bossStateInfo.IsName("death"))
        {
            StopCoroutine("ShootMissile");
        }
    }

    //停止发射追踪弹
    IEnumerator StopShootMissile()
    {
        yield return new WaitForSeconds(5f);
        isShootMissile = false;
        StopCoroutine("ShootMissile");
    }


    public override void Hurt(float hurt)
    {
        base.Hurt(hurt);
        iboss.Hurt(hurt);
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
        bossStateInfo = bossAnimator.GetCurrentAnimatorStateInfo(0);
        if ((collider.gameObject.tag == "Player" && bossStateInfo.IsName("jump")))
        {
            Debug.Log("跳跃踩中");
            player.Hurt(50f);
        }
        else if ((collider.gameObject.tag == "Player" && bossStateInfo.IsName("trample")))
        {
            Debug.Log("踩踏踩中");
            player.Hurt(20f);
        }
    }


    

}
