using UnityEngine;

[CreateAssetMenu(menuName = "Create New loot force")]
public class LootForce : ScriptableObject
{
    public Vector2 minForceDirection;
    public Vector2 maxForceDirection;
    public Force itemDropForce;
    public Vector2 baseSize;
}
