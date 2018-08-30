using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartridgeBox : MonoBehaviour {

    public RectTransform button;
    private Player player;

    private void Start()
    {
        button.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (player!=null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                foreach (GunBase item in player.guns)
                {
                    item.data.CarryCap = item.data.MaxCarryCap;
                    EventCenterManager._Instance.SendMessage(EventType.UpdateBattleGunUI, item.data.ID, item.data.Cartridge, item.data.CarryCap);
                }

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag=="Player")
        {
            player = other.gameObject.GetComponent<Player>();
            button.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        player = null;
        button.gameObject.SetActive(false);
    }
}