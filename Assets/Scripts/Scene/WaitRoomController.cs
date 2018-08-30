using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitRoomController : MonoBehaviour {
	
	void Awake () {
        UIManager._Instance.OpenWindow(WindowName.ReviceRoom);
    }

    private void OnDestroy()
    {
        UIManager._Instance.UnloadWindow(WindowName.Room);
        UIManager._Instance.UnloadWindow(WindowName.ReviceRoom);
        UIManager._Instance.UnloadWindow(WindowName.WaitStartRoom);
    }
}
