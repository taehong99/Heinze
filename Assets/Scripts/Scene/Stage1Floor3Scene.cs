using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Floor3Scene : BaseStageScene
{
    public override IEnumerator LoadingRoutine()
    {
        Manager.Sound.PlayBGM(Manager.Sound.AudioClips.floor3BGM);
        Manager.Game.SpawnRooms(Stage.Boss);
        Manager.Game.CreatePools();
        yield return null;
    }
}
