using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour {

    public AudioSource boomVoice, onWallVoice;
    public bool isWall;
    public float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        boomVoice.volume = 2 - timer;
        onWallVoice.volume = 3.5f - timer;
        if (boomVoice.volume < 0)
        {
            boomVoice.volume = 0;
        }
        if (onWallVoice.volume < 0)
        {
            onWallVoice.volume = 0;
        }
    }
    // Use this for initialization
    void Start () {
        onWallVoice.volume = 0;
        Destroy(gameObject, 5f);
        boomVoice.Play();
        if (isWall)
        {
            Invoke("OnWallPlayVoice", 0.5f);
        }

	}
    void OnWallPlayVoice()
    {
        onWallVoice.Play();
    }

    public void SetBoomTarget(bool mybool)
    {
        isWall = mybool;
    }

}
