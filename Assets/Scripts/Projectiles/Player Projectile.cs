using UnityEngine;

public class PlayerProjectile : NORMALPROJECTILES
{
    public GameObject player;
    public Player playerScript;
    public float projectileSpeed;
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        ProjectileFlight(projectileSpeed); // projectile flies straight
    }
    private void OnTriggerEnter(Collider other) // handles collisions
    {
        if (other.CompareTag("Enemy")) // applies damage to enemies
        {
            Entity targetEntity = other.GetComponent<Entity>();
            if (targetEntity != null)
            {
                targetEntity.TakeDamage(playerScript.currentDamage);
            }
            Destroy(gameObject); // remove the projectile
        }
        else if (other.CompareTag("Tower") || other.CompareTag("Tree")) // removes the prrojectile when hitting Tower or tree trunk
        {
            Destroy(gameObject);
        }
    }
}
