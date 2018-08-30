using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBullet : MonoBehaviour {


    public BulletType bulletTyp=BulletType.grenade;
    public float moveSpeed=100f;
    public float exploretionHurt;//爆炸伤害
    public float exploretionDis;//爆炸伤害半径
    public GameObject smoke, trailing;
    public GameObject[] effect;
    public Transform tranlingPos;
    public Rigidbody self;
    public Boom boom;

    private float hSpeed;//水平速度
    private float vSpeed;//垂直速度
    private float gravitySpeed;//重力加速度
    private Vector3 hDir;//水平方向
    public Transform player;
    private bool isOnWall;

    void Start()
    {
        self = GetComponent<Rigidbody>();
        isOnWall = false;
        //Instantiate<GameObject>(trailing, tranlingPos.position, Quaternion.Euler(transform.forward), transform);
        
    }

    void Update()
    {
        fly();
    }

    void fly()
    {
        //self.AddForce(transform.TransformDirection(Vector3.forward * moveSpeed), ForceMode.Impulse);
        //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        vSpeed += gravitySpeed * Time.deltaTime;
        transform.Translate(hDir * hSpeed * Time.deltaTime, Space.World);
        transform.Translate(Vector3.up * vSpeed * Time.deltaTime);
        //timeCount += Time.deltaTime;
    }

    public void SetFly(float hS, float vS, float gS, Vector3 hDir)
    {
        this.hSpeed = hS;
        this.vSpeed = vS;
        this.gravitySpeed = gS;
        this.hDir = hDir;
    }

    /// <summary>
    /// 爆炸伤害敌人
    /// </summary>
    void HurtEnemy()
    {
        ///相交球检测半径内的所有敌人
        Collider[] coll_arr = Physics.OverlapSphere(tranlingPos.position, exploretionDis, 1 << 10 | 1 << 12);
        for (int i = 0; i < coll_arr.Length; i++)
        {
            if (coll_arr[i].gameObject.tag == "enemy" && coll_arr[i].gameObject.name.Equals("Body"))
            {
                GetEnemyBase e = coll_arr[i].transform.GetComponent<GetEnemyBase>();
                e.enemy.SetTarget(player);
                e.enemy.Hurt(exploretionHurt);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "enemy")
        {
            
            Instantiate<Boom>(boom, tranlingPos.position, Quaternion.identity, other.transform);
            //Instantiate<GameObject>(smoke, tranlingPos.position, Quaternion.identity, other.transform);
            if(other.gameObject.tag == "Ground")
            {
                isOnWall = true;
                boom.SetBoomTarget(isOnWall);   
            }
            else
            {
                boom.SetBoomTarget(isOnWall);
            }

            HurtEnemy();

            ///爆炸时获取接口，调用接口的方法
            IExploretion ex = GetComponent<IExploretion>();
            if (ex != null)
            {
                ex.Exploretion();
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Boss")
        {
            if (other.gameObject.GetComponentInParent<BossBase>() != null)
            {
                Instantiate<Boom>(boom, tranlingPos.position, Quaternion.identity, other.transform);
                other.gameObject.GetComponentInParent<BossBase>().SetHurtState();
                other.gameObject.GetComponentInParent<BossBase>().Hurt(exploretionHurt);
                IExploretion ex = GetComponent<IExploretion>();
                if (ex != null)
                {
                    ex.Exploretion();
                }
                Destroy(gameObject);
            }
        }

    }
}

