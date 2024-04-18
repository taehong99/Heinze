using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Floor1Scene : BaseStageScene
{
    public override IEnumerator LoadingRoutine()
    {
        Manager.Sound.PlayBGM(Manager.Sound.AudioClips.floor1BGM);
        Manager.Game.SpawnRooms(Stage.Normal);
        Manager.Game.CreatePools();
        Manager.Game.SpawnPlayer();
        yield return null;
    }
}
