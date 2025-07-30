using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    public EntityStats stats; // assign stats

    // current stats to be changed at runtime
    public float currentHealth;
    public float currentSpeed;
    public float currentDamage;
    public float currentRange;
    public float currentAttackSpeed;
    public float currentMaxHealth;

    // for color changing
    private Renderer[] rend;
    private List<Color> originalColors = new List<Color>();
    private float flashDuration = 0.1f;

    public ParticleSystem deathPuff; // particle effect on death
    void Awake()
    {
        // load current stats
        currentHealth = stats.maxHealth;
        currentDamage = stats.damage;
        currentSpeed = stats.speed;
        currentRange = stats.range;
        currentAttackSpeed = stats.attackSpeed;

        currentMaxHealth = stats.maxHealth;

        
    }
    public virtual void TakeDamage(float amountOfDamage) // take damage
    {
        currentHealth -= amountOfDamage;

        //Debug.Log($"{gameObject.name} took {amountOfDamage} damage, and has {currentHealth} health left");
        StartCoroutine(FlashRed()); // flash red
        if (currentHealth <= 0) // die when at zero health
        {
            Die();
        }
    }
    protected virtual void Die() // if he dies, he dies
    {
        if (deathPuff != null) // detaches the particle system to finish playing when entity dies
        {
            deathPuff.transform.parent = null;
            deathPuff.Play();
            Destroy(deathPuff.gameObject, deathPuff.main.duration);
        }
        Destroy(gameObject);
    }
    protected IEnumerator FlashRed() // turn red for a bit
    {
        rend = GetComponentsInChildren<Renderer>(); // gets all renderers
        foreach (Renderer r in rend) // excludes particle systems
        {
            if (r.GetComponent<ParticleSystemRenderer>() != null) continue;

            originalColors.Add(r.material.color); // remember original colors
            r.material.color = Color.red; // changes renderers to red
        }
        yield return new WaitForSeconds(flashDuration); // stay red for a bit

        // return old colors
        int colorIndex = 0;
        foreach (Renderer r in rend)
        {
            if (r.GetComponent<ParticleSystemRenderer>() != null) continue; // skip particle systems again

            r.material.color = originalColors[colorIndex]; // assign color
            colorIndex++;
        }
    }
    protected virtual void RegenerateHealth(float percentage) // regeneration method
    {
        if (currentHealth < currentMaxHealth) // unless at max health
        {
        currentHealth += (percentage * stats.maxHealth * Time.deltaTime); // increase health by a percentage - ! DOESN'T SCALE WITH HEALTH UPGRADES !
        }
    }
}
