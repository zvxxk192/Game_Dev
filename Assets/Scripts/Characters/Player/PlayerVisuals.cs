 using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement movement;

    private static readonly int hashSpeed = Animator.StringToHash("Speed");
    private static readonly int hashIsLockedOn = Animator.StringToHash("isLockedOn");
    private static readonly int hashIsGrounded = Animator.StringToHash("isGrounded");
    private static readonly int hashJump = Animator.StringToHash("Jumping Up");
    private static readonly int hashRoll = Animator.StringToHash("Roll_Forward");

    void Awake()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    void OnEnable()
    {
        movement.OnJumpTriggered += PlayJumpAnim;
        movement.OnRollTriggered += PlayRollAnim;
    }
    void OnDisable()
    {
        movement.OnJumpTriggered -= PlayJumpAnim;
        movement.OnRollTriggered -= PlayRollAnim;
    }
    void Update()
    {
        if (anim == null) return;
        anim.SetBool(hashIsGrounded, movement.isGrounded);
        anim.SetBool(hashIsLockedOn, movement.isLockedOn);
        anim.SetFloat(hashSpeed, movement.CurrentSpeedMagnitude, 0.1f, Time.deltaTime);
    }

    void PlayJumpAnim()
    {
        anim.CrossFade(hashJump, 0.03f);
    }
    void PlayRollAnim()
    {
        anim.CrossFade(hashRoll, 0.03f);
    }
}
