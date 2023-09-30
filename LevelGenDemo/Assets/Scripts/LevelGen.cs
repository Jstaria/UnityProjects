using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    [SerializeField] private GameObject roomPoint;
    [SerializeField] private GameObject mapFloor;
    [SerializeField] private GameObject roomFloor;

    [Header("Floor")]
    [SerializeField][Range(1, 100)] private int floorHeight;
    [SerializeField][Range(1, 100)] private int floorWidth;
    [SerializeField][Range(1, 100)] private int numberOfRooms = 5;

    [Header("Room")]
    [SerializeField][Range(4, 20)] private int maxRoomWidth = 10;
    [SerializeField][Range(4, 20)] private int maxRoomHeight = 10;
    [SerializeField][Range(4, 10)] private int minRoomWidth = 10;
    [SerializeField][Range(4, 10)] private int minRoomHeight = 10;

    // The minimum space between room spawns
    [SerializeField] private float minDistance;

    [SerializeField] private bool ToggleRegeneration = false;

    private int roomSpawnTries = 0;
    private int trueNumberRooms;
    private List<GameObject> roomPoints = new List<GameObject>();
    private int currentRoomIndex = 0;
    private Transform parentContainer;

    private void Start()
    {
        trueNumberRooms = numberOfRooms;
        parentContainer = GameObject.Find("FloorGen").transform;
        mapFloor = Instantiate(mapFloor, new Vector3(0, 0, 10), Quaternion.identity);
        mapFloor.transform.parent = parentContainer;
    }

    // Start is called before the first frame update
    void Update()
    {
        if (ToggleRegeneration)
        {
            SetupRoomLayout();
            SpawnRooms();
        }
        else
        {
            numberOfRooms = trueNumberRooms;
            currentRoomIndex = 0;

            for (int i = 0; i < roomPoints.Count; i++)
            {
                roomPoints[i].GetComponent<RoomGen>().BeDestroyed();
                GameObject.Destroy(roomPoints[i]);
            }

            roomPoints.Clear();
        }
    }

    private void SetupRoomLayout()
    {
        mapFloor.transform.localScale = new Vector3(-floorWidth, -floorHeight, 0);

        // So it doesn't always have to run all of this code
        if (roomPoints.Count == numberOfRooms) { return; }

        // First room location will always be 0,0
        if (roomPoints.Count == 0)
        {
            roomPoints.Add(Instantiate(roomPoint, Vector3.zero, Quaternion.identity));
            roomPoints[0].transform.parent = parentContainer;
        }

        // Generates up to number of rooms allowed
        if (roomPoints.Count < numberOfRooms)
        {
            int ranX = Random.Range(-floorWidth / 2, floorWidth / 2);
            int ranY = Random.Range(-floorHeight / 2, floorHeight / 2);

            if (CheckDistance(ranX, ranY))
            {
                roomPoints.Add(Instantiate(roomPoint, new Vector3(ranX, ranY, 0), Quaternion.identity));
                roomPoints[roomPoints.Count - 1].transform.parent = parentContainer;
            }
            else { roomSpawnTries++; }

            if (roomSpawnTries > 300) { numberOfRooms = roomPoints.Count; roomSpawnTries = 0; }
        }

        // Destroys any excess rooms, mainly for visual/debugging purposes, pretty to look at
        else if (roomPoints.Count > numberOfRooms)
        {
            roomPoints[roomPoints.Count - 1].GetComponent<RoomGen>().BeDestroyed();
            GameObject.Destroy(roomPoints[roomPoints.Count - 1]);
            roomPoints.RemoveAt(roomPoints.Count - 1);
        }
    }

    /// <summary>
    /// Spacing check method
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool CheckDistance(int x, int y)
    {
        for (int j = 0; j < roomPoints.Count; j++)
        {
            if ((roomPoints[j].transform.position - new Vector3(x, y, 0)).sqrMagnitude < Mathf.Pow(minDistance, 2))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Stand in random room sizes for hand crafted rooms
    /// </summary>
    private void SpawnRooms()
    {
        if (roomPoints.Count != numberOfRooms) { return; }

        if (currentRoomIndex == numberOfRooms) { return; }

        int ranWidth = Random.Range(minRoomWidth / 2, maxRoomWidth / 2 + 1) * 2;
        int ranHeight = Random.Range(minRoomHeight / 2, maxRoomHeight / 2 + 1) * 2;
            
        int stuckCount = 0;

        if (currentRoomIndex != 0)
        {
            // I dont think check for overlap is currently working as I intended
            while (CheckOverlap(ranWidth, ranHeight))
            {
                ranWidth = Random.Range(minRoomWidth / 2, maxRoomWidth / 2 + 1) * 2;
                ranHeight = Random.Range(minRoomHeight / 2, maxRoomHeight / 2 + 1) * 2;

                stuckCount++;

                if (stuckCount > 100)
                {
                    GameObject.Destroy(roomPoints[currentRoomIndex]);
                    roomPoints.RemoveAt(currentRoomIndex);
                    numberOfRooms = roomPoints.Count;
                    return;
                }
            }
        }

        roomPoints[currentRoomIndex].GetComponent<RoomGen>().SetupRoom(ranWidth, ranHeight);

        currentRoomIndex++;
    }

    private bool CheckOverlap(int width, int height)
    {
        float xMin = roomPoints[currentRoomIndex].transform.position.x - width / 2 - 1;
        float xMax = roomPoints[currentRoomIndex].transform.position.x + width / 2 + 1;
        float yMax = roomPoints[currentRoomIndex].transform.position.y + height / 2 + 1;
        float yMin = roomPoints[currentRoomIndex].transform.position.y - height / 2 - 1;

        for (int i = 0; i < currentRoomIndex; i++)
        {
            Vector3 scale = roomPoints[i].GetComponent<RoomGen>().RoomFloor.transform.localScale;

            float xMin_ = roomPoints[i].transform.position.x - scale.x / 2;
            float xMax_ = roomPoints[i].transform.position.x + scale.x / 2;
            float yMax_ = roomPoints[i].transform.position.y + scale.y / 2;
            float yMin_ = roomPoints[i].transform.position.y - scale.y / 2;

            if (xMin < xMax_ && xMax > xMin_ &&
                yMin < yMax_ && yMax > yMin_)
            {
                return true;
            }
        }
        return false;
    }

}
