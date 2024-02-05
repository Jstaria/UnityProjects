using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bounding box that player cannot pass through
/// </summary>
public class BoundingBox : MonoBehaviour
{
    private float scaleX;
    private float scaleY;

    private float xMin { get; set; }
    private float xMax { get; set; }
    private float yMin { get; set; }
    private float yMax { get; set; }

    /// <summary>
    /// returns Vector4 (xMin, xMax, yMin, yMax)
    /// </summary>
    public Vector4 Bounds { get { return new Vector4(xMin, xMax, yMin, yMax); } }

    /// <summary>
    /// Generates bounding box
    /// </summary>
    public void GenerateBB(int scaleX, int scaleY)
    {
        this.scaleX = scaleX;
        this.scaleY = scaleY;

        xMin = transform.position.x - transform.localScale.x * .5f;
        yMax = transform.position.y + transform.localScale.y * .5f;
        xMax = xMin + scaleX * transform.localScale.x;
        yMin = yMax - scaleY * transform.localScale.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        float localX = transform.localScale.x;
        float localY = transform.localScale.y;

        Gizmos.DrawLine(new Vector3(xMin, yMin), new Vector3(xMax, yMin));
        Gizmos.DrawLine(new Vector3(xMax, yMin), new Vector3(xMax, yMax));
        Gizmos.DrawLine(new Vector3(xMax, yMax), new Vector3(xMin, yMax));
        Gizmos.DrawLine(new Vector3(xMin, yMax), new Vector3(xMin, yMin));

        Gizmos.color = Color.yellow;

        for (int i = -1; i < scaleX - 1; i++)
        {
            if (i != -1) { Gizmos.DrawLine(new Vector3(i * localX + localX / 2 + transform.position.x, yMin), new Vector3(i * localX + localX / 2 + transform.position.x, yMax)); }

            for (int j = 0; j < scaleY - 1; j++)
            {
                Gizmos.DrawLine(new Vector3(xMin, -j * localY - localY / 2 + transform.position.y), new Vector3(xMax, -j * localY - localY / 2 + transform.position.y) );
            }
        }
    }
}
