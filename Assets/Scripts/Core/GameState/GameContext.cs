using UnityEngine;

public class GameContext : MonoBehaviour
{
    [field: SerializeField] public PlayerMovement PlayerMovement { get; private set; }

    public void Setup(GameObject levelPlayer)
    {
         PlayerMovement = levelPlayer.GetComponent<PlayerMovement>();
    }
}
