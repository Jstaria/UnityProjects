using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class Wiggle : MonoBehaviour
{
    private float count;

    private float wigMag = .05f;
    private float rotMag = 2f;
    private float speed = 1;

    private float lerpSpeed;
    private float randOffset;

    public delegate void Squiggle(float size, float wiggleMag, float time, float rotationMag, float randOffset);

    public event Squiggle OnSquiggle;

    [SerializeField] private float maxWiggleMagnitude = .05f;
    [SerializeField] private float minWiggleMagnitude = .01f;
    [SerializeField] private float wiggleOffset = .1f;
    [SerializeField] private float maxRotationMagnitude = 2;
    [SerializeField] private float minRotationMagnitude = .2f;
    [SerializeField] private float maxSpeed = 1;
    [SerializeField] private float minSpeed = .5f;
    [SerializeField] private float defaultLerpSpeed = 7;
    [SerializeField] private float clickedLerpSpeed = 4;

    public bool WiggleMax { get; set; }
    public bool LerpMax { get; set; }
    public float SizeScale { get; set; }

    private void Start()
    {
        randOffset = Random.Range(0, 100);

        if (OnSquiggle != null)
        {
            OnSquiggle(SizeScale, wigMag, count, rotMag, randOffset);
        }
    }

    private void Update()
    {
        count += Time.deltaTime * speed;

        lerpSpeed = LerpMax ? clickedLerpSpeed : defaultLerpSpeed;

        float blend = 1 - MathF.Pow(.5f, lerpSpeed * Time.deltaTime);

        if (WiggleMax)
        {
            wigMag = Mathf.Lerp(wigMag, maxWiggleMagnitude, blend);
            rotMag = Mathf.Lerp(rotMag, maxRotationMagnitude, blend);
            speed = Mathf.Lerp(speed, maxSpeed, lerpSpeed);
        }

        else
        {
            wigMag = Mathf.Lerp(wigMag, minWiggleMagnitude, blend);
            rotMag = Mathf.Lerp(rotMag, minRotationMagnitude, blend);
            speed = Mathf.Lerp(speed, minSpeed, blend);
        }

        if (OnSquiggle != null)
        {
            OnSquiggle(SizeScale, wigMag, count, rotMag, randOffset);
        }
    }

    public void OnClick()
    {
        wigMag = maxWiggleMagnitude + wiggleOffset;
        LerpMax = true;
    }
}
