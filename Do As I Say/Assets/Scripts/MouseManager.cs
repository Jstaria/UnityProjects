using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum MouseState
{
    Normal,
    Hover
}

public class MouseManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> mouseSprites;

    GameObject mouseObject;

    private MouseState currentState;
    private MouseState prevState;

    private float transitionTime;

    private void Start()
    {
        Cursor.visible = false;
        mouseObject = new GameObject("Cursor", typeof(SpriteRenderer));
        SetMouseState(MouseState.Normal);
        mouseObject.GetComponent<SpriteRenderer>().sortingOrder = 5;
        mouseObject.transform.localScale = Vector3.one / 5;
    }

    public void SetMouseState(MouseState state)
    {
        prevState = currentState;
        currentState = state;

        UpdateCursorSprite();
    }

    private void UpdateCursorSprite()
    {
        if (transitionTime > 0 && prevState != currentState) return;

        transitionTime = .4f;

        mouseObject.GetComponent<SpriteRenderer>().sprite = mouseSprites[(int)currentState];
    }

    private void Update()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = -1;

        transitionTime -= Time.deltaTime;

        mouseObject.transform.position = position;
    }
}
