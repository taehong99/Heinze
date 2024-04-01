using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RangedMonster : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] float lostDistance; // 목표와의 최대 거리
    [SerializeField] float attackRange; // 원거리 공격 사정거리
    [SerializeField] float attackCooldown; // 공격 쿨다운
    [SerializeField] GameObject projectilePrefab; // 발사체 프리팹
    [SerializeField] Transform projectileSpawnPoint; // 발사체 발사 위치

    Transform target;
    NavMeshAgent nmAgent;
    Animator anim;

    bool canAttack = true;

    enum State
    {
        IDLE,
        CHASE,
        ATTACK,
        KILLED
    }

    State state;

    void Start()
    {
        anim = GetComponent<Animator>();
        nmAgent = GetComponent<NavMeshAgent>();

        hp = 1;
        state = State.IDLE;
        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (hp > 0)
        {
            yield return StartCoroutine(state.ToString());
        }
    }

    IEnumerator IDLE()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            anim.Play("Idle", 0, 0);
        }
        yield return null;
    }

    IEnumerator CHASE()
    {
        while (target != null)
        {
            nmAgent.SetDestination(target.position);

            var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

            if (!curAnimStateInfo.IsName("Walk"))
            {
                anim.Play("Walk", 0, 0);
                yield return null;
            }

            if (nmAgent.remainingDistance <= nmAgent.stoppingDistance)
            {
                ChangeState(State.ATTACK);
                nmAgent.isStopped = true; // 공격 시 이동 멈춤
                yield break;
            }
            else if (Vector3.Distance(transform.position, target.position) >= lostDistance)
            {
                target = null;
                ChangeState(State.IDLE);
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator ATTACK()
    {
        if (canAttack)
        {
            anim.Play("Attack1", 0, 0);
            canAttack = false;

            // 공격 애니메이션의 길이만큼 대기
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

            ShootProjectile();
            yield return new WaitForSeconds(attackCooldown); // 쿨다운 시간만큼 대기
            canAttack = true;
            nmAgent.isStopped = false; // 공격 후 다시 이동 시작
            ChangeState(State.CHASE); // 쿨다운이 끝나면 다시 추적 상태로 변경
        }
        else
        {
            ChangeState(State.CHASE);
        }
    }

    void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Projectile script = projectile.GetComponent<Projectile>();
        if (script != null && target != null)
        {
            script.SetTarget(target);
        }
    }

    IEnumerator KILLED()
    {
        anim.Play("Idle", 0, 0);
        Destroy(gameObject, 2f);
        yield return null;
    }

    void ChangeState(State newState)
    {
        StopCoroutine(state.ToString());
        state = newState;
        StartCoroutine(state.ToString());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
            ChangeState(State.CHASE);
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            ChangeState(State.KILLED);
        }
    }
}
