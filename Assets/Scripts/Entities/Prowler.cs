using UnityEngine;

public class Prowler : Enemy
{
    protected override void Start()
    {
        base.Start();
        closestTarget = GameObject.FindGameObjectWithTag("Player"); // targets only player
    }
    protected override void UpdatePath() // pathing logic
    {
        if (towerBase != null && player != null && agent != null && agent.isOnNavMesh)
        {
            Vector3 newTargetPoint = playerCollider.ClosestPoint(transform.position); // find closest point on player's collider

            if (newTargetPoint != currentTargetPoint || Vector3.Distance(currentTargetPoint, newTargetPoint) > 0.5f) // if target point has changed
            {
                agent.SetDestination(playerCollider.ClosestPoint(transform.position));
                currentTargetPoint = newTargetPoint;
            }
        }
    }
    protected override void Update()
    {
        if (behaviorEnabled) // when done emerging 
        { 
            base.Update();

            //attack cooldown
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0f)
            {
                BasicAttack();
            }
        }
    }
}
