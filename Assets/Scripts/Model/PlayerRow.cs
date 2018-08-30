using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerRow
{
    [SerializeField]
    private int ID;//人物ID
    [SerializeField]
    private float hp;//人物血量
    [SerializeField]
    private float maxHp;//人物最大血量
    [SerializeField]
    private int armor;//人物护甲
    [SerializeField]
    private int maxArmor;//人物最大护甲
    [SerializeField]
    private float speed;//人物移动速度
    [SerializeField]
    private string path;//模型路径
    [SerializeField]
    private int mapping;//贴图种类（颜色）

    public float Hp { get { return hp; } set { hp = value; if (hp < 0) { hp = 0;  } } }
    public float MaxHp { get { return maxHp; } }
    public int Armor { get { return armor; } }
    public int MaxArmor { get { return maxArmor; } }
    public float Speed { get { return speed; } }
    public string Path { get { return path; } }
    public int Mapping { get { return mapping; } }
}
