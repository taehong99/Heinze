using UnityEngine;

public class Projectile3 : MonoBehaviour
{
    private Vector3 direction; // ����ü ����
    public float speed = 10f; // ����ü �ӵ�
    public GameObject explosionEffectPrefab;

    // ���� ���� �޼���
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = direction * speed;
    }

    // ����ü�� �ٸ� ��ü���� �浹 ó��
    void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� ������ �±׸� ������ �ִ��� Ȯ��
        if (other.CompareTag("Obstacle") || other.CompareTag("Player"))
        {
            // �浹 ����Ʈ ����
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

            // ����ü �ı�
            Destroy(gameObject);
        }
    }
}
