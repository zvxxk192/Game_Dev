using UnityEngine;

public class PlayingWorldSceneContext : MonoBehaviour
{
    [Header("Current Scene Instance Reference")]
    [SerializeField] private GameObject levelPlayer;

    public GameObject LevelPlayer => levelPlayer;
}
