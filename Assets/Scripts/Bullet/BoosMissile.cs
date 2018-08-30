using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosMissile : MonoBehaviour {

	protected float speed;
	public Collider[] playerColliders;
	protected GameObject player;
	protected bool isRotate = true;
    protected bool isGetPlayer = false;//是否获取到玩家

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 6f);
		speed = Random.Range (70, 100);
		StartCoroutine ("Patrol");
		StartCoroutine ("CloseRotate");
	}

	/// <summary>
	/// 收集附近所有玩家信息并随机选择一个玩家
	/// </summary>
	IEnumerator Patrol()
	{
		yield return new WaitForSeconds(1.0f);
		playerColliders = Physics.OverlapSphere(transform.position, 100f, 1 << 9 | 1 << 8);
       // Debug.Log("附近玩家：" + playerColliders.Length);
		if (playerColliders.Length > 0)
		{
			int playerid = Random.Range (0, playerColliders.Length);
			for (int i = 0; i < playerColliders.Length; i++)
			{
                if (playerColliders[i].GetComponent <PlayerBase>() != null && i == playerid)
				{
					player = playerColliders[i].gameObject;
                    isGetPlayer = true;

				}
			}
		}
	}


	/// <summary>
	/// 停止对玩家进行追踪
	/// </summary>
	/// <returns>The rotate.</returns>
	IEnumerator CloseRotate()
	{
		yield return new WaitForSeconds(2.0f);
        if (player != null)
        {
            //停止追踪时在玩家脚下生成导弹落地特效
            Vector3 playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + 0.2f, player.transform.position.z);
            GameObject AoeGameobject = (GameObject)Instantiate(Resources.Load("Prefab/Aoe/Prefab/Aoe"), playerPosition, Quaternion.identity);

        }
        isRotate = false;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (isGetPlayer)
        {
            //转向玩家的位置
            if (player.GetComponent<PlayerBase>() != null && isRotate == true)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), 0.2f);
            }
        }
		
	}

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            GetComponent<IExploretion>().Exploretion();
            Destroy(gameObject);
           
            if (collider.gameObject.GetComponent<PlayerBase>() != null)
            {
                collider.gameObject.GetComponent<PlayerBase>().Hurt(4);
            }
        }
        else if (collider.gameObject.tag == "Ground" || collider.gameObject.tag == "PlayerAI")
        {
            GetComponent<IExploretion>().Exploretion();
            Destroy(gameObject);
          
        }
    }
}
