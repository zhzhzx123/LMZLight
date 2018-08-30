using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShootPoint : MonoBehaviour {
    
	public PlayerBase player;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (player!=null) {
			transform.LookAt (player.transform.Find("Point").position);
		}
	}
}
