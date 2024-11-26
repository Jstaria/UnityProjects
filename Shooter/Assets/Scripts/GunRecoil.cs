using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [SerializeField] private float recoilAmount;
    [SerializeField] private Transform gunTransform;

    private float currentRecoilAmount;
    private float targetRecoilAmount;
    private Vector3 defaultPosition;

    private Spring spring = new Spring(500, 1, 0, true);

    public Vector3 RecoilOffset {  get; private set; }

    private void Start()
    {
        defaultPosition = gunTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        spring.Update();

        RecoilOffset = new Vector3(0, 0, Mathf.Clamp(spring.Position, -3f, 0));


        spring.RestPosition = 0;
        //gunTransform.localPosition = defaultPosition + RecoilOffset;
    }

    public void SetRecoil()
    {
        spring.RestPosition = -recoilAmount;
    }
}
