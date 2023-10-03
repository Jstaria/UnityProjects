using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private int numberOfRooms;
    [SerializeField] private GameObject tileSample;
    [SerializeField] private GameObject room;

    [SerializeField] private int minRoomWidth;
    [SerializeField] private int maxRoomWidth;
    [SerializeField] private int minRoomHeight;
    [SerializeField] private int maxRoomHeight;

    private List<GameObject> rooms = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        while (rooms.Count < numberOfRooms)
        {
            int x, y, wRan, hRan;
            int[] AvailableDoorSpots = new int[] { 0, 0, 0, 0 };

            do
            {
                wRan = Random.Range(minRoomWidth / 2, maxRoomWidth / 2) * 2;
                hRan = Random.Range(minRoomHeight / 2, maxRoomHeight / 2) * 2;

                
            }
            while (CheckOverlap(0, 0, wRan, hRan));

            int totalRooms = 0;

            for (int i = 0; i < AvailableDoorSpots.Length; i++)
            {
                totalRooms += AvailableDoorSpots[i];
            }

            if (totalRooms == 0 || totalRooms == 1) { continue; }

            rooms.Add(Instantiate(room));
            rooms[rooms.Count - 1].GetComponent<RoomGen>().GenerateRoomGrid(0, 0, wRan, hRan, AvailableDoorSpots);
            rooms[rooms.Count - 1].transform.parent = transform;
        }
    }

    private bool CheckOverlap(int x, int y, int width, int height)
    {
        //float xMin = roomPoints[currentRoomIndex].transform.position.x - width / 2 - 1;
        //float xMax = roomPoints[currentRoomIndex].transform.position.x + width / 2 + 1;
        //float yMax = roomPoints[currentRoomIndex].transform.position.y + height / 2 + 1;
        //float yMin = roomPoints[currentRoomIndex].transform.position.y - height / 2 - 1;

        //for (int i = 0; i < currentRoomIndex; i++)
        //{
        //    Vector3 scale = roomPoints[i].GetComponent<RoomGen>().RoomFloor.transform.localScale;

        //    float xMin_ = roomPoints[i].transform.position.x - scale.x / 2;
        //    float xMax_ = roomPoints[i].transform.position.x + scale.x / 2;
        //    float yMax_ = roomPoints[i].transform.position.y + scale.y / 2;
        //    float yMin_ = roomPoints[i].transform.position.y - scale.y / 2;

        //    if (xMin < xMax_ && xMax > xMin_ &&
        //        yMin < yMax_ && yMax > yMin_)
        //    {
        //        return true;
        //    }
        //}
        return false;
    }
}
