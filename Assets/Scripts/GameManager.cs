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

    public GameObject[] prefabs_EnemyShip;
    public GameObject[] prefabs_FriendlyShip;

    public Tower tower;
    public List<EnemyShip> enemyShips = new List<EnemyShip>();

    public float spawnDistance = 10f;

    public int wealth = 100;
    public int resourceMultiplier = 1;
    public int[] resourceWorth = new int[]{ 1, 2, 5 };


    // HP
    public static int maxHP = 100;
    public static int curHP = 100;

    // waves info
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
    }

    private void OnDisable()
    {
        Ship.OnShipDestroyed -= HandleShipDestroyed;
    }

    private void Update()
    {
        if (debugOn && Input.GetKeyDown(KeyCode.I))
        {
            GenerateEnemyShip();
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

    public void GenerateEnemyShip()
    {
        Vector2 circlePoint = Random.insideUnitCircle.normalized * spawnDistance;
        Vector3 spawnLocation = tower.transform.position + new Vector3(circlePoint.x, 0, circlePoint.y);

        // TODO: int shipType = Random.Range(0, Mathf.Max(prefabs_EnemyShip.Length, waveNumber % 3));

        GameObject newEnemyShip = Instantiate(prefabs_EnemyShip[0], spawnLocation, Quaternion.identity);
        EnemyShip enemyShip = newEnemyShip.GetComponent<EnemyShip>();
        enemyShip.target = tower.transform;
        enemyShips.Add(enemyShip);
    }

    public void HandleShipDestroyed(Ship ship, bool isEnemy)
    {
        if(isEnemy)
        {
            enemyShips.Remove((EnemyShip)ship);
        }
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
