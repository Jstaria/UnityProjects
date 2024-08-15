using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GuideFaceController), typeof(GuideInteraction), typeof(HitBox))]
public class GuideCon : MonoBehaviour
{
    private GuideInteraction interact;

    public void Start()
    {
        interact = GetComponent<GuideInteraction>();

        interact.Start();

        GetComponent<HitBox>().OnHitBoxTrigger += interact.OnClick;
    }

    public void Update()
    {
        interact.Update();
    }
}
