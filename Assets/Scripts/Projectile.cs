using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 1;
    public int damage = 10;
    public Vector3 direction;
    

    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        transform.position += direction * speed;
    }

}
