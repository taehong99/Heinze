using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target; // 플레이어 추적을 위한 목표
    private float projectileSpeed = 10f; // 속도

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target != null)
        {
            // 목표 지점 방향 벡터 계산
            Vector3 direction = (target.position - transform.position).normalized;

            // 목표 지점으로 이동
            transform.position += direction * projectileSpeed * Time.deltaTime;

            // 목표 지점에 도달했을 때 파괴
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // 목표가 없으면 자신도 파괴
            Destroy(gameObject);
        }
    }
}
