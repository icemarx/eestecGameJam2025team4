using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damageTaken)
    {
        health = Mathf.Max(health - damageTaken, 0);

        if(health <= 0)
        {
            GameManager.LoseGame();
        }
    }
}
