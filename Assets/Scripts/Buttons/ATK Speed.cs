using UnityEngine;

public class ATKSpeed : FloorButton
{
    protected override void ActivateButton()
    {
        base.ActivateButton();

        player.currentAttackSpeed *= 0.9f;  // increase attack speed by 10 %
        tower.currentAttackSpeed *= 0.95f;
        level++;
        buttonText.text = "ATK SPEED LV." + level;  // change button level text
    }
}
