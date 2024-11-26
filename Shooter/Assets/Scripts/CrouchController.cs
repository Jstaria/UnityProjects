using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchController : MonoBehaviour
{
    private Spring spring;

    [SerializeField] private float angularFrequency;
    [SerializeField] private float dampingRatio;
    [SerializeField] private float crouchOffset;
    [SerializeField] private float yScaleOffset;

    [SerializeField] private Camera cam;
    [SerializeField] private CharacterController playerBody;

    [SerializeField] private LayerMask headBonkLayers;
    [SerializeField] private PlayerMovement playerMov;

    public bool IsCrouched { get { return isCrouched; } set { isCrouched = value; } }

    private bool isCrouched;

    private float defaultScale;
    private float defaultCamY;

    // Start is called before the first frame update
    void Start()
    {
        spring = new Spring(angularFrequency, dampingRatio, 0, true);

        defaultScale = playerBody.height;
        defaultCamY = cam.transform.localPosition.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Ray ray = new Ray(transform.position, Vector3.up);

        bool isClearAbove = !Physics.Raycast(ray, defaultScale - spring.Position * yScaleOffset, headBonkLayers);
        isCrouched = Input.GetKey(KeyCode.C) && playerMov.IsGrounded;

        spring.RestPosition = isCrouched ? 1 : (isClearAbove ? 0 : 1);
        spring.Update();

        playerBody.height = defaultScale - spring.Position * yScaleOffset;
        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, defaultCamY - spring.Position * yScaleOffset, cam.transform.localPosition.z);

    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, transform.position + Vector3.up * (defaultScale - spring.Position * yScaleOffset));
    }
}
