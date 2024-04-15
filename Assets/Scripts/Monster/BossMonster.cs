using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BossMonster : MonoBehaviour, IDamagable
{
    [SerializeField] int hp;
    [SerializeField] float lostDistance;
    [SerializeField] float attackRange;
    [SerializeField] float attackCooldown;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float skillDuration;
    MonserSensor sensor;
    NavMeshAgent nmAgent;
    Animator anim;
    int attackCount = 0;
    public GameObject hudDamageText;
    public Transform hudPos;
    public GameObject[] skillEffectPrefab; // 스킬 이펙트 참조 변수
    public Image healthBarImage;
    private int currentHealth;
    public GameObject effectPrefab;
    public GameObject effectPrefab1;
    void UpdateHealthBar()
    {
        if (healthBarImage != null)
            healthBarImage.fillAmount = ((float)currentHealth) / hp;
    }
    enum State
    {
        IDLE,
        CHASE,
        ATTACK,
        KILLED,
        SKIL,
        DAMAGED
    }

    [SerializeField] State state;

    void Start()
    {
        anim = GetComponent<Animator>();
        nmAgent = GetComponent<NavMeshAgent>();
        sensor = GetComponentInChildren<MonserSensor>();

        // 몬스터의 hp
        hp = 30;
        state = State.IDLE;
        currentHealth = hp;
        UpdateHealthBar();
        StartCoroutine(StateMachine());
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
        while (true)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                anim.Play("Idle");
            }
            if (sensor.target != null)
            {
                ChangeState(State.CHASE);
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator CHASE()
    {
        Debug.Log("Chasing");

        // CHASE 상태에서는 계속해서 이동
        while (sensor.target != null)
        {
            nmAgent.SetDestination(sensor.target.position);

            // 현재 애니메이션 상태 확인
            var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

            // WalkFWD 애니메이션이 아니면 재생
            if (!curAnimStateInfo.IsName("Walk"))
            {
                anim.Play("Walk");
            }

            // 목표까지의 남은 거리가 멈추는 지점보다 작거나 같으면
            if (nmAgent.remainingDistance <= nmAgent.stoppingDistance)
            {
                // ATTACK 상태로 변경
                if (attackCount < 3)
                {
                    ChangeState(State.ATTACK);
                }
                else
                {
                    ChangeState(State.SKIL);
                }
                yield break; // CHASE 상태를 빠져나옴
            }

            // 목표 위치로 이동
            yield return null;
        }
        ChangeState(State.IDLE);
    }
    IEnumerator ATTACK()
    {
        Debug.Log("attack");
        nmAgent.velocity = Vector3.zero;
        anim.Play("Attack", 0, 0);
        ShootProjectile();
        attackCount++; // 어택 카운트 증가하는거 세기
        yield return new WaitForSeconds(1.2f);
        anim.Play("Idle", 0, 0);
        yield return new WaitForSeconds(1.8f);
        nmAgent.isStopped = false;
        ChangeState(State.CHASE);

    }
    public void TakeDamage(int damage)
    {
        GameObject hudText = Instantiate(hudDamageText);
        hudText.GetComponent<DamageText>().damage = damage;
        hudText.transform.position = hudPos.position;
        Debug.Log("데미지 숫자를 받음");
        currentHealth -= damage;
        if (hp <= 0)
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


    IEnumerator DAMAGED()
    {
        Debug.Log("이펙트 발동");
        GameObject effectObject = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        //effectObject.transform.position = new Vector3 (0f, 0f, 0f);
        effectObject.GetComponent<ParticleSystem>().Play();
        // 데미지를 입은 후에 잠시 대기합니다. 이 시간 동안 몬스터는 애니메이션이 재생됩니다.
        yield return new WaitForSeconds(1.0f);

    }



    IEnumerator SKIL()
    {
        Debug.Log("스킬 발동 ! ");

        attackCount = 0;
        anim.Play("Skil", 0, 0);
        yield return new WaitForSeconds(4.1f);

        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int randomIndex = Random.Range(0, skillEffectPrefab.Length);
        GameObject skillEffect = Instantiate(skillEffectPrefab[randomIndex], transform.position, Quaternion.identity);
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        skillEffect.transform.rotation = Quaternion.LookRotation(direction);

        yield return new WaitForSeconds(3f);
        Destroy(skillEffect);
        nmAgent.isStopped = false;
        ChangeState(State.CHASE);
    }


    void ShootProjectile()
    {
        GameObject Projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        projectilePrefab.GetComponent<ParticleSystem>().Play();
        Projectile script = Projectile.GetComponent<Projectile>();
        if (script != null && sensor.target != null)
        {
            script.SetTarget(sensor.target);
        }
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
        Debug.Log(newState.ToString());
        // 변경된 상태에 맞는 코루틴 시작
        StartCoroutine(state.ToString());
    }

    //public void Detect(Transform target)
    //{
    //    // 플레이어를 감지하면 목표를 설정하고 CHASE 상태로 변경
    //    this.target = target;
    //    ChangeState(State.CHASE);
    //}
}

