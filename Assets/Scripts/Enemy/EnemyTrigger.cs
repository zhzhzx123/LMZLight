using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public List<Transform> point;
    private List<Enemy> linkEnemys = new List<Enemy>();
    private List<Transform> initPosition = new List<Transform>();
    public EnemyManager manager;
    public int TiggerNum;
    private bool isOn = false;//只生成一次
    public bool isMove = false;//移动到目标点
    public Transform list;
    public Transform stratPoint;
    public Transform endPoint;

    private void Update()
    {
        TargetChain();
        ListMove();
    }


    private void ListMove()
    {
        if (list!=null&&isOn)
        {
            if (list.localPosition.z >= endPoint.position.z && !isMove)
            {
                list.Translate(Vector3.forward * 10f * Time.deltaTime);
            }
            else if (list.localPosition.z < endPoint.position.z)
            {
                isMove = true;
            }
            if (list.localPosition.z <= stratPoint.position.z && isMove)
            {
                list.Translate(Vector3.forward * -10f * Time.deltaTime);
            }
            else if(list.localPosition.z > stratPoint.position.z)
            {
                isMove = false;
            }
        }
    }


    /// <summary>
    /// 仇恨连锁
    /// </summary>
    private void TargetChain()
    {
        if (isOn)
        {
            for (int i = 0; i < linkEnemys.Count; i++)
            {
                if (linkEnemys[i].target != null)
                {
                    foreach (Enemy item in linkEnemys)
                    {
                        item.target = linkEnemys[i].target;
                    }
                }
            }
        }
    }

    void CreateEnemy()
    {
        for (int i = 0; i < point.Count; i++)
        {
            if (manager.death.Count==0)
            {
                GameObject obj = Instantiate(manager.enemy[Random.Range(0,manager.enemy.Count)], point[i].position, point[i].rotation, transform);
                obj.GetComponent<Enemy>().enemyData.Listnum = TiggerNum+i;
                obj.name = (TiggerNum + i).ToString();
                manager.enemies.Add(obj.GetComponent<Enemy>());
                linkEnemys.Add(obj.GetComponent<Enemy>());
                initPosition.Add(obj.transform);
            }
            else
            {
                manager.death[manager.death.Count - 1].transform.position = point[i].position;
                manager.death[manager.death.Count - 1].gameObject.SetActive(true);
                manager.enemies.Add(manager.death[manager.death.Count - 1]);
                manager.death.RemoveAt(manager.death.Count - 1);
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (!isOn)
        {
            CreateEnemy();
            isOn = true;
        }
    }
}
