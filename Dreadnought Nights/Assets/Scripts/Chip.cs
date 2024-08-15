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

    public bool isInCooldown;

    private void Awake()
    {
        cam = Camera.main;
        isInCooldown = false;
    }

    public bool CheckMouseClicked()
    {
        return Input.GetMouseButtonDown(0) && CheckMouseOver() && !isInCooldown;
    }

    private bool CheckMouseOver()
    {
        bool wasHit = false;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;

        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

        //Debug.Log("Rayed");

        if (Physics.Raycast(ray, out hit, 5, chip))
        {
            Chip chip = hit.collider.GetComponent<Chip>();
            if (chip != null && chip == this)
            {
                wasHit = true;
            }
        }

        return wasHit;
    }

    internal IEnumerator MoveTo(Vector3 position, float startingLerpSpeed, float desiredLerpSpeed)
    {
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
