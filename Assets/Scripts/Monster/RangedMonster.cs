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
            // ���� ���¿� ���� �ڷ�ƾ ����
            yield return StartCoroutine(state.ToString());
        }
    }

    IEnumerator IDLE()
    {
        // �ִϸ��̼��� IDLE ���°� �ƴϸ� ���
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

        // CHASE ���¿����� ����ؼ� �̵�
        if(sensor.target != null)
        { 
            nmAgent.SetDestination(sensor.target.position);

            // ���� �ִϸ��̼� ���� Ȯ��
            var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

            // WalkFWD �ִϸ��̼��� �ƴϸ� ���
            if (!curAnimStateInfo.IsName("Walk"))
            {
                anim.Play("Walk", 0, 0);
                yield return null;
            }

            // ��ǥ������ ���� �Ÿ��� ���ߴ� �������� �۰ų� ������
            if (nmAgent.remainingDistance <= nmAgent.stoppingDistance)
            {
                // ATTACK ���·� ����
                ChangeState(State.ATTACK);
                yield break; // CHASE ���¸� ��������
            }
            // ��ǥ���� �Ÿ��� �־��� ���
            else if (Vector3.Distance(transform.position, sensor.target.position) >= lostDistance)
            {
                sensor.target = null;
                // IDLE ���·� ����
                ChangeState(State.IDLE);
                yield break; // CHASE ���¸� ��������
            }

            // ��ǥ ��ġ�� �̵�
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
        Collider[] colliders = GetComponentsInChildren<Collider>(); // ������ ��� �ݶ��̴� ��������
        foreach (Collider collider in colliders)
        {
            collider.enabled = false; // �� �ݶ��̴��� ��Ȱ��ȭ
        }
    }
    void ChangeState(State newState)
    {
        StopCoroutine(state.ToString());
        state = newState;
        // ����� ���¿� �´� �ڷ�ƾ ����
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
        // �÷��̾ �����ϸ� ��ǥ�� �����ϰ� CHASE ���·� ����
        this.target = target;
        ChangeState(State.CHASE);
    }
}