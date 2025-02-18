using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Transform button;
    [SerializeField] private Transform casing;

    [SerializeField] private float angularFrequency;
    [SerializeField] private float dampingRatio;

    [SerializeField] private float PressHeight;
    [SerializeField] private float PressWidth;
    [SerializeField] private float CasingWidth;

    private float defaultButtonHeight;
    private float defaultButtonWidth;
    private float defaultCasingWidth;

    private Dictionary<string, Spring> springs;
    private float pressTimer;

    private void Start()
    {
        defaultButtonHeight = button.localScale.y;
        defaultButtonWidth = button.localScale.x;
        defaultCasingWidth = casing.localScale.x;

        springs = new Dictionary<string, Spring>();

        springs.Add("ButtonHeight", new Spring(angularFrequency, dampingRatio, 1, true));
        springs.Add("ButtonStretch", new Spring(angularFrequency, dampingRatio, 1, true));
        springs.Add("CasingStretch", new Spring(angularFrequency, dampingRatio, 1, true));
    }

    private void Update()
    {
        foreach (Spring spring in springs.Values)
        {
            spring.Update();
            spring.SetValues(angularFrequency, dampingRatio);

            if (pressTimer < 0)
                spring.RestPosition = 1;
        }

        Vector3 buttonTransform = button.transform.localScale;
        Vector3 casingTransform = casing.transform.localScale;

        buttonTransform.y = defaultButtonHeight * springs["ButtonHeight"].Position;
        buttonTransform.x = defaultButtonWidth * springs["ButtonStretch"].Position;
        buttonTransform.z = defaultButtonWidth * springs["ButtonStretch"].Position;
        casingTransform.x = defaultCasingWidth * springs["CasingStretch"].Position;
        casingTransform.z = defaultCasingWidth * springs["CasingStretch"].Position;

        button.transform.localScale = buttonTransform;
        casing.transform.localScale = casingTransform;

        pressTimer -= Time.deltaTime;
    }

    public void OnInteract()
    {
        pressTimer = .1f;
        springs["ButtonHeight"].RestPosition = PressHeight;
        springs["ButtonStretch"].RestPosition = PressWidth;
        springs["CasingStretch"].RestPosition = CasingWidth;
    }
}
