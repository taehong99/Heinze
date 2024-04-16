using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RangedMonster : MonoBehaviour, IDamagable
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
    public float projectileSpeed = 10f; // 투사체 속도

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
            anim.Play("Attack", 0, 0);
            ShootProjectile();
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

            if (target != null)
            {
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
        Vector3 direction = (target.transform.position + Vector3.up * 2) - projectileSpawnPoint.position;
        //direction.y = 1f;
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        projectile.transform.forward = direction.normalized;


        Projectile script = projectile.GetComponent<Projectile>();
        if (script != null && target != null)
        {
            script.SetTarget(target);
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
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            ChangeState(State.KILLED);
        }
        else
        {
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