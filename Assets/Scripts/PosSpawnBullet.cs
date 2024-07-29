using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosSpawnBullet : MonoBehaviour
{
    public string bulletTag = "Bullet";
    public float bulletInterval = 0.3f;
    private bool canShoot = true;

    public GameObject Shoot(Vector2 direction)
    {
        if (canShoot)
        {
            StartCoroutine(ShootCoroutine(direction));
        }
        return null; 
    }

    IEnumerator ShootCoroutine(Vector2 direction)
    {
        canShoot = false;

        GameObject bullet = ObjectPool.instance.SpawnFromPool(bulletTag, transform.position, Quaternion.identity);
        BulletControl bulletControl = bullet.GetComponent<BulletControl>();
        if (bulletControl != null)
        {
            bulletControl.SetMoveDirection(direction);
        }

        yield return new WaitForSeconds(bulletInterval);
        canShoot = true;
 
    }
}
