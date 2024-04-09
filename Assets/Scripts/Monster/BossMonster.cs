using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

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
    Transform target;
    NavMeshAgent nmAgent;
    Animator anim;
    int attackCount = 0;
    public GameObject hudDamageText;
    public Transform hudPos;
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
        sensor = GetComponentInChildren<MonserSensor>();
        hp = 5;
        state = State.IDLE;
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
        if (sensor.target != null)
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
            // 목표와의 거리가 멀어진 경우
            else if (Vector3.Distance(transform.position, sensor.target.position) >= lostDistance)
            {
                sensor.target = null;
                // IDLE 상태로 변경
                ChangeState(State.IDLE);
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
        ShootProjectile();
        anim.Play("Attack", 0, 0);
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
        hp -= damage;
        if (hp <= 0)
        {
            ChangeState(State.KILLED);
        }
        else
        {
            Debug.Log("데미지를 받음 ㄷㄷ");
            StartCoroutine(DAMAGED());
        }
    }

    IEnumerator DAMAGED()
    {

        anim.Play("Damaged");
        // 데미지를 입은 후에 잠시 대기합니다. 이 시간 동안 몬스터는 애니메이션이 재생됩니다.
        yield return new WaitForSeconds(1.0f);

    }



    IEnumerator SKIL()
    {
        Debug.Log("Skill activated"); // 스킬 발동을 디버그 로그로 출력

        attackCount = 0;
        anim.Play("Skil", 0, 0);
        //GameObject skillEffect = Instantiate(skillEffectPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(4.1f);

        yield return new WaitForSeconds(skillDuration); // 스킬 지속 시간만큼 대기
        nmAgent.isStopped = false; //멈춤 상태 해제
        ChangeState(State.CHASE);
    }

    void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Projectile script = projectile.GetComponent<Projectile>();
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
        // 변경된 상태에 맞는 코루틴 시작
        StartCoroutine(state.ToString());
    }

    public void Detect(Transform target)
    {
        // 플레이어를 감지하면 목표를 설정하고 CHASE 상태로 변경
        this.target = target;
        ChangeState(State.CHASE);
    }
}

