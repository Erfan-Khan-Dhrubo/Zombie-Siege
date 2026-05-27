using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public Camera PlayerCamera;

    //Shooting
    public bool isShooting, readyToShoot;
    private bool _allowReset = true;
    public float shootingDelay = 2f;


    // Burst
    public int bulletsPerBurst;
    public int burstBulletsLeft;

    // Speed
    public float spreadIntensity;

    // It stores all your input actions.
    private PlayerControls _controls;

    // Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn; // This is the location where bullets appear.
    public float bulletVelocity;
    public float bulletPrefabLifetime;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;


    private void Awake()
    {
        // Creates the input system object.
        _controls = new PlayerControls();
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Update()
    {
        // AUTO MODE
        if (currentShootingMode == ShootingMode.Auto)
        {
            // True while mouse button is held
            isShooting = _controls.Player.Fire.IsPressed();
        }
        // SINGLE / BURST MODE
        else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            // True only on the frame button is pressed
            isShooting = _controls.Player.Fire.triggered;
        }

        // Fire weapon
        if (readyToShoot && isShooting)
        {
            burstBulletsLeft = bulletsPerBurst;

            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpeed().normalized;

        // Instantiate() creates a copy of an object.
        GameObject bullet = Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation
        );

        // Pointing the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;

        // Shoot the Bullet
        bullet.GetComponent<Rigidbody>().AddForce(
            shootingDirection * bulletVelocity, // Z = forward/back
            ForceMode.Impulse // apply instant force one time
        );

        // waits for some time (Coroutine)
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifetime));

        // Checking if we are done shooting
        if (_allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            _allowReset = false;
        }

        // Burst Mode
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        _allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpeed()
    {
        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }

    // IEnumerator is what makes it a Coroutine.
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        // yield return tells Unity Stop here temporarily, resume later
        yield return new WaitForSeconds(delay);

        Destroy(bullet);
    }
}