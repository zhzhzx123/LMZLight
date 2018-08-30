using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class Player_2 : PlayerBase {

    public float gunRotateSpeed = 45f;

    public float interval = 0.1f;//网络同步时间，测试

    private float timeCount = 0f;

    private float gunAngle = 0;

    private float h;
    private float v;
    private float mouseX;
    private float mouseY;


    private Vector3 myRotation, watchPointRotation;
    public Transform watchPoint, physicGunPos;
    //private float gunV, gunH,gunBackV;
    public GunBase gun;
  
    


    private CharacterController cc;

    /// <summary>
    /// 网络同步的发送
    /// </summary>
    private BattlePlayerClient client;

    void Awake()
    {
        base.BaseAwake();
        cc = GetComponent<CharacterController>();
    }

    // Use this for initialization
    void Start()
    {
        client = new BattlePlayerClient();
        base.BaseStart();
        myRotation.y = transform.rotation.eulerAngles.y;
        watchPointRotation.x = watchPoint.rotation.eulerAngles.x;
        watchPointRotation.y = watchPoint.rotation.eulerAngles.y;
        gunAngle=watchPointRotation.x;
        Cursor.visible = false;//控制鼠标显示和隐藏， true显示， false时隐藏
        Cursor.lockState = CursorLockMode.Locked;//把鼠标锁定在窗口内，防止鼠标拖到边缘是显示
        gun = currentGun;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();//键鼠输入值更新
        base.BaseUpdate();//人物状态更新
        UpdateNetwork();//网络数据更新

        currentGun.LookOffset();
        //测试
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchPlayerState(PlayerState.Death);
        }
    }

    private void OnDestroy()
    {
        client.CloseClient();
    }

    void GetGunOffset()
    {
     
       // Debug.Log(gunH + "," + gunV + "," + gunBackV);
    }


    #region  玩家控制
    void GetInput()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
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
        //人物左右转向
        myRotation.y += mouseX*rotateSpeed*Time.deltaTime;
        //观察点上下转向
        watchPointRotation.x += -mouseY * rotateSpeed * Time.deltaTime;
        watchPointRotation.x = watchPointRotation.x > 30 ? 30 : watchPointRotation.x;
        watchPointRotation.x = watchPointRotation.x < -30 ? -30 : watchPointRotation.x;
        //观察点左右旋转
        watchPointRotation.y += mouseX * rotateSpeed * Time.deltaTime;
        //获取枪口后坐力导致的旋转值
        //get gunH,gunV
        watchPointRotation.x += -currentGun.gunV * Time.deltaTime;
        watchPointRotation.y += currentGun.gunH * Time.deltaTime;
        watchPointRotation.x += currentGun.gunBackV * Time.deltaTime;
        myRotation.y = watchPointRotation.y;
        //开始旋转
        transform.rotation = Quaternion.Euler(myRotation);
        watchPoint.rotation = Quaternion.Euler(watchPointRotation);
        //transform.Rotate(Vector3.up * mouseX * rotateSpeed * Time.deltaTime, Space.World);
       
    }
    
    /// <summary>
    /// 控制枪口角度
    /// </summary>
    void GunCtrl()
    {
        gunAngle += gunRotateSpeed * mouseY * Time.deltaTime;
        gunAngle += currentGun.gunV * Time.deltaTime;
        gunAngle += -currentGun.gunBackV * Time.deltaTime;
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
                Move(walkSpeed);
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

                selfAnim.SetTrigger("BulletUp");
                atkState = state;
                break;
            case AtkState.SwitchGun:
                int index = currentIndex == 0 ? 1 : 0;//只有两把枪
                SwitchGun(index);
                selfAnim.SetTrigger("SwitchGun");
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
                currentGun.Shoot();//开枪
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
                if (Input.GetKeyDown(KeyCode.R))
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
                else if (Input.GetMouseButton(0))
                {
                    SwitchAtkState(AtkState.Shoot);
                }
                break;
            case AtkState.Shoot:
                if (Input.GetKeyDown(KeyCode.R))
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