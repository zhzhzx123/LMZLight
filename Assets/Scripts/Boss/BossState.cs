public enum BossState
{
	Idle,//站立
	Walk,//走
	Run,//跑
	RunAndJump,//跑跳
    Jump,//原地跳跃
	JumpForward,//向前方跳跃
	JumpBackwards,//向后跳跃
    GetHit,//受击状态
	Death,//死亡
}

public enum BossAtkState
{
	None,//无攻击状态
	Shoot,//射击
	Trample,//践踏
    Skill,//技能
}