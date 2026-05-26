using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    // It stores all your input actions.
    private PlayerControls _controls;

    public GameObject bulletPrefab;
    public Transform bulletSpawn; // This is the location where bullets appear.

    public float bulletVelocity;
    public float bulletPrefabLifetime;

    private void Awake()
    {
        // Creates the input system object.
        _controls = new PlayerControls();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    void Update()
    {
        if (_controls.Player.Fire.triggered)
        {
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        // Instantiate() creates a copy of an object.
        GameObject bullet = Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation
        );

        bullet.GetComponent<Rigidbody>().AddForce(
            bulletSpawn.forward * bulletVelocity, // Z = forward/back
            ForceMode.Impulse // apply instant force one time
        );

        // waits for some time (Coroutine)
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifetime));
    }

    // IEnumerator is what makes it a Coroutine.
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        // yield return tells Unity Stop here temporarily, resume later
        yield return new WaitForSeconds(delay);

        Destroy(bullet);
    }
}