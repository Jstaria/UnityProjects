using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    [SerializeField] private GameObject room;
    [SerializeField] private int numberOfRooms = 5;

    // The minimum space between room spawns
    [SerializeField] private float minDistance;

    private List<GameObject> rooms = new List<GameObject> ();

    // Start is called before the first frame update
    void Update()
    {
       SetupRoomLayout();
    }

    private void SetupRoomLayout()
    {
        // First room location will always be 0,0
        if (rooms.Count == 0)
        {
            rooms.Add(Instantiate(room, Vector3.zero, Quaternion.identity));
        }

        if (rooms.Count < numberOfRooms)
        {
            int ranX = Random.Range(-20, 20);
            int ranY = Random.Range(-20, 20);

            rooms.Add(Instantiate(room, new Vector3(ranX, ranY, 0), Quaternion.identity));
        }
    }

    /// <summary>
    /// O(n^2) check method
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool CheckDistance(int x, int y, int index)
    {
        for (int i = 0; i < numberOfRooms; i++)
        {
            for (int j = 0; j < numberOfRooms; i++)
            {
                if (i != j) 
                {
                    
                }
            }
        }

        return false;
    }
}
