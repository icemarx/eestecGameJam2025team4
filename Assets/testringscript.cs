using UnityEngine;

public class testringscript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    public float rotationSpeed = 100f; // Adjust the speed of rotation

    void Update()
    {
        // Check for input from A and D keys
        float rotationInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            rotationInput = 1f; // Rotate counter-clockwise
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotationInput = -1f; // Rotate clockwise
        }

        // Apply rotation to the object around the Y-axis
        transform.Rotate(0f, 0f, rotationInput * rotationSpeed * Time.deltaTime);
    }
}
