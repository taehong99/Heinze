using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class RangedMonster : MonoBehaviour, IDamagable
{
    [SerializeField] int hp;
    [SerializeField] float lostDistance;
    [SerializeField] float attackRange;
    [SerializeField] float attackCooldown;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;
    MonserSensor sensor;
    Transform target;
    NavMeshAgent nmAgent;
    Animator anim;
    enum State
    {
        IDLE,
        CHASE,
        ATTACK,
        KILLED,
        SKIL
    }

    [SerializeField] State state;

    void Start()
    {
        anim = GetComponent<Animator>();
        nmAgent = GetComponent<NavMeshAgent>();
        sensor = GetComponentInChildren<MonserSensor>();

        hp = 1;
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
        // 애니메이션이 IDLE 상태가 아니면 재생
        while(true)
        {
            Debug.Log("Idle");
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                anim.Play("Idle", 0, 0);
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
        if(sensor.target != null)
        { 
            nmAgent.SetDestination(sensor.target.position);

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
        anim.Play("Attack1", 0, 0);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        ShootProjectile();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        nmAgent.isStopped = false;
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

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            ChangeState(State.KILLED);
        }
    }

    public void Detect(Transform target)
    {
        // 플레이어를 감지하면 목표를 설정하고 CHASE 상태로 변경
        this.target = target;
        ChangeState(State.CHASE);
    }
}