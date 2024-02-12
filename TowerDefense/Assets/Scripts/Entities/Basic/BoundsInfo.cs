using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Creates a bounding box
/// </summary>
public class BoundsInfo : MonoBehaviour
{
    [SerializeField] float boundingBoxSize = .1f;
    public float xMin { get; private set; }
    public float xMax { get; private set; }
    public float yMin { get; private set; }
    public float yMax { get; private set; }

    // Update is called once per frame
    void Awake()
    {
        Vector2 position = transform.position;
        xMin = position.x - boundingBoxSize / 2;
        xMax = position.x + boundingBoxSize / 2;
        yMin = position.y - boundingBoxSize / 2;
        yMax = position.y + boundingBoxSize / 2;
    }

    void Update()
    {
        Vector2 position = transform.position;
        xMin = position.x - boundingBoxSize / 2;
        xMax = position.x + boundingBoxSize / 2;
        yMin = position.y - boundingBoxSize / 2;
        yMax = position.y + boundingBoxSize / 2;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(boundingBoxSize, boundingBoxSize));
    }
}
