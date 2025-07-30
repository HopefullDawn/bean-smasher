using UnityEngine;

public class Ranger : Enemy
{
    protected override void Start()
    {
        base.Start();
        agent.stoppingDistance = currentRange / 3; // Rangers stop to attack from distance
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
                RangedAttack();
            }
        }
    }
}
