using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private List<HitBox> hitBoxes = new List<HitBox>();

    private MouseManager mouseManager;

    private void Start()
    {
        mouseManager = GetComponent<MouseManager>();
    }

    private void Update()
    {
        MouseState state = MouseState.Normal;

        for (int i = 0; i < hitBoxes.Count; i++)
        {
            if (hitBoxes[i].Contains(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && Input.GetMouseButtonDown((int)MouseButton.Left))
            {
                state = MouseState.Hover;
                hitBoxes[i].InvokeHitboxEvent();
                break;
            }
        }

        mouseManager.SetMouseState(state);
    }
}
