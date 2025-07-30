using UnityEngine;

public class Damage : FloorButton
{
    protected override void ActivateButton()
    {
        base.ActivateButton();

        player.currentDamage *= 1.1f;  // increase damage by 10 %
        tower.currentDamage *= 1.05f;
        level++;
        buttonText.text = "DMG LV." + level;  // change button level text
    }
}
