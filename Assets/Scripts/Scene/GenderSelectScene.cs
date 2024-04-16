using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenderSelectScene : BaseScene
{
    bool chosen;

    public void ChooseMale()
    {
        if (!chosen)
        {
            Manager.Sound.PlaySFX(Manager.Sound.AudioClips.clickSFX);
            Manager.Scene.LoadScene("Stage1-1");
            chosen = true;
        }
    }

    public void ChooseFemale()
    {

    }

    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
}
