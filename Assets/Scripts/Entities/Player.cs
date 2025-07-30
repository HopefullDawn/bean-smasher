using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    private float bounds = 48f; // bounds for movement
    public float shootTimer = 0; // for cooldown handling

    private Rigidbody rb; // for movement

    public GameObject projectilePrefab; // player projectiles

    public GameManager gameManager;
    public WaveManager waveManager;

    // for healthbar management
    public Image healthbar;
    public float healthRegen = 0.05f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    

    private void FixedUpdate() // movement
    {
        // get inputs
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(-x, 0, -z).normalized; // sets move direction, duh
        Vector3 moveTo = transform.position + direction * currentSpeed * Time.deltaTime; // sets the actual move vector
        // stay inside bounds
        moveTo.x = Mathf.Clamp(moveTo.x, -bounds, bounds);
        moveTo.z = Mathf.Clamp(moveTo.z, -bounds, bounds);

        if (direction != Vector3.zero)
        {
            rb.MovePosition(moveTo); // moves player if he want to
        }
    }
    private void Update()
    {
        if (!gameManager.isPaused) // prevents rotation if game is paused
        {
        RotateToMouse();
        }
        // attack input and cooldown
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f && Input.GetMouseButton(0) && !gameManager.isPaused)
        {
            Shoot();
            shootTimer = currentAttackSpeed;
        }
        // regenerate health, but not between waves
        if (waveManager.enemiesAlive.Count > 0)    
        {
            RegenerateHealth(healthRegen);
            healthbar.fillAmount = (float)currentHealth / stats.maxHealth;
        }
    }
    private void Shoot()
    {
        Instantiate(projectilePrefab, transform.position, transform.rotation);
    }
    void RotateToMouse()  // rotates player to face the mouse position. Made by AI but I understand what's going on if that helps :).
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 direction = targetPoint - transform.position;
            direction.y = 0f; // Flatten so it only turns horizontally

            if (direction.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
    protected override void Die() // stops healthbar from residual regeneration after defeat, calls game over
    {
        base.Die();
        healthbar.fillAmount = 0;
        gameManager.GameOver();
    }
}
/*      ISSUES - 
 *      
 *      JITTERY ENEMY MOVE ANIMATION - HOPEFULLY SOLVED IN GAME MANAGER Start()
 *  
 *  
 *  STUFF TO ADD LATER IF I FEEL LIKE IT
 *  - COLOR INDICATORS FOR PURCHASES
 *  - VFX
 *  - CONTROLS DESCRIPTION FOR MAIN MENU
 *  - STATS TWEAKS
 *  - MONEY & UPGRADE
 *  - SAVE GAME & HIGH SCORE
 *  - BOSSES
 *  - SPECIAL ATTACKS
 * 
 */