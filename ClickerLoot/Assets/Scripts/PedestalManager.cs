using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalManager : MonoBehaviour
{
    public HitBox hitBox;

    private void Start()
    {
        //CameraArm.Instance.desiredPosition = transform.position + new Vector3(2, 2, 0);
        //CameraArm.Instance.desiredRotation = Quaternion.LookRotation(transform.position - CameraArm.Instance.desiredPosition).eulerAngles;

        //CameraArm.Instance.transform.position = transform.position + new Vector3(2, 2, 0);
        //CameraArm.Instance.transform.LookAt(transform.position);

        hitBox.OnClick += PointManager.Instance.AddPoints;
        hitBox.OnClick += GetComponent<Squish>().OnClick;
    }
}
