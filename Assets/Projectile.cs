using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target; // 플레이어를 추적하기 위한 목표
    private float projectileSpeed = 10f; // 발사체 속도

    // 목표 설정
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target != null)
        {
            // 목표 방향으로 발사체 이동
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * projectileSpeed * Time.deltaTime;

            // 발사체가 목표에 도달하면 파괴
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // 목표가 없으면 발사체 파괴
            Destroy(gameObject);
        }
    }
}
