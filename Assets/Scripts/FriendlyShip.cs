using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyShip : Ship
{

    public override void HandleCollision(Collision collision)
    {

        if (canHandleCollisions && collision.gameObject.CompareTag("Tower"))
        {
            EnterTower();
        }
    }

    public void EnterTower()
    {
        canHandleCollisions = false;
        speed = 0;
        if (myCollider) // not sure why this is needed but there's an error without it
            myCollider.enabled = false;

        GameManager.HandleShipGift();
        GameManager.HandleShipDestroyed(this);
        /*
         TODO: INSERT VFX HERE
         */


        StartCoroutine(DestroyAfterDelay());
    }

    public override void DestroyEntity()
    {
        canHandleCollisions = false;
        goModel.SetActive(false);
        if (myCollider) // not sure why this is needed but there's an error without it
            myCollider.enabled = false;

        GameManager.HandleShipDestroyed(this);
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
