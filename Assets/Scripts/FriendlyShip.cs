using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyShip : Ship
{
    public override void HandleCollision(Collision collision)
    {

        if (collision.gameObject.CompareTag("Tower"))
        {
            collision.gameObject.GetComponent<Tower>().TakeDamage(damage);
            EnterTower();
        }
    }

    public void EnterTower()
    {
        speed = 0;
        if(myCollider) // not sure why this is needed but there's an error without it
            myCollider.enabled = false;

        NotifyGift();
        NotifyDestroyed(false);
        /*
         TODO: INSERT VFX HERE
         */

        StartCoroutine(DestroyAfterDelay());
    }

    public override void DestroyEntity()
    {
        goModel.SetActive(false);
        if (myCollider) // not sure why this is needed but there's an error without it
            myCollider.enabled = false;

        NotifyDestroyed(false);
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
