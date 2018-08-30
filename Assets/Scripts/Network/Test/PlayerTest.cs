using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class PlayerTest : MonoBehaviour {

    BattlePlayerClient client;

    public PlayerState state = PlayerState.Idle;

    public float invert = 0.2f;

    public float count = 0f;

	// Use this for initialization
	void Start () {
        client = new BattlePlayerClient();
    }
	
	// Update is called once per frame
	void Update () {
        count += Time.deltaTime;
        if(count >= invert)
        {
            //同步位置
            client.SendTranfromToServer(transform, 0);
            count -= invert;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Mathf.Abs(h) >= 0.01f || Mathf.Abs(v) >= 0.01f)
        {
            if (Input.GetKey(KeyCode.LeftShift) && v > 0)
            {
                SwitchState(PlayerState.Run);
                transform.Translate(new Vector3(h, 0, v) * 5 * Time.deltaTime);
            }
            else
            {
                SwitchState(PlayerState.Walk);
                transform.Translate(new Vector3(h, 0, v) * 2 * Time.deltaTime);
            }
        }
        else
        {
            SwitchState(PlayerState.Idle);
        }
    }


    void SwitchState(PlayerState state)
    {
        if (this.state == state)
        {
            return;
        }
        this.state = state;
        //client.SendPlayerStateToServer((int)state);
    }




}
