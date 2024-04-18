using UnityEngine;

public class ParticleProjectile : MonoBehaviour
{
    public class ParticleTracker : MonoBehaviour
    {
        public Transform player; // 플레이어의 Transform 컴포넌트
        private ParticleSystem particleSystem; // 파티클 시스템 컴포넌트

        void Start()
        {
            // 파티클 시스템 컴포넌트를 가져옴
            particleSystem = GetComponent<ParticleSystem>();
        }

        void Update()
        {
            if (player != null && particleSystem != null)
            {
                // 플레이어를 향하는 방향 벡터 계산
                Vector3 direction = (player.position - transform.position).normalized;

                // 방향 벡터를 회전값으로 변환하여 파티클 시스템의 방향을 설정
                Quaternion rotation = Quaternion.LookRotation(direction);
                particleSystem.transform.rotation = rotation;
            }
        }
    }
}

