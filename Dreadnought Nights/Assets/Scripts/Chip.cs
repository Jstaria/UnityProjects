using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chip : MonoBehaviour
{
    public int value;
    public bool inBank;
    private Vector3 endingPosition;
    private float lerpSpeed;
    private float desiredLerpSpeed;
    public Camera cam;
    public LayerMask chip;

    public RayCheck rayCheck;

    public bool isInCooldown;
    public bool isStationary;

    private void Awake()
    {
        cam = Camera.main;
        isInCooldown = false;
        isStationary = true;
    }

    public bool CheckMouseClicked()
    {
        return Input.GetMouseButtonDown(0) && rayCheck.CheckMouseOver() && !isInCooldown;
    }

    internal IEnumerator MoveTo(Vector3 position, float startingLerpSpeed, float desiredLerpSpeed)
    {
        isStationary = false;
        endingPosition = position;
        lerpSpeed = startingLerpSpeed;
        this.desiredLerpSpeed = desiredLerpSpeed;

        while (Vector3.Distance(transform.position, endingPosition) > .01f)
        {
            lerpSpeed = Mathf.Lerp(desiredLerpSpeed, lerpSpeed, Mathf.Pow(.5f, 5 * Time.deltaTime));

            float lerp = Mathf.Pow(.5f, Time.deltaTime * lerpSpeed);
            transform.position = Vector3.Lerp(endingPosition, transform.position, lerp);

            yield return new WaitForEndOfFrame();
        }

        isStationary = true;
        yield return null;
    }

    internal IEnumerator MoveSequence(Vector3 position, float startingLerpSpeed, float desiredLerpSpeed)
    {
        isInCooldown = true;
        Vector3 up = new Vector3(0, .5f, 0);

        StartCoroutine(MoveTo(up, startingLerpSpeed / 2, desiredLerpSpeed / 2));

        //yield return StartCoroutine(MoveTo(transform.position + (position - transform.position) * .75f  + up, startingLerpSpeed * 2, desiredLerpSpeed));

        yield return StartCoroutine(MoveTo(position + up, startingLerpSpeed * 2, desiredLerpSpeed));

        yield return StartCoroutine(MoveTo(position, startingLerpSpeed, desiredLerpSpeed));

        isInCooldown = false;
    }
}
