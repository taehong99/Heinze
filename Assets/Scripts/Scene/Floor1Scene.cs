using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor1Scene : BaseScene
{
    bool transitioned;

    private void Start()
    {
        if (!transitioned)
        {
            //Manager.Game.CreatePools();
        }
    }

    public override IEnumerator LoadingRoutine()
    {
        Manager.Game.CreatePools();
        transitioned = true;
        yield return null;
    }
}
