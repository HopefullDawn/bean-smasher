using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class Tower : Entity
{
    public GameObject projectile; // references tower projectiles
    public Transform orbTransform; // shots come from here
    public float shootTimer = 2f; // manages cooldown, starting value runs only once
    public Transform rangeOrigin; // calculate range lower down
    public string target = "Enemy"; // tag for the tower to target

    public Image healthbar;
    public float healthRegen = 0.025f;

    public GameManager gameManager;
    public WaveManager waveManager;
    void Update()
    {
        Transform nearestTarget = GetNearestTarget(target); // find nearest target

        // attack cooldown and fire
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f && nearestTarget != null)
        {
            Shoot();
            shootTimer = currentAttackSpeed;
        }
        // health regen logic
        if (waveManager.enemiesAlive.Count > 0)   // regenerate health, but not between waves
        {
            RegenerateHealth(healthRegen);
            healthbar.fillAmount = (float)currentHealth / stats.maxHealth;
        }
    }
    void Shoot() // Fire, shoot, discharge, launch, ran out of synonyms
    {
        Instantiate(projectile, orbTransform.position, Quaternion.identity);  
    }
    public Transform GetNearestTarget(string tag) // find the nearest target
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag); // gets all enemies alive

        Transform nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject target in targets) // for each enemy, check if they are in range and if they are closer than current closest enemy
        {
            float distance = Vector3.Distance(rangeOrigin.position, target.transform.position);
            if (distance <= currentRange && distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = target.transform;
            }
        }
        return nearest;
    }
    protected override void Die() // stops healthbar from residual regeneration after defeat, calls game over
    {
        base.Die();
        healthbar.fillAmount = 0;
        gameManager.GameOver();
    }
}
