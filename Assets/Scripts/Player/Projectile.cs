using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target; // �÷��̾� ������ ���� ��ǥ
    private float projectileSpeed = 10f; // �ӵ�

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target != null)
        {
            // ��ǥ ���� ���� ���� ���
            Vector3 direction = (target.position - transform.position).normalized;

            // ��ǥ �������� �̵�
            transform.position += direction * projectileSpeed * Time.deltaTime;

            // ��ǥ ������ �������� �� �ı�
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // ��ǥ�� ������ �ڽŵ� �ı�
            Destroy(gameObject);
        }
    }
}
