using UnityEngine;

public class Melee : Enemy
{
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
