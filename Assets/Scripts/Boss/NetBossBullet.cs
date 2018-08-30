using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetBossBullet : NetworkBullet
{
   
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (type == BulletType.bossBullet)
            return;
        Move();
        Rotate();
	}

    public override void Explosion()
    {
        if (type == BulletType.bossBullet)
            return;
        if (null != gameObject)
        {
            Destroy(gameObject);
        }
        
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        if (type == BulletType.bossBullet)
            return;
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("击中");
            if (collider.gameObject.GetComponent<Player>() != null)
            {
                collider.gameObject.GetComponent<Player>().Hurt(4);
            }
        }
        else if (collider.gameObject.tag == "Ground")
        {

        }
    }

}
