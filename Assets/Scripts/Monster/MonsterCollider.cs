using UnityEngine;

public class MonsterCollider : MonoBehaviour
{
    [SerializeField] Collider sightCollider;
    [SerializeField] Collider deathCollider;

    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("MonsterDeath"))
        {
            Destroy(gameObject);
        }
    }
}
