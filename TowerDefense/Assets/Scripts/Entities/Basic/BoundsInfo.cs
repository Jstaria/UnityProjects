using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Creates a bounding box
/// </summary>
public class BoundsInfo : MonoBehaviour
{
    [SerializeField] float boundingBoxSizeX = .1f;
    [SerializeField] float boundingBoxSizeY = .1f;
    public float xMin { get; private set; }
    public float xMax { get; private set; }
    public float yMin { get; private set; }
    public float yMax { get; private set; }

    // Update is called once per frame
    void Awake()
    {
        Vector2 position = transform.position;
        xMin = position.x - boundingBoxSizeX / 2;
        xMax = position.x + boundingBoxSizeX / 2;
        yMin = position.y - boundingBoxSizeY / 2;
        yMax = position.y + boundingBoxSizeY / 2;
    }

    void Update()
    {
        Vector2 position = transform.position;
        xMin = position.x - boundingBoxSizeX / 2;
        xMax = position.x + boundingBoxSizeX / 2;
        yMin = position.y - boundingBoxSizeY / 2;
        yMax = position.y + boundingBoxSizeY / 2;
    }

    public void SetBounds(float width, float height)
    {
        boundingBoxSizeX = width;
        boundingBoxSizeY = height;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(boundingBoxSizeX, 0, boundingBoxSizeY));
    }
}
