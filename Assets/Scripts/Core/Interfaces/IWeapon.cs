using UnityEngine;

public interface IWeapon
{
    void RequestAttack();
    void RequestCounterAttack();

    bool IsAttacking { get; }
    bool CanCounterAttack { get; }

    void OnCombatAnimationEvent(string eventName);
    void InterruptAttack();

    //void OnAnimationEvent_DetectHit();
    //void OnAnimationEvent_OpenComboWindow();
    //void OnAnimationEvent_PlayAttackSound();
}
