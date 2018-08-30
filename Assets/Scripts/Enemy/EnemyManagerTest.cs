using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class EnemyManagerTest : MonoBehaviour {

    public GameObject enemy;

    public List<Enemy> enemies;

    public Transform target;

    public void Awake()
    {
        if(!NetworkTools.GetLocalIP().Equals(RoomSingle.roomIP))
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        target = other.transform;
        if (other.tag=="Player"&&enemies.Count==0)
        {
            CreateEnemy();
        }
    }

    public void CreateEnemy()
    {
        GameObject go = Instantiate<GameObject>(enemy,transform);
        go.transform.localPosition = Vector3.zero;
        go.name = "enemy";
        Enemy emy = go.GetComponent<Enemy>();
        //emy.enemyManager = this;
        enemies.Add(emy);
    }

}
