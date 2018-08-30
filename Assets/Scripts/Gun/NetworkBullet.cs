using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class NetworkBullet : MonoBehaviour {

    public BulletType type;
    public GameObject[] effect_arr;

    private Vector3 targetPosition;
    private Vector3 targetRotation;
	
	// Update is called once per frame
	void Update () {
        Move();
        Rotate();
    }

    protected void Move()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 15 * Time.deltaTime);
    }

    protected void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation), 30 * Time.deltaTime);
    }

    public void SetTarget(BulletMessage t)
    {
        targetPosition.x = t.x;
        targetPosition.y = t.y;
        targetPosition.z = t.z;
        targetRotation = new Vector3(t.angleX, t.angleY, t.angleZ);
    }

    public virtual void Explosion()
    {

        for (int i = 0; i < effect_arr.Length; i++)
        {
            GameObject effect = Instantiate<GameObject>(effect_arr[i], transform.position, Quaternion.identity);
            Destroy(effect, 5f);
        }

        Destroy(gameObject);
    }
}
