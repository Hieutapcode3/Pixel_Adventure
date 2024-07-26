using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosSpawnBullet : MonoBehaviour
{
    public string bulletTag = "Bullet";
    public float bulletInterval = 0.5f;
    private bool canShoot = true;

    public void Shoot()
    {
        if (canShoot)
        {
            StartCoroutine(ShootCoroutine());
        }
    }

    IEnumerator ShootCoroutine()
    {
        canShoot = false;

        yield return new WaitForSeconds(bulletInterval);
        ObjectPool.instance.SpawnFromPool(bulletTag, transform.position, Quaternion.identity);
        canShoot = true;
    }
}
