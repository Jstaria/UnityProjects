using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardSuit suit;
    public int value;
    public bool isAce;

    private float lerpSpeed;
    private float desiredLerpSpeed;

    private Vector3 endingPosition;
    private Vector3 endingRotation;

    public bool isFlipped;

    internal IEnumerator MoveTo(Vector3 position, Vector3 rotation, float startingLerpSpeed, float desiredLerpSpeed)
    {
        endingPosition = position;
        endingRotation = rotation;
        lerpSpeed = startingLerpSpeed;
        this.desiredLerpSpeed = desiredLerpSpeed;

        while (Vector3.Distance(transform.position,endingPosition) > .01f)
        {
            lerpSpeed = Mathf.Lerp(desiredLerpSpeed, lerpSpeed, Mathf.Pow(.5f, 5 * Time.deltaTime));

            float lerp = Mathf.Pow(.5f, Time.deltaTime * lerpSpeed);
            transform.position = Vector3.Lerp(endingPosition, transform.position, lerp);

            lerp = Mathf.Pow(.5f, Time.deltaTime * lerpSpeed);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(endingRotation, transform.rotation.eulerAngles, lerp));

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    internal IEnumerator MoveTo(Vector3 position, Quaternion rotation, float startingLerpSpeed, float desiredLerpSpeed)
    {
        endingPosition = position;
        endingRotation = rotation.eulerAngles;
        lerpSpeed = startingLerpSpeed;
        this.desiredLerpSpeed = desiredLerpSpeed;

        while (Vector3.Distance(transform.position, endingPosition) > .01f)
        {
            lerpSpeed = Mathf.Lerp(desiredLerpSpeed, lerpSpeed, Mathf.Pow(.5f, 5 * Time.deltaTime));

            float lerp = Mathf.Pow(.5f, Time.deltaTime * lerpSpeed);
            transform.position = Vector3.Lerp(endingPosition, transform.position, lerp);

            lerp = Mathf.Pow(.5f, Time.deltaTime * lerpSpeed);
            transform.rotation = Quaternion.Lerp(rotation, transform.rotation, lerp);

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}
