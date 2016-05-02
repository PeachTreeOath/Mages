using UnityEngine;
using System.Collections;

public class BarrelRain : Barrel {
    public float rainWidth;

    protected override void FireShot()
    {
        float randx = Random.Range(0f, rainWidth) - (rainWidth / 2f);
        Vector3 newPos = new Vector3(randx, transform.position.y, 0f);
        Bullet bullet = ((GameObject)Instantiate(bulletPrefab, newPos, transform.rotation)).GetComponent<Bullet>();
        bullet.SetType(type);
        bullet.owner = GetComponentInParent<Player>();
        bullet.isPassable = isPassable;
        bullet.speed = speed;

        //bullet.transform.parent = transform;
        bullet.Fire();


    }
}
