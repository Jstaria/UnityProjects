using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBar : MonoBehaviour
{
    public float MaxValue;
    public float subtractValue;

    public float value;
    public float length;
    public float startLength;

    public bool reduce;

    private float transformScale;
    public RectTransform barTransform;

    // Start is called before the first frame update

    private void Start()
    {
        length = startLength;
        transformScale = barTransform.localScale.x;
    }

    private void Update()
    {
        length = Mathf.Clamp01(length);

        Vector3 scale = barTransform.localScale;
        scale.x = length * transformScale;

        barTransform.localScale = scale;

        if (reduce) length -= subtractValue * Time.deltaTime;
        else length += subtractValue * 2 * Time.deltaTime;

        value = MaxValue * length;
    }
}
