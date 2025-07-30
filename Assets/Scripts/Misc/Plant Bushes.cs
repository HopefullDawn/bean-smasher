using UnityEngine;

public class PlantBushes : MonoBehaviour
{
    void Start()
    {
        Vector3 currentPosition = transform.position - new Vector3(0, 1.08f, 0); // sink bushes into the ground (instantiating a bush makes it above ground, thanks Unity)
        transform.position = currentPosition;
    }
}
