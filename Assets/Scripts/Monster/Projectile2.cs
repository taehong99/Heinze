using UnityEngine;

public class Projectile2 : MonoBehaviour
{
    public float speed = 10f; // 투사체 속도
    public float damage = 10f; // 투사체 피해

    private Transform target; // 추적할 대상 

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // 대상이 없으면 투사체 제거
            return;
        }

        // 대상 방향으로 이동
        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        if (direction.magnitude <= distanceThisFrame)
        {
            // 대상에 도달했을 때 피해를 입히고 투사체 제거
            HitTarget();
            return;
        }
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);

        // 투사체 방향으로 회전
        transform.LookAt(target);
    }

    void HitTarget()
    {
        // 대상에게 피해를 입히고 투사체 제거
        // 예를 들어, 대상이 적인 경우 적의 체력을 감소시키는 등의 작업을 수행할 수 있음
        // 여기서는 예시로 Debug.Log로 피해량을 출력함
        Debug.Log("투사체가 대상에게 " + damage + "의 피해를 입혔습니다.");
        Destroy(gameObject);
    }
}
