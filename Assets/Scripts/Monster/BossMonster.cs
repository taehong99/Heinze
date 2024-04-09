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
            // ���� ���¿� ���� �ڷ�ƾ ����
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

        // CHASE ���¿����� ����ؼ� �̵�
        if (sensor.target != null)
        {
            nmAgent.SetDestination(sensor.target.position);

            // ���� �ִϸ��̼� ���� Ȯ��
            var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

            // WalkFWD �ִϸ��̼��� �ƴϸ� ���
            if (!curAnimStateInfo.IsName("Walk"))
            {
                anim.Play("Walk");
            }

            // ��ǥ������ ���� �Ÿ��� ���ߴ� �������� �۰ų� ������
            if (nmAgent.remainingDistance <= nmAgent.stoppingDistance)
            {
                // ATTACK ���·� ����
                if (attackCount < 3)
                {
                    ChangeState(State.ATTACK);
                }
                else
                {
                    ChangeState(State.SKIL);
                }
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
        ShootProjectile();
        anim.Play("Attack", 0, 0);
        attackCount++; // ���� ī��Ʈ �����ϴ°� ����
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
        Debug.Log("������ ���ڸ� ����");
        hp -= damage;
        if (hp <= 0)
        {
            ChangeState(State.KILLED);
        }
        else
        {
            Debug.Log("�������� ���� ����");
            StartCoroutine(DAMAGED());
        }
    }

    IEnumerator DAMAGED()
    {

        anim.Play("Damaged");
        // �������� ���� �Ŀ� ��� ����մϴ�. �� �ð� ���� ���ʹ� �ִϸ��̼��� ����˴ϴ�.
        yield return new WaitForSeconds(1.0f);

    }



    IEnumerator SKIL()
    {
        Debug.Log("Skill activated"); // ��ų �ߵ��� ����� �α׷� ���

        attackCount = 0;
        anim.Play("Skil", 0, 0);
        //GameObject skillEffect = Instantiate(skillEffectPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(4.1f);

        yield return new WaitForSeconds(skillDuration); // ��ų ���� �ð���ŭ ���
        nmAgent.isStopped = false; //���� ���� ����
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

    public void Detect(Transform target)
    {
        // �÷��̾ �����ϸ� ��ǥ�� �����ϰ� CHASE ���·� ����
        this.target = target;
        ChangeState(State.CHASE);
    }
}

