using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject prefab_Projectile;
    public Transform ring;
    public Transform projectileSpawnPoint;

    public int health = 100;

    public float ringRotationSpeed = 100f; // Adjust the speed of rotation


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


        RingRotation();
    }

    public void RingRotation()
    {
        // Check for input from A and D keys
        float rotationInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            rotationInput = 1f; // Rotate counter-clockwise
        } else if (Input.GetKey(KeyCode.D))
        {
            rotationInput = -1f; // Rotate clockwise
        }

        // Apply rotation to the object around the Y-axis
        ring.Rotate(0f, 0f, rotationInput * ringRotationSpeed * Time.deltaTime);
    }

    public void ShootProjectile()
    {
        GameObject projectileGO = Instantiate(prefab_Projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        Vector2 direction = new Vector2(projectileSpawnPoint.position.x, projectileSpawnPoint.position.z) - new Vector2(transform.position.x, transform.position.z);
        projectile.direction = new Vector3(direction.x, 0, direction.y).normalized;
    }
}
