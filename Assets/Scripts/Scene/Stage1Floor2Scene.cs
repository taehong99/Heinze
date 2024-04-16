using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Floor2Scene : BaseScene
{
    public override IEnumerator LoadingRoutine()
    {
        Manager.Sound.PlayBGM(Manager.Sound.AudioClips.floor2BGM);
        Manager.Game.SpawnRooms(Stage.MidBoss);
        Manager.Game.CreatePools();
        yield return null;
    }
}
