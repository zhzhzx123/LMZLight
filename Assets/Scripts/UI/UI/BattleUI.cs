using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;

public class BattleUI : WindowBase {

    //private Button button;
    private CanvasGroup group;
    private Animator hurtTipAnim;

    private GameObject playerPrefab;
    private GameObject playerParent;

    private GameObject boss;

    private Dictionary<string, BattlePlayerUI> playerDic = new Dictionary<string, BattlePlayerUI>();

    private Image gun1Image;
    private Image gun2Image;
    private Text gun1Text;
    private Text gun2Text;
    private Image hpImage;

    private GameObject gun1Use;
    private GameObject gun2Use;

    private Dictionary<int, Image> gunIm = new Dictionary<int, Image>();
    private Dictionary<int, Text> gunTxt = new Dictionary<int, Text>();

    public override void Init()
    {
        base.Init();
        group = GetComponent<CanvasGroup>();
        // button = transform.Find("Button").GetComponent<Button>();
        //button.onClick.AddListener(CloseBattle);
        hurtTipAnim = transform.Find("HurtTip").GetComponent<Animator>();
        playerPrefab = transform.Find("HpGroup/Friend").gameObject;
        playerParent = transform.Find("HpGroup/PlayerGroup").gameObject;
        boss = transform.Find("HpGroup/Boss").gameObject;
        boss.SetActive(false);
        playerPrefab.SetActive(false);
        gun1Image = transform.Find("Bottom/WeaponGroup/LeftWeapon/Image/Weapon").GetComponent<Image>();
        gun2Image = transform.Find("Bottom/WeaponGroup/RightWeapon/Image/Weapon").GetComponent<Image>();
        gun1Text = transform.Find("Bottom/WeaponGroup/LeftWeapon/Text").GetComponent<Text>();
        gun2Text = transform.Find("Bottom/WeaponGroup/RightWeapon/Text").GetComponent<Text>();
        hpImage = transform.Find("Bottom/HP/HpRed").GetComponent<Image>();
        gun1Use = transform.Find("Bottom/WeaponGroup/LeftWeapon/Image/Use").gameObject;
        gun2Use = transform.Find("Bottom/WeaponGroup/RightWeapon/Image/Use").gameObject;
    }


    public override void Open()
    {
        gunTxt.Clear();
        gunIm.Clear();
        base.Open();
        foreach (var item in playerDic.Keys)
        {
            Destroy(playerDic[item]);
        }
        playerDic.Clear();
        foreach (var item in RoomSingle.GetInfos().Keys)
        {
            if (NetworkTools.GetLocalIP().Equals(item))
            {
                continue;//忽略自己
            }
            GameObject obj = Instantiate<GameObject>(playerPrefab, playerParent.transform);
            obj.SetActive(true);
            obj.transform.localScale = Vector3.one;
            BattlePlayerUI bpui = obj.GetComponent<BattlePlayerUI>();
            bpui.UpdatePanel(item, 1, 1);//初始让其满血
            playerDic.Add(item, bpui);
        }

        gunIm.Add(RoomSingle.GetInfos()[NetworkTools.GetLocalIP()].gun1ID, gun1Image);
        gunIm.Add(RoomSingle.GetInfos()[NetworkTools.GetLocalIP()].gun2ID, gun2Image);
        gunTxt.Add(RoomSingle.GetInfos()[NetworkTools.GetLocalIP()].gun1ID, gun1Text);
        gunTxt.Add(RoomSingle.GetInfos()[NetworkTools.GetLocalIP()].gun2ID, gun2Text);

        gun1Image.sprite = Resources.Load<Sprite>("UI/WeaponIcon/Gun" + RoomSingle.GetInfos()[NetworkTools.GetLocalIP()].gun1ID);
        gun2Image.sprite = Resources.Load<Sprite>("UI/WeaponIcon/Gun" + RoomSingle.GetInfos()[NetworkTools.GetLocalIP()].gun2ID);

        GunRow g1Row = GunModel_S.Instance.GetGunInfo(RoomSingle.GetInfos()[NetworkTools.GetLocalIP()].gun1ID);
        UpdateGunMessage(RoomSingle.GetInfos()[NetworkTools.GetLocalIP()].gun1ID, g1Row.Cartridge, g1Row.CarryCap);
        GunRow g2Row = GunModel_S.Instance.GetGunInfo(RoomSingle.GetInfos()[NetworkTools.GetLocalIP()].gun2ID);
        UpdateGunMessage(RoomSingle.GetInfos()[NetworkTools.GetLocalIP()].gun2ID, g2Row.Cartridge, g2Row.CarryCap);

        hpImage.fillAmount = 1;
        gun1Use.SetActive(true);
        gun2Use.SetActive(false);
        EventCenterManager._Instance.AddListener(EventType.UpdatePlayerUI, UpdatePlayerMsgCallBack);
        EventCenterManager._Instance.AddListener(EventType.UpdateBattleGunUI, UpdateGunMsgCallBack);
        EventCenterManager._Instance.AddListener(EventType.SwitchGun, SwitchGunCallBack);
        EventCenterManager._Instance.AddListener(EventType.OpenAimed, OpenAniedCallBack);
    }

    public override void Close()
    {
        base.Close();
        EventCenterManager._Instance.RemoveListener(EventType.UpdatePlayerUI, UpdatePlayerMsgCallBack);
        EventCenterManager._Instance.RemoveListener(EventType.UpdateBattleGunUI, UpdateGunMsgCallBack);
        EventCenterManager._Instance.RemoveListener(EventType.SwitchGun, SwitchGunCallBack);
        EventCenterManager._Instance.RemoveListener(EventType.OpenAimed, OpenAniedCallBack);
    }

    void UpdatePlayerMsgCallBack(params object[] obj_arr)
    {
        string ip = (string)obj_arr[0];
        float max = (float)obj_arr[1];
        float current = (float)obj_arr[2];
        UpdatePlayerMessage(ip, max, current);
    }

    void UpdatePlayerMessage(string ip, float max, float current)
    {
        if (NetworkTools.GetLocalIP().Equals(ip))
        {
            float temp = current / max;
            if (temp < hpImage.fillAmount)
            {
                hurtTipAnim.SetTrigger("Hurt");
            }
            hpImage.fillAmount = temp;
            return;
        }

        if (playerDic.ContainsKey(ip))
        {
            playerDic[ip].UpdatePanel(ip, max, current);
        }
    }


    void UpdateGunMsgCallBack(params object[] obj_arr)
    {
        int id = (int)obj_arr[0];
        int current = (int)obj_arr[1];
        int all = (int)obj_arr[2];
        
        UpdateGunMessage(id, current, all);
    }

    void UpdateGunMessage(int id, int current, int all)
    {
       
        if (gunTxt.ContainsKey(id))
        {
            string text = "";
            text += current <= 0 ? "<color=red>" + current.ToString() + "</color>": current.ToString();
            text += "/";
            text += all <= 0 ? "<color=red>" + all.ToString() + "</color>" : all.ToString();
            gunTxt[id].text = text;
        }
    }

    void SwitchGunCallBack(params object[] obj_arr)
    {
        int id = (int)obj_arr[0];
        if (id == RoomSingle.GetInfos()[NetworkTools.GetLocalIP()].gun1ID)
        {
            gun1Use.SetActive(true);
            gun2Use.SetActive(false);
        }
        else if (id == RoomSingle.GetInfos()[NetworkTools.GetLocalIP()].gun2ID)
        {
            gun1Use.SetActive(false);
            gun2Use.SetActive(true);
        }
    }


    void OpenAniedCallBack(params object[] obj_arr)
    {
        bool isOpen = (bool)obj_arr[0];
        group.alpha = isOpen ? 0 : 1;
    }



    /*
    void CloseBattle()
    {
        //LoadSceneManager._Instance.LoadScene(SceneName.WaitRoom);
    }
    */

}
