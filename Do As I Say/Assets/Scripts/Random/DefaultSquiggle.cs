using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Wiggle), typeof(HitBox))]
public class DefaultWiggle : MonoBehaviour
{
    private Wiggle wiggle;
    private HitBox hitBox;

    public bool isIn;

    private void Awake()
    {
        hitBox = GetComponent<HitBox>();

        wiggle = GetComponent<Wiggle>();
        wiggle.OnSquiggle += Squiggle;
        wiggle.SizeScale = 1;
    }

    private void Update()
    {
        isIn = wiggle.WiggleMax = hitBox.ContainsScreenSpace(Input.mousePosition);
    }

    private void Squiggle(float size, float wigMag, float time, float rotMag, float randOffset)
    {
        this.transform.localScale = new Vector3(
            size + Mathf.Cos(time * Mathf.PI * 2) * wigMag,
            size + Mathf.Sin(time * Mathf.PI * 2 + randOffset) * wigMag,
            0);
        this.transform.localRotation = Quaternion.Euler(new Vector3(
            Mathf.Cos(time * Mathf.PI * 2) * rotMag,
            Mathf.Sin(time * Mathf.PI * 2) * rotMag,
            Mathf.Cos(time * Mathf.PI * 2 + randOffset) * rotMag));

    }
}
