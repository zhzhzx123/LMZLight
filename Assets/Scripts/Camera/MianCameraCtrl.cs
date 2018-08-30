using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MianCameraCtrl : MonoBehaviour {

    public Transform playerHand;
    public Vector3 initLocalPosition;
    Vector3 dir;
    float rayDis;
    RaycastHit hit;

	// Use this for initialization
	void Start () {
        rayDis = (playerHand.position - transform.position).magnitude;
        initLocalPosition = transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
        dir = (transform.position - playerHand.position).normalized;

        if (Physics.Raycast(playerHand.transform.position, dir, out hit, rayDis, 1 << 11))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.localPosition = initLocalPosition;
        }   
        
	}
}
