using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BossMonster1 : MonoBehaviour, IDamagable
{
    [SerializeField] int hp;
    [SerializeField] float lostDistance; // ��ǥ���� �ִ� �Ÿ�
    Transform target;
    NavMeshAgent nmAgent;
    Animator anim;
    public GameObject hudDamageText;
    public Transform hudPos;
    public Image healthBarImage;
    public int currentHealth;
    public GameObject effectPrefab;
    // ���� hp�� ������Ʈ
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
        DAMAGED,
        SKIL
    }

    State state;

    void Start()
    {
        anim = GetComponent<Animator>();
        nmAgent = GetComponent<NavMeshAgent>();
        state = State.IDLE;
        hp = 100;
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
                ChangeState(State.ATTACK);
                yield break; // CHASE ���¸� ��������
            }
            // ��ǥ���� �Ÿ��� �־��� ���
            else if (Vector3.Distance(transform.position, target.position) >= lostDistance)
            {
                target = null;
                // IDLE ���·� ����
                ChangeState(State.IDLE);
                yield break; // CHASE ���¸� ��������
            }
            // ��ǥ ��ġ�� �̵�
            yield return null;
        }
    }

    IEnumerator SKIL()
    {
        GameObject effectObject = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        effectObject.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        effectObject.GetComponent<ParticleSystem>().Play();
        yield return null;
    }
    IEnumerator ATTACK()
    {
        Debug.Log("Attacking");
        // ATTACK ���¿����� ���� �ִϸ��̼��� ����ϰ� ���� �ð� ���
        anim.Play("Attack", 0, 0);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        // ������ ������ �ٽ� CHASE ���·� ����
        ChangeState(State.CHASE);
    }

    IEnumerator DAMAGED()
    {
        anim.Play("Damaged");
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
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            ChangeState(State.KILLED);
        }
        else
        {
            Debug.Log("�������� ���� ����");

            // ü���� 10�� ����� �������� �� �߰� ����Ʈ �߻�
            if (currentHealth % 10 == 0)
            {
                StartCoroutine(SKIL());
            }
            else
            {
                StartCoroutine(DAMAGED());
            }
        }
    }

    public void Detect(Transform target)
    {
        // �÷��̾ �����ϸ� ��ǥ�� �����ϰ� CHASE ���·� ����
        this.target = target;
        ChangeState(State.CHASE);
    }
}

