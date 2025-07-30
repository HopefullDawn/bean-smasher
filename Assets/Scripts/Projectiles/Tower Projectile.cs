using Unity.VisualScripting;
using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    // homing projectile

    // targeting variables
    Vector3 targetDirection;
    public Transform target;

    public float speed = 10f;
    public Tower tower;

    // bounds for despawn
    public float boundsX = 100;
    public float boundsZ = 100;
    public float boundsY = 10;
    void Start()
    {
        tower = GameObject.FindGameObjectWithTag("Tower").GetComponent<Tower>();
        target = tower.GetNearestTarget("Enemy"); // assigns the nearest enemy as the target
    }
    void FixedUpdate()
    {
        if (target != null) // if we have a target...
        {
            targetDirection = (target.transform. position - transform.position).normalized; // calculates movement direction
            transform.Translate(targetDirection * speed * Time.deltaTime); // move towards the closest enemy
        }
        else // we find a target...
        {
            target = tower.GetNearestTarget("Enemy");
            if (target == null)
            {
                // no new target found, keep flying in last direction
                transform.Translate(targetDirection * speed * Time.deltaTime);
            }
        }
        if (transform.position.x > boundsX || transform.position.x < -boundsX
            || transform.position.z > boundsZ || transform.position.z < -boundsZ || transform.position.y > boundsY || transform.position.y < -boundsY)
        {
            Destroy(gameObject);  // destroy out of bounds
        }
    }
    private void OnTriggerEnter(Collider other) // handles collisions
    {
        if (other.CompareTag("Enemy")) // damages enemies
        {
            Entity targetEntity = other.GetComponent<Entity>();
            if (targetEntity != null)
            {
                targetEntity.TakeDamage(tower.currentDamage);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Tree")) // destroy when touching a tree trunk
        {
            Destroy(gameObject);
        }
    }
}
