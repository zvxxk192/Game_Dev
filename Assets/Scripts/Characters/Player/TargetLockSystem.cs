using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

public class TargetLockSystem : MonoBehaviour
{
    [Header("References")]
    public CinemachineCamera freeLookCam;
    public CinemachineCamera lockOnCam;
    public CinemachineTargetGroup targetGroup;

    [Header("Setting")]
    public float scanRadius = 10f;
    public LayerMask enemyLayer;

    private bool isLockedOn = false;
    private bool hasAutoLockOn = false;
    private Transform currentTarget;
    private PlayerMovement movement;
    private PlayerInput input;
    private PlayerStateMachine stateMachine;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        movement = GetComponent<PlayerMovement>();
        stateMachine = GetComponent<PlayerStateMachine>();
    }
    void Start()
    {
        if (targetGroup != null)
        {
            targetGroup.Targets[0].Object = transform;
            targetGroup.Targets[0].Weight = 1f;
            targetGroup.Targets[0].Radius = 1f;
        }

        if (freeLookCam != null) freeLookCam.Priority = 10;
        if (lockOnCam != null) lockOnCam.Priority = 9;
    }
    void Update()
    {
        if (!hasAutoLockOn) AutoLockOn();

        if (isLockedOn)
        {
            if(currentTarget == null)
            {
                Unlock();
            }
            else if((transform.position - currentTarget.position).sqrMagnitude > scanRadius * scanRadius)
            {
                Unlock();
                hasAutoLockOn = false;
            }
            else if (isTargetDead())
            {
                Unlock();
            }
            LockOn();
        }
    }
    void AutoLockOn()
    {
        LockOn();
        if(isLockedOn) hasAutoLockOn = true;
    }
    bool isTargetDead()
    {
        if (currentTarget != null)
        {
            return currentTarget.GetComponent<EnemyReaction>().isEnemyDead;
        }
        return false;
    }
    void LockOn()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, scanRadius, enemyLayer);
        
        if (enemies.Length > 0)
        {
            currentTarget = enemies[0].transform;
            isLockedOn = true;

            lockOnCam.Follow = transform;
            lockOnCam.LookAt = targetGroup.transform;

            targetGroup.Targets[1].Object = currentTarget;
            targetGroup.Targets[1].Weight = 1f;
            targetGroup.Targets[1].Radius = 2f;

            lockOnCam.Priority = 20;
            movement.SetLockOnState(true, currentTarget);
        }
    }
    void Unlock()
    {
        isLockedOn = false;
        currentTarget = null;

        targetGroup.Targets[1].Object = null;
        targetGroup.Targets[1].Weight = 0;

        lockOnCam.Priority = 0;
        lockOnCam.LookAt = null;
        lockOnCam.Follow = transform;

        movement.SetLockOnState(false, null);
    }
    public void RequestLockOn()
    {
        if (isLockedOn) Unlock();
        else LockOn();
    }
    public void FaceTargetWhenAttack()
    {
        if (currentTarget != null)
        {
            Vector3 targetDir = currentTarget.position - transform.position;
            targetDir.y = 0;
            transform.rotation = Quaternion.LookRotation(targetDir);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, scanRadius);
    }
}
