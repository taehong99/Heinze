using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RangedBomb : MonoBehaviour, IDamagable
{
    [SerializeField] int hp;
    [SerializeField] float lostDistance; // 목표와의 최대 거리
    [SerializeField] float attackCooldownTime = 2.0f; // 공격 쿨다운 시간 (예: 2초)
    float attackCoolDown = 0.0f; // 공격 쿨다운 초기값

    Transform target;
    NavMeshAgent nmAgent;
    Animator anim;
    private int currentHealth;
    public GameObject hudDamageText;
    public Transform hudPos;
    public Image healthBarImage;
    public GameObject effectPrefab;
    public GameObject minimapMarkerPrefab;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public int damage;
    public GameObject itemPrefab;

    enum State
    {
        IDLE,
        CHASE,
        ATTACK,
        KILLED,
        DAMAGED
    }

    State state;

    void UpdateHealthBar()
    {
        if (healthBarImage != null)
            healthBarImage.fillAmount = ((float)currentHealth) / hp;
    }
    void Start()
    {
        AddMinimapMarker();
        anim = GetComponent<Animator>();
        nmAgent = GetComponent<NavMeshAgent>();

        // 몬스터의 hp
        hp = 1;
        state = State.IDLE;
        currentHealth = hp;
        StartCoroutine(StateMachine());
    }

    private void AddMinimapMarker()
    {
        GameObject marker = Instantiate(minimapMarkerPrefab);
        marker.transform.parent = transform;
        marker.transform.localPosition = new Vector3(0, 0.5f, 0);
    }

    IEnumerator StateMachine()
    {
        while (hp > 0)
        {
            // 현재 상태에 따라 코루틴 시작
            yield return StartCoroutine(state.ToString());
        }
    }

    IEnumerator IDLE()
    {
        // 애니메이션이 IDLE 상태가 아니면 재생
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            anim.Play("Idle", 0, 0);
        }
        yield return null;
    }

    IEnumerator CHASE()
    {
        Debug.Log("Chasing");

        yield return null;

        // CHASE 상태에서는 계속해서 이동
        while (target != null)
        {
            nmAgent.SetDestination(target.position);

            // 현재 애니메이션 상태 확인
            var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

            // WalkFWD 애니메이션이 아니면 재생
            if (!curAnimStateInfo.IsName("Walk"))
            {
                anim.Play("Walk", 0, 0);
                yield return null;
            }

            // 목표까지의 남은 거리가 멈추는 지점보다 작거나 같으면
            if (nmAgent.remainingDistance <= nmAgent.stoppingDistance)
            {
                // ATTACK 상태로 변경
                ChangeState(State.ATTACK);
                yield break; // CHASE 상태를 빠져나옴
            }
            // 목표와의 거리가 멀어진 경우
            //else if (Vector3.Distance(transform.position, target.position) >= lostDistance)
            //{
            //    target = null;
            //    // IDLE 상태로 변경
            //    ChangeState(State.IDLE);
            //    yield break; // CHASE 상태를 빠져나옴
            //}

            // 목표 위치로 이동
            yield return null;
        }
    }

    IEnumerator ATTACK()
    {
        yield return null;
        if (attackCoolDown <= 0)
        {
            Debug.Log("Attacking");
            anim.Play("Attack", 0, 0);
            ShootProjectile();
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

            if (target != null)
            {
                Debug.Log("Attack!!!");
                IDamagable playerDamagable = target.GetComponent<IDamagable>();
                if (playerDamagable != null)
                {
                    playerDamagable.TakeDamage(damage);
                }
            }

            ChangeState(State.CHASE);
            attackCoolDown = attackCooldownTime;
        }
        else
        {
            ChangeState(State.CHASE);
            Debug.Log("cooldown");
        }
    }

    void Update()
    {
        if (attackCoolDown > 0)
        {
            attackCoolDown -= Time.deltaTime;
        }
    }
    void ShootProjectile()
    {
        // 투사체를 생성하고 그 결과를 저장
        GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        // 생성된 투사체가 유효한지 확인
        if (newProjectile != null)
        {
            Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
            if (rb != null && target != null)
            {
                Vector3 direction = target.position - projectileSpawnPoint.position;
                float distance = direction.magnitude;
                direction.Normalize();

                // 투사체의 초기 속도 및 발사 각도 설정 (원하는 값으로 조절해야 함)
                float launchAngle = 80f; // 발사 각도 (45도)
                float gravity = Physics.gravity.magnitude; // 중력 가속도
                float initialVelocity = Mathf.Sqrt((distance * gravity) / Mathf.Sin(2 * launchAngle * Mathf.Deg2Rad)); // 초기 속도 계산

                // 초기 속도를 투사체의 전방 방향으로 적용
                Vector3 launchVelocity = direction * initialVelocity * 1.3f;

                // 투사체에 초기 속도 및 중력 적용
                rb.velocity = launchVelocity;
                rb.useGravity = true; // 중력 사용

                // 투사체의 회전 설정 (필요에 따라 조절)
                Quaternion rotation = Quaternion.LookRotation(direction);
                newProjectile.transform.rotation = rotation;

                Projectile projectileScript = newProjectile.GetComponent<Projectile>();
                if (projectileScript != null)
                {
                    projectileScript.SetTarget(target);
                }
            }
        }
        else
        {
            Debug.LogError("투사체를 생성하지 못했습니다.");
        }
    }



    IEnumerator DAMAGED()
    {
        anim.Play("Damaged");
        GameObject effectObject = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        effectObject.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1f);
    }

    IEnumerator KILLED()
    {
        Debug.Log("Killed");
        anim.Play("Die", 0, 0);
        DisableCollider();
        Destroy(gameObject, 3f);
        yield return null;
    }
    void DisableCollider()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>(); // 몬스터의 모든 콜라이더 가져오기
        foreach (Collider collider in colliders)
        {
            collider.enabled = false; // 각 콜라이더를 비활성화
        }
    }
    void ChangeState(State newState)
    {
        StopCoroutine(state.ToString());
        state = newState;
        // 변경된 상태에 맞는 코루틴 시작
        StartCoroutine(state.ToString());
    }

    public void TakeDamage(int damage)
    {
        GameObject hudText = Instantiate(hudDamageText);
        hudText.GetComponent<DamageText>().damage = damage;
        hudText.transform.position = hudPos.position;
        Debug.Log("데미지 숫자를 받음");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            ChangeState(State.KILLED);
        }
        else
        {
            Debug.Log("데미지를 받음 ㄷㄷ");
            StartCoroutine(DAMAGED());
        }
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            ChangeState(State.KILLED);
        }
        else
        {
            StartCoroutine(DAMAGED());
        }
    }
    public void Detect(Transform target)
    {
        // 플레이어를 감지하면 목표를 설정하고 CHASE 상태로 변경
        this.target = target;
        ChangeState(State.CHASE);
    }

    private void OnDisable()
    {
        Manager.Event.voidEventDic["enemyDied"].RaiseEvent();
    }

    void DropItem()
    {
        Instantiate(itemPrefab, transform.position, Quaternion.identity);
    }

}