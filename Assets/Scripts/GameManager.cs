using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class GameManager : MonoBehaviour {

    IManager network;

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(gameObject);

        LoadSceneManager._Instance.LoadScene(SceneName.WaitRoom, false);
        network = NetworkManager._Instance as IManager;

    }

    private void Start()
    {
        network.Enter();
    }

    // Update is called once per frame
    void Update () {
        network.Update();
    }

    private void OnDestroy()
    {
        network.Exit();
    }
}
