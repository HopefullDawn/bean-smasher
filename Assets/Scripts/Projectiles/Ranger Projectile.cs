using UnityEngine;

public class RangerProjectile : NORMALPROJECTILES
{
    public float projectileSpeed;
    public Enemy shooter;  // reference to the ranger for damage info
    void Update()
    {
        ProjectileFlight(projectileSpeed); // fly straight
    }
    private void OnTriggerEnter(Collider other) // handles colisions
    {
        if (other.CompareTag("Player") || other.CompareTag("Tower")) // damage tower and player
        {
            Entity targetEntity = other.GetComponent<Entity>();
            if (targetEntity != null)
            {
                targetEntity.TakeDamage(shooter.currentDamage);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Tree")) // gets destroyed when hitting tree trunks
        {
            Destroy(gameObject);
        }
    }

}
