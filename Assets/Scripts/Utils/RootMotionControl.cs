using UnityEngine;

public class RootMotionControl : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }
    void OnAnimatorMove()
    {
        if (animator)
        {
            // 拿出動畫這一個 Frame 想移動的距離
            Vector3 newPosition = animator.deltaPosition;
            // 注意：這裡不需要乘 Time.deltaTime，因為 deltaPosition 已經是計算好的量
            characterController.Move(newPosition);
            // 旋轉也要同步
            transform.rotation = animator.deltaRotation * transform.rotation;
        }
    }
}
