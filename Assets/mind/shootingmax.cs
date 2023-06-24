using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class shootingmax : MonoBehaviour
{
    [Header("參考物件")]
    public Camera PlayerCamera;
    public Transform attackPoint;

    [Header("子彈預置物件")]
    public GameObject bullet;

    [Header("槍枝設定")]
    public int magazineSize;
    public int bulletsLeft;
    public float reloadTime;
    public float recoilForce;

    bool reloading;

    [Header("UI物件")]
    public TextMeshProUGUI ammunitionDisplay;
    public TextMeshProUGUI reloadingDisplay;

    private void Start()
    {
        bulletsLeft = magazineSize;
        reloadingDisplay.enabled = false;

        ShowAmmoDisplay();
    }

    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            if (bulletsLeft > 0 && !reloading)
            {
                Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
            Reload();
    }

    private void Shoot()
    {
        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit) == true)
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Debug.DrawRay(ray.origin, targetPoint - ray.origin, Color.red, 10);

        Vector3 shootingDirection = targetPoint - attackPoint.position;
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = shootingDirection.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(currentBullet.transform.forward * 100,ForceMode.Impulse);

        bulletsLeft--;

        this.GetComponent<Rigidbody>().AddForce(-shootingDirection.normalized * recoilForce, ForceMode.Impulse);

        ShowAmmoDisplay();
    }

    private void Reload()
    {
        reloading = true;
        reloadingDisplay.enabled = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
        reloadingDisplay.enabled = false;
        ShowAmmoDisplay();
    }

    private void ShowAmmoDisplay()
    {
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText($"Ammo {bulletsLeft} / {magazineSize}");
    }
}