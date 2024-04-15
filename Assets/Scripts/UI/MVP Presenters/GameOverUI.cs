using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : PopUpUI
{
    public void Restart()
    {
        Manager.UI.ClosePopUpUI();
        Manager.Scene.LoadScene("1.TitleScene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
