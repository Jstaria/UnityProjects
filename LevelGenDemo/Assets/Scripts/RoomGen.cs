using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomGen : MonoBehaviour
{
    [SerializeField] private GameObject roomFloor;
    [SerializeField] private LineRenderer lineRenderer;

    private List<GameObject> connectedGameObj = new List<GameObject>();

    internal bool IsVisited { get; set; }
    internal bool IsPermenant { get; set; }
    internal bool RoomFinished { get; private set; }
    internal GameObject RoomFloor { get { return roomFloor; } }

    private void Start()
    {
        lineRenderer.SetPosition(0, transform.position + new Vector3(0, 0, 2));
    }

    public void SetupRoom(int ranWidth, int ranHeight) 
    {
        roomFloor = Instantiate(roomFloor, transform.position + new Vector3(0, 0, 1), Quaternion.identity);
        roomFloor.transform.localScale = new Vector3(ranWidth, ranHeight, 0);
        roomFloor.transform.parent = transform;

        RoomFinished = true;
    }

    public void BeDestroyed()
    {
        if (!RoomFinished) { return; }
        GameObject.Destroy(roomFloor);
    }

    public void AddLine(GameObject gameObj)
    {
        Vector3 position = gameObj.transform.position;
        connectedGameObj.Add(gameObj);
        lineRenderer.positionCount = lineRenderer.positionCount + 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position + new Vector3(0, 0, 2));
    }
    public void SetLine(Vector3 position)
    {
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position + new Vector3(0, 0, 2));
    }
}
