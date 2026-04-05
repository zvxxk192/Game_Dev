using UnityEngine;

public class Bonfire : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject fireParticles;
    private bool _isLit = false;

    public string GetInteractPrompt()
    {
        if (_isLit)
            return $"<color=green>篝火已點燃 (已存檔)</color>";
        else
            return "點燃篝火";
    }

    public void Interact(Transform interactor)
    {
        if (_isLit) return;

        _isLit = true;

        if (fireParticles != null)
        {
            fireParticles.SetActive(true);
        }
        Debug.Log("篝火已點燃");

        // SaveManager.Instance.SaveGame();
        // PlayerEventsManager,TriggerBonfireLit();
    }
}
