using UnityEngine;

[System.Serializable]
public class EnemyRow
{
    [SerializeField]
    private int id;
    [SerializeField]
    private int listnum;
    [SerializeField]
    private string name;
    [SerializeField]
    private float hp;
    [SerializeField]
    private float maxHp;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float hitSpeed;
    [SerializeField]
    private float recoverTime;
    [SerializeField]
    private string path;
    [SerializeField]
    private int atk;
    [SerializeField]
    private float range;
    [SerializeField]
    private float interval;

    public int ID { get { return id; } }
    public int Listnum { get { return listnum; } set { listnum = value; } }
    public string Name { get { return name; } }
    public float Hp { get { return hp; } set { hp = value; if (hp < 0) { hp = 0;  } } }
    public float MaxHp { get { return maxHp; } }
    public float Speed { get { return speed; } }
    public float HitSpeed { get { return hitSpeed; } }
    public float RecoverTime { get { return recoverTime; } }
    public string Path { get { return path; } }
    public int Atk { get { return atk; } }
    public float Range { get { return range; } }
    public float Interval { get { return interval; } }
}
