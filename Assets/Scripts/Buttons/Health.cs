using UnityEngine;

public class Health : FloorButton
{
    protected override void ActivateButton()
    {
        base.ActivateButton();

        player.currentMaxHealth *= 1.1f;  // increase health by 10 %
        tower.currentMaxHealth *= 1.05f;
        level++;
        buttonText.text = "HP LV." + level;  // change button level text
    }
}
