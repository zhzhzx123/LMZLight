using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoBehaviour {

    public BossState bossState = BossState.Idle;
    public BossAtkState bossAtkState = BossAtkState.None;

    protected Animator bossAnimator;

    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float rotateSpeed = 30f;

    public float bossHP = 5000;
    public float bossMaxHP = 50000;

    protected float leftRightAngle = 0;
    protected float upDownAngle = 0;
    protected float onWeight;
    

    public virtual void SwitchBossState(BossState state)
    { }

    protected virtual void UpdateBossState()
    { }

    protected virtual void CheckBossState()
    { }

    public virtual void SwitchAtkState(BossAtkState state)
    { }

    protected virtual void UpdateAtkState()
    { }

    protected virtual void CheckAtkState()
    { }

    protected virtual void CheckPlayerPosition()
    { }


    public virtual void SetHurtState()
    { }


    /// <summary>
    /// 伤害
    /// </summary>
    /// <param name="hurt"></param>
    public virtual void Hurt(float hurt)
    {
        
    }

    /// <summary>
    /// 掉血
    /// </summary>
    /// <param name="hurt"></param>
    public virtual void ReduceHP(float hurt)
    {
        bossHP -= hurt;
    }



}
