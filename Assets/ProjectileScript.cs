using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public GameObject transformer;

    // Update is called once per frame
    void FixedUpdate()
    {
        transformer.transform.Rotate(new Vector3(0, 0, Time.fixedDeltaTime * 40));

    }
}
