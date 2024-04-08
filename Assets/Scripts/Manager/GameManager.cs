using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject warriorPrefab;
    private PlayerSkillDataSO[] skillSlots = new PlayerSkillDataSO[4];

    public void Test()
    {
        Debug.Log(GetInstanceID());
    }

    public void UpdateSkillSlot(int idx, PlayerSkillDataSO skillData)
    {
        skillSlots[idx] = skillData;
    }

    public int GetSkillIDInSlot(int idx)
    {
        return skillSlots[idx].skillID;
    }

    public void CreatePools()
    {
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorPierce1"), 2, 4);
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorSlash"), 2, 4);
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorSlam"), 2, 4);
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill1"), 2, 4);
        Manager.Pool.CreatePool(Manager.Resource.Load<PooledObject>("Effects/WarriorSkill2"), 2, 4);
    }


    void Start()
    {

    }
}
