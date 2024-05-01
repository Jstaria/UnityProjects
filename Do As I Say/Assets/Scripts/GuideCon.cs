using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GuideFaceController), typeof(GuideInteraction), typeof(HitBox))]
public class GuideCon : MonoBehaviour
{
    private GuideInteraction interact;

    private void Start()
    {

        interact = GetComponent<GuideInteraction>();
    }

    private void Update()
    {
        interact.Update();
    }
}
