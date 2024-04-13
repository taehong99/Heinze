using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour
{
    [SerializeField] PlayerSkillDataSO[] skills = new PlayerSkillDataSO[4]; 

    // Start is called before the first frame update
    void Start()
    {
        Manager.Game.CreatePools();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            foreach(var skill in skills)
            {
                Manager.Game.AnnounceSkillPicked(skill);
            }
        }
    }
}
