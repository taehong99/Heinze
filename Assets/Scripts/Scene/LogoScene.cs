using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoScene : BaseScene
{
    private void Start()
    {
        StartCoroutine(LoadingRoutine());
    }
    public override IEnumerator LoadingRoutine()
    {
        yield return new WaitForSeconds(1);
        Manager.Scene.LoadScene("1.TitleScene");
    }
}
