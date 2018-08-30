using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SightUI : MonoBehaviour {

    public enum SightType
    {
        Rifle, //自动步枪
        Sniper,//狙击步枪
        Shot,//霞弹枪
        Rpg,//Rpg
        Grenade,//榴弹微妙
    }

    private Dictionary<SightType, GameObject> sightDic = new Dictionary<SightType, GameObject>();

    private SightType currentSight;
    // Use this for initialization
    void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = transform.Find(((SightType)i).ToString()).gameObject;
            sightDic.Add((SightType)i, obj);
            obj.SetActive(false);
        }

        EventCenterManager._Instance.AddListener(EventType.SwitchSight, SwitchSightCallBack);
    }

    private void OnDestroy()
    {
        EventCenterManager._Instance.RemoveListener(EventType.SwitchSight, SwitchSightCallBack);
    }

    void SwitchSightCallBack(params object[] obj_arr)
    {
        SwitchSight((SightType)obj_arr[0]);
    }

    void SwitchSight(SightType type)
    {
        sightDic[currentSight].SetActive(false);
        currentSight = type;
        sightDic[currentSight].SetActive(true);
    }
}
