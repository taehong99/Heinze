using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gender { Male, Female }
public class GenderSelectScene : BaseScene
{
    bool chosen;

    public void ChooseMale()
    {
        if (!chosen)
        {
            Manager.Sound.PlaySFX(Manager.Sound.AudioClips.clickSFX);
            Manager.Game.SelectGender(Gender.Male);
            Manager.Scene.LoadScene("Stage1-1");
            chosen = true;
        }
    }

    public void ChooseFemale()
    {
        if (!chosen)
        {
            Manager.Sound.PlaySFX(Manager.Sound.AudioClips.clickSFX);
            Manager.Game.SelectGender(Gender.Female);
            Manager.Scene.LoadScene("Stage1-1");
            chosen = true;
        }
    }

    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
}
