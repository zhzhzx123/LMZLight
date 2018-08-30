using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanelUI : WindowBase {

    private Button closeButton;

    private Image gun1Image;
    private Image gun2Image;

    public int gun1;
    public int gun2;
    public List<GameObject> equipmentBarList = new List<GameObject>();
    public List<Image> weaponsList = new List<Image>();

    private void Update()
    {
        CheckWeapons();
    }
    /// <summary>
    /// 检测装备栏武器是否使用
    /// </summary>
    void CheckWeapons()
    {
        for (int i = 0; i < equipmentBarList.Count; i++)
        {

            if (equipmentBarList[i].transform.Find("GunIcon") != null)
            {
                //当装备栏里遍历到的武器image的sprite名字与武器栏武器名字一样时执行
                if (equipmentBarList[i].transform.Find("GunIcon").GetComponent<Image>().sprite.name.Equals(weaponsList[0].sprite.name) ||
                    equipmentBarList[i].transform.Find("GunIcon").GetComponent<Image>().sprite.name.Equals(weaponsList[1].sprite.name))
                {
                    equipmentBarList[i].GetComponent<GridUi>().GunIcon1.SetActive(false);
                    equipmentBarList[i].transform.GetComponentInParent<GridUi>().Locked.SetActive(true);
                }
                else
                {
                    equipmentBarList[i].GetComponent<GridUi>().GunIcon1.SetActive(true);
                    equipmentBarList[i].transform.GetComponentInParent<GridUi>().Locked.SetActive(false);
                }
            }

        }
    }

    public override void Init()
    {
        base.Init();

        gun1Image = transform.Find("Right/TempParent/MainWeapon/GunIcon").GetComponent<Image>();
        gun2Image = transform.Find("Right/TempParent/DeputyWeapon/GunIcon").GetComponent<Image>();

        closeButton = transform.Find("Right/Exit").GetComponent<Button>();
        closeButton.onClick.AddListener(ClickClose);
    }

    public override void Open()
    {

        EventCenterManager._Instance.AddListener(EventType.SendPlayerInfo, SendPlayerInfoCallBack);
        EventCenterManager._Instance.SendMessage(EventType.GetPlayerInfo);
        base.Open();

    }

    public override void Close()
    {
        if (gameObject.activeSelf)
        {
            EventCenterManager._Instance.RemoveListener(EventType.SendPlayerInfo, SendPlayerInfoCallBack);
        }
        base.Close();

    }


    void SendPlayerInfoCallBack(params object[] obj_Arr)
    {
        gun1 = (int)obj_Arr[0];
        gun2 = (int)obj_Arr[1];
    }


    void ClickClose()
    {
        gun1 = int.Parse( gun1Image.sprite.name.Replace("Gun", ""));
        gun2 = int.Parse( gun2Image.sprite.name.Replace("Gun", ""));
        Debug.Log("gun1: " + gun1 + " --- gun2: " + gun2);
        EventCenterManager._Instance.SendMessage(EventType.SetPlayerInfo, gun1, gun2);
        UIManager._Instance.CloseWindow(WindowName.BagPanel);
    }

}
