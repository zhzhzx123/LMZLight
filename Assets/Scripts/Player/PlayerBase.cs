using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour {

	public PlayerState playerState = PlayerState.Idle;

	public AtkState atkState;

    public GameObject hand;//持枪点
    public GameObject rear;//背枪点

    public PlayerRow playerData;

    public GunBase[] guns;//几把枪

    public GunBase currentGun;//当前使用的枪
    public int currentIndex;//当前使用的枪的索引 

    protected Animator selfAnim;
    
	public float walkSpeed = 3f;
	public float runSpeed = 6f;
	public float rotateSpeed = 30f;

	protected float animH;
	protected float animV;
	protected float ontWeight;

    protected void BaseAwake()
    {
        //guns = GetComponentsInChildren<GunBase>();

       
    }

	// Use this for initialization
	protected void BaseStart() {
		selfAnim = GetComponent<Animator> ();
        InitGun();
    }

	// Update is called once per frame
	protected void BaseUpdate() {
        UpdatePlayerState();
        CheckPlayerState ();
		
        UpdateAtkState();
        CheckAtkState ();
		
	}


    private void InitGun()
    {

        //guns[0] 
        if (guns.Length > 0)
        {
            currentGun = guns[0];
            currentIndex = 0;
            currentGun.transform.SetParent(hand.transform);
            currentGun.transform.localPosition = currentGun.data.UsePosition;
            currentGun.transform.localRotation = Quaternion.Euler(currentGun.data.UseRotation);
            currentGun.transform.localScale = currentGun.data.UseScale;

            guns[1].transform.SetParent(rear.transform);
            guns[1].transform.localPosition = guns[1].data.StandbyPosition;
            guns[1].transform.localRotation = Quaternion.Euler(guns[1].data.StandbyRotation);
            guns[1].transform.localScale = guns[1].data.StandbyScale;
            currentGun.SetUse(true) ;
            guns[1].SetUse(false) ;
        }
    }

    /// <summary>
    /// 换枪
    /// </summary>
    /// <param name="index"></param>
    protected virtual void SwitchGun(int index)
    {
        if(index >= 0 && index < guns.Length && index != currentIndex)
        {
            currentGun.transform.SetParent(rear.transform);
            currentGun.transform.localPosition = currentGun.data.StandbyPosition;
            currentGun.transform.localRotation = Quaternion.Euler(currentGun.data.StandbyRotation);
            currentGun.transform.localScale = currentGun.data.StandbyScale;
            currentGun.SetUse(false); 
            currentGun = guns[index];
            currentIndex = index;
            currentGun.transform.SetParent(hand.transform);
            currentGun.transform.localPosition = currentGun.data.UsePosition;
            currentGun.transform.localRotation = Quaternion.Euler(currentGun.data.UseRotation);
            currentGun.transform.localScale = currentGun.data.UseScale;
            currentGun.SetUse(true);
        }
    }

	protected virtual void SwitchPlayerState(PlayerState state)
	{}

	protected virtual void UpdatePlayerState()
	{}

	protected virtual void CheckPlayerState()
	{}

	protected virtual void SwitchAtkState(AtkState state)
	{}

	protected virtual void UpdateAtkState()
	{}

	protected virtual void CheckAtkState()
	{}

    /// <summary>
    /// 伤害
    /// </summary>
    /// <param name="hurt"></param>
    public virtual void Hurt(float hurt)
    {
        //Debug.Log("玩家受到伤害");
    }

    /// <summary>
    /// 掉血
    /// </summary>
    /// <param name="hurt"></param>
    public virtual void ReduceHP(float hurt)
    {
        playerData.Hp -= hurt;
    }


}
