using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        Running,
        Paused,
        Over
    }
    public static GameState gameState = GameState.Running;
    public bool debugOn = true;

    [Header("Prefabs")]
    public GameObject[] prefabs_EnemyShip;
    public GameObject[] prefabs_FriendlyShip;

    [Header("GameObject links")]
    public Tower tower;
    public List<EnemyShip> enemyShips = new List<EnemyShip>();
    public List<FriendlyShip> friendlyShips = new List<FriendlyShip>();

    [Header("Spawning")]
    public float spawnDistance = 10f;
    public float friendlyShipChance = 0.25f;

    [Header("Resources")]
    public int wealth = 100;
    public int resourceMultiplier = 1;
    public int[] resourceWorth = new int[]{ 1, 2, 5 }; // picked resources


    [Header("Health")]
    public static int maxHP = 100;
    public static int curHP = 100;

    [Header("Wave info")]
    public int currentWaveNum = 0;

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }

    private void OnEnable()
    {
        Ship.OnShipDestroyed += HandleShipDestroyed;
        Ship.OnShipGift += HandleShipGift;
    }

    private void OnDisable()
    {
        Ship.OnShipDestroyed -= HandleShipDestroyed;
        Ship.OnShipGift -= HandleShipGift;
    }

    private void Update()
    {
        if (debugOn && Input.GetKeyDown(KeyCode.I))
        {
            GenerateShip();
        }
    }


    public static void ChangeState(GameState newState)
    {

    }

    public static void WinGame()
    {
        if (gameState == GameState.Running)
        {
            Debug.Log("YOU WIN!");
            ChangeState(GameState.Over);
        }
    }

    public static void LoseGame()
    {
        if (gameState == GameState.Running)
        {
            Debug.Log("You lose");
            ChangeState(GameState.Over);
        }
    }

    public void GenerateShip()
    {
        Vector2 circlePoint = Random.insideUnitCircle.normalized * spawnDistance;
        Vector3 spawnLocation = tower.transform.position + new Vector3(circlePoint.x, 0, circlePoint.y);

        if (Random.value <= friendlyShipChance)
        {
            int shipType = Random.Range(0, Mathf.Max(prefabs_EnemyShip.Length, currentWaveNum % 3));

            GameObject newFriendlyShip = Instantiate(prefabs_FriendlyShip[0], spawnLocation, Quaternion.identity);
            FriendlyShip friendlyShip = newFriendlyShip.GetComponent<FriendlyShip>();
            friendlyShip.target = tower.transform;
            friendlyShips.Add(friendlyShip);
        } else
        {
            // TODO: int shipType = Random.Range(0, Mathf.Max(prefabs_EnemyShip.Length, waveNumber % 3));

            GameObject newEnemyShip = Instantiate(prefabs_EnemyShip[0], spawnLocation, Quaternion.identity);
            EnemyShip enemyShip = newEnemyShip.GetComponent<EnemyShip>();
            enemyShip.target = tower.transform;
            enemyShips.Add(enemyShip);

        }
    }

    public void HandleShipDestroyed(Ship ship, bool isEnemy)
    {
        if(isEnemy)
        {
            enemyShips.Remove((EnemyShip)ship);
        }
    }

    public void HandleShipGift(Ship ship, bool isEnemy)
    {
        wealth += resourceWorth[ship.type] * resourceMultiplier;
        Debug.Log("Current Wealth: " + wealth);
    }

    public static void UpdateHealth(int newHealth)
    {
        curHP = Mathf.Clamp(newHealth, 0, maxHP);
        if (curHP <= 0)
        {
            GameManager.LoseGame();
        }
    }
}
