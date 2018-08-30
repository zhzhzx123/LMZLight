using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class BattletClientStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
        NetworkManager._Instance.StartBattleClient();
	}
	
	// Update is called once per frame
	void OnDestroy () {
        NetworkManager._Instance.CloseBattleClient();
    }
}
