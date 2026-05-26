using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    private PlayerControls _controls;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public float bulletVelocity;
    public float bulletPrefabLifetime;

    private void Awake()
    {
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
        GameObject bullet = Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation
        );

        bullet.GetComponent<Rigidbody>().AddForce(
            bulletSpawn.forward * bulletVelocity,
            ForceMode.Impulse
        );

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifetime)
        );
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(bullet);
    }
}