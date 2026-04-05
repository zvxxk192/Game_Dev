using UnityEngine;

public interface IInteractable
{
    // 互動時出發
    void Interact(Transform interactor);

    // UI呼叫顯示文字
    string GetInteractPrompt();
}
