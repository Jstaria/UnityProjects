using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shadow))]
public class ButtonShadow : MonoBehaviour
{
    Shadow shadow;

    private void Start()
    {
        shadow = GetComponent<Shadow>();

        List<GameObject> list = new List<GameObject>();
        list.Add(transform.GetChild(0).gameObject);

        shadow.SetShadowImage(list);
    }
}
