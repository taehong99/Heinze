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
    public GameObject[] skillEffectPrefab; // ��ų ����Ʈ ���� ����
    public Image healthBarImage;
    private int currentHealth;
    public GameObject effectPrefab;
    public int damage = 1;
    public GameObject itemPrefab;
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

        // ������ hp
        hp = 180;
        state = State.IDLE;
        currentHealth = hp;
        UpdateHealthBar();
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
        while (sensor.target != null)
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

            // ��ǥ ��ġ�� �̵�
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
        attackCount++; // ���� ī��Ʈ �����ϴ°� ����
        yield return new WaitForSeconds(1.2f);
        if (sensor.target != null)
        {
            Debug.Log("Attack!!!");
            IDamagable playerDamagable = sensor.target.GetComponent<IDamagable>();
            if (playerDamagable != null)
            {
                playerDamagable.TakeDamage(damage);
            }

        }
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
        currentHealth -= damage;
        if (hp <= 0)
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


    IEnumerator DAMAGED()
    {
        Debug.Log("����Ʈ �ߵ�");
        Manager.Sound.PlaySFX(Manager.Sound.AudioClips.monsterHitSFX);
        GameObject effectObject = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        effectObject.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1.0f);

    }



    IEnumerator SKIL()
    {
        Debug.Log("��ų �ߵ� ! ");

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
        Manager.Event.voidEventDic["bossDefeated"].RaiseEvent();
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
        Debug.Log(newState.ToString());
        // ����� ���¿� �´� �ڷ�ƾ ����
        StartCoroutine(state.ToString());
    }

    void DropItem()
    {
        Instantiate(itemPrefab, transform.position, Quaternion.identity);
    }

    //public void Detect(Transform target)
    //{
    //    // �÷��̾ �����ϸ� ��ǥ�� �����ϰ� CHASE ���·� ����
    //    this.target = target;
    //    ChangeState(State.CHASE);
    //}
}

