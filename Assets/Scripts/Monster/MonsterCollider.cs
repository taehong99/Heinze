using UnityEngine;

public class MonsterCollider : MonoBehaviour
{
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
