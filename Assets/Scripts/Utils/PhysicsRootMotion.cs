using UnityEngine;

public class PhysicsRootMotion : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;

    public bool isKnockedBack = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // 這個函式會攔截 Animator 的 Apply Root Motion
    private void OnAnimatorMove()
    {
        if (isKnockedBack) return;

        // 1. 如果 Animator 沒有在播有位移的動畫，就不做事
        if (Mathf.Approximately(Time.deltaTime, 0f)) return;

        // 2. 計算動畫想給的速度
        Vector3 animationVelocity = anim.deltaPosition / Time.deltaTime;

        // 3. 【關鍵】保留原本的垂直速度 (重力)
        animationVelocity.y = rb.linearVelocity.y;

        // 4. 把計算好的速度餵給 Rigidbody
        rb.linearVelocity = animationVelocity;

        // 5. 這裡用 MoveRotation 確保物理運算正確
        rb.MoveRotation(rb.rotation * anim.deltaRotation);
    }
}
