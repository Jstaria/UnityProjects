using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    private Vector3 velocity;
    private float speed;
    private float bounceY;
    private float bounceCheckTimer;

    private void Update()
    {
        Vector3 gravity = Vector3.down * 9.82f;

        velocity += gravity * Time.deltaTime;
        velocity.z = Mathf.Lerp(0, velocity.z, Mathf.Pow(.5f, Time.deltaTime * 2));

        transform.position -= velocity * speed * Time.deltaTime;

        bounceCheckTimer -= Time.deltaTime;

        if (CheckBounce() && bounceCheckTimer < 0)
        {
            velocity.y = -velocity.y * .75f;
            bounceCheckTimer = .05f;
        }
    }

    private bool CheckBounce()
    {
        Vector3 position = transform.position;
        position.y = Mathf.Clamp(position.y, bounceY, int.MaxValue);
        transform.position = position;

        return MathF.Round(position.y, 2) == MathF.Round(bounceY, 2);
    }

    public void StartBounceY(float initialSpeed, float yBounceLine)
    {
        velocity = UnityEngine.Random.insideUnitSphere * initialSpeed;
        velocity.y = 3;
        velocity.x = 0;
        velocity.z = Mathf.Clamp(velocity.z, -.5f, .5f);

        bounceY = yBounceLine;

        speed = initialSpeed;
    }
}
