using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ProjectGun : MonoBehaviour
{
    private Animator animator;
    public AudioClip Shot;
    public AudioClip reload;
    private AudioSource audioSource;
    //子彈
    public GameObject Bullet;

    //子彈力道
    public float shootForce, upwardForce;

    //槍械狀態
    public float timeBetweenShooting, spread, reloadTime, timeBetweenshot;
    public int magazinSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    //最大備彈數量
    public int maxReserveAmmmo = 120;
    private int ReserveAmmo;

    //射擊狀態
    bool shooting, readyToShot, reloading;

    //其他設定
    public Camera fpscam;
    public Transform attackPoint;

    //Text UI 元件
    public Text currentGunAmmoText;

    //子彈生成點
    public GameObject muzzleFlash;

    //bug檢查
    public bool allowInvoke = true;
    
    private void Awake()
    {
        bulletsLeft = magazinSize;
        readyToShot = true;
        ReserveAmmo = maxReserveAmmmo;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        UpdateAmmoUI();
    }

    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        if(allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazinSize && !reloading)
        {
            Reload();
        }
        if(readyToShot && shooting && !reloading && bulletsLeft <= 0)
        {
            Reload();
        }

        if(readyToShot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            animator.SetTrigger("Shot");
            shoot();
        }
    }

    private void shoot()
    {
        readyToShot = false;

        Ray ray = fpscam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint = Physics.Raycast(ray, out hit) ? hit.point : ray.GetPoint(75); ;
        
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x,y,z);

        GameObject currentBullet = Instantiate(Bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpscam.transform.up * upwardForce, ForceMode.Impulse);

        

        if(muzzleFlash != null)
        {
            GameObject flash = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
            flash.transform.forward = fpscam.transform.forward;
        }

        bulletsLeft--;
        bulletsShot++;

        UpdateAmmoUI();

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
            audioSource.PlayOneShot(Shot);
        }

        if(bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("shoot", timeBetweenShooting);
            audioSource.PlayOneShot(Shot);
        }
    }

    private void ResetShot()
    {
        readyToShot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        if(ReserveAmmo == 0) return;
        
        animator.SetTrigger("Reload");
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    public void ReloadSound()
    {
        audioSource.PlayOneShot(reload);
    }

    private void ReloadFinished()
    {
        int bulletsToReload = magazinSize - bulletsLeft;

        if(ReserveAmmo >= bulletsToReload)
        {
            bulletsLeft += bulletsToReload;
            ReserveAmmo -= bulletsToReload;
        }
        else
        {
            bulletsLeft += ReserveAmmo;
            ReserveAmmo = 0;
        }
        reloading = false;
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        if(currentGunAmmoText != null)
        {
            currentGunAmmoText.text = $"{bulletsLeft} / {ReserveAmmo}";
        }
    }
}
