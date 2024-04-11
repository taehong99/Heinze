using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSelectScene : BaseScene
{
    public void ChooseWarrior()
    {
        Manager.Player.ChooseJob(PlayerJob.Warrior);
        Manager.Scene.LoadScene("3.GenderSelectScene");
    }

    public void ChooseArcher()
    {

    }

    public void ChooseMagician()
    {

    }

    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
}
