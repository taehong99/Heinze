using UnityEngine;

public class ParticleProjectile : MonoBehaviour
{
    public class ParticleTracker : MonoBehaviour
    {
        public Transform player; // �÷��̾��� Transform ������Ʈ
        private ParticleSystem particleSystem; // ��ƼŬ �ý��� ������Ʈ

        void Start()
        {
            // ��ƼŬ �ý��� ������Ʈ�� ������
            particleSystem = GetComponent<ParticleSystem>();
        }

        void Update()
        {
            if (player != null && particleSystem != null)
            {
                // �÷��̾ ���ϴ� ���� ���� ���
                Vector3 direction = (player.position - transform.position).normalized;

                // ���� ���͸� ȸ�������� ��ȯ�Ͽ� ��ƼŬ �ý����� ������ ����
                Quaternion rotation = Quaternion.LookRotation(direction);
                particleSystem.transform.rotation = rotation;
            }
        }
    }
}

