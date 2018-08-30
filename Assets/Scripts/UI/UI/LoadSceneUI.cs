using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneUI : WindowBase {

    private Slider slider;
    private Text valueText;

    public override void Init()
    {
        base.Init();
        slider = transform.Find("Slider").GetComponent<Slider>();
        valueText = transform.Find("Slider/Hand/Value").GetComponent<Text>();
    }
    public override void Open()
    {
        base.Open();
        UpdatePanel(0);
        LoadSceneManager._Instance.AddLoadingCallBack(UpdatePanel);
    }

    public override void Close()
    {
        base.Close();
        LoadSceneManager._Instance.RemoveLoadingCallBack(UpdatePanel);
    }

    void UpdatePanel(float value)
    {
        slider.normalizedValue = value;
        string str1 = String.Format("{0:F}", value * 100);//默认为保留两位
        valueText.text = str1 + "%";
    }
}
