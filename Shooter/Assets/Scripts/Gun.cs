using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, aimSpread, reloadTime, timeBetweenShots, range;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    public Animator animator;

    [SerializeField] private Vector3 aimPos;
    [SerializeField] private Vector3 aimRotation;
    [SerializeField] private float aimLerpSpeed;

    private List<Vector3> bulletHolePoints = new List<Vector3>();
    private Vector3 ShotFromPoint;
    private Vector3 originalPos;
    private Quaternion originalRotation;
    private float currentSpread;

    [SerializeField] private GameObject trailObject;

    [SerializeField] private GunRecoil recoil;

    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;
    //public CamShake camShake;
    public float camShakeMagnitude, camShakeDuration;
    public TextMeshProUGUI text;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        originalPos = transform.localPosition;
        originalRotation = transform.localRotation;
    }
    private void Update()
    {
        MyInput();

        //SetText
        //text.SetText(bulletsLeft + " / " + magazineSize);
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
            //animator.Play("Reloading");
        }
        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletHolePoints = new List<Vector3>();
            ShotFromPoint = attackPoint.position;
            bulletsShot = bulletsPerTap;
            Shoot();
        }

        AimDownSights();
    }

    private void AimDownSights()
    {
        if (Input.GetButton("Fire2") && !reloading)
        {
            currentSpread = Mathf.Lerp(currentSpread, aimSpread, aimLerpSpeed * Time.deltaTime);
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPos + recoil.RecoilOffset, aimLerpSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(aimRotation), aimLerpSpeed * Time.deltaTime);
        }       
        else
        {
            currentSpread = Mathf.Lerp(currentSpread, spread, aimLerpSpeed * Time.deltaTime);
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos + recoil.RecoilOffset, aimLerpSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, aimLerpSpeed * Time.deltaTime);
        }
    }

    private void Shoot()
    {
        recoil.SetRecoil();

        readyToShoot = false;

        //Spread
        float x = Random.Range(-currentSpread, currentSpread);
        float y = Random.Range(-currentSpread, currentSpread);
        float z = Random.Range(-currentSpread, currentSpread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, z);
        Ray ray = new Ray(fpsCam.transform.position, direction);
        //RayCast
        if (Physics.Raycast(ray, out rayHit, range, whatIsEnemy))
        {
            //Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy"))
            {
                //rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
            }
        }

        //ShakeCamera
        //camShake.Shake(camShakeDuration, camShakeMagnitude);

        //Graphics
        if (rayHit.point != Vector3.zero)
        {
            Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.LookRotation(rayHit.normal));

            bulletHolePoints.Add(rayHit.point);
        }
        else
        {
            rayHit.point = ray.origin + ray.direction * range;
        }

        SpawnTrail(rayHit, attackPoint);

        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private void SpawnTrail(RaycastHit rayHit, Transform attackPoint)
    {
        TrailScript trail = Instantiate(trailObject, attackPoint.position, Quaternion.identity).GetComponent<TrailScript>();
        trail.SetEndPos(rayHit.point);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        foreach (Vector3 point in bulletHolePoints)
        {
            Gizmos.DrawLine(point, ShotFromPoint);
        }
    }
}
