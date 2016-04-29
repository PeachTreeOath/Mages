using UnityEngine;
using System.Collections;

public class BarrelBurst : Barrel {

    //public GameObject bulletPrefab;
    public float radius = 1f;
    public int numberOfBullets = 16;
    public float angularSpeed = 0f;
    public float angularDrag = 0f;
    //public float speed = 1f;
    //public int type;
    //public bool isPassable = false;
    //public float shotDelay = .3f;
    //public float burstTime = 1f;
    //public float burstDelay = 3f;
    //public float perturbation = .1f;

    //private bool readyToShoot = true;
    //private bool inBurst = false;
    //private float burstTimer;
    //private Rigidbody2D body;
    private float preTheta;
    private float twoPi;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        //body = gameObject.GetComponentInParent<Rigidbody2D>();

        preTheta = (2f * Mathf.PI) / numberOfBullets; //calculate this outside of the loop
        twoPi = Mathf.PI * 2f;
        //burstTimer = Time.time;
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        transform.Rotate(0f, 0f, angularSpeed * Time.deltaTime, Space.World);

    }

    protected override void FireShot()
    {
        for (int i = 0; i < numberOfBullets; i++)
        {
            float theta = i * preTheta;
            float thetaInDegs = (360 * theta) / twoPi;

            Vector3 bulletPosition = transform.position;
            bulletPosition.x = transform.position.x + radius * Mathf.Cos(theta);
            bulletPosition.y = transform.position.y + radius * Mathf.Sin(theta);


            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, thetaInDegs - 90f);

            Bullet bullet = ((GameObject)Instantiate(bulletPrefab, bulletPosition, bulletRotation)).GetComponent<Bullet>();
            bullet.SetType(type);
            bullet.owner = GetComponentInParent<Player>();
            bullet.isPassable = isPassable;
            bullet.speed = speed;

            //bullet.transform.parent = transform;
            bullet.Fire();
        }

    }
}
