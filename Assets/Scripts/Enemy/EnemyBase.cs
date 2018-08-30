using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {

    public EnemyState enemyState = EnemyState.Idle;
    protected Animator enemyAnim;
    public EnemyRow enemyData;
    protected float shootIntervalCount = 0f;
    public Transform target;
    public ParticleSystem effect;//特效

    protected virtual void BaseAwake()
    {

    }

    protected virtual void BaseStart()
    {
        enemyAnim = GetComponent<Animator>();
    }

    protected virtual void BaseUpdate()
    {
        UpdateEnemyState();
        shootIntervalCount += Time.deltaTime;
    }

    protected virtual void UpdateEnemyState() { }

    public virtual void SwitchEnemyState(EnemyState enemyState) { }

    /// <summary>
    /// 是否攻击
    /// </summary>
    /// <returns></returns>
    public virtual bool GetIsShoot()
    {
        if (shootIntervalCount >= enemyData.Interval)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 攻击
    /// </summary>
    protected virtual void Shoot()
    {

    }

    public virtual void Hurt(float v)
    {
        //受到伤害
        //Debug.Log("受到伤害: " + gameObject.name);
    }

    /// <summary>
    /// 降低血量
    /// </summary>
    /// <param name="Hp"></param>
    public virtual void ReduceHP(float Hp)
    {
        //ebug.Log("降低血量");
        enemyData.Hp -= Hp;

    }

    public virtual void SetTarget(Transform tag)
    {

    }
}
