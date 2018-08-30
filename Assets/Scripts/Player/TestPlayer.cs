using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour {

    RaycastHit hit;
    public float speed;
    public float rotateSpeed;
    public Transform camera;
    public CharacterController cc;

    void Start () {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Shoot();
        }
        Move();
    }

    void Shoot()
    {
        if (Physics.Raycast(camera.position, camera.forward, out hit, 100))
        {
            if (hit.transform.tag == "enemy")
            {
                if (hit.transform.parent.GetComponent<Enemy>().target == null)
                {
                    hit.transform.parent.GetComponent<Enemy>().target = transform;
                }
                Debug.Log("敌人扣血");
            }
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = transform.forward * v;
        dir += transform.right * h;
        cc.Move(dir*Time.deltaTime*speed);
        cc.Move(Vector3.down * 9.8f * Time.deltaTime);
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        transform.Rotate(Vector3.up * x * Time.deltaTime * rotateSpeed);
        camera.Rotate(Vector3.right * -y * Time.deltaTime * rotateSpeed);
    }

}
