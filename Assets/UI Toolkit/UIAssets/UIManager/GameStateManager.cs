using UnityEngine;
using UnityEngine.UIElements;
using System;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager _instance;
    public static GameStateManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UnityEngine.Object.FindFirstObjectByType<GameStateManager>();
            }
            return _instance;
        }
    }

    [Header("State")]
    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnGameStateChanged;

    void Awake()
    {
        if (_instance == null) _instance = this;
        else if (_instance != this) Destroy(gameObject);
    }
    void OnEnable()
    {
        ChangeState(GameState.Playing);
    }

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;
        CurrentState = newState;
 
        OnGameStateChanged?.Invoke(newState);
    }
}
