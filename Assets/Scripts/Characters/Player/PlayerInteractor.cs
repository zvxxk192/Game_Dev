using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interact Settings")]
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRadius = 1.5f;
    [SerializeField] private LayerMask interactableLayer;

    private IInteractable _currentInteractable;

    void Update()
    {
        // 掃描所有可互動物件
        DetectInteractables();
    }
    void DetectInteractables()
    {
        Collider[] colliders = Physics.OverlapSphere(interactionPoint.position, interactionRadius, interactableLayer);

        if (colliders.Length > 0)
        {
            // 待優化 : 取距離最短的
            IInteractable interactable = colliders[0].GetComponent<IInteractable>();

            if (interactable != null )
            {
                _currentInteractable = interactable;

                // 呼叫UI
                // UIManager.Instance.ShowPrompt
                //   (_currentInteractable.GetInteractPrompt());
            }
            else
            {
                _currentInteractable = null;

                // UIManager.Instance.HidePrompt();
            }
        }   
    }

    public void PlayerInteract()
    {
        if (_currentInteractable != null)
            _currentInteractable.Interact(this.transform);
    }
}
