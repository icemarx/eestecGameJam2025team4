using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Ship
{
    public override void HandleCollision(Collision collision)
    {

        if (collision.gameObject.CompareTag("Tower"))
        {
            collision.gameObject.GetComponent<Tower>().TakeDamage(damage);
            DestroyEntity();
        }
    }

    public override void DestroyEntity()
    {
        goModel.SetActive(false);
        if (myCollider) // not sure why this is needed but there's an error without it
            myCollider.enabled = false;

        NotifyDestroyed(true);
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
