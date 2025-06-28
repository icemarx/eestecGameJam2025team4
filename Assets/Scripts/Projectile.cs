using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10;
    // public int damage = 10;
    public Vector3 direction;

    public float timeToLive = 3f;
    

    void FixedUpdate()
    {
        Move();
        timeToLive -= Time.deltaTime;
        if(timeToLive <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Move()
    {
        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ship"))
        {
            collision.gameObject.GetComponent<Ship>().TakeDamage(GameManager.bulletDamage);
            Destroy(this.gameObject);
        }
    }


}
