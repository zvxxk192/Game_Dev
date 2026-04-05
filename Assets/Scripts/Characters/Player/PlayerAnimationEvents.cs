using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private WeaponController weaponController;
    private PlayerReaction playerReact;
    private PlayerMovement playerMov;

    void Awake()
    {
        weaponController = GetComponent<WeaponController>();
        playerReact = GetComponent<PlayerReaction>();
        playerMov = GetComponent<PlayerMovement>();
    }

    public void AnimationEvent_DetectHit()
    {
        if (weaponController.currentWeapon != null)
            weaponController.currentWeapon.OnCombatAnimationEvent("DetectHit");
    }
    public void AnimationEvent_OpenComboWindow()
    {
        if (weaponController.currentWeapon != null)
            weaponController.currentWeapon.OnCombatAnimationEvent("OpenComboWindow");
    }
    public void AnimationEvent_PlayAttackSound()
    {
        if (weaponController.currentWeapon != null)
            weaponController.currentWeapon.OnCombatAnimationEvent("PlayAttackSound");
    }
    public void AnimationEvent_StopCounterAttack()
    {
        if (weaponController.currentWeapon != null)
            weaponController.currentWeapon.OnCombatAnimationEvent("StopCounterAttack");
    }
    public void AnimationEvent_Jumpable()
    {
        if (playerMov != null) 
            playerMov.OnAnimationEvent_Jumpable();
    }
    public void AnimationEvent_StopRoll()
    {
        if (playerMov != null)
            playerMov.OnAnimationEvent_StopRoll();
    }
    public void AnimationEvent_EnableIFram()
    {
        if (playerReact != null) 
            playerReact.OnAnimationEvent_SetInvincible(true);
    }
    public void AnimationEvent_DisableIFram()
    {
        if (playerReact != null) 
            playerReact.OnAnimationEvent_SetInvincible(false);
    }
}
