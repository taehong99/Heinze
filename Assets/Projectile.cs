using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target; // �÷��̾ �����ϱ� ���� ��ǥ
    private float projectileSpeed = 10f; // �߻�ü �ӵ�

    // ��ǥ ����
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target != null)
        {
            // ��ǥ �������� �߻�ü �̵�
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * projectileSpeed * Time.deltaTime;

            // �߻�ü�� ��ǥ�� �����ϸ� �ı�
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // ��ǥ�� ������ �߻�ü �ı�
            Destroy(gameObject);
        }
    }
}
