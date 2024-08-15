using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum FaceState
{
    SmilingTalking,
    Smiling,
    Pondering,
    Sad
}

public enum BodyState
{
    ArmsOut,
    ArmsIn,
    Pondering,
}

[RequireComponent(typeof(Shadow))]
public class GuideFaceController : MonoBehaviour
{
    [SerializeField] private List<Sprite> faces;
    [SerializeField] private List<Sprite> bodies;

    private FaceState currentFace;
    private BodyState currentBody;

    private GameObject body;
    private GameObject face;

    private Shadow shadow;

    private void Start()
    {
        shadow = GetComponent<Shadow>();
        SetLooks(FaceState.SmilingTalking, BodyState.ArmsOut);
    }

    public void SetLooks(FaceState face, BodyState body)
    {
        currentFace = face;
        currentBody = body;

        UpdateBody();
    }

    private void UpdateBody()
    {
        GameObject.Destroy(body);
        body = new GameObject("Body", typeof(SpriteRenderer));
        body.GetComponent<SpriteRenderer>().sprite = bodies[(int)currentBody];
        body.transform.parent = transform;
        body.transform.position = transform.position;
        body.GetComponent<SpriteRenderer>().sortingOrder = 3;

        GameObject.Destroy(face);
        face = new GameObject("Face", typeof(SpriteRenderer));
        face.GetComponent<SpriteRenderer>().sprite = faces[(int)currentFace];
        face.transform.parent = transform;
        face.transform.position = transform.position;
        face.GetComponent<SpriteRenderer>().sortingOrder = 4;

        List<GameObject> list = new List<GameObject>() { face, body };

        shadow.SetShadow(list);
    }

    public void Squiggle(float size, float magnitude, float time, float rotationMagnitude, float randOffset)
    {
        body.transform.localScale = new Vector3(
            size + Mathf.Cos(time * Mathf.PI * 2) * magnitude,
            size + Mathf.Sin(time * Mathf.PI * 2 + randOffset) * magnitude,
            0);
        body.transform.localRotation = Quaternion.Euler(new Vector3(
            Mathf.Cos(time * Mathf.PI * 2) * rotationMagnitude,
            Mathf.Sin(time * Mathf.PI * 2) * rotationMagnitude,
            Mathf.Cos(time * Mathf.PI * 2 + randOffset) * rotationMagnitude));

        face.transform.localScale = new Vector3(
            size + Mathf.Cos(time * Mathf.PI * 2) * magnitude,
            size + Mathf.Sin(time * Mathf.PI * 2 + .75f + randOffset) * magnitude,
            0);
        face.transform.localRotation = Quaternion.Euler(new Vector3(
            Mathf.Cos(time * Mathf.PI * 2) * rotationMagnitude,
            Mathf.Sin(time * Mathf.PI * 2) * rotationMagnitude,
            Mathf.Cos(time * Mathf.PI * 2 + .75f + randOffset) * rotationMagnitude));
    }
}
