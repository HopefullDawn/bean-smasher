using UnityEngine;
[CreateAssetMenu(fileName = "NewEntityStats", menuName = "Stats/Entity Stats")]
public class EntityStats : ScriptableObject
{
    // stats used by game entities
    public float maxHealth;
    public float speed;
    public float damage;
    public float range;
    public float attackSpeed;
}
