using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        Idle,
        WaveRunning,
        WaveOver,
        Over
    }
    public static GameState gameState = GameState.Idle;
    public bool isPaused = false;
    public bool debugOn = true;

    [Header("Prefabs")]
    public GameObject[] prefabs_EnemyShip;
    public GameObject[] prefabs_FriendlyShip;

    [Header("GameObject links")]
    public Tower tower;
    public UIManager uiManager;
    public UpgradeManager upgradeManager;
    public static List<EnemyShip> enemyShips = new List<EnemyShip>();
    public static List<FriendlyShip> friendlyShips = new List<FriendlyShip>();

    [Header("Spawning")]
    public float spawnDistance = 15f;
    public float friendlyShipChance = 0.20f;

    [Header("Resources")]
    public int wealth = 100;
    public static int resourceMultiplier = 1;
    public static int[] resourceWorth = new int[] { 1, 2, 5 }; // picked resources


    [Header("Health")]
    public static int maxHP = 100;
    public static int curHP = 100;
    public static int healingFactor = 10;
    public static int bulletDamage = 10;
    public static float fireCooldown = 1f;
    public static int rateOfFireBoost = 0;

    [Header("Wave info")]
    public static int currentWaveNum = 0;
    public static int shipsLeftInWave = 0;

    [Header("Scores")]
    public static int score = 0;
    public static int numOfEnemies = 0;
    public static int friendlyScore = 1000;
    public static int[] enemyScores = new int[] { 100, 300, 700 };

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject); // Persist across scenes
    }

    private void Start()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        wealth = 100;
        maxHP = 100;
        healingFactor = 10;
        bulletDamage = 10;
        curHP = maxHP;
        currentWaveNum = 0;
        shipsLeftInWave = 0;
        gameState = GameState.Idle;
        isPaused = false;
        enemyShips.Clear();
        friendlyShips.Clear();
        score = 0;
        numOfEnemies = 0;
        rateOfFireBoost = 0;
        fireCooldown = 1f;

        upgradeManager.ResetUpgrades();
        ChangeState(GameState.WaveRunning);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePaused();
        }

        if (debugOn && Input.GetKeyDown(KeyCode.I))
        {
            GenerateShip();
        }

        if (debugOn && Input.GetKeyDown(KeyCode.O) && (gameState == GameState.Idle || gameState == GameState.WaveOver))
        {
            // start wave
            ChangeState(GameState.WaveRunning);
        }
    }


    public static bool ChangeState(GameState newState)
    {
        if (gameState == newState)
        {
            Debug.Log("New state is the same as old state <" + newState + ">");
            return false;
        }

        if(gameState == GameState.Over)
        {
            Debug.Log("Game over");
            return false;
        }

        switch (newState)
        {
            case GameState.WaveRunning:
            // start wave
            currentWaveNum++;
            shipsLeftInWave = currentWaveNum * 7;
            Instance.StartCoroutine(Instance.RunWave());
            break;
            case GameState.WaveOver:
            Instance.uiManager.DisplayUpgradeMenu();
            break;
            case GameState.Over:
            Instance.uiManager.DisplayGameOver(score, currentWaveNum, numOfEnemies);
            break;
            default:
            Debug.LogError("Unknown state: " + newState);
            break;
        }

        gameState = newState;
        return true;
    }

    public void TogglePaused()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = Mathf.Epsilon;
            uiManager.DisplayPauseMenu();
        }
        else
        {
            uiManager.HidePauseMenu();
            Time.timeScale = 1;
        }
    }

    public static void WinGame()
    {
        if (ChangeState(GameState.Over))
            Debug.Log("YOU WIN!");
    }

    public static void LoseGame()
    {
        if (ChangeState(GameState.Over))
            Debug.Log("You lose");
    }

    public void GenerateShip()
    {
        Vector2 circlePoint = Random.insideUnitCircle.normalized * spawnDistance;
        Vector3 spawnLocation = tower.transform.position + new Vector3(circlePoint.x, 0, circlePoint.y);

        if (Random.value <= friendlyShipChance)
        {
            GameObject newFriendlyShip = Instantiate(prefabs_FriendlyShip[0], spawnLocation, Quaternion.identity);
            FriendlyShip friendlyShip = newFriendlyShip.GetComponent<FriendlyShip>();
            friendlyShip.target = tower.transform;
            friendlyShips.Add(friendlyShip);
        }
        else
        {
            int shipType = Random.Range(0, Mathf.Min(prefabs_EnemyShip.Length, (int)(currentWaveNum * 1.4f - 1f)));

            GameObject newEnemyShip = Instantiate(prefabs_EnemyShip[shipType], spawnLocation, Quaternion.identity);
            EnemyShip enemyShip = newEnemyShip.GetComponent<EnemyShip>();
            enemyShip.target = tower.transform;
            enemyShips.Add(enemyShip);

        }
    }

    public IEnumerator RunWave()
    {
        Debug.Log("Starting wave #" + currentWaveNum);
        yield return new WaitForSeconds(3.0f);

        bool isWaveRunning = true;
        while (isWaveRunning)
        {
            if (shipsLeftInWave > 0)
            {
                GenerateShip();
                shipsLeftInWave--;
                yield return new WaitForSeconds(1f);
            }
            else if (enemyShips.Count + friendlyShips.Count == 0)
            {
                isWaveRunning = false;
            }
            yield return new WaitForSeconds(0.125f);
        }

        Debug.Log("Wave #" + currentWaveNum + " over");
        ChangeState(GameState.WaveOver);
    }

    public static void HandleShipDestroyed(EnemyShip ship)
    {
        enemyShips.Remove(ship);
    }

    public static void HandleShipDestroyed(FriendlyShip ship)
    {
        friendlyShips.Remove(ship);
    }

    public static void HandleShipGift()
    {
        UpdateWealth(Instance.wealth + resourceWorth[0] * resourceMultiplier);
        // Debug.Log("Current Wealth: " + wealth);
    }

    public static void UpdateHealth(int newHealth)
    {
        curHP = Mathf.Clamp(newHealth, 0, maxHP);
        if (curHP <= 0)
        {
            GameManager.LoseGame();
        }
        Instance.uiManager.UpdateHealthText(curHP, maxHP);
        Instance.uiManager.UpdateButtons();
    }

    public void Heal()
    {
        UpdateHealth(curHP + healingFactor);
    }

    public static void UpdateWealth(int newWealth)
    {
        Instance.wealth = newWealth;
        Instance.uiManager.UpdateWealthText(Instance.wealth);
        Instance.uiManager.UpdateButtons();
    }

    public static void UpdateScore(int idx, bool isEnemy)
    {
        if(isEnemy)
        {
            score += enemyScores[idx];
        } else
        {
            score += friendlyScore;
        }
    }

    public static void KilledEnemyNotification()
    {
        numOfEnemies++;
    }
}
