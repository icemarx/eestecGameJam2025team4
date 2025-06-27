using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Running,
        Paused,
        Over
    }
    public static GameState gameState = GameState.Running;
    public bool debugOn = true;

    public GameObject prefab_EnemyShip;
    public GameObject prefab_FriendlyShip;

    public Tower tower;
    public List<EnemyShip> enemyShips = new List<EnemyShip>();

    public float spawnDistance = 10f;

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

        GameObject newEnemyShip = Instantiate(prefab_EnemyShip, spawnLocation, Quaternion.identity);
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
}
