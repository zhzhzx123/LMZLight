using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePlayerUI : MonoBehaviour {

    private Text nameText;
    private Image hpImage;


    public void UpdatePanel(string ip, float max, float current)
    {
        if (nameText == null)
        {
            nameText = transform.Find("Name").GetComponent<Text>();
        }

        if (hpImage == null)
        {
            hpImage = transform.Find("health/Image").GetComponent<Image>();
        }
        nameText.text = ip;
        hpImage.fillAmount = current / max;
    }


}
