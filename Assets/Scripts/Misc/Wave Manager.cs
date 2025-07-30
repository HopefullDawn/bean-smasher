using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject tower;
    private float towerBaseRange = 30;  // enemies can spawn beyond this range
    private Vector3 fallbackSpawnPoint = new Vector3(-37, -2, 24);  // default spawn point if finding one fails
    private Vector3 towerDistanceCalcPoint;   //calculate distance from tower base, not middle
    private float radiusAroundSpawn = 1.35f;  //radius of free space around each spawn point
    LayerMask obstacleMask;

    public GameObject floorButtons;

    public List<Enemy> enemiesAlive = new();
    public EnemyData[] enemyTypes; // self explanatory I think, but you read this anyway...
    private float meleePercentage = 0.4f;  //percentage of each wave are guaranteed melees
    private int currentWave = 1;
    public int supply = 5;   // base supply for spawning enemies
    public int supplyUsed;   
    public int supplyCreep = 2; // increment per wave

    public TextMeshProUGUI waveCounterText;
    public TextMeshProUGUI moneyCounterText;

    public int money;

    private void Start()
    {
        obstacleMask = LayerMask.GetMask("Trunks", "Entities");  //things to not find spawn points around
        towerDistanceCalcPoint = new Vector3(tower.transform.position.x, 0, tower.transform.position.z);
    }
    private Vector3 FindValidSpawnPoint()  // find spawn point vector for enemy units
    {
        float spawnBoundsCorner = 48;    // outer bounds
        float spawnBoundsTowerRadius = 20;  // inner bounds
        int attempts = 0;
        int maxAttempts = 30;
        Vector3 spawnPoint;
        do
        {
            // select spawn point in a circle
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float distance = Random.Range(spawnBoundsTowerRadius, spawnBoundsCorner);
            float coordinateX = Mathf.Cos(angle) * distance; 
            float coordinateZ = Mathf.Sin(angle) * distance;

            spawnPoint = new Vector3(coordinateX, 1, coordinateZ);
            // check if trees or entities are close by 
            if (!Physics.CheckSphere(spawnPoint, radiusAroundSpawn, obstacleMask) && (Vector3.Distance(spawnPoint, towerDistanceCalcPoint) > towerBaseRange ))
            {
                return spawnPoint;
            }
            else
            {
                attempts++;
            }
        }
        while (attempts < maxAttempts);
        Debug.Log("Failed to find a spawn point");
        return fallbackSpawnPoint;  // spawn the enemy here if algorithm fails
    }
    public void SpawnWave()  // spawns the next wave
    {
        waveCounterText.text = "Wave: " + currentWave;  // wave counter
        supplyUsed = 0;
        enemiesAlive.Clear();  // idk why even...
        floorButtons.SetActive(false);
        int guaranteedMelee = Mathf.RoundToInt(supply * meleePercentage);  //how many melee enemies spawns
        Debug.Log($"This wave should spawn at least {guaranteedMelee} melees");
        for (int i = 0; i < guaranteedMelee; i++)
        {
            InstantiateEnemy(0);  // spawn melee enemy
        }

        while (supplyUsed < supply)  // spawn random enemy types until we run out of supply
        {
            int supplyLeft = supply - supplyUsed;

            // build list of affordable enemies
            List<int> affordableEnemies = new(); 
            for (int i = 0; i < enemyTypes.Length; i++) 
            {
                if (enemyTypes[i].supplyCost <= supplyLeft)
                    affordableEnemies.Add(i);
            }

            if (affordableEnemies.Count == 0)
            {
                break; // no enemy can be spawned with remaining supply
            }
            int selectedIndex = affordableEnemies[Random.Range(0, affordableEnemies.Count)];
            InstantiateEnemy(selectedIndex);  //spawn selected enemy type
        }
    }
    private void onEnemyDied(Enemy deadEnemy)  // when enemy dies remove it from the list
    {
        enemiesAlive.Remove(deadEnemy);
        money += deadEnemy.dataReference.supplyCost;  // add money based on supply
        moneyCounterText.text = "Coins: " + money;

        // if list is empty, increase difficulty of the next wave and conjure button
        if (enemiesAlive.Count == 0 )
        {
            Debug.Log($"Wave {currentWave} defeated");
            currentWave++;  // wave counter
            IncreaseSupply();  // increase difficulty
            Debug.Log($"Supply for wave {currentWave} will be {supply}");
           floorButtons.SetActive(true);
        }
    }
    private void InstantiateEnemy(int enemyIndex)  // spawn enemy
    {
        GameObject enemyObj = Instantiate(enemyTypes[enemyIndex].prefab, FindValidSpawnPoint(), transform.rotation);
        Enemy enemyScript = enemyObj.GetComponent<Enemy>();
        enemyScript.dataReference = enemyTypes[enemyIndex];  // tells the enemies their worth 
        enemiesAlive.Add(enemyScript);  
        supplyUsed += enemyTypes[enemyIndex].supplyCost; // increase supply used
        enemyScript.onDeath += onEnemyDied;  // when enemy dies, invoke onEnemyDied()
    }
    private void IncreaseSupply()  // increase the supply for the next wave
    {
        supply += supplyCreep;
        if (currentWave % 5 == 0)
        {
            supplyCreep++;
        }
    }
}
