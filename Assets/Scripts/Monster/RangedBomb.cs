using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RangedBomb : MonoBehaviour, IDamagable
{
    [SerializeField] int hp;
    [SerializeField] float lostDistance; // ��ǥ���� �ִ� �Ÿ�
    [SerializeField] float attackCooldownTime = 2.0f; // ���� ��ٿ� �ð� (��: 2��)
    float attackCoolDown = 0.0f; // ���� ��ٿ� �ʱⰪ

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

        // ������ hp
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
            // ���� ���¿� ���� �ڷ�ƾ ����
            yield return StartCoroutine(state.ToString());
        }
    }

    IEnumerator IDLE()
    {
        // �ִϸ��̼��� IDLE ���°� �ƴϸ� ���
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

        // CHASE ���¿����� ����ؼ� �̵�
        while (target != null)
        {
            nmAgent.SetDestination(target.position);

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
            //else if (Vector3.Distance(transform.position, target.position) >= lostDistance)
            //{
            //    target = null;
            //    // IDLE ���·� ����
            //    ChangeState(State.IDLE);
            //    yield break; // CHASE ���¸� ��������
            //}

            // ��ǥ ��ġ�� �̵�
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
        // ����ü�� �����ϰ� �� ����� ����
        GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        // ������ ����ü�� ��ȿ���� Ȯ��
        if (newProjectile != null)
        {
            Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
            if (rb != null && target != null)
            {
                Vector3 direction = target.position - projectileSpawnPoint.position;
                float distance = direction.magnitude;
                direction.Normalize();

                // ����ü�� �ʱ� �ӵ� �� �߻� ���� ���� (���ϴ� ������ �����ؾ� ��)
                float launchAngle = 80f; // �߻� ���� (45��)
                float gravity = Physics.gravity.magnitude; // �߷� ���ӵ�
                float initialVelocity = Mathf.Sqrt((distance * gravity) / Mathf.Sin(2 * launchAngle * Mathf.Deg2Rad)); // �ʱ� �ӵ� ���

                // �ʱ� �ӵ��� ����ü�� ���� �������� ����
                Vector3 launchVelocity = direction * initialVelocity * 1.3f;

                // ����ü�� �ʱ� �ӵ� �� �߷� ����
                rb.velocity = launchVelocity;
                rb.useGravity = true; // �߷� ���

                // ����ü�� ȸ�� ���� (�ʿ信 ���� ����)
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
            Debug.LogError("����ü�� �������� ���߽��ϴ�.");
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
        GameObject hudText = Instantiate(hudDamageText);
        hudText.GetComponent<DamageText>().damage = damage;
        hudText.transform.position = hudPos.position;
        Debug.Log("������ ���ڸ� ����");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            ChangeState(State.KILLED);
        }
        else
        {
            Debug.Log("�������� ���� ����");
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
        // �÷��̾ �����ϸ� ��ǥ�� �����ϰ� CHASE ���·� ����
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