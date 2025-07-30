using UnityEngine;

public class NORMALPROJECTILES : MonoBehaviour
{
    // for straight flying projectiles

    // bounds for projectile flight
    public float boundsX = 100;
    public float boundsZ = 100;
     
    protected void ProjectileFlight(float projectileSpeed) // move the projectile
    {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime; // fly straight
        if (transform.position.x > boundsX || transform.position.x < -boundsX
            || transform.position.z > boundsZ || transform.position.z < -boundsZ)
        {
            Destroy(gameObject); // destroy out of bounds
        }
    }
}
