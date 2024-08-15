using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Squish : MonoBehaviour
{
    public float squishStrength = 1;
    public float squishTime = .5f;
    public float squishSpeed = 1;

    private float currentSquishStrength;
    private float currentSquishTime;

    public void OnClick()
    {
        currentSquishStrength = squishStrength;

        StartCoroutine(SquishTransform());
    }

    private void Update()
    {


    }

    private IEnumerator SquishTransform()
    {
        while (currentSquishStrength > 0)
        {
            currentSquishStrength -= Time.deltaTime;

            this.transform.localScale = new Vector3(
            1 + Mathf.Cos(Time.time * Mathf.PI * squishSpeed) / 8 * currentSquishStrength,
            1 + Mathf.Sin(Time.time * Mathf.PI * squishSpeed) / 8 * currentSquishStrength,
            1 + Mathf.Cos(Time.time * Mathf.PI * squishSpeed) / 8 * currentSquishStrength);

            yield return null;
        }
    }
}
