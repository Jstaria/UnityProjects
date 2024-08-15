using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardInfo : MonoBehaviour
{
    public Card card;

    public Vector3 DesiredPos { get; set; }
    public float LerpSpeed { get; set; }
    public bool InFocus { get; set; }

    private void Update()
    {
        transform.position = Vector3.Lerp(DesiredPos, transform.position, Mathf.Pow(.5f, LerpSpeed * Time.deltaTime));
    }

    public float DistanceFromCenter(Vector3 position)
    {
        return position.x - transform.position.x;
    }
}
