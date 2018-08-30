using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour {


	public float speed = 10f;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, 5f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            Debug.Log("击中");
            if (collider.gameObject.GetComponent<Player>() != null)
            {
                collider.gameObject.GetComponent<Player>().Hurt(4);
            }
        }
        else if (collider.gameObject.tag == "Ground" || collider.gameObject.tag == "PlayerAI")
        {
            Destroy(gameObject);
        }
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Destroy(gameObject);
    //        Debug.Log("击中");
    //    }
    //}
}
