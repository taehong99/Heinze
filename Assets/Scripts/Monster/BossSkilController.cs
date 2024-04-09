using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkilController : MonoBehaviour
{
    BossMonster1 bossMonster1;
    void Start()
    {
        bossMonster1 = GetComponent<BossMonster1>();
    }

    // Update is called once per frame
    void Update()
    {
        //bossMonster1.StartSkillCoroutine();      
    }
}
