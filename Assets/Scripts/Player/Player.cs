using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class Player : PlayerBase {
    public float gunRotateSpeed = 45f;

    public float interval = 0.1f;//网络同步时间，测试

    private float timeCount = 0f;

    private float gunAngle = 0;

    private float h;
    private float v;
    private float mouseX;
    private float mouseY;
    public AudioSource reloadGun;


    private Vector3 myRotation, watchPointRotation;
    public Transform watchPoint, physicGunPos,myCamera;
    


    private CharacterController cc;

    /// <summary>
    /// 网络同步的发送
    /// </summary>
    private BattlePlayerClient client;

    void Awake()
    {
        base.BaseAwake();
        cc = GetComponent<CharacterController>();
        client = new BattlePlayerClient();
    }

    // Use this for initialization
    void Start()
    {
        guns = new GunBase[2];
        GameObject gun1 = Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/Gun/Gun_" + RoomSingle.GetInfos()[NetworkTools.GetLocalIP()].gun1ID));
        guns[0] = gun1.GetComponent<GunBase>();
        GameObject gun2 = Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/Gun/Gun_" + RoomSingle.GetInfos()[NetworkTools.GetLocalIP()].gun2ID));
        guns[1] = gun2.GetComponent<GunBase>();
        
        base.BaseStart();
        myRotation.y = transform.rotation.eulerAngles.y;
        watchPointRotation.x = watchPoint.rotation.eulerAngles.x;
        watchPointRotation.y = watchPoint.rotation.eulerAngles.y;
        gunAngle = watchPointRotation.x;
        Cursor.visible = false;//控制鼠标显示和隐藏， true显示， false时隐藏
        Cursor.lockState = CursorLockMode.Locked;//把鼠标锁定在窗口内，防止鼠标拖到边缘是显示

        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].SetPhysicGunPos(physicGunPos, myCamera, watchPoint);
        }
        currentGun.InitSight();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();//键鼠输入值更新
        base.BaseUpdate();//人物状态更新
        UpdateNetwork();//网络数据更新

        currentGun.LookOffset();

    }

    private void OnDestroy()
    {
        client.CloseClient();
    }


    protected override void SwitchGun(int index)
    {
        base.SwitchGun(index);
        currentGun.InitSight();
    }

    public void ReplaceBullet()
    {
        currentGun.ReplaceBullet();
    }

    public override void Hurt(float hurt)
    {
        base.Hurt(hurt);
        //发送网络同步
        client.SendPlayerHurtToServer(hurt);
        ReduceHP(hurt);
    }

    public override void ReduceHP(float hurt)
    {
        base.ReduceHP(hurt);
        EventCenterManager._Instance.SendMessage(EventType.UpdatePlayerUI, NetworkTools.GetLocalIP(), playerData.MaxHp, playerData.Hp);
        if (playerData.Hp <= 0)
        {
            SwitchPlayerState(PlayerState.Death);
        }
    }

    #region  玩家控制
    void GetInput()
    {
        if (!Cursor.visible)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }
        else
        {
            h = 0;
            v = 0;
            mouseX = 0;
            mouseY = 0;
        }
    }



    /// <summary>
    /// 玩家移动
    /// </summary>
    /// <param name="speed">Speed.</param>
    void Move(float speed)
    {
        //transform.Translate ((new Vector3(h, 0, v)).normalized * speed * Time.deltaTime);
        Vector3 moveDir = transform.forward * v;
        moveDir += transform.right * h;
        cc.Move(moveDir.normalized * speed * Time.deltaTime);
        cc.Move(Vector3.down * 9.8f * Time.deltaTime);
    }

    /// <summary>
    /// 玩家旋转
    /// </summary>
    void Rotate()
    {
        float currentSpeed = currentGun.isOpen ?  rotateSpeed / currentGun.data.Sighting : rotateSpeed;
        //人物左右转向
        myRotation.y += mouseX * currentSpeed * Time.deltaTime;
        //观察点上下转向
        watchPointRotation.x += -mouseY * currentSpeed * Time.deltaTime;
        watchPointRotation.x = watchPointRotation.x > 7 ? 7 : watchPointRotation.x;
        watchPointRotation.x = watchPointRotation.x < -53 ? -53 : watchPointRotation.x;
        //观察点左右旋转
        watchPointRotation.y += mouseX * currentSpeed * Time.deltaTime;
        //获取枪口后坐力导致的旋转值
        //get gunH,gunV
        watchPointRotation.x += -currentGun.gunV * Time.deltaTime;
        watchPointRotation.y += currentGun.gunH * Time.deltaTime;
        watchPointRotation.x += currentGun.gunBackV * Time.deltaTime;
        myRotation.y = watchPointRotation.y;
        //开始旋转
        transform.rotation = Quaternion.Euler(myRotation);
        watchPoint.rotation = Quaternion.Euler(watchPointRotation);


    }

    /// <summary>
    /// 控制枪口角度
    /// </summary>
    void GunCtrl()
    {
        gunAngle = -(watchPointRotation.x + 23);
        gunAngle += currentGun.gunV * Time.deltaTime;
        gunAngle+= -currentGun.gunBackV * Time.deltaTime; 
        gunAngle = gunAngle > 30 ? 30 : gunAngle;
        gunAngle = gunAngle < -30 ? -30 : gunAngle;
        selfAnim.SetLayerWeight(1, 0.6f);
        selfAnim.SetFloat("Angle", gunAngle);        
    }

    #endregion


    #region 有限状态机

    /// <summary>
    /// 切换玩家状态
    /// </summary>
    /// <param name="state">State.</param>
    protected override void SwitchPlayerState(PlayerState state)
    {
        if (playerState == PlayerState.Death)
            return;
        if (playerState == state)
            return;
        switch (state)
        {
            case PlayerState.Idle:

                break;
            case PlayerState.Walk:

                break;
            case PlayerState.Run:

                break;
            case PlayerState.Death:
                selfAnim.SetBool("Death", true);
                cc.enabled = false;
                selfAnim.SetLayerWeight(2, 0);
                break;
        }
        playerState = state;
        //网络同步状态
        client.SendPlayerStateToServer(playerState, atkState);
    }

    /// <summary>
    /// 每个状态应该执行的行为
    /// </summary>
    protected override void UpdatePlayerState()
    {
        if (playerState == PlayerState.Death)
            return;
        switch (playerState)
        {
            case PlayerState.Idle:
                GunCtrl();
                Rotate();
                Move(0);
                animV = Mathf.Lerp(animV, 0, 10 * Time.deltaTime);
                animH = Mathf.Lerp(animH, 0, 10 * Time.deltaTime);
                break;
            case PlayerState.Walk:
                GunCtrl();
                Rotate();
                Move(walkSpeed);
                animH = Mathf.Lerp(animH, h, 10 * Time.deltaTime);
                if (v > 0)
                {
                    //向前走
                    animV = Mathf.Lerp(animV, 1, 10 * Time.deltaTime);
                }
                else
                {
                    //向后走
                    animV = Mathf.Lerp(animV, -1, 10 * Time.deltaTime);
                }
                break;
            case PlayerState.Run:
                GunCtrl();
                Rotate();
                Move(runSpeed);
                animV = Mathf.Lerp(animV, 2, 10 * Time.deltaTime);
                animH = Mathf.Lerp(animH, h, 10 * Time.deltaTime);
                break;
        }

        selfAnim.SetFloat("H", animH);
        selfAnim.SetFloat("V", animV);
    }


    /// <summary>
    /// 判断是否切换玩家状态
    /// </summary>
    protected override void CheckPlayerState()
    {
        if (playerState == PlayerState.Death)
            return;
        if (Mathf.Abs(h) >= 0.1f || Mathf.Abs(v) >= 0.01f)
        {
            if (Input.GetKey(KeyCode.LeftShift) && v > 0)
            {
                SwitchPlayerState(PlayerState.Run);
            }
            else
            {
                SwitchPlayerState(PlayerState.Walk);
            }
        }
        else
        {
            SwitchPlayerState(PlayerState.Idle);
        }
    }

    /// <summary>
    /// 切换攻击状态
    /// </summary>
    protected override void SwitchAtkState(AtkState state)
    {
        if (playerState == PlayerState.Death)
            return;
        if (atkState == state)
            return;
        switch (state)
        {
            case AtkState.None:
                atkState = state;
                break;
            case AtkState.Shoot:
                if (currentGun.GetIsShoot())
                {
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
                }
                break;
            case AtkState.BulletUp:
                currentGun.InitSight();
                currentGun.isOpenMicro = false;
                selfAnim.SetTrigger("BulletUp");
                atkState = state;
                break;
            case AtkState.SwitchGun:
                int index = currentIndex == 0 ? 1 : 0;//只有两把枪
                SwitchGun(index);
                selfAnim.SetTrigger("SwitchGun");
                EventCenterManager._Instance.SendMessage( EventType.SwitchGun, currentGun.data.ID);
                atkState = state;
                break;
        }

        //网络同步状态
        client.SendPlayerStateToServer(playerState, atkState);
    }

    /// <summary>
    /// 各种状态执行的行为
    /// </summary>
    protected override void UpdateAtkState()
    {
        if (playerState == PlayerState.Death)
            return;
        switch (atkState)
        {
            case AtkState.None:
                ontWeight = Mathf.Lerp(ontWeight, 0, 15 * Time.deltaTime);
                selfAnim.SetLayerWeight(2, ontWeight);
                break;
            case AtkState.Shoot:
                if (currentGun.Shoot())
                {
                    client.SendPlayerShootToServer();
                }//开枪
                ontWeight = Mathf.Lerp(ontWeight, 1, 30 * Time.deltaTime);
                selfAnim.SetLayerWeight(2, ontWeight);
                break;
            case AtkState.BulletUp:
                ontWeight = Mathf.Lerp(ontWeight, 1, 15 * Time.deltaTime);
                selfAnim.SetLayerWeight(2, ontWeight);
                break;
            case AtkState.SwitchGun:
                ontWeight = Mathf.Lerp(ontWeight, 1, 15 * Time.deltaTime);
                selfAnim.SetLayerWeight(3, ontWeight);
                break;
        }
    }

    /// <summary>
    /// 检测攻击的状态是否切换
    /// </summary>
    protected override void CheckAtkState()
    {
        if (playerState == PlayerState.Death)
            return;
        switch (atkState)
        {
            case AtkState.None:
                if ((Input.GetKeyDown(KeyCode.R) && currentGun.data.CarryCap > 0) ||
                (currentGun.data.Cartridge <= 0 && currentGun.data.CarryCap > 0))
                {
                    reloadGun.Play();
                    SwitchAtkState(AtkState.BulletUp);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha1) && currentIndex == 1)
                {
                    SwitchAtkState(AtkState.SwitchGun);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) && currentIndex == 0)
                {
                    SwitchAtkState(AtkState.SwitchGun);
                }
                else if (Input.GetMouseButton(0) && !Cursor.visible&& currentGun.data.IsAuto)
                {
                    SwitchAtkState(AtkState.Shoot);
                }
                else if (Input.GetMouseButtonDown(0) && !Cursor.visible && !currentGun.data.IsAuto)
                {
                    SwitchAtkState(AtkState.Shoot);
                }
                break;
            case AtkState.Shoot:
                if (Input.GetKeyDown(KeyCode.R)&&currentGun.data.CarryCap!=0)
                {
                   SwitchAtkState(AtkState.BulletUp);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha1) && currentIndex == 1)
                {
                    SwitchAtkState(AtkState.SwitchGun);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) && currentIndex == 0)
                {
                    SwitchAtkState(AtkState.SwitchGun);
                }

                AnimatorStateInfo shoot = selfAnim.GetCurrentAnimatorStateInfo(2);
                string str = currentGun.data.IsAuto ? "MultiShoot" : "SingleShoot";
                if (!shoot.IsName(str) && !selfAnim.IsInTransition(2))
                {
                    SwitchAtkState(AtkState.None);
                }

                break;
            case AtkState.BulletUp:
                {
                    AnimatorStateInfo info = selfAnim.GetCurrentAnimatorStateInfo(2);
                    if (!info.IsName("BulletUp") && !selfAnim.IsInTransition(2))
                    {
                        ReplaceBullet();
                        currentGun.isOpenMicro = true;
                        SwitchAtkState(AtkState.None);
                    }
                }
                break;
            case AtkState.SwitchGun:
                {
                    AnimatorStateInfo info = selfAnim.GetCurrentAnimatorStateInfo(3);
                    if (!info.IsName("SwitchGun") && !selfAnim.IsInTransition(3))
                    {
                        SwitchAtkState(AtkState.None);
                    }
                }
                break;
        }
    }

    #endregion


    #region 网络位置同步

    void UpdateNetwork()
    {
        timeCount += Time.deltaTime;
        if (timeCount >= interval)
        {
            //位置同步
            //同步位置
            client.SendTranfromToServer(transform, gunAngle);
            timeCount -= interval;
        }
    }

    #endregion
}