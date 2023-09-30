using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomGen : MonoBehaviour
{
    [SerializeField] private GameObject roomFloor;
    internal bool RoomFinished { get; private set; }
    internal GameObject RoomFloor { get { return roomFloor; } }

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
}
