using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public Dictionary<string, VoidEventChannelSO> voidEventDic = new Dictionary<string, VoidEventChannelSO>();
    public Dictionary<string, DirectionEventChannelSO> dirEventDic = new Dictionary<string, DirectionEventChannelSO>();

    private void Start()
    {
        // Monster Spawn Events
        voidEventDic.Add("enemySpawned", Manager.Resource.Load<VoidEventChannelSO>("Events/EnemySpawnedEvent"));
        voidEventDic.Add("enemyDied", Manager.Resource.Load<VoidEventChannelSO>("Events/EnemyDiedEvent"));

        // Room Transition Events
        dirEventDic.Add("movedRoom", Manager.Resource.Load<DirectionEventChannelSO>("Events/MovedRoomEvent"));
    }

    private void OnDestroy()
    {
        foreach (var entry in voidEventDic)
        {
            entry.Value.OnEventRaised = null;
        }
        foreach (var entry in dirEventDic)
        {
            entry.Value.OnEventRaised = null;
        }
    }
}