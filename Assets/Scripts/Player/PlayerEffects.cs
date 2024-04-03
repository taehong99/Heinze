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

    private void Start()
    {
        Pierce1 = Manager.Resource.Load<PooledObject>("Effects/WarriorPierce1");
        Slash = Manager.Resource.Load<PooledObject>("Effects/WarriorSlash");
        Slam = Manager.Resource.Load<PooledObject>("Effects/WarriorSlam");
    }

    public void PlayEffect(string effect)
    {
        switch (effect)
        {
            case "Pierce1":
                Debug.Log("Pricked");
                Manager.Pool.GetPool(Pierce1, transform.position + Vector3.up * 0.8f, transform.rotation);
                break;
            case "Pierce2":
                PierceEffect2.Play(true);
                break;
            case "Slash":
                Manager.Pool.GetPool(Slash, transform.position + Vector3.up * 0.8f, transform.rotation);
                break;
            case "Slam":
                Manager.Pool.GetPool(Slam, transform.position, transform.rotation);
                break;
            default:
                break;
        }
    }
}
