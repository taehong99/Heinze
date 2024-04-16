using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    private void Start()
    {
        Manager.Sound.PlayBGM(Manager.Sound.AudioClips.lobbyBGM);
        Manager.Game.RestartGame();
    }

    public void StartGame()
    {
        Manager.Sound.PlaySFX(Manager.Sound.AudioClips.clickSFX);
        Manager.Scene.LoadScene("2.JobSelectScene");
    }

    public void ExitGame()
    {
        Manager.Sound.PlaySFX(Manager.Sound.AudioClips.clickSFX);
        Application.Quit();
    }

    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
}
