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
    [SerializeField] float lostDistance; 
    [SerializeField] float attackCooldownTime = 2.0f; 
    float attackCoolDown = 0.0f;


    Transform target;
    NavMeshAgent nmAgent;
    Animator anim;
    public GameObject hudDamageText;
    public Transform hudPos;
    public Image healthBarImage;
    public int currentHealth;
    public GameObject effectPrefab;
    public GameObject effectPrefab2;
    public int damage;
    // 몬스터 hp바 업데이트
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
        currentHealth = hp;
        UpdateHealthBar();
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

        // CHASE 상태에서는 계속해서 이동
        while (target != null)
        {
            nmAgent.SetDestination(target.position);

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
                ChangeState(State.ATTACK);
                yield break; // CHASE 상태를 빠져나옴
            }
            // 목표와의 거리가 멀어진 경우
            //else if (Vector3.Distance(transform.position, target.position) >= lostDistance)
            //{
            //    target = null;
            //    // IDLE 상태로 변경
            //    ChangeState(State.IDLE);
            //    yield break; // CHASE 상태를 빠져나옴
            //}
            // 목표 위치로 이동
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
        yield return null;
        if (attackCoolDown <= 0)
        {
            anim.Play("Attack");
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
    IEnumerator DAMAGED()
    {
        anim.Play("Damaged");
        GameObject effectObject = Instantiate(effectPrefab2, transform.position, Quaternion.identity);
        effectObject.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1f);
    }

    IEnumerator KILLED()
    {
        Debug.Log("Killed");
        anim.Play("Die", 0, 0);
        DisableCollider();
        Manager.Event.voidEventDic["bossDefeated"].RaiseEvent();
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
        GameObject hudText = Instantiate(hudDamageText);
        hudText.GetComponent<DamageText>().damage = damage;
        hudText.transform.position = hudPos.position;
        Debug.Log("데미지 숫자를 받음");
        currentHealth -= damage;
        Manager.Sound.PlaySFX(Manager.Sound.AudioClips.monsterHitSFX);
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            ChangeState(State.KILLED);
        }
        else
        {
            Debug.Log("데미지를 받음 ㄷㄷ");
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
        // 플레이어를 감지하면 목표를 설정하고 CHASE 상태로 변경
        this.target = target;
        ChangeState(State.CHASE);
    }
}

