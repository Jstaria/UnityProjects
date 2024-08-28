using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    public List<Light> playerLights;
    public List<GameObject> lightMaterials;

    public Material M_lightOn;
    public Material M_lightOff;

    public TVScreen screen;
    public Light spotlight;

    public AudioManager AudioManager;
    private void Start()
    {
        SetAllLightsOff();
        //StartCoroutine(RotateLights());
    }

    private void SetLightOff(int light)
    {
        if (light == 1)
        {
            screen.ScreenShadow(false);
        }

        Material[] materials = lightMaterials[light].GetComponent<Renderer>().materials;
        materials[1] = M_lightOff;
        lightMaterials[light].GetComponent<Renderer>().materials = materials;

        playerLights[light].intensity = 0;

        AudioManager.PlayClip(0, .05f, light);
    }

    private void SetAllLightsOff()
    {
        for (int i = 0; i < playerLights.Count; i++)
        {
            SetLightOff(i);
        }
    }

    private IEnumerator RotateLights()
    {
        int i = 0;

        while (true)
        {
            i = (i + 1) % 3;

            TurnOnLight(i);
            TurnOffLight((i + 1) % 3);
            TurnOffLight((i + 2) % 3);

            //if (i++ % 2 == 0)
            //{
            //    TurnAllLightsOn();
            //}
            //else
            //{
            //    TurnAllLightsOff();
            //}

            yield return new WaitForSeconds(.125f);
        }
    }

    public void TurnOffLight(int light)
    {
        if (light == 1)
        {
            screen.ScreenShadow(false);
        }

        Material[] materials = lightMaterials[light].GetComponent<Renderer>().materials;
        materials[1] = M_lightOff;
        lightMaterials[light].GetComponent<Renderer>().materials = materials;

        //playerLights[light].gameObject.SetActive(false);
        StartCoroutine(ChangeLightIntensity(playerLights[light], 0, 20));

        AudioManager.PlayClip(0, .1f, light);
    }

    public void TurnOnLight(int light)
    {
        if (light == 1)
        {
            screen.ScreenShadow(true);
        }
        //Debug.Log(light);
        Material[] materials = lightMaterials[light].GetComponent<Renderer>().materials;
        materials[1] = M_lightOn;
        lightMaterials[light].GetComponent<Renderer>().materials = materials;

        //playerLights[light].gameObject.SetActive(true);
        StartCoroutine(ChangeLightIntensity(playerLights[light], 3, 200));
        AudioManager.PlayClip(1, .1f, light);
    }

    public IEnumerator SweepLights()
    {
        //StopAllCoroutines();

        SetAllLightsOff();


        TurnOffSpotlight();

        yield return new WaitForSeconds(2);
    }

    public void TurnOnSpotlight()
    {
        StartCoroutine(ChangeLightIntensity(spotlight, 1.25f, 200));
        AudioManager.PlayClip(2, .1f, 3);
    }

    public void TurnOffSpotlight()
    {
        StartCoroutine(ChangeLightIntensity(spotlight, 0, 50));
        AudioManager.PlayClip(3, .1f, 3);
    }

    private IEnumerator ChangeLightIntensity(Light light, float intensity, float lerp)
    {
        while (light.intensity < intensity - .01 || light.intensity > intensity + .01)
        {
            float lerpSpeed = Mathf.Pow(.5f, lerp * Time.deltaTime);
            light.intensity = Mathf.Lerp(intensity, light.intensity, lerpSpeed);

            yield return new WaitForEndOfFrame();
        }
    }

    public void StartBlackJack(List<int> ints,int light)
    {
        StartCoroutine(BlackJack(ints, light));
    }

    private IEnumerator BlackJack(List<int> ints, int light)
    {
        yield return new WaitForSeconds(1);

        TurnOffSpotlight();
        SetAllLightsOff();

        yield return new WaitForSeconds(2);

        AudioManager.LoopClip(4, 4);

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < ints.Count; j++)
            {
                TurnOnLight(ints[j]);
            }

            yield return new WaitForSeconds(.25f);

            for (int j = 0; j < ints.Count; j++)
            {
                SetLightOff(ints[j]);
            }

            yield return new WaitForSeconds(.15f);
        }

        AudioManager.UnLoopSource();

        yield return new WaitForSeconds(2);

        light %= playerLights.Count;

        //TurnOnLight(light);
        TurnOnSpotlight();

        GlobalVariables.IsInBlackJackPhase = false;
    }

    public void TurnAllLightsOn()
    {
        for (int i = 0; i < playerLights.Count; i++)
        {
            TurnOnLight(i);

            i %= playerLights.Count;
        }
    }

    public void TurnAllLightsOff()
    {
        for (int i = 0; i < playerLights.Count; i++)
        {
            TurnOffLight(i);

            i %= playerLights.Count;
        }
    }
}
