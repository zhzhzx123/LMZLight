using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : StandEnemyBase
{
    public EnemyManager enemyManager;
    public IEnemySend send;
    protected float range;
    protected PlayerBase player;

    //public Transform raycast;

    void Awake()
    {
        base.BaseAwake();
        range = enemyData.Range;
        send = GetComponent<IEnemySend>();
    }

    // Use this for initialization
    void Start () {
        base.BaseStart();
        send.SetIDandNum(enemyData.ID, enemyData.Listnum);
    }
	
	// Update is called once per frame
	void Update () {
        base.BaseUpdate();
        //测试
        /*
        if (Input.GetKeyDown(KeyCode.K))
        {
            Hurt(10);
        }
        */
    }

    /// <summary>
    /// 切换敌人状态
    /// </summary>
    /// <param name="state">切换的状态</param>
    public override void SwitchEnemyState(EnemyState state)
    {
        if (enemyState == EnemyState.Death)
            return;
        if (enemyState == state)
            return;
        switch (state)
        {
            case EnemyState.Idle:
                enemyAnim.SetBool("Atk", false);
                enemyAnim.SetBool("Move", false);
                break;
            case EnemyState.Move:
                enemyAnim.SetBool("Atk", false);
                enemyAnim.SetBool("Move", true);
                break;
            case EnemyState.Shoot:
                enemyAnim.SetBool("Move", false);
                enemyAnim.SetBool("Atk", true);
                break;
            case EnemyState.Death:
                if (!nav.isStopped)
                {
                    nav.isStopped = true;
                }
                enemyAnim.SetBool("Death", true);
                Collider[] coll_arr = GetComponentsInChildren<Collider>();
                for (int i = 0; i < coll_arr.Length; i++)
                {
                    Destroy(coll_arr[i]);
                }
                Destroy(gameObject, 5f);
                break;
        }
        enemyState = state;
        //网络同步
        send.SwitchState((int)enemyState);
        if (enemyState == EnemyState.Death)
        {
            send.SwitchState((int)state);
            send.DestroyThis();
        }
    }

    /// <summary>
    /// 每个状态应该执行的行为
    /// </summary>
    protected override void UpdateEnemyState()
    {
        if (enemyState == EnemyState.Death)
            return;
        switch (enemyState)
        {
            case EnemyState.Idle:

                break;
            case EnemyState.Move:
                nav.isStopped = false;
                nav.SetDestination(target.position);
                break;
            case EnemyState.Shoot:
                transform.LookAt(target.position-Vector3.up*0.5f);
                Shoot();
                break;
        }
    }

    protected override void CheckEnemyState()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    isDeath = true;
        //    SwitchEnemyState(EnemyState.Death);
        //}
        //if (target == null && !isDeath)
        //{
        //    transform.Rotate(Vector3.up);
        //}
        if (target==null&&!isDeath)
        {
            SwitchEnemyState(EnemyState.Idle);
        }
        if (target != null && !isDeath)
        {
            CheckDistance();
        }

    }

    protected virtual void CheckDistance()
    {
        if (Vector3.Distance(transform.position, target.position) < range)
        {
            //if (player == null)
            //{
            //    target = null;
            //    SwitchEnemyState(EnemyState.Idle);
            //    return;
            //}
            player = target.GetComponent<PlayerBase>();
            if (player.playerState == PlayerState.Death)
            {
                target = null;
                SwitchEnemyState(EnemyState.Idle);
                return;
            }
            //raycast.LookAt(target.position);
            //if (CheckTarget())
            if(CheckTarget())
            {
                nav.isStopped = true;
                SwitchEnemyState(EnemyState.Shoot);
            }
        }
        else
        {
            if (effect!=null)
            {
                effect.Stop();
            }
            SwitchEnemyState(EnemyState.Move);
        }
    }

    public override void Hurt(float v)
    {
        base.Hurt(v);
        //同步伤害
        send.Hurt(v);
        base.ReduceHP(v);
       
    }

    protected override void Shoot()
    {
        if (GetIsShoot())
        {
            effect.Play();
            //开枪
            //Debug.Log("开枪");
            RaycastHit hitInfo;
            if (/*CheckTarget()*/true)
            {
                //Debug.Log("打到物体");
                if (Physics.Raycast(transform.position + Vector3.up * 1f, Quaternion.Euler(Random.Range(-2, 2), Random.Range(-2, 2), 0) * transform.forward, out hitInfo, 50, 1 << 9 | 1 << 8))
                {
                    PlayerBase player = hitInfo.collider.gameObject.GetComponent<PlayerBase>();
                    player.Hurt(enemyData.Atk);
                    //Debug.Log("玩家掉血");
                    if (player.playerState == PlayerState.Death)
                    {
                        target = null;
                        SwitchEnemyState(EnemyState.Idle);
                    }
                }
            }
            shootIntervalCount = 0;
        }
    }

    protected bool CheckTarget()
    {
        RaycastHit hitTarget;
        Vector3 dir = target.position - transform.position;
        if (Physics.Raycast(transform.position+Vector3.up*1, dir.normalized, out hitTarget, 50, 1 << 9 | 1 << 11 | 1 << 8))
        {
            if (hitTarget.transform.tag == "Ground")
            {
                range -= 1;
                return false;
            }
            else if (hitTarget.transform.tag == "Player" || hitTarget.transform.tag == "PlayerAI")
            {
                range = enemyData.Range;
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter");
        if (other.transform.tag == "Player" || other.transform.tag == "PlayerAI")
        {
            SetTarget(other.transform);
        }
    }

    public override void SetTarget(Transform tag)
    {
        if (target == null)
        {
            target = tag;
        }
    }
}
