using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static string[] tags = { "Tower", "Projectile", "Ship" };

    public enum GameState
    {
        Running,
        Paused,
        Over
    }
    public static GameState gameState = GameState.Running;


    public static void ChangeState(GameState newState)
    {

    }

    public static void WinGame()
    {
        if(gameState == GameState.Running)
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
}
