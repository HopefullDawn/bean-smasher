using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject Player;
    public Vector3 offset;
    void Start()
    {
        offset = new Vector3(0.00f, 9.46f, 11.86f);
        transform.position = Player.transform.position + offset;
    }
    void LateUpdate()
    {
        if (Player != null)
        {
            Vector3 targetPosition = Player.transform.position + offset;
            transform.position = targetPosition;
        }
    }
}
