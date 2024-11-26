using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public List<GameObject> lights;

    private bool isOn;

    public void SwitchLight()
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(isOn);
        }  

        isOn = !isOn;
    }
}
