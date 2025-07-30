using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

public class Enemy : Entity
{
    public Action<Enemy> onDeath;
    public EnemyData dataReference;

    // mostly for homing
    public GameObject towerBase;
    public GameObject player;
    protected Collider towerCollider;
    protected CapsuleCollider playerCollider;
    protected GameObject closestTarget;
    protected Vector3 currentTargetPoint;

    public GameObject projectilePrefab;  // prefab for ranged enemies

    protected NavMeshAgent agent;

    // for path updating cooldown
    float updateTimer = 0f;
    float updateCooldown = 0.1f;  // previously 0.02 if future me cares
    
    protected float damageTimer = 1f; // attack timer

    protected bool behaviorEnabled = false; // prevents actions before enemy emerges

    // controls how fast enemy emerges and how deep they spawn
    private float spawnDelay = 2;
    private float spawnSubmerge = 2.5f;
    protected virtual void Start() // find all the stuff we need 
    {
        towerBase = GameObject.FindGameObjectWithTag("TowerBase");
        player = GameObject.FindGameObjectWithTag("Player");
        towerCollider = GameObject.FindGameObjectWithTag("Tower").GetComponent<Collider>();
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();

        agent.speed = currentSpeed;
        StartCoroutine(Emerge(transform.position, spawnDelay)); // enemy starts emerging - spawning in
    }

    protected virtual void Update()
    {   
        if (!behaviorEnabled) return; // if not done emerging

        // update path cooldown for performance reasons
        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0f)
        {
            UpdatePath();
            updateTimer = updateCooldown;
        }
    }
    protected virtual void UpdatePath() // pathing logic
    {
        if (towerBase != null && player != null && agent != null && agent.isOnNavMesh)
        {
            // gets distances to tower and player
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            float distanceToTower = Vector3.Distance(transform.position, towerBase.transform.position);

            // selects the closest target and the closest point on them
            Vector3 newTargetPoint = (distanceToPlayer < distanceToTower) ? playerCollider.ClosestPoint(transform.position) : towerCollider.ClosestPoint(transform.position);
            closestTarget = (distanceToPlayer < distanceToTower) ? player : towerBase;

            // if the target has changed or the target has moved far enough, recalculate path
            if (newTargetPoint != currentTargetPoint || Vector3.Distance(currentTargetPoint, newTargetPoint) > 0.5f)
            {
                agent.SetDestination(newTargetPoint);
                currentTargetPoint = newTargetPoint;
            }
        }
    }
    public void BasicAttack() // melee attack
    {
        float distanceToTarget = Vector3.Distance(transform.position, currentTargetPoint); // gets distance
        if (distanceToTarget <= currentRange && closestTarget != null) // check if in range
        {
            Entity targetEntity = closestTarget.GetComponent<Entity>(); // fetches target's soul
            if (targetEntity == null)
            {
                targetEntity = closestTarget.GetComponentInParent<Entity>(); // towerbase has entity on parent Tower
            }
            targetEntity.TakeDamage(currentDamage); // apply damage 
            //Debug.Log($"{targetEntity.name} took {currentDamage} damage, and has {targetEntity.currentHealth} health left");
            damageTimer = currentAttackSpeed; // start cooldown
        }
    }
    public void RangedAttack() // ranged attack
    {
        float distanceToTarget = Vector3.Distance(transform.position, currentTargetPoint); // gets distance
        if (distanceToTarget <= currentRange) // check if in range
        {
            Vector3 direction = (currentTargetPoint - transform.position).normalized; //aim vector
            Quaternion lookRotation = Quaternion.LookRotation(direction); // rotate towards target

            GameObject projectileObj = Instantiate(projectilePrefab, transform.position, lookRotation); // fire!

            // projectile remembers who shot it for damage info
            RangerProjectile projectile = projectileObj.GetComponent<RangerProjectile>();
            projectile.shooter = this;
        }
        damageTimer = currentAttackSpeed; // start cooldown
    }
    public IEnumerator Emerge(Vector3 targetPosition, float emergeDuration) // spawning animation - emerging from the ground
    {
        // start underground
        Vector3 startPos = new Vector3(targetPosition.x, targetPosition.y - spawnSubmerge, targetPosition.z);
        transform.position = startPos;

        float elapsed = 0f;

        while (elapsed < emergeDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPosition, elapsed / emergeDuration); // float into position
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition; // final snap to position
        behaviorEnabled = true; // gives enemies rights
    }
    protected override void Die()
    {
        onDeath?.Invoke(this); // calls OnEnemyDied in Wave Manager
        base.Die();
    }
}
