using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suck : MonoBehaviour
{
    private Vector3 velocity;
    private float speed;
    private bool isBeingSucked;
    private Vector3 suckPosition;
    private Vector3 acceleration;

    private Spring suckSpringZ;
    private Spring suckSpringY;

    private void Start()
    {
        suckSpringZ = new Spring(5, 1, 0, true);
        suckSpringY = new Spring(5, 2, 0, true);
    }

    private void Update()
    {
        if (!isBeingSucked)
            return;

        suckSpringZ.Update();
        suckSpringY.Update();

        Vector3 position = transform.position;

        position = new Vector3(position.x, suckSpringY.Position, suckSpringZ.Position);

        transform.position = position;

                    //Vector3 horizontalAccel = new Vector3(0, 0, suckPosition.z - transform.position.z);

        //acceleration = horizontalAccel * 2 + Vector3.up;

        //velocity += acceleration * Time.deltaTime;

        //transform.Translate(velocity * speed * Time.deltaTime);
    }

    public void StartSuck(Vector3 suckPos, float speed)
    {
        isBeingSucked = true;
        suckPosition = suckPos;

        suckSpringZ.Position = transform.position.z;
        suckSpringY.Position = transform.position.y;

        suckSpringZ.RestPosition = suckPos.z;
        suckSpringY.RestPosition = suckPos.y;

        this.speed = speed;
    }
}
