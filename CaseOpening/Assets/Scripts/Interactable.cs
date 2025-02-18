using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;

    [SerializeField] private bool interactDebug;

    public void InteractWith()
    {
        OnInteract.Invoke();

        if (interactDebug)
            Debug.Log(String.Format("Interacted with: {0}", transform.name));
    }
}
