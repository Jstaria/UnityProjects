using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private List<HitBox> hitBoxes;

    [SerializeField] private MouseManager mouseManager;

    public void Update()
    {
        MouseState state = MouseState.Normal;

        for (int i = 0; i < hitBoxes.Count; i++)
        {
            if (hitBoxes[i].Contains(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                // Just hover area

                if (Input.GetMouseButtonDown((int)MouseButton.Left))
                {

                    state = MouseState.Hover;
                    hitBoxes[i].InvokeHitboxEvent();
                }
            }
        }

        mouseManager.SetMouseState(state);
        mouseManager.Update();
    }
}
