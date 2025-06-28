using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ship : MonoBehaviour
{
    // type
    [Header("Type")]
    public int type = 0;
    public const int C_FRIENDLY = 0;
    public const int C_STRAIGHT = 0;
    public const int C_ZIGZAG = 1;
    public const int C_SPIRAL = 2;

    // movement
    [Header("Movement")]
    public float speed = 1;
    public Transform target;
    private float timeAlive;

    // health and damage
    [Header("Health and Damage")]
    public bool canHandleCollisions = true;
    public int damage = 10;
    public int health = 30;
    public float destroyDelay = 3f;

    public GameObject goModel;
    protected Collider myCollider;


    public ParticleSystem particleSystem;
    public AudioSource audioSource;

    public AudioSource towerHitSfx1;
    public AudioSource towerHitSfx2;



    private void Start()
    {
        OnCreatedEntity();
        myCollider = GetComponent<Collider>();
        if (!myCollider)
            Debug.LogError("No collider present!");

        timeAlive = Random.value * 2 * Mathf.PI;
        particleSystem = GetComponentInChildren<ParticleSystem>();
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
                transform.position += transform.right * Mathf.Sin(2 * timeAlive) / 40f;
                timeAlive += Time.fixedDeltaTime;
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
            audioSource.pitch = 1f + (Random.value - 0.5f) * 0.2f;
            audioSource.Play();
            particleSystem.Emit(10);
            DestroyEntity();
        }
    }

    public virtual void HandleCollision(Collision collision) { Debug.Log("Unhandled collision!"); }
    public abstract void DestroyEntity();
}
