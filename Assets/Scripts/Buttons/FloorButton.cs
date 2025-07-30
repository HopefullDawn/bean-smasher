using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloorButton : MonoBehaviour
{
    public Image fillImage; 
    public float fillTime = 2;  // time it takes to activate the button
    protected float fillAmount = 0f; // start at zero fill

    protected bool playerOnButton = false;  // is player on the button?
    //protected bool isActivated = false;

    public WaveManager waveManager;
    // references to upgrade stats
    public Player player;
    public Tower tower;

    protected int level = 1;
    protected float upgradeCost = 4; // default upgrade price

    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI upgradeCostText;

    protected virtual void Update()
    {
        if (playerOnButton /*&& !isActivated*/ ) // is player on a ready to use button?
        {
            if (Mathf.RoundToInt(upgradeCost) <= waveManager.money) // can player afford the upgrade?
            {
                fillAmount += Time.deltaTime / fillTime;
                fillImage.fillAmount = fillAmount;

                if (fillAmount >= 1f)
                {
                    ActivateButton(); // activate if full
                    waveManager.money -= Mathf.RoundToInt(upgradeCost);  // reduce money
                    upgradeCost *= 1.2f; // increase price of the next upgrade
                    waveManager.moneyCounterText.text = "Coins: " + waveManager.money;  // update money display
                    upgradeCostText.text = "cost: " + Mathf.RoundToInt(upgradeCost);   // update cost display
                } 
            }
        }
        else if (!playerOnButton /*&& !isActivated*/) // drain the button
        {
            fillAmount = Mathf.MoveTowards(fillAmount, 0f, Time.deltaTime / fillTime); // gradually reset
            fillImage.fillAmount = fillAmount;
        }
    }

    protected virtual void ActivateButton() // Activates button functions
    {
        // reset before next use
        fillAmount = 0f; 
        //isActivated = false;
        playerOnButton = false;

        // here apply specific logic in child scripts

        //Debug.Log("Floor button activated!");
    }
    private void OnTriggerEnter(Collider other) // notify when player steps on button
    {
        if (other.CompareTag("Player"))
        {
            playerOnButton = true;
        }
    }

    private void OnTriggerExit(Collider other)  // notify when player leaves button
    {
        if (other.CompareTag("Player"))
        {
            playerOnButton = false;
        }
    }
}
