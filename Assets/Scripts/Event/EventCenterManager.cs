using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenterManager : Single<EventCenterManager>, IManager
{
    public delegate void EventCallBack(params object[] obj_arr);

    private Dictionary<EventType, EventCallBack> eventDic = new Dictionary<EventType, EventCallBack>();

    #region 接口
    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        
    }

    #endregion

    public void AddListener(EventType type, EventCallBack callback)
    {
        if (eventDic.ContainsKey(type))
        {
            eventDic[type] += callback;
        }
        else
        {
            eventDic.Add(type, callback);
        }
    }

    public void RemoveListener(EventType type, EventCallBack callback)
    {
        if (eventDic.ContainsKey(type))
        {
            eventDic[type] -= callback;
        }
    }

    public void SendMessage(EventType type, params object[] obj_arr)
    {
        if (eventDic.ContainsKey(type) && eventDic[type] != null)
        {
            eventDic[type](obj_arr);
        }
    }

}
