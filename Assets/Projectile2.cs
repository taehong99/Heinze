using UnityEngine;

public class Projectile2 : MonoBehaviour
{
    public float speed = 10f; // ����ü �ӵ�
    public float damage = 10f; // ����ü ����

    private Transform target; // ������ ��� 

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // ����� ������ ����ü ����
            return;
        }

        // ��� �������� �̵�
        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        if (direction.magnitude <= distanceThisFrame)
        {
            // ��� �������� �� ���ظ� ������ ����ü ����
            HitTarget();
            return;
        }
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);

        // ����ü �������� ȸ��
        transform.LookAt(target);
    }

    void HitTarget()
    {
        // ��󿡰� ���ظ� ������ ����ü ����
        // ���� ���, ����� ���� ��� ���� ü���� ���ҽ�Ű�� ���� �۾��� ������ �� ����
        // ���⼭�� ���÷� Debug.Log�� ���ط��� �����
        Debug.Log("����ü�� ��󿡰� " + damage + "�� ���ظ� �������ϴ�.");
        Destroy(gameObject);
    }
}
