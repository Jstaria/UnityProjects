using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    [SerializeField] private GameObject room;
    [SerializeField] private int numberOfRooms;

    // The minimum space between room spawns
    [SerializeField] private float minDistance;

    private List<GameObject> rooms = new List<GameObject> ();

    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void SetupRoomLayout()
    {
        // First room location will always be 0,0
        rooms.Add(Instantiate(room, Vector3.zero, Quaternion.identity));

        for (int i = 1; i < numberOfRooms; i++)
        {

        }
    }
}
