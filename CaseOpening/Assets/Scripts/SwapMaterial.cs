using Mono.CSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMaterial : MonoBehaviour
{
    [SerializeField] private List<Material> mats;

    [SerializeField] private Renderer renderer;

    private int index;

    private void Start()
    {
        index = 0;
        renderer.material = mats[index];
    }

    public void Switch()
    {
        index = (index + 1) % mats.Count;

        renderer.material = mats[index];
    }

    public void Switch(int index)
    {
        this.index = index;
        renderer.material = mats[index];
    }
}
