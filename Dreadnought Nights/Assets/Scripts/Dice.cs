using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private void Start()
    {
        //GetComponent<Rigidbody>().angularDrag = .01f;
        StartCoroutine(StartSpin());
    }

    private IEnumerator StartSpin()
    {
        GetComponent<Rigidbody>().useGravity = false;

        for (int i = 0; i < 100; i++)
        {
            GetComponent<Rigidbody>().angularVelocity = new Vector3(100, 0) * 1000;
            yield return new WaitForEndOfFrame();
        }

        GetComponent<Rigidbody>().useGravity = true;
    }
}
