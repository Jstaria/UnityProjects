using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Aim : MonoBehaviour
{

    private Vector3 originalPos;
    private Quaternion originalRotation;

    [SerializeField] private Vector3 aimPos;
    [SerializeField] private Vector3 aimRotation;
    [SerializeField] private float aimLerpSpeed;

    [SerializeField] private GunRecoil gunRecoil;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 aim;
        Quaternion aimRot;

        if (Input.GetMouseButton(1))
        {
            aim = aimPos;
            aimRot = Quaternion.Euler(aimRotation);
        }
        else
        {
            aim = originalPos;
            aimRot = originalRotation;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, aim + gunRecoil.RecoilOffset, aimLerpSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, aimRot, aimLerpSpeed * Time.deltaTime);
    }
}
