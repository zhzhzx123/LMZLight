using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitBattleUI : WindowBase
{
    private Button quitButton;
    private Button backButton;
    private Button returnButton;

    public override void Init()
    {
        base.Init();
        quitButton = transform.Find("Quit").GetComponent<Button>();
        backButton = GetComponent<Button>();
        returnButton = transform.Find("Return").GetComponent<Button>();

        quitButton.onClick.AddListener(ClickQuit);
        backButton.onClick.AddListener(ClickBack);
        returnButton.onClick.AddListener(ClickReturn);
    }

    public override void Open()
    {
        base.Open();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    void ClickQuit()
    {
        LoadSceneManager._Instance.LoadScene(SceneName.WaitRoom);
    }

    void ClickBack()
    {

        UIManager._Instance.CloseWindow(WindowName.QuitBattle);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void ClickReturn()
    {
        UIManager._Instance.CloseWindow(WindowName.QuitBattle);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
