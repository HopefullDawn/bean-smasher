using UnityEngine;

public class Miniboss : Enemy
{
    protected override void Start() // get references - shoots at player, moves only towards tower
    {
        base.Start();
        closestTarget = GameObject.FindGameObjectWithTag("Tower");
        player = GameObject.FindGameObjectWithTag("Player");
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
                BasicAttack(); // attack the tower
                currentTargetPoint = player.transform.position; // get player's position
                float prevRange = currentRange; // remember melee range
                currentRange = 30; // temporarily change range to be able to shoot
                RangedAttack(); // shoot at player
                currentRange = prevRange; // reset range to melee
            }
        }
    }
    protected override void UpdatePath() // pathing logic
    {
        if (towerBase != null && player != null && agent != null && agent.isOnNavMesh)
        {
            Vector3 newTargetPoint = towerCollider.ClosestPoint(transform.position); // gets closest point on tower

            if (newTargetPoint != currentTargetPoint || Vector3.Distance(currentTargetPoint, newTargetPoint) > 0.5f) // check if destination has changed
            {
                agent.SetDestination(towerCollider.ClosestPoint(transform.position)); // set agent destination
                currentTargetPoint = newTargetPoint;
            }
        }
    }
}
