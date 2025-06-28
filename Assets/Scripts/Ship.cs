using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ship : MonoBehaviour
{
    // type
    public int type = 0;
    public bool canHandleCollisions = true;
    public const int C_FRIENDLY = 0;
    public const int C_STRAIGHT = 0;
    public const int C_ZIGZAG = 1;
    public const int C_SPIRAL = 2;

    // movement
    public float speed = 1;
    public Transform target;
    private float zigzagFrequency = 5f;
    private float zigzagMagnitude = 0.5f;
    private float spiralSpeed = 2f;
    private float spiralRadius = 0.5f;
    private float timeAlive = 0f;

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
        if (!myCollider)
            Debug.LogError("No collider present!");
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
        // switch this.type

        // if this.type == straight:
        switch (type)
        {
            // case C_FRIENDLY:
            case C_STRAIGHT:
                Vector3 direction = (target.position - transform.position).normalized;
                transform.position += direction * speed * Time.fixedDeltaTime;
                break;
            case C_ZIGZAG:
                direction = (target.position - transform.position).normalized;
                transform.position += direction * speed * Time.fixedDeltaTime;
                break;
            case C_SPIRAL:
                Vector3 forward = (target.position - transform.position).normalized;
                Vector3 right = Vector3.Cross(forward, Vector3.up).normalized * 2f;
                Vector3 offsetTarget = (forward + right);
                Vector3 prev = transform.position;
                transform.position += offsetTarget.normalized * speed * Time.fixedDeltaTime;

                transform.LookAt(transform.position + (transform.position - prev));
                break;
        }

        if (type == C_FRIENDLY || type == C_STRAIGHT)
        {
        }

        // if this.type == "zigzag":
        //transform.position.x += sin(Time.time) * 2 - 1 ;

        // if this.type == "spiral":
        // idk bro look up spiral math
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
