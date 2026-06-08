using UnityEngine;

public class Bonfire : MonoBehaviour, IInteractable
{
    [Header("VFX / SFX")]
    [SerializeField] private GameObject fireParticles;

    [Header("UI Prompt")]
    [SerializeField] private string prompt;
    public string Prompt => prompt;

    private bool _isLit = false;

    public string GetInteractPrompt() => Prompt;

    public void Interact(Transform interactor)
    {
        if (_isLit) return;

        _isLit = true;

        if (fireParticles != null)
        {
            fireParticles.SetActive(true);
        }
        Debug.Log("êº¤õ¤wÂI¿U");

        // SaveManager.Instance.SaveGame();
        // PlayerEventsManager,TriggerBonfireLit();
    }
}
