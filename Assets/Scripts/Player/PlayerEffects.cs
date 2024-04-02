using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] ParticleSystem PierceEffect1;
    [SerializeField] ParticleSystem PierceEffect2;
    [SerializeField] ParticleSystem SlashEffect;
    [SerializeField] ParticleSystem SlamEffect;

    private void Start()
    {
        PierceEffect1.Stop();
        PierceEffect2.Stop();
        SlashEffect.Stop();
        SlamEffect.Stop();
    }

    public void PlayEffect(string effect)
    {
        switch (effect)
        {
            case "Pierce1":
                Debug.Log("Pricked");
                PierceEffect1.Play(true);
                break;
            case "Pierce2":
                PierceEffect2.Play(true);
                break;
            case "Slash":
                SlashEffect.Play(true);
                break;
            case "Slam":
                SlamEffect.Play(true);
                break;
            default:
                break;
        }
    }
}
