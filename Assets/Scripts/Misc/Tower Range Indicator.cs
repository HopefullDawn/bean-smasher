using UnityEngine;

public class TowerRangeIndicator : MonoBehaviour
{
    public Tower Tower;
    private float radius;
    private int segments = 60;

    void Start()
    {
            Tower = GetComponentInParent<Tower>(); // try to find the tower
            radius = Tower.currentRange/ transform.lossyScale.x; //calcilate the radius
            DrawCircle(); // draw a circle, duh
    }

    void DrawCircle()
    {
        LineRenderer line = GetComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.loop = true;
        line.positionCount = segments;

        float angle = 0f;

        for (int i = 0; i < segments; i++)
        {
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            line.SetPosition(i, new Vector3(x, -0.4f, z)); 
            angle += 2 * Mathf.PI / segments;
        }
    }
}
