using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class TableUIHandler : MonoBehaviour
{
    public UnityEvent OnClick;

    public bool isActive;

    public Material M_glow;

    public float lerpSpeedStart = 10;
    public float lerpSpeedEnd = 1;
    public float lerpSpeedStep = .75f;

    private float currentLerp;

    public LayerMask UI;
    public Camera cam;

    public float minGlowIntensity = 3;
    public float maxGlowIntensity = 50;

    private float glowIntensity;
    private float sinceClick;

    public AudioManager audioManager;

    private void Start()
    {
        isActive = false;

        SetGlowOFF();
        currentLerp = lerpSpeedStart;

        OnClick.AddListener(Flash);
        OnClick.AddListener(SetClickTime);
    }

    private void Update()
    {
        Highlight();

        if (CheckMouseClicked()) OnClick.Invoke();

        ChangeIntensity();
    }

    private void Flash()
    {
        M_glow.SetFloat("_Intensity", maxGlowIntensity + 20);
    }

    private void ChangeIntensity()
    {
        if (isActive) currentLerp = lerpSpeedStart;
        else currentLerp = lerpSpeedEnd;

        float lerp = Mathf.Pow(.5f, Time.deltaTime * currentLerp);
        M_glow.SetFloat("_Intensity", Mathf.Lerp(glowIntensity, M_glow.GetFloat("_Intensity"), lerp));
    }

    private void Highlight()
    {
        if (Time.time - sinceClick < .1f) return;

        glowIntensity = CheckMouseOver() ? maxGlowIntensity : minGlowIntensity;

        if (!isActive) glowIntensity = maxGlowIntensity;
    }

    private bool CheckMouseClicked()
    {
        return isActive && Input.GetMouseButtonDown(0) && CheckMouseOver();
    }

    private IEnumerator ChangeActiveState()
    {
        isActive = false;

        yield return new WaitForSeconds(1.25f);

        isActive = true;
    }

    private void SetClickTime()
    {
        sinceClick = Time.time;
    }

    private bool CheckMouseOver()
    {
        bool wasHit = false;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;

        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, UI))
        {
            TableUIHandler UIHandler = hit.collider.GetComponent<TableUIHandler>();
            if (UIHandler != null && UIHandler == this)
            {
                wasHit = true;
            }
        }

        return wasHit;
    }

    public void BeginStartGlow()
    {
        if (isActive) return;

        StartCoroutine(StartGlow());
    }

    public IEnumerator StartGlow()
    {
        float currentLerp = lerpSpeedStart;

        while (M_glow.GetFloat("_ClippingValue") <= .91)
        {
            float lerp = Mathf.Pow(.5f, Time.deltaTime * currentLerp);

            float clip = Mathf.Lerp(1, M_glow.GetFloat("_ClippingValue"), lerp);

            M_glow.SetFloat("_ClippingValue", clip);

            currentLerp = Mathf.Clamp(currentLerp * lerpSpeedStep * Time.deltaTime, lerpSpeedEnd, lerpSpeedStart);

            if (M_glow.GetFloat("_ClippingValue") >= .85) isActive = true;

            yield return new WaitForEndOfFrame();
        }

        
    }

    public void BeginEndGlow()
    {
        if (!isActive) return;

        StopAllCoroutines();

        StartCoroutine(EndGlow());
    }

    private IEnumerator EndGlow()
    {
        isActive = false;

        float currentLerp = lerpSpeedStart;

        while (M_glow.GetFloat("_ClippingValue") >= .09)
        {
            float lerp = Mathf.Pow(.5f, Time.deltaTime * currentLerp);

            float clip = Mathf.Lerp(0, M_glow.GetFloat("_ClippingValue"), lerp);

            M_glow.SetFloat("_ClippingValue", clip);

            currentLerp = Mathf.Clamp(currentLerp * lerpSpeedStep * Time.deltaTime, lerpSpeedEnd, lerpSpeedStart);

            yield return new WaitForEndOfFrame();
        }
    }

    private void SetGlowOFF()
    {
        M_glow.SetFloat("_ClippingValue", 0);
    }
}
