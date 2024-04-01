using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RangedMonster : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] float lostDistance; // ��ǥ���� �ִ� �Ÿ�
    [SerializeField] float attackRange; // ���Ÿ� ���� �����Ÿ�
    [SerializeField] float attackCooldown; // ���� ��ٿ�
    [SerializeField] GameObject projectilePrefab; // �߻�ü ������
    [SerializeField] Transform projectileSpawnPoint; // �߻�ü �߻� ��ġ

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
                nmAgent.isStopped = true; // ���� �� �̵� ����
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

            // ���� �ִϸ��̼��� ���̸�ŭ ���
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

            ShootProjectile();
            yield return new WaitForSeconds(attackCooldown); // ��ٿ� �ð���ŭ ���
            canAttack = true;
            nmAgent.isStopped = false; // ���� �� �ٽ� �̵� ����
            ChangeState(State.CHASE); // ��ٿ��� ������ �ٽ� ���� ���·� ����
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
