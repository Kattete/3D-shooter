using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProjectileGun : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float shootForce, upwardForce;
    [SerializeField] private float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    [SerializeField] private int magazineSize, bulletsPerTap;
    [SerializeField] private bool allowedButtonHold, shooting, readyToShoot, reloading;
    [SerializeField] private int bulletsLeft, bulletsShot;
    [SerializeField] private Camera cameraFps;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private bool allowedInvoke = true;

    [Header("Grpahics")]
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] TMP_Text ammunitionDisplay;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        if(ammunitionDisplay != null) ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
    }

    private void MyInput()
    {
        if (allowedButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        Ray ray = cameraFps.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit)) targetPoint = hit.point;
        else targetPoint = ray.GetPoint(75);

        Vector3 directionWithoutSpread = targetPoint - shootingPoint.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        GameObject currentBullet = Instantiate(bullet, shootingPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(cameraFps.transform.up * upwardForce, ForceMode.Impulse);

        //instantiate muzzleFlash
        if (muzzleFlash != null) Instantiate(muzzleFlash, shootingPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        if (allowedInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowedInvoke = false;
        }

        if (bulletsShot < bulletsPerTap & bulletsLeft > 0) Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowedInvoke = true;
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

}
