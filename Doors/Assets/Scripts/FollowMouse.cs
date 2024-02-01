using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : Agent
{
    [SerializeField] private bool useX = true;
    [SerializeField] private bool useY = true;

    [SerializeField] private float lerpSpeed;
    [SerializeField] private float pullForce = 1;

    protected override void CalcSteeringForce()
    {
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 MouseForce =  MousePos - phyObj.Position;
        Vector3 TotalForce = new Vector3(useX ? MouseForce.x : 0, useY ? MouseForce.y : 0) * (MouseForce.magnitude / pullForce);

        phyObj.ApplyForce(TotalForce);
        phyObj.Position = Vector3.Lerp(phyObj.Position, MousePos, lerpSpeed);
    }
}
