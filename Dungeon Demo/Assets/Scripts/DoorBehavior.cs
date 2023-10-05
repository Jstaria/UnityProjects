using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    private Vector2 tpPos;
    [SerializeField] GameObject player;

    // Start is called before the first frame update

    private void Update()
    {
        player = GameObject.Find("Player(Clone)");

        if (CheckCollision(player.transform.position))
        {
            player.transform.position = (Vector3)tpPos + new Vector3(0,0,-5);
        }
    }

    public void CreateDoor(Vector2 tpPos)
    {
        this.tpPos = tpPos;
    }

    public bool CheckCollision(Vector3 playerPos)
    {
        Vector2 scale = transform.localScale;

        return
            playerPos.x > transform.position.x - scale.x / 2 && playerPos.x <= transform.position.x + scale.x / 2 &&
            playerPos.y > transform.position.y - scale.y / 2 && playerPos.y <= transform.position.y + scale.y / 2;
    }
}
