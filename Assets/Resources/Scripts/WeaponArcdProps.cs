using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Use this for weapon props that travel in arcs
public class WeaponArcdProps : WeaponProps {


    [SerializeField]
    public GameObject tether; //tether, if applicable
    [SerializeField]
    public float radiusFromTether;
    [SerializeField]
    public float radius;
    [SerializeField]
    public float angularSpeed;
    [SerializeField]
    public float angularDrag;
    [SerializeField]
    public float angularVelocity;

    private float preTheta = 0;
    private const float twoPi = Mathf.PI * 2f;

    public override void startInit(Weapon weaponInstance) {
        base.startInit(weaponInstance);
        Debug.Log("WeaponArc init called on " + gameObject.name);

        body = gameObject.GetComponentInParent<Rigidbody2D>();

        body.angularVelocity = angularVelocity;

        preTheta = twoPi / numberOfBullets; //calculate this outside of the loop

    }


    // Update is called once per frame
    public override void doUpdate() {
        transform.Rotate(0f, 0f, angularVelocity * Time.deltaTime, Space.World);
        base.doUpdate();
    }

    protected override GameObject createBullet(Vector3 pos, Quaternion rot) {
        GameObject go = base.createBullet(pos, rot);
        Bullet b = go.GetComponent<Bullet>();
        b.SetSpeed(speed, angularVelocity, angularDrag);
        return go;
    }
    public override List<GameObject> doFireCallback() {
        List<GameObject> objs = new List<GameObject>();
        for (int i = 0; i < numberOfBullets; i++) {
            float theta = i * preTheta;
            float thetaInDegs = (360 * theta) / twoPi;

            Vector3 bulletPosition = transform.position;
            bulletPosition.x = transform.position.x + radius * Mathf.Cos(theta);
            bulletPosition.y = transform.position.y + radius * Mathf.Sin(theta);

            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, thetaInDegs + 180f);

            GameObject bulletgo = createBullet(bulletPosition, bulletRotation);
            Bullet bullet = bulletgo.GetComponent<Bullet>();

            objs.Add(bulletgo);

        }

        decrementAmmo();

        return objs;

    }

    //IDK wtf this is 
    //    public override void doUpdate() {
    //        if (tether != null) {
    //            RotatingTether mytether = ((GameObject)Instantiate(tether, transform.position, transform.rotation)).GetComponent<RotatingTether>();
    //            //tether.objectToCircle = gameObject;
    //            mytether.transform.parent = transform;

    //            bullet = ((GameObject)Instantiate(bulletPrefab, mytether.transform.position, mytether.transform.rotation)).GetComponent<Bullet>();
    //            //bullet.transform.parent = tether.transform;
    //            //Vector3 bulletPosition = tether.transform.position;
    //            //bulletPosition.y = bulletPosition.y + radiusFromTether;
    //            //bullet.transform.position = bulletPosition;
    //            //bullet.transform.rotation = tether.transform.rotation;
    //            bullet.speed = 1;

    //            //currentWeapon = (Weapon)Instantiate(nextWeapon, transform.position, transform.rotation);
    //            //currentWeapon.transform.parent = this.transform;
    //        } else {
    //            bullet = ((GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation)).GetComponent<Bullet>();
    //            bullet.SetSpeed(speed, angularSpeed, angularDrag);
    //        }
    //    }


}