using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StandEnemyBase : EnemyBase
{
    protected NavMeshAgent nav;

    public bool isDeath = false;

    protected override  void BaseAwake()
    {
        base.BaseAwake();
    }

    protected override void BaseStart()
    {
        base.BaseStart();
        nav = GetComponent<NavMeshAgent>();
        Init();
    }

    protected override void BaseUpdate()
    {
        UpdateEnemyState();
        CheckEnemyState();
        shootIntervalCount += Time.deltaTime;
    }

    public override void ReduceHP(float Hp)
    {
        base.ReduceHP(Hp);
        if (enemyData.Hp <= 0)
        {
            SwitchEnemyState(EnemyState.Death);
        }
    }

    protected virtual void Init()
    {
        nav.speed = enemyData.Speed;
        shootIntervalCount = enemyData.Interval;
    }

    protected virtual void CheckEnemyState() { }

}
