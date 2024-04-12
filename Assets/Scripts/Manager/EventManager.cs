using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public Dictionary<string, VoidEventChannelSO> voidEventDic = new Dictionary<string, VoidEventChannelSO>();

    private void Start()
    {
        // boss events
        voidEventDic.Add("enemySpawned", Manager.Resource.Load<VoidEventChannelSO>("Events/EnemySpawnedEvent"));
        voidEventDic.Add("enemyDied", Manager.Resource.Load<VoidEventChannelSO>("Events/EnemyDiedEvent"));
    }

    private void OnDestroy()
    {
        foreach (var entry in voidEventDic)
        {
            entry.Value.OnEventRaised = null;
        }
    }
}