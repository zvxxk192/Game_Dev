using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy State")]
    public bool isWaiting { get; private set; } = false;          // 是否正在發呆中
    public bool isStaggered { get; private set; } = false;

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

    public float distSqr { get; private set; } = 0f;

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
        distSqr = (Player.position - transform.position).sqrMagnitude;
    }
    public void RequestChasePlayer()
    {
        isWaiting = false;
        agent.isStopped = false;
        agent.speed = stats.ChaseSpeed;
        agent.SetDestination(Player.position);
        UpdateAnimation();
    }
    public void RequestPatrol()
    {
        if (waypoints.Length == 0) return;

        agent.speed = stats.PatrolSpeed;
        if (isWaiting)
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
        isWaiting = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(data.PatrolWaitTime);
        isWaiting = false;
        agent.isStopped = false;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    public void RequestStagger(Vector3 attackerPos)
    {
        if (isStaggered) return;

        isStaggered = true;
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

        //yield return new WaitForSeconds(stats.StaggerTime);

        isStaggered = false;
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
 