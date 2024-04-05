using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target; // 플레이어 추적 위한 목표
    private float projectileSpeed = 10f; // 속도

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target != null)
        {

            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * projectileSpeed * Time.deltaTime;
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
