using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ship : MonoBehaviour
{
    // movement
    public float speed = 1;
    public Transform target;

    // health and damage
    public int damage = 10;
    public int health = 30;
    public float destroyDelay = 3f;

    public GameObject goModel;
    protected Collider myCollider;

    private void Start()
    {
        OnCreatedEntity();
        myCollider = GetComponent<Collider>();
    }

    void FixedUpdate()
    {
        if (health > 0)
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
        HandleCollision(collision);
    }


    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;

        if (health <= 0)
        {
            DestroyEntity();
        }
    }

    public virtual void HandleCollision(Collision collision) { Debug.Log("Unhandled collision!"); }
    public abstract void DestroyEntity();
}
