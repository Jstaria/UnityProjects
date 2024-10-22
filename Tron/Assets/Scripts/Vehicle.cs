using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private List<GameObject> Balls;
    [SerializeField] private List<Transform> ballAttachPoints;

    private List<Spring> springs;

    [SerializeField] private float angularVelocity;
    [SerializeField] private float dampingRatio;

    [SerializeField] private Rigidbody vehicleBody;

    private void Start()
    {
        springs = new List<Spring>();

        for (int i = 0; i < Balls.Count; i++)
        {
            float distance = Vector3.Distance(ballAttachPoints[i].position, Balls[i].transform.position);

            springs.Add(new Spring(angularVelocity, dampingRatio, distance));
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Spring spr in springs)
        {
            spr.Update();
        }

        for (int i = 0; i < Balls.Count; i++)
        {
            Balls[i].transform.position = new Vector3(Balls[i].transform.position.x, springs[i].Position, Balls[i].transform.position.z);

            vehicleBody.AddForceAtPosition(Vector3.up * springs[i].Velocity, Balls[i].transform.position);
        }
    }
}
