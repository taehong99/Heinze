using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] ParticleSystem PierceEffect1;
    [SerializeField] ParticleSystem PierceEffect2;
    [SerializeField] ParticleSystem SlashEffect;
    [SerializeField] ParticleSystem SlamEffect;

    private PooledObject Pierce1;
    private PooledObject Slash;
    private PooledObject Slam;
    private PooledObject Skill1;
    private PooledObject Skill2;
    private PooledObject Skill3;

    private void Start()
    {
        Pierce1 = Manager.Resource.Load<PooledObject>("Effects/WarriorPierce1");
        Slash = Manager.Resource.Load<PooledObject>("Effects/WarriorSlash");
        Slam = Manager.Resource.Load<PooledObject>("Effects/WarriorSlam");
        Skill1 = Manager.Resource.Load<PooledObject>("Effects/WarriorSkill1");
        Skill2 = Manager.Resource.Load<PooledObject>("Effects/WarriorSkill2");
        Skill3 = Manager.Resource.Load<PooledObject>("Effects/WarriorSkill3");
    }

    public void PlayEffect(string effect)
    {
        Vector3 playerTorsoOffset = Vector3.up * 0.8f;
        switch (effect)
        {
            case "Pierce1":
                Manager.Pool.GetPool(Pierce1, transform.position + playerTorsoOffset, transform.rotation);
                break;
            case "Pierce2":
                PierceEffect2.Play(true);
                break;
            case "Slash":
                Manager.Pool.GetPool(Slash, transform.position + playerTorsoOffset, transform.rotation);
                break;
            case "Slam":
                Manager.Pool.GetPool(Slam, transform.position, transform.rotation);
                break;
            case "Skill1":
                Manager.Pool.GetPool(Skill1, transform.position, transform.rotation);
                break;
            case "Skill2":
                Manager.Pool.GetPool(Skill2, transform.position, transform.rotation);
                break;
            case "Skill3":
                Manager.Pool.GetPool(Skill3, transform.position + playerTorsoOffset, transform.rotation);
                break;
            default:
                break;
        }
    }
}
