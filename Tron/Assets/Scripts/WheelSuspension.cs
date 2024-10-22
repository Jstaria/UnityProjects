using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSuspension : MonoBehaviour
{
    [SerializeField] private Transform restPosition;
    [SerializeField] private Transform attachPosition;

    [SerializeField] private float angularVelocity;
    [SerializeField] private float dampingRatio;

    private Spring horizontalSpring;
    private Spring verticalSpring;

    [SerializeField] private LayerMask ground;

    [SerializeField] private Rigidbody vehicleBody;

    // Start is called before the first frame update
    void Start()
    {
        horizontalSpring = new Spring(angularVelocity, dampingRatio, restPosition.position.x);
        verticalSpring = new Spring(angularVelocity, dampingRatio, restPosition.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSprings();
    }

    private void UpdateSprings()
    {
        horizontalSpring.Update();
        verticalSpring.Update();

        CheckGroundCollision();

        transform.position = new Vector3(horizontalSpring.Position, verticalSpring.Position);
    }

    private void CheckGroundCollision()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 1, ground))
        {
            vehicleBody.AddForceAtPosition(new Vector3(horizontalSpring.Velocity, verticalSpring.Velocity), attachPosition.position);
        }
    }
}
