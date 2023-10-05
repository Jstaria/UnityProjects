using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DungeonLoader : MonoBehaviour
{
    [SerializeField] DungeonGenerator generator;
    [SerializeField] GameObject player;

    [SerializeField] bool showFloor;

    private int[] prevRoom = new int[2];

    private void Start()
    {
        player = Instantiate(player, new Vector3(0,0,-5), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        int[] currentRoom = new int[2];

        bool currentlyInRoom = false;

        if (!generator.DoneGenerating) { return; }

        RoomGen room = generator.Rooms[prevRoom[0], prevRoom[1]].GetComponent<RoomGen>();

        bool prevFlag = false;

        if (CheckInRoom(room, player.transform.position))
        {
            prevFlag = true;
        }

        if (!prevFlag)
        {
            for (int i = 0; i < generator.Rooms.GetLength(0); i++)
            {
                for (int j = 0; j < generator.Rooms.GetLength(1); j++)
                {
                    RoomGen roomInfo = generator.Rooms[i, j].GetComponent<RoomGen>();

                    if (CheckInRoom(roomInfo, player.transform.position))
                    {
                        currentlyInRoom = true;
                        currentRoom = new int[2] { i, j };

                        generator.Rooms[i, j].SetActive(true);
                    }
                    else
                    {
                        if (!showFloor) { generator.Rooms[i, j].SetActive(false); }
                    }
                }
            }
        }

        if (currentlyInRoom)
        {
            Vector2[] directions = new Vector2[]
                {
                    new Vector2(0, 1), // Up
                    new Vector2(1, 0), // Right
                    new Vector2(0, -1), // Down
                    new Vector2(-1, 0), // Left
                };

            for (int i = 0; i < 4; i++)
            {
                if (currentRoom[0] + directions[i].x >= 0 && currentRoom[1] + directions[i].y >= 0 &&
                    currentRoom[0] + directions[i].x < generator.Rooms.GetLength(0) && currentRoom[1] + directions[i].y < generator.Rooms.GetLength(1) &&
                    generator.Rooms[currentRoom[0] + (int)directions[i].x, currentRoom[1] + (int)directions[i].y].GetComponent<RoomGen>().IsGenerated)
                {
                    generator.Rooms[currentRoom[0] + (int)directions[i].x, currentRoom[1] + (int)directions[i].y].SetActive(true);
                }
            }

            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, generator.Rooms[currentRoom[0], currentRoom[1]].transform.position + new Vector3(0,0,-10), .025f);
        }

        prevRoom = currentRoom;
    }

    private bool CheckInRoom(RoomGen room, Vector3 playerPos)
    {
        return
            playerPos.x > room.xMin && playerPos.x < room.xMax &&
            playerPos.y > room.yMin && playerPos.y < room.yMax;
    }
}
