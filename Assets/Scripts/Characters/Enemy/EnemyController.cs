using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[Pausable]
public class EnemyController : MonoBehaviour
{
    [Header("Enemy State")]
    public bool IsWaiting { get; private set; } = false;          // 是否正在發呆中
    public bool IsStaggered { get; private set; } = false;

    [Header("Data Source")]
    public EnemyData data;

    [Header("Scene References")]
    [SerializeField] private Transform[] waypoints;
    public Transform Player { get; private set; }

    [Header("Reference")]
    private NavMeshAgent agent;
    private Animator anim;
    private EnemyCombat combat;
    private EnemyReaction reaction;
    private Rigidbody rb;
    private EnemyStats stats;
    private EnemyEventsManager events;
    private EnemyStateMachine stateMachine;

    public float DistSqr { get; private set; } = float.MaxValue;

    private int currentWaypointIndex = 0;   // 目前走到第幾個點

    private static readonly int speedID = Animator.StringToHash("Speed");

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        combat = GetComponent<EnemyCombat>();
        reaction = GetComponent<EnemyReaction>();
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<EnemyStats>();
        events = GetComponent<EnemyEventsManager>();
        stateMachine = GetComponent<EnemyStateMachine>();
    }
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        if (waypoints.Length > 0 && stats != null)
        {
            agent.speed = stats.PatrolSpeed;
            agent.SetDestination(waypoints[0].position);
        }
    }
    void Update()
    {
        DistSqr = (Player.position - transform.position).sqrMagnitude;
    }
    public void RequestChasePlayer()
    {
        IsWaiting = false;
        agent.isStopped = false;
        agent.speed = stats.ChaseSpeed;
        agent.SetDestination(Player.position);
        UpdateAnimation();
    }
    public void RequestPatrol()
    {
        if (waypoints.Length == 0) return;

        agent.speed = stats.PatrolSpeed;
        if (IsWaiting)
        {
            UpdateAnimation();
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                // 到達目的地
                StartCoroutine(WaitAndMoveToNext());
            }
        }
        UpdateAnimation();
    }
    public void RequestStopMoving()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        UpdateAnimation();
    }
    void UpdateAnimation()
    {
        anim.SetFloat(speedID, agent.velocity.magnitude);
    }
    IEnumerator WaitAndMoveToNext()
    {
        IsWaiting = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(data.PatrolWaitTime);
        IsWaiting = false;
        agent.isStopped = false;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    public void RequestStagger(Vector3 attackerPos)
    {
        if (IsStaggered) return;

        IsStaggered = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        // 轉向攻擊者
        Vector3 dir = (attackerPos - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero) transform.rotation = Quaternion.LookRotation(dir);

        int num = Random.Range(1, 3);
        anim.CrossFade($"StandingReact{num}", 0.1f);
        // 擊退 (反方向)
        rb.AddForce(-dir * stats.KnockbackForce + Vector3.up * 5f, ForceMode.Impulse);

        IsStaggered = false;
        agent.isStopped = false;
    }
    public void RequestDie()
    {
        agent.isStopped = true;
        anim.CrossFade("Death", 0.1f);
        GetComponent<Collider>().enabled = false;
        rb.isKinematic = true;
        Destroy(gameObject, 3f);
    }
}
 