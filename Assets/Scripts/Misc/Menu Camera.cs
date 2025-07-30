using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed = 50.0f;

    void Update()
    {
        transform.Rotate(-Vector3.up, rotationSpeed * Time.deltaTime); // spin the camera around the center point
    }
}
