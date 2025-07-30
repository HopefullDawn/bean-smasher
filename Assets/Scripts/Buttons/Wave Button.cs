using UnityEngine;

public class WaveButton : FloorButton
{
    protected override void Update() // Wave button doesn't cost money
    {
        {
            if (playerOnButton /*&& !isActivated*/) // is player on a ready to use button?
            {
                fillAmount += Time.deltaTime / fillTime;
                fillImage.fillAmount = fillAmount;
                if (fillAmount >= 1f)
                {
                    {
                        ActivateButton(); // activate if full

                    }
                }
            }
            else if (!playerOnButton /*&& !isActivated*/) // drain the button
            {
                fillAmount = Mathf.MoveTowards(fillAmount, 0f, Time.deltaTime / fillTime); // gradually reset
                fillImage.fillAmount = fillAmount;
            }
        }
    }
    protected override void ActivateButton()
    {
        base.ActivateButton();
        waveManager.SpawnWave(); // spawn next wave
    }
}
