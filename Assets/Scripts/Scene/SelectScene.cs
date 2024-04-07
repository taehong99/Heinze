using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectScene : BaseScene
{
    public void ChooseWarrior()
    {
        Manager.Scene.LoadScene("Floor1");
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
