using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Combat/Combo List")]
public class ComboSO : ScriptableObject
{
    public List<AttackAction> comboSteps;

    public AttackAction counterAttack;
}
