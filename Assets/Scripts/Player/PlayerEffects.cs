using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    //[SerializeField] ParticleSystem PierceEffect1;
    //[SerializeField] ParticleSystem PierceEffect2;
    //[SerializeField] ParticleSystem SlashEffect;
    //[SerializeField] ParticleSystem SlamEffect;

    private PooledObject Pierce;
    private PooledObject Slash;
    private PooledObject Slam;
    private PooledObject Skill1;
    private PooledObject Skill2;
    private PooledObject Skill3;
    private PooledObject Skill4;
    private PooledObject Skill5;
    private PooledObject Skill6;

    private void Start()
    {
        Pierce = Manager.Resource.Load<PooledObject>("Effects/WarriorPierce1");
        Slash = Manager.Resource.Load<PooledObject>("Effects/WarriorSlash");
        Slam = Manager.Resource.Load<PooledObject>("Effects/WarriorSlam");
        Skill1 = Manager.Resource.Load<PooledObject>("Effects/WarriorSkill1");
        Skill2 = Manager.Resource.Load<PooledObject>("Effects/WarriorSkill2");
        Skill3 = Manager.Resource.Load<PooledObject>("Effects/WarriorSkill3");
        Skill4 = Manager.Resource.Load<PooledObject>("Effects/WarriorSkill4");
        Skill5 = Manager.Resource.Load<PooledObject>("Effects/WarriorSkill5");
        Skill6 = Manager.Resource.Load<PooledObject>("Effects/WarriorSkill6");
    }

    public void PlayEffect(string effect)
    {
        Vector3 playerTorsoOffset = Vector3.up * 0.8f;
        switch (effect)
        {
            case "Pierce":
                Manager.Pool.GetPool(Pierce, transform.position + playerTorsoOffset, transform.rotation);
                break;
            case "Slash":
                Manager.Pool.GetPool(Slash, transform.position + playerTorsoOffset, transform.rotation);
                break;
            case "Slam":
                Manager.Pool.GetPool(Slam, transform.position, transform.rotation);
                break;
            case "Skill1":
                Manager.Pool.GetPool(Skill1, transform.position + playerTorsoOffset, transform.rotation);
                break;
            case "Skill2":
                Manager.Pool.GetPool(Skill2, transform.position + transform.forward * 7f, transform.rotation);
                break;
            case "Skill3":
                Manager.Pool.GetPool(Skill3, transform.position + playerTorsoOffset, Quaternion.LookRotation(transform.forward));
                break;
            case "Skill4":
                Vector3 spawnPos;
                Quaternion swordRotation = Quaternion.Euler(-90, 0, 0);
                for (int i = 0; i < 5; i++)
                {
                    float xOffset = Random.Range(-5, 5);
                    float zOffset = Random.Range(-5, 5);
                    spawnPos = new Vector3(transform.position.x + xOffset, 8, transform.position.z + zOffset);
                    Manager.Pool.GetPool(Skill4, spawnPos, swordRotation);
                }
                break;
            case "Skill5":
                Manager.Pool.GetPool(Skill5, transform.position, transform.rotation);
                break;
            case "Skill6":
                Manager.Pool.GetPool(Skill6, transform.position, transform.rotation);
                break;
            default:
                break;
        }
    }
}
