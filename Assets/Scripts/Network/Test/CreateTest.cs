using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnDestroy()
    {
        Network.NetworkManager._Instance.DestroyRoom();
    }

    public void CreateRoom()
    {
        Network.NetworkManager._Instance.CreateRoom();
    }

}
