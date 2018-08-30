using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class NetEnemy_AI_1 : NetEnemyBase
{
    void Awake()
    {
        base.BaseAwake();
    }

    // Use this for initialization
    void Start()
    {
        base.BaseStart();
    }

    // Update is called once per frame
    void Update()
    {

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
                enemyAnim.SetBool("Death", true);
                Destroy(gameObject, 5f);
                break;
        }
        enemyState = state;
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
                break;
            case EnemyState.Shoot:
                Shoot();
                break;
        }
    }

    protected override void Shoot()
    {
        if (GetIsShoot())
        {
            effect.Play();
            Debug.Log("开枪");
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position + Vector3.up * 1, Quaternion.Euler(Random.Range(-2, 2), Random.Range(-2, 2), 0) * transform.forward, out hitInfo, 50, 1 << 9))
            {
                PlayerBase player = hitInfo.collider.gameObject.GetComponent<PlayerBase>();
                player.Hurt(enemyData.Atk);
                Debug.Log(" AI  ----   玩家掉血");
            }
            shootIntervalCount = 0;
        }
    }



}
