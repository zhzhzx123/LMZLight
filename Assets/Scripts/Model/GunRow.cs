using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GunRow {

    [SerializeField]
    private int id;
    [SerializeField]
    private string path;//模型路径
    [SerializeField]
    private int gunType;//狙击枪 0  步枪 1  冲锋枪 2  散弹枪 3 (绑定枪口特效)
    [SerializeField]
    private bool isAuto;//是否是自动枪械
    [SerializeField]
    private float range;//有效射程
    [SerializeField]
    private float rangDiscount;//超出有效射程后的伤害折损系数
    [SerializeField]
    private int cartridge;//弹夹子弹数
    [SerializeField]
    private int maxCartridge;//弹夹最大子弹数
    [SerializeField]
    private int carryCap;//带弹数
    [SerializeField]
    private int maxCarryCap;//最大带弹数
    [SerializeField]
    private float reloadTime;//换弹时间
    [SerializeField]
    private float singelAtk;//单发子弹伤害
    [SerializeField]
    private float singelBulletCount;//单发子弹数量(除了散弹枪,基本都为1)
    [SerializeField]
    private float bulletOffsetMax;//子弹的最大偏离角度(只适用一枪多发子弹的枪械)
    [SerializeField]
    private float interval;//最小射击间隔(射速)
    [SerializeField]
    private float offset;//每发子弹的准心偏移量
    [SerializeField] 
    private float sighting;//倍镜放大系数
    [SerializeField]
    private Vector3 usePosition ;//使用时的绑定位置偏移
    [SerializeField]
    private Vector3 useRotation;//使用时的绑定位置旋转
    [SerializeField]
    private Vector3 useScale;//使用时的绑定位置缩放
    [SerializeField]
    private Vector3 standbyPosition ;//背上的绑定位置偏移
    [SerializeField]
    private Vector3 standbyRotation ;//背上的绑定位置旋转
    [SerializeField]
    private Vector3 standbyScale ;//背上的绑定位置缩放


    public GunRow() { }
    public GunRow GetGunRow(GunRow row) {
        GunRow ne = new GunRow();
        ne.id = row.id;
        ne.path = row.path;
        ne.gunType = row.gunType;
        ne.isAuto = row.isAuto;
        ne.range = row.range;
        ne.rangDiscount = row.rangDiscount;
        ne.cartridge = row.cartridge;
        ne.maxCartridge = row.maxCartridge;
        ne.carryCap = row.carryCap;
        ne.maxCarryCap = row.maxCarryCap;
        ne.reloadTime = row.reloadTime;
        ne.singelAtk = row.singelAtk;
        ne.singelBulletCount = row.singelBulletCount;
        ne.bulletOffsetMax = row.bulletOffsetMax;
        ne.interval = row.interval;
        ne.offset = row.offset;
        ne.sighting = row.sighting;
        ne.usePosition = row.usePosition;
        ne.useRotation = row.useRotation;
        ne.useScale = row.useScale;
        ne.standbyPosition = row.standbyPosition;
        ne.standbyRotation = row.standbyRotation;
        ne.standbyScale = row.standbyScale;
        return ne;
    }


    public float GetRealAtkDamage(float dis)
    {
        if(dis<= range)
        {
            return singelAtk;
        }
        else
        {
            return singelAtk * (range/dis)* rangDiscount;
        }
    }
	
    public int ID
    {
        get
        {
            return id;
        }
    }
    public string Path
    {
        get
        {
            return Path;
        }
    }
    public int GunType
    {
        get
        {
            return gunType;
        }
    }
    public bool IsAuto
    {
        get
        {
            return isAuto;
        }
    }
    public float Range
    {
        get
        {
            return range;
        }
    }
    public float RangeDiscount
    {
        get
        {
            return rangDiscount;
        }
    }
    public int Cartridge
    {
        get
        {
            return cartridge;
        }
        set
        {
            cartridge = value;
            if (cartridge < 0)
            {
                cartridge = 0;
            }
        }
    }
    public int MaxCartridge
    {
        get
        {
            return maxCartridge;
        }
    }
    public int CarryCap
    {
        get
        {
            return carryCap;
        }
        set
        {
            carryCap = value;
            if (carryCap < 0)
            {
                carryCap = 0;
            }
        }
    }
    public int MaxCarryCap
    {
        get
        {
            return maxCarryCap;
        }
    }
    public float ReloadTime
    {
        get
        {
            return reloadTime;
        }
    }
    public float SingelAtk
    {
        get
        {
            return singelAtk;
        }
    }
    public float SingelBulletCount
    {
        get
        {
            return singelBulletCount;
        }
    }
    public float BulletOffsetMax
    {
        get
        {
            return bulletOffsetMax;
        }
    }
    public float Interval
    {
        get
        {
            return interval;
        }
    }
    public float Offset
    {
        get
        {
            return offset;
        }
    }
    public float Sighting
    {
        get
        {
            return sighting;
        }
    }
    public Vector3 UsePosition
    {
        get
        {
            return usePosition;
        }
    }
    public Vector3 UseRotation
    {
        get
        {
            return useRotation;
        }
    }
    public Vector3 UseScale
    {
        get
        {
            return useScale;
        }
    }
    public Vector3 StandbyPosition
    {
        get
        {
            return standbyPosition;
        }
    }
    public Vector3 StandbyRotation
    {
        get
        {
            return standbyRotation;
        }
    }
    public Vector3 StandbyScale
    {
        get
        {
            return standbyScale;
        }
    }






}
