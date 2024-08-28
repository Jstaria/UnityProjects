using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;

public class TVScreen : MonoBehaviour
{
    public MeshRenderer meshRen;
    public Light light;

    public Material M_On;
    public Material M_Off;

    public TextMeshPro text;

    public RayCheck rayCheck;

    public bool mouseOver;
    public bool prevMouseOver;

    public bool MouseOver()
    {
        mouseOver = false;
        if ((rayCheck.CheckMouseOver() && GlobalVariables.InBettingStage))
        {
            mouseOver = true;
        }

        if (!prevMouseOver && mouseOver) SetText("BET?", 50, 0);

        return mouseOver;
    }

    public bool ScreenClicked()
    {
        return Input.GetMouseButtonDown(0) && mouseOver;
    }

    public void TurnOn()
    {
        meshRen.material = M_On;
        light.intensity = 1.25f;

        mouseOver = false;
        prevMouseOver = true;
    }

    public void TurnOff()
    {
        meshRen.material = M_Off;
        light.intensity = 0;
        ScreenShadow(true);
    }

    public void ScreenShadow(bool onOff)
    {
        meshRen.receiveShadows = onOff;
    }

    public void SetText(string displayText, int textSize, int startingTextIndex)
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(.025f, .05f, displayText, textSize, startingTextIndex));
        prevMouseOver = mouseOver;
    }

    private IEnumerator TypeText(float maxIntervalOffset, float interval, string displayText, int textSize, int startingTextIndex)
    {
        text.text = "";
        text.fontSize = textSize;

        for (int i = 0; i < startingTextIndex; i++)
        {
            text.text += displayText[i];
        }

        for (int i = startingTextIndex; i < displayText.Length; i++)
        {
            text.text += displayText[i];

            yield return new WaitForSeconds(interval + Random.Range(-maxIntervalOffset, maxIntervalOffset));
        }
    }
}
