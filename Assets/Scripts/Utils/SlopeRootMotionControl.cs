using UnityEngine;

public class SlopeRootMotionControl : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask groundLayer;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    void OnAnimatorMove()
    {
        if (animator == null) return;

        // 拿出動畫這一個 Frame 想移動的距離
        Vector3 rmDelta = animator.deltaPosition;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f, groundLayer))
        {
            //if (Vector3.Angle(hit.normal, Vector3.up) > 1f)
            //{
            //    // 確保若是在斜坡，能夠把位移沿著斜率做動態改變
            //    Vector3 slopeDelta = Vector3.ProjectOnPlane(rmDelta, hit.normal);
            //    rb.MovePosition(rb.position + slopeDelta);
            //}
            //else
            //{
            //    // 若在平地就用原本的位移
            //    rb.MovePosition(rb.position + rmDelta);
            //}

            Vector3 slopeDelta = Vector3.ProjectOnPlane(rmDelta, hit.normal);
            rb.MovePosition(rb.position + slopeDelta);
        }
        else
        {
            // 確保在空中也有位移
            rb.MovePosition(rb.position + rmDelta);
        }

        rb.MoveRotation(rb.rotation * animator.deltaRotation);
    }
}
