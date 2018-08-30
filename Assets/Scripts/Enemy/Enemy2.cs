using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy {
    protected override void Shoot()
    {
        if (GetIsShoot())
        {
            //开枪
            //Debug.Log("开枪");
            RaycastHit hitInfo;
            if (CheckTarget())
            {
                //Debug.Log("打到物体");
                if (Physics.Raycast(transform.position + Vector3.up * 1f, transform.forward, out hitInfo, enemyData.Range, 1 << 9 | 1 << 8))
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

    protected override void UpdateEnemyState()
    {
        if (enemyState == EnemyState.Death)
            return;
        switch (enemyState)
        {
            case EnemyState.Idle:

                break;
            case EnemyState.Move:
                RaycastHit hitTarget;
                transform.LookAt(new Vector3(target.position.x, 0, target.position.z));
                if (Physics.Raycast(transform.position + Vector3.up * 1, transform.forward, out hitTarget, 50, 1 << 9 | 1 << 11 | 1 << 8))
                {
                    if (hitTarget.transform.tag == "Ground")
                    {
                        nav.isStopped = false;
                        nav.SetDestination(target.position);
                    }
                    else
                    {
                        nav.isStopped = true;
                        transform.Translate(transform.forward * enemyData.Speed * Time.deltaTime, Space.World);
                    }
                }
                break;
            case EnemyState.Shoot:
                transform.LookAt(new Vector3(target.position.x, 0, target.position.z));
                Shoot();
                break;
        }
    }
}
