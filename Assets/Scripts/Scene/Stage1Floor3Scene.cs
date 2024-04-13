using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Floor3Scene : BaseScene
{
    public override IEnumerator LoadingRoutine()
    {
        Manager.Game.SpawnRooms(Stage.Boss);
        Manager.Game.CreatePools();
        yield return null;
    }
}
