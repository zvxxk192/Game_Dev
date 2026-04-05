using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Health & Survival")]
    public float BaseMaxHp = 100f;

    [Header("Movement Stats")]
    public float BaseWalkSpeed = 2f;
    public float BaseRunSpeed = 3.5f;

    [Header("Leveling Cost")]
    public int BaseExpToNextLevel = 100;

    [Header("Scaling Curves (X: Level, Y: Magnification)")]
    public AnimationCurve HealthScaleCurve;
    public AnimationCurve ExpScaleCurve;
}
