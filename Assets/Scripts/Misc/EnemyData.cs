using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemies/Enemy Data")]
public class EnemyData : ScriptableObject
{
    // gives Wave Manager access to enemy prefabs and assigned supply cost
    public GameObject prefab;
    public int supplyCost;

    public string enemyName; // "I am useless."
}
