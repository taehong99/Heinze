using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/BasicAttack", fileName = "BasicAttack")]
public class AttackSO : ScriptableObject
{
    public AnimatorOverrideController animatorOC;
    public ParticleSystem effect;
}
