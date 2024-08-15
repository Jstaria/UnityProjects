using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public enum MouseState
{
    Normal,
    Hover
}

public class MouseManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> mouseSprites;

    [SerializeField] private GameObject mouseObject;
    [SerializeField] private float size = .2f;

    private MouseState currentState;
    private MouseState prevState;

    private float transitionTime;

    private Wiggle wiggle;

    private void Start()
    {
        Cursor.visible = false;
        mouseObject = Instantiate(mouseObject);
        SetMouseState(MouseState.Normal);
        mouseObject.GetComponent<SpriteRenderer>().sortingOrder = 5;

        if (mouseObject.TryGetComponent<Wiggle>(out wiggle))
        {
            wiggle.OnSquiggle += Squiggle;
            wiggle.SizeScale = size;
        }
    }

    public void SetMouseState(MouseState state)
    {
        currentState = state;

        UpdateCursorSprite();
    }

    private void UpdateCursorSprite()
    {
        if (transitionTime > 0) return;

        if (prevState != currentState)
        {
            transitionTime = .2f;
        }


        mouseObject.GetComponent<SpriteRenderer>().sprite = mouseSprites[(int)currentState];
    }

    private void Squiggle(float size, float wigMag, float time, float rotMag, float randOffset)
    {
        mouseObject.transform.localScale = new Vector3(
            size + Mathf.Cos(time * Mathf.PI * 2) * wigMag,
            size + Mathf.Sin(time * Mathf.PI * 2 + randOffset) * wigMag,
            0);
        mouseObject.transform.localRotation = Quaternion.Euler(new Vector3(
            Mathf.Cos(time * Mathf.PI * 2) * rotMag,
            Mathf.Sin(time * Mathf.PI * 2) * rotMag,
            Mathf.Cos(time * Mathf.PI * 2 + randOffset) * rotMag));

    }

    public void Update()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = -1;

        transitionTime -= Time.deltaTime;

        mouseObject.transform.position = position;
        prevState = currentState;

        if (Input.GetMouseButtonDown((int)MouseButton.Left) && wiggle != null)
        {
            wiggle.OnClick();
        } 
    }
}
