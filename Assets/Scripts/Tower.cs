using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject prefab_Projectile;
    public Transform projectileSpawnPoint;

    public int health = 100;


    public void TakeDamage(int damageTaken)
    {
        health = Mathf.Max(health - damageTaken, 0);

        if(health <= 0)
        {
            GameManager.LoseGame();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootProjectile();
        }
    }

    public void ShootProjectile()
    {
        GameObject projectileGO = Instantiate(prefab_Projectile, projectileSpawnPoint);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        Vector2 direction = new Vector2(projectileSpawnPoint.position.x, projectileSpawnPoint.position.z) - new Vector2(transform.position.x, transform.position.z);
        projectile.direction = new Vector3(direction.x, 0, direction.y).normalized;
    }
}
