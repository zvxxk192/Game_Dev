using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

public class GameUpdateToggler : MonoBehaviour
{
    private List<MonoBehaviour> targetScripts;

    private void Awake()
    {
        targetScripts = new List<MonoBehaviour>();
        MonoBehaviour[] allScripts = GetComponents<MonoBehaviour>();

        foreach(MonoBehaviour script in allScripts)
        {
            if (script == null || script == this) continue;

            var attribues = script.GetType().GetCustomAttributes(typeof(PausableAttribute), true);

            if (attribues.Length > 0)
            {
                targetScripts.Add(script);
            }
        }
    }
    private void OnEnable()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged += HandleStateChanged;
    }
    private void OnDisable()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged -= HandleStateChanged;
    }
    private void HandleStateChanged(IGameState currentState)
    {
        bool shouldBeActive = (currentState == GameStateManager.Instance.GamePlayingState);
        foreach (MonoBehaviour ud in targetScripts)
        {
            if (ud != null)
                ud.enabled = shouldBeActive;
        }
    }
}
