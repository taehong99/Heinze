using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ExplodeMonster : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] float lostDistance; // ��ǥ���� �ִ� �Ÿ�
    Transform target;
    NavMeshAgent nmAgent;
    Animator anim;
    public GameObject hudDamageText;
    public Transform hudPos;
    private int currentHealth;
    public Image healthBarImage;
    public GameObject effectPrefab;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material originalMat;
    public Material redMat;

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
        SKIL,
        BOMB
    }

    State state;

    void Start()
    {
        anim = GetComponent<Animator>();
        nmAgent = GetComponent<NavMeshAgent>();
        // ������ hp
        hp = 3;
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
                // ATTACK ���·� ����
                ChangeState(State.BOMB);
                yield return null;
               
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
    IEnumerator BOMB()
    {
        Coroutine colorRoutine = StartCoroutine(ChangeColor());
        yield return new WaitForSeconds(3);
        StopCoroutine(colorRoutine);

        Debug.Log("������");
        GameObject effectObject = Instantiate(effectPrefab, transform.position, Quaternion.identity);

        // ����Ʈ�� ����մϴ�.
        effectObject.GetComponent<ParticleSystem>().Play();
        ChangeState(State.KILLED);

        yield return null;
    }

    IEnumerator ChangeColor()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            skinnedMeshRenderer.material = redMat;
            yield return new WaitForSeconds(0.1f);
            skinnedMeshRenderer.material = originalMat;
            yield return null;
        }
    }

    IEnumerator ATTACK()
    {
        Debug.Log("BomB!!!!");
        // ATTACK ���¿����� ���� �ִϸ��̼��� ����ϰ� ���� �ð� ���
        anim.Play("Attack");
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
}
