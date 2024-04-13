using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ExplodeMonster : MonoBehaviour, IDamagable
{
    [SerializeField] int hp;
    [SerializeField] float lostDistance; // 목표와의 최대 거리
    Transform target;
    NavMeshAgent nmAgent;
    Animator anim;
    public GameObject hudDamageText;
    public Transform hudPos;
    private int currentHealth;
    public Image healthBarImage;
    public GameObject effectPrefab;
    public GameObject effectPrefab2;
    public GameObject minimapMarkerPrefab;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material originalMat;
    public Material redMat;

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
        SKIL,
        BOMB
    }

    State state;

    void Start()
    {
        AddMinimapMarker();
        anim = GetComponent<Animator>();
        nmAgent = GetComponent<NavMeshAgent>();
        state = State.IDLE;
        hp = 1;
        currentHealth = hp;
        UpdateHealthBar();
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
                // 터지는 몬스터이므로 바로 BOMB 로 이동한다.
                ChangeState(State.BOMB);
                yield return null;
               
            }
            // 목표와의 거리가 멀어진 경우
            else if (Vector3.Distance(transform.position, target.position) >= lostDistance)
            {
                target = null;
                // IDLE 상태로 변경
                ChangeState(State.IDLE);
                yield break; // CHASE 상태를 빠져나옴
            }
            // 목표 위치로 이동
            yield return null;
        }
    }
    IEnumerator BOMB()
    {
        Coroutine colorRoutine = StartCoroutine(ChangeColor());
        yield return new WaitForSeconds(3);
        StopCoroutine(colorRoutine);

        Debug.Log("터진다");
        GameObject effectObject = Instantiate(effectPrefab2, transform.position, Quaternion.identity);

        // 이펙트를 재생합니다.
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
        // ATTACK 상태에서는 공격 애니메이션을 재생하고 일정 시간 대기
        anim.Play("Attack");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        // 공격이 끝나면 다시 CHASE 상태로 변경
        ChangeState(State.CHASE);
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
        if (currentHealth <= 0)
        {
            ChangeState(State.KILLED);
        }
        else
        {
            Debug.Log("데미지를 받음 ㄷㄷ");
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
        // 플레이어를 감지하면 목표를 설정하고 CHASE 상태로 변경
        this.target = target;
        ChangeState(State.CHASE);
    }

    private void OnDisable()
    {
        Manager.Event.voidEventDic["enemyDied"].RaiseEvent();
    }
}
