using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Ship
{
    // movement
    public float speed = 1;

    // health and damage
    public int damage = 10;
    public int health = 30;
    public float destroyDelay = 3f;

    public GameObject goModel;
    private Collider myCollider;

    private void Start()
    {
        OnCreatedEntity();
        myCollider = GetComponent<Collider>();
    }

    void FixedUpdate()
    {
        if(health > 0)
        {
            Move();
        }
    }

    public void OnCreatedEntity()
    {
        this.transform.LookAt(target);
    }

    public void Move()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            collision.gameObject.GetComponent<Tower>().TakeDamage(damage);
            DestroyEntity();
        }
    }

    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;

        if (health <= 0)
        {
            DestroyEntity();
        }
    }

    public void DestroyEntity()
    {
        goModel.SetActive(false);
        myCollider.enabled = false;

        /*
         TODO: INSERT VFX HERE
         */

        StartCoroutine(DestroyAfterDelay());
    }


    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(this.gameObject);
    }
}
