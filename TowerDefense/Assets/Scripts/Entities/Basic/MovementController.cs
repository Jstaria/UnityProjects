using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    // speed
    // object moving
    // direction
    // position

    private Vector3 position;
    private Vector3 direction;
    internal Vector3 Direction { get { return direction; } set { direction = value.normalized; } }
    public Vector3 Position { set { position = value; } }

    [SerializeField] private BoundsInfo floorBounds;
    //[SerializeField] private CamController camCon;
    [SerializeField] private SprintController sprintCon;
    [SerializeField] private float speed = 5f;

    private Camera cam;
    private float currentSpeed;
    private float totalCamHeight;
    private float totalCamWidth;

    private Vector3 vel;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = speed;
        position = transform.position;

        cam = Camera.main;

        totalCamHeight = cam.orthographicSize;
        totalCamWidth = totalCamHeight * cam.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if (!sprintCon.CanSprint || !sprintCon.IsSprinting)
        {
            currentSpeed = speed;
        }
        else
        {
            currentSpeed = sprintCon.SprintSpeed;
        }
        vel = direction * currentSpeed * Time.deltaTime;

        position += vel;

        RestrictMovement();

        transform.position = position;

    }

    private void RestrictMovement()
    {
        position.x = Mathf.Clamp(position.x, floorBounds.xMin, floorBounds.xMax - 1);
        position.z = Mathf.Clamp(position.z, floorBounds.yMin, floorBounds.yMax - 1);

        //Vector3 camPos = position;
        //
        //float camMinX = floorBounds.xMin + totalCamWidth;
        //float camMaxX = floorBounds.xMax - totalCamWidth;
        //float camMinY = floorBounds.yMin + totalCamHeight;
        //float camMaxY = floorBounds.yMax - totalCamHeight;
        //
        //camPos.x = Mathf.Clamp(position.x, camMinX, camMaxX);
        //camPos.y = Mathf.Clamp(position.y, camMinY, camMaxY);

        //camCon.UpdateCamPos(camPos);
    }
}
