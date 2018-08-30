using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    UpdatePlayerUI,//更新玩家的血量信息, 参数一 IP， 参数二 最大血量， 参数三 当前血量
	UpdateBattleGunUI, //更新战斗场景 枪的信息, 参数一  枪的ID， 参数二  枪的当前弹夹弹药数， 参数三， 备用弹夹的弹药数
    SwitchSight,//切换准星， 参数一 准星的类型
    SwitchGun,//换枪，  参数一，当前枪的id
    OpenAimed,//开镜,  参数一  是否开启

    GetPlayerInfo,//获取玩家枪械数据 
    SendPlayerInfo,//发送玩家枪械数据 参数1是 gun1 

    SetPlayerInfo,//设置玩家枪械数据  参数1是 gun1 

}
